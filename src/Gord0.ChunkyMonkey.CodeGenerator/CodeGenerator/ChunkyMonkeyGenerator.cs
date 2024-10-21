using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories;
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

                    spc.AddSource($"{classRecord.Name}_ChunkeyMonkey.cs", SourceText.From(generatedCode, Encoding.UTF8));
                }
            });
        }

        /// <summary>
        /// Checks if the property symbol represents a generic type with the specified type name.
        /// </summary>
        /// <param name="symbol">The property symbol to check.</param>
        /// <param name="typeName">The type name to match.</param>
        /// <returns>True if the property symbol represents a generic type with the specified type name, otherwise false.</returns>
        private bool IsGenericType(IPropertySymbol symbol, string typeName)
        {
            if (symbol.Type is INamedTypeSymbol namedType && namedType.IsGenericType)
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
            var mergeChunksCodeFactory = new MergePopertyValuesFromChunkFactory();
            var preMergeChunksCodeFactory = new PreMergeChunksCodeFactory();
            var postMergeChunksCodeFactory = new PostMergeChunksCodeFactory();

            typeRules.AddRange(
                [
                    new TypeRecord(
                        Name: "List",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.List<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForListProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForListProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),
                    new TypeRecord(
                        Name: "Collection",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.ObjectModel.Collection<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),
                    new TypeRecord(
                        Name: "Dictionary",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.Dictionary<TKey, TValue>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForDictionaryProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForDictionaryProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),
                    new TypeRecord(
                        Name: "Array",
                        TypeMatcher: x => x.Type.Kind == SymbolKind.ArrayType && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Length",
                        ChunkCodeFactory: chunkCodeFactory.ForArrayProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForArrayProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForArrayProperty),
                    new TypeRecord(
                        Name: "HashSet",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.HashSet<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForHashSetProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForHashSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),
                    new TypeRecord(
                        Name: "SortedSet",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.SortedSet<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForSortedSetProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForSortedSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),
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

            var classProperties = classRecord.Properties
                .Select(p =>
                {
                    var typeRule = typeRules.FirstOrDefault(r => r.TypeMatcher(p));
                    var declarationType = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier | SymbolDisplayMiscellaneousOptions.UseSpecialTypes));
                    var isArray = p.Type.Kind == SymbolKind.ArrayType;
                    var chunkProperty = typeRule is not null
                            && p.DeclaredAccessibility == Accessibility.Public &&
                            (classChunkAttribute is not null || GetAttributeForSymbol(AttributeFullTypeNames.ChunkMember, p) is not null);
                    var lastValueVariableName = "lastValue_" + p.Name;
                    var temporaryListVariableNameForArrays = isArray ? "tempArrayList_" + p.Name : null;
                    var arrayElementType = isArray ? ((IArrayTypeSymbol)p.Type).ElementType : null;
                    return new PropertyRecord(p, typeRule, declarationType, isArray, arrayElementType, chunkProperty, lastValueVariableName, temporaryListVariableNameForArrays);
                })
                .ToArray();

            var chunkCollectionProperties = classProperties
                .Where(x => x.ChunkProperty)
                .ToImmutableArray();

            var nonChunkedProperties = classProperties
                .Where(x => !x.ChunkProperty)
                .ToImmutableArray();

            if (chunkCollectionProperties.Any())
            {
                sb.AppendLine($"            long biggestCollectionLength = new long[] {{");
                foreach (var property in chunkCollectionProperties)
                {
                    sb.AppendLine($"                (this.{property.Symbol.Name} is not null) ? this.{property.Symbol.Name}.{property.TypeRule!.LengthPropertyName} : 0");
                    if (property != chunkCollectionProperties.Last())
                    {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine($"            }}.Max();");
            }
            else
            {
                sb.AppendLine($"            long biggestCollectionLength = 0;");
            }

            sb.AppendLine($"");
            sb.AppendLine($"            for (int i = 0; i < biggestCollectionLength; i += chunkSize)");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                var instance = new {className}();");

            foreach (var property in classProperties)
            {
                if (property.ChunkProperty)
                {
                    var line = property.TypeRule!.ChunkCodeFactory(property);
                    sb.AppendLine(line);
                }
                else
                {
                    sb.AppendLine($"                instance.{property.Symbol.Name} = this.{property.Symbol.Name};");
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
            sb.AppendLine($"            long chunkNumber = 0;");


            foreach (var property in nonChunkedProperties)
            {
                bool isNullable = property.Symbol.Type.NullableAnnotation == NullableAnnotation.Annotated;
                var nullableSuffix = isNullable ? string.Empty : "?"; // make non-nullable types nullable for the last value variables

                sb.AppendLine($"            {property.DeclarationType}{nullableSuffix} {property.LastValueVariableName} = default;");
            }

            foreach (var property in classProperties)
            {
                if (property.ChunkProperty && property.TypeRule?.PreMergeChunksCodeFactory is not null)
                {
                    var line = property.TypeRule.PreMergeChunksCodeFactory(property);
                    sb.AppendLine(line);
                }
            }

            sb.AppendLine("");
            sb.AppendLine($"            foreach(var chunk in chunks)");
            sb.AppendLine($"            {{");


            sb.AppendLine($"                if (chunkNumber > 0)");
            sb.AppendLine($"                {{");

            foreach (var property in nonChunkedProperties)
            {
                sb.AppendLine($"                    if ({property.LastValueVariableName} != chunk.{property.Symbol.Name})");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        throw new InvalidDataException(\"Chunks contain different values for non-chunked property '{property.Symbol.Name}'\");");
                sb.AppendLine($"                    }}");
            }
            sb.AppendLine($"                }}");
            sb.AppendLine("");

            foreach (var property in classProperties)
            {
                if (property.ChunkProperty)
                {
                    var line = property.TypeRule!.MergePopertyValuesFromChunkFactory(property);
                    sb.AppendLine(line);
                }
                else
                {
                    sb.AppendLine($"                instance.{property.Symbol.Name} = chunk.{property.Symbol.Name};");
                    sb.AppendLine($"                {property.LastValueVariableName} = chunk.{property.Symbol.Name};");
                    sb.AppendLine("");
                }
            }

            sb.AppendLine($"                chunkNumber++;");
            sb.AppendLine($"            }}");
            sb.AppendLine("");

            var postMergeAdded = false;
            foreach (var property in classProperties)
            {
                if (property.ChunkProperty && property.TypeRule?.PostMergeChunksCodeFactory is not null)
                {
                    var line = property.TypeRule.PostMergeChunksCodeFactory(property);
                    sb.AppendLine(line);
                    postMergeAdded = true;
                }
            }

            if (postMergeAdded)
            {
                sb.AppendLine("");
            }

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
            var symbols = classSymbol
                .GetMembers()
                .OfType<IPropertySymbol>();

            foreach (var symbol in symbols)
            {
                var hasChunkMemberAttribute = GetAttributeForSymbol(attributeFullTypeName, symbol) is not null;
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