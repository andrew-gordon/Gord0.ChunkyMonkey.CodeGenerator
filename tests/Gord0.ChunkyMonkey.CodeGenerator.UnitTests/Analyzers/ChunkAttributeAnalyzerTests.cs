using Gord0.ChunkyMonkey.CodeGenerator.Analyser;
using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Analyzers
{
    /*
     Useful resources:
     - https://www.meziantou.net/how-to-test-a-roslyn-analyzer.htm
    */

    public class ChunkAttributeAnalyzerTests
    {
        [Fact]
        public async Task ChunkAttributeAnalyzer_WhenNoChunkableProperties_NoChunkablePropertiesRuleIsFired()
        {
            const string testCode =
                """
                #nullable enable 
                using Gord0.ChunkMonkey.Attributes;

                [Chunk]
                public partial class ClassWithChunkAttributeButNoChunkableProperties
                {
                    public string? Name { get; set; }
                    public int? Age { get; set; }
                }
                """;

            var test = AnalyzerTestHelper.CreateAnalyzerTest<ChunkAttributeAnalyzer>(testCode);

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NoChunkablePropertiesRule.Id, DiagnosticSeverity.Warning)
                    .WithMessage("ChunkAttribute should only be applied to a class with at least one chunkable collection property")
                    .WithSpan(5, 22, 5, 69));

            await test.RunAsync();
        }

        [Fact]
        public async Task ChunkAttributeAnalyzer_WhenNoChunkablePropertiesForSpecifiedMemberAccessor_GetterAndSetterMissingForChunkableCollectionPropertyInClassDecoratedWithChunkAttributeRuleIsFired()
        {
            const string testCode =
                """
                #nullable enable 
                using Gord0.ChunkMonkey.Attributes;

                [Chunk(Accessibility.Public)]
                public sealed partial class Chunk_PublicFields_ClassWithPrivateArrayProperty
                {
                    public string? Name { get; set; }
                    public int? Age { get; set; }
                    private int[]? Numbers { get; set; } // This private property won't get chunked as class is decorated with [Chunk(MemberAccessor.Public)]
                }
                """;

            var test = AnalyzerTestHelper.CreateAnalyzerTest<ChunkAttributeAnalyzer>(testCode);

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.GetterAndSetterMissingForChunkableCollectionPropertyInClassDecoratedWithChunkAttributeRule.Id, DiagnosticSeverity.Warning)
                    .WithMessage("ChunkAttribute cannot be applied to class 'Chunk_PublicFields_ClassWithPrivateArrayProperty' as it contains chunkable property 'Numbers' that has no accessible getter and setter. Property 'Numbers' does not not have a public getter (required by class ChunkAttribute) and a public setter (required by class ChunkAttribute).")
                    .WithSpan(5, 29, 5, 77)
                    .WithArguments("Property 'Numbers' does not not have a public getter (required by class ChunkAttribute) and a public setter (required by class ChunkAttribute)."));

            await test.RunAsync();
        }
    }
}
