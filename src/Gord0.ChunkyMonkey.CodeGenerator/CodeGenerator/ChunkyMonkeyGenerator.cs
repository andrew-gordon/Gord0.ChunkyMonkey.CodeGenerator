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
            // System.Diagnostics.Debugger.Launch();

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

                    var diagnosticDescriptor = new DiagnosticDescriptor(
                        "CMG001",
                        "ChunkyMonkey",
                        $"ChunkyMonkey generated code for class '{classRecord!.Name}'",
                        "ChunkyMonkey",
                        DiagnosticSeverity.Info,
                        true);

                    var diagnostic = Diagnostic.Create(
                        diagnosticDescriptor,                        
                        Location.None,
                        classRecord!.Name,
                        generatedSourceFilename);

                    spc.ReportDiagnostic(diagnostic);
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

            var usings = new[] {
                "System",
                "System.Collections.Generic",
                "System.Collections.Immutable",
                "System.Collections.ObjectModel",
                "System.Linq",
                "System.Collections.Specialized"
            };

            foreach(var @using in usings)
            {
                sb.AppendLine($"using {@using};");
            }

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
            var classProperties = classPropertyEvaluator.GetProperties(classRecord)
                .Where(p => p.HasGetter && p.HasSetter);

            var chunkCollectionProperties = classProperties
                .Where(x => x.TypeRecord is not null)
                .ToImmutableArray();

            var nonChunkedProperties = classProperties
                .Where(x => x.TypeRecord is null)
                .ToImmutableArray();

            switch(chunkCollectionProperties.Length)
            {
                case 0:
                    sb.AppendLine($"            long biggestCollectionLength = 0;");
                    break;

                case 1:
                    {
                        var property = chunkCollectionProperties[0];
                        string line = property.IsValueType
                            ? $"this.{property.Symbol.Name}.{property.TypeRecord!.LengthPropertyName}"
                            : $"(this.{property.Symbol.Name} is not null) ? this.{property.Symbol.Name}.{property.TypeRecord!.LengthPropertyName} : 0";
                        sb.AppendLine($"            long biggestCollectionLength = {line};");
                    }
                    break;

                default:
                    {
                        sb.AppendLine($"            long biggestCollectionLength = new long[] {{");
                        foreach (var property in chunkCollectionProperties)
                        {
                            var line = property.IsValueType
                                ? $"                this.{property.Symbol.Name}.{property.TypeRecord!.LengthPropertyName}"
                                : $"                (this.{property.Symbol.Name} is not null) ? this.{property.Symbol.Name}.{property.TypeRecord!.LengthPropertyName} : 0";


                            if (property != chunkCollectionProperties.Last())
                            {
                                line += ",";
                            }

                            sb.AppendLine(line);
                        }
                        sb.AppendLine($"            }}.Max();");
                    }
                    break;
            }

            sb.AppendLine($"");
            sb.AppendLine($"            for (int i = 0; i < biggestCollectionLength; i += chunkSize)");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                var instance = new {className}();");

            foreach (var property in classProperties)
            {
                var line = property.HasSupportedTypeForChunking 
                    ? property.TypeRecord!.ChunkCodeFactory(property)
                    : $"                instance.{property.Symbol.Name} = this.{property.Symbol.Name};";

                sb.AppendLine(line);
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

            if (nonChunkedProperties.Any())
            {
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
            }

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
    }
}