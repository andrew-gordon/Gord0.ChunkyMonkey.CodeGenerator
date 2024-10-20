using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator
{
    /// <summary>
    /// ChunkyMonkeyGenerator.
    /// </summary>
    [Generator(LanguageNames.CSharp)]
    public class ChunkyMonkeyGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the ChunkyMonkeyGenerator.
        /// </summary>
        /// <param name="context">The IncrementalGeneratorInitializationContext.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // System.Diagnostics.Debugger.Launch();

            var classRecords = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: (context, _) =>
                    {
                        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
                        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                        bool requiresChunking = classSymbol != null &&
                            (GetAttributeForSymbol(AttributeFullTypeNames.Chunk, classSymbol) is not null ||
                            IsAttributeAppliedToAnyProperty(AttributeFullTypeNames.ChunkMember, classSymbol));

                        return new ClassRecord(classDeclarationSyntax, classSymbol!, requiresChunking);
                    })
                .Where(c => c is not null && c.ClassSymbol is not null && c.RequiresChunking);

            // Collect all the filtered classes for further processing
            var collectedClasses = classRecords.Collect();

            // Register a source output for processing and generating source files
            context.RegisterSourceOutput(collectedClasses, (spc, classRecords) =>
            {
                foreach (var classRecord in classRecords)
                {
                    // Generate source code for each class
                    var generatedCode = GenerateChunkingCode(classRecord!);

                    spc.AddSource($"{classRecord.Name}_ChunkGenerated.cs", SourceText.From(generatedCode, Encoding.UTF8));
                }
            });
        }

        /// <summary>
        /// Checks if the property symbol represents a generic type with the specified type name.
        /// </summary>
        /// <param name="propertySymbol">The property symbol to check.</param>
        /// <param name="typeName">The type name to match.</param>
        /// <returns>True if the property symbol represents a generic type with the specified type name, otherwise false.</returns>
        private bool IsGenericType(IPropertySymbol propertySymbol, string typeName)
        {
            if (propertySymbol.Type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var from = namedType.ConstructedFrom.ToString();
                return from == typeName;
            }

            return false;
        }

        /// <summary>
        /// Generates the chunking code for a class record.
        /// </summary>
        /// <param name="classRecord">The class record to generate the code for.</param>
        /// <returns>The generated chunking code.</returns>
        private string GenerateChunkingCode(ClassRecord classRecord)
        {
            var className = classRecord.Name;
            var namespaceText = classRecord.Namespace ?? "Unknown_Namespace";
            var sealedModifier = classRecord.IsSealed ? "sealed " : string.Empty;

            var classChunkAttribute = GetAttributeForSymbol(AttributeFullTypeNames.Chunk, classRecord.ClassSymbol);

            var typeRules = new List<TypeRecord>();
            var chunkCodeFactory = new ChunkCodeFactory();
            var MergeChunksCodeFactory = new MergeChunksCodeFactory();

            typeRules.AddRange(
                [
                    new TypeRecord(
                        Name: "List",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.List<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForListProperty,
                        MergeChunksCodeFactory: MergeChunksCodeFactory.ForListProperty),
                    new TypeRecord(
                        Name: "Collection",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.ObjectModel.Collection<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForCollectionProperty,
                        MergeChunksCodeFactory: MergeChunksCodeFactory.ForCollectionProperty),
                    new TypeRecord(
                        Name: "Dictionary",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.Dictionary<TKey, TValue>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForDictionaryProperty,
                        MergeChunksCodeFactory: MergeChunksCodeFactory.ForDictionaryProperty),
                    new TypeRecord(
                        Name: "Array",
                        TypeMatcher: propertySymbol => propertySymbol.Type.Kind == SymbolKind.ArrayType && propertySymbol.GetMethod!=null && propertySymbol.SetMethod!=null,
                        LengthPropertyName: "Length",
                        ChunkCodeFactory: chunkCodeFactory.ForArrayProperty,
                        MergeChunksCodeFactory: propertySymbol => MergeChunksCodeFactory.ForArrayProperty(
                            propertySymbol,
                            ps => {
                                var arrayTypeElement = "Unknown";

                                if (ps.Type is IArrayTypeSymbol arrayTypeSymbol) {
                                    arrayTypeElement = arrayTypeSymbol.ElementType.Name;
                                }

                                var t = $"Array.Empty<{arrayTypeElement}>()";
                                return t;
                            })),
                    new TypeRecord(
                        Name: "HashSet",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.HashSet<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForHashSetProperty,
                        MergeChunksCodeFactory: MergeChunksCodeFactory.ForHashSetProperty),
                    new TypeRecord(
                        Name: "SortedSet",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.SortedSet<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForSortedSetProperty,
                        MergeChunksCodeFactory: MergeChunksCodeFactory.ForSortedSetProperty)
                ]
             );

            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Collections.ObjectModel;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("");

            sb.AppendLine($"namespace {namespaceText}");
            sb.AppendLine("{");

            sb.AppendLine($"    public {sealedModifier}partial class {className}");
            sb.AppendLine("    {");
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        /// Chunks the instance into multiple instances based on the specified chunk size.");
            sb.AppendLine("        /// </summary>");
            sb.AppendLine("        /// <param name=\"chunkSize\">The size of each chunk.</param>");
            sb.AppendLine("        /// <returns>An enumerable of chunked instances.</returns>");
            sb.AppendLine("        public IEnumerable<" + className + "> Chunk(int chunkSize)");
            sb.AppendLine("        {");

            sb.AppendLine($"            // Find the length of the biggest collection.");
            sb.AppendLine($"            int biggestCollectionLength = 0;");

            var properties = classRecord.Properties
                .Select(p => new
                {
                    PropertySymbol = p,
                    TypeRule = typeRules.FirstOrDefault(r => r.TypeMatcher(p))
                })
                .ToArray();

            var propertySymbolWithRules = properties
                .Where(x => x.TypeRule is not null)
                .ToImmutableArray();

            foreach (var propertySymbolWithRule in propertySymbolWithRules)
            {
                var p = propertySymbolWithRule.PropertySymbol;
                var r = propertySymbolWithRule.TypeRule;

                sb.AppendLine($"            if (this.{p.Name}.{r.LengthPropertyName} > biggestCollectionLength)");
                sb.AppendLine($"            {{");
                sb.AppendLine($"                biggestCollectionLength = this.{p.Name}.{r.LengthPropertyName};");
                sb.AppendLine($"            }}");
            }

            sb.AppendLine($"");
            sb.AppendLine($"            for (int i = 0; i < biggestCollectionLength; i += chunkSize)");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                var instance = new {className}();");

            foreach (var propertySymbolWithRule in properties)
            {
                var p = propertySymbolWithRule.PropertySymbol;
                var r = propertySymbolWithRule.TypeRule;

                var chunkMemberAttribute = GetAttributeForSymbol(AttributeFullTypeNames.ChunkMember, p);
                var propertyIsPublic = p.DeclaredAccessibility == Accessibility.Public;
                var memberToBeChunked = propertyIsPublic && (chunkMemberAttribute is not null || classChunkAttribute is not null);

                if (memberToBeChunked && r is not null)
                {
                    var line = r.ChunkCodeFactory(p);
                    sb.AppendLine(line);
                }
                else
                {
                    sb.AppendLine($"                instance.{p.Name} = this.{p.Name};");
                }
            }

            sb.AppendLine($"                yield return instance;");
            sb.AppendLine($"            }}");
            sb.AppendLine($"        }}");
            sb.AppendLine("");

            sb.AppendLine($"        /// <summary>");
            sb.AppendLine($"        /// Merges the specified chunks into a single instance.");
            sb.AppendLine($"        /// </summary>");
            sb.AppendLine($"        /// <param name=\"chunks\">The chunks to merge.</param>");
            sb.AppendLine($"        /// <returns>The merged instance.</returns>");
            sb.AppendLine($"        public static {className} MergeChunks(IEnumerable<{className}> chunks)");
            sb.AppendLine($"        {{");
            sb.AppendLine($"            var instance = new {className}();");
            sb.AppendLine("");
            sb.AppendLine($"            foreach(var chunk in chunks)");
            sb.AppendLine($"            {{");

            foreach (var propertySymbolWithRule in properties)
            {
                var p = propertySymbolWithRule.PropertySymbol;
                var r = propertySymbolWithRule.TypeRule;

                var chunkMemberAttribute = GetAttributeForSymbol(AttributeFullTypeNames.ChunkMember, p);

                var memberToBeChecked = chunkMemberAttribute is not null || classChunkAttribute is not null;
                if (memberToBeChecked && r is not null)
                {
                    var line = r.MergeChunksCodeFactory(p);
                    sb.AppendLine(line);
                }
                else
                {
                    sb.AppendLine($"                  instance.{p.Name} = chunk.{p.Name};");
                }
            }

            sb.AppendLine($"            }}");
            sb.AppendLine("");
            sb.AppendLine($"            return instance;");
            sb.AppendLine($"        }}");
            sb.AppendLine("   }");
            sb.AppendLine("}");

            var code = sb.ToString();

            return code;
        }

        /// <summary>
        /// Checks if the specified attribute is applied to any property of the class symbol.
        /// </summary>
        /// <param name="attributeFullTypeName">The full type name of the attribute.</param>
        /// <param name="classSymbol">The class symbol to check.</param>
        /// <returns>True if the attribute is applied to any property, otherwise false.</returns>
        private bool IsAttributeAppliedToAnyProperty(string attributeFullTypeName, INamedTypeSymbol classSymbol)
        {
            var propertySymbols = classSymbol
                .GetMembers()
                .OfType<IPropertySymbol>();

            foreach (var propertySymbol in propertySymbols)
            {
                var hasChunkMemberAttribute = GetAttributeForSymbol(attributeFullTypeName, propertySymbol) is not null;
                if (hasChunkMemberAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the attribute data for the specified attribute full type name and symbol.
        /// </summary>
        /// <param name="attributeFullTypeName">The full type name of the attribute.</param>
        /// <param name="symbol">The symbol to check for the attribute.</param>
        /// <returns>The attribute data if the attribute is found, otherwise null.</returns>
        private AttributeData GetAttributeForSymbol(string attributeFullTypeName, ISymbol symbol)
        {
            var result = symbol.GetAttributes()
                .Where(attr =>
                {
                    var fullAttributeName = attr.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                    bool hasChunkAttribute = $"global::{attributeFullTypeName}" == fullAttributeName;
                    return hasChunkAttribute;
                })
                .FirstOrDefault();

            return result;
        }
    }
}