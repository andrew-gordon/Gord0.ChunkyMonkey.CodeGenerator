using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
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
            //System.Diagnostics.Debugger.Launch();

            var classRecords = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: (context, _) =>
                    {
                        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
                        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);

                        return new ClassRecord(classSymbol!);
                    })
                .Where(c => c is not null && c.ClassSymbol is not null &&
                            (c.ClassSymbol.GetAttribute(AttributeFullTypeNames.Chunk) is not null ||
                            c.ClassSymbol.IsAttributeAppliedToAnyProperty(AttributeFullTypeNames.ChunkMember)));

            // Collect all the filtered classes for further processing
            var collectedClasses = classRecords.Collect();

            // Register a source output for processing and generating source files
            context.RegisterSourceOutput(collectedClasses, (spc, classRecords) =>
            {
                var usedFileNames = new HashSet<string>();

                foreach (var classRecord in classRecords)
                {
                    // Generate source code for each class
                    var generatedCode = GenerateChunkingCode(classRecord!);

                    var generatedSourceFilename = $"{classRecord!.Name}_ChunkyMonkey.cs";
                    int counter = 1;

                    // Esnure generatedSourceFilename is unique
                    while (usedFileNames.Contains(generatedSourceFilename))
                    {
                        generatedSourceFilename = $"{classRecord!.Name}_ChunkyMonkey_{counter}.cs";
                        counter++;
                    }

                    spc.AddSource(generatedSourceFilename, SourceText.From(generatedCode, Encoding.UTF8));
                    usedFileNames.Add(generatedSourceFilename);
                }
            });
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

            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Collections.Immutable;");
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


            var classPropertyEvaluator = new ClassPropertyEvaluator();
            var classProperties = classPropertyEvaluator.GetProperties(classRecord);

            var chunkCollectionProperties = classProperties
                .Where(x => x.TypeRecord is not null)
                .ToImmutableArray();

            var nonChunkedProperties = classProperties
                .Where(x => x.TypeRecord is null)
                .ToImmutableArray();

            if (chunkCollectionProperties.Any())
            {
                sb.AppendLine($"            long biggestCollectionLength = new long[] {{");
                foreach (var property in chunkCollectionProperties)
                {
                    if (property.IsValueType)
                    {
                        sb.Append($"                this.{property.Symbol.Name}.{property.TypeRecord!.LengthPropertyName}");
                    } 
                    else
                    {
                        sb.Append($"                (this.{property.Symbol.Name} is not null) ? this.{property.Symbol.Name}.{property.TypeRecord!.LengthPropertyName} : 0");
                    }

                    if (property != chunkCollectionProperties.Last())
                    {
                        sb.AppendLine(", ");
                    }
                    else
                    {
                        sb.AppendLine("");
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
                if (property.HasSupportedTypeForChunking)
                {
                    var line = property.TypeRecord!.ChunkCodeFactory(property);
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
                if (property.HasSupportedTypeForChunking && property.TypeRecord?.PreMergeChunksCodeFactory is not null)
                {
                    var line = property.TypeRecord.PreMergeChunksCodeFactory(property);
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
                if (property.HasSupportedTypeForChunking)
                {
                    var line = property.TypeRecord!.MergePopertyValuesFromChunkFactory(property);
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
                if (property.HasSupportedTypeForChunking && property.TypeRecord?.PostMergeChunksCodeFactory is not null)
                {
                    var line = property.TypeRecord.PostMergeChunksCodeFactory(property);
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