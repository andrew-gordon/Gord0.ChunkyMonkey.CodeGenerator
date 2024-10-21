using Gord0.ChunkMonkey.Attributes;
using Gord0.ChunkyMonkey.CodeGenerator.Analyser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests
{

    /*
     See https://www.meziantou.net/how-to-test-a-roslyn-analyzer.htm
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

            var test = new CSharpAnalyzerTest<ChunkAttributeAnalyzer, DefaultVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.All
            };        

            test.TestState.Sources.Add(testCode);   
            test.TestState.AdditionalReferences.Add(typeof(ChunkAttribute).Assembly);
            test.TestState.ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            test.DisabledDiagnostics.Add("CS1591"); // xml comments
            
            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NoChunkablePropertiesRule.Id, DiagnosticSeverity.Warning)
                    .WithMessage("ChunkAttribute should only be applied to a class with at least one chunkable collection property")
                    .WithSpan(5, 22, 5, 69));

            await test.RunAsync();
        }

        [Fact]
        public async Task ChunkAttributeAnalyzer_WhenNoChunkablePropertiesForSpecifiedMemberAccessor_NoChunkablePropertiesRuleIsFired()
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

            var test = new CSharpAnalyzerTest<ChunkAttributeAnalyzer, DefaultVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.All
            };

            test.TestState.Sources.Add(testCode);
            test.TestState.AdditionalReferences.Add(typeof(ChunkAttribute).Assembly);
            test.TestState.ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            test.DisabledDiagnostics.Add("CS1591"); // xml comments
            
            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NoChunkablePropertiesRule.Id, DiagnosticSeverity.Warning)
                    .WithMessage("ChunkAttribute should only be applied to a class with at least one chunkable collection property")
                    .WithSpan(5, 29, 5, 77));

            await test.RunAsync();
        }        
    }
}
