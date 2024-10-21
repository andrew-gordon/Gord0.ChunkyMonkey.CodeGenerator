using Gord0.ChunkyMonkey.CodeGenerator.Analyser;
using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests
{
    /*
     Useful resources:
     - https://www.meziantou.net/how-to-test-a-roslyn-analyzer.htm
    */

    public class ChunkMemberAttributeAnalyzerTests
    {
        [Fact]
        public async Task ChunkMemberAttributeAnalyzer_WhenMemberIsStatic_NonStaticMemberRuleIsFired()
        {
            const string testCode =
                """
                #nullable enable 
                using Gord0.ChunkMonkey.Attributes;

                public partial class TestClass
                {
                    [ChunkMember]
                    public static string[] Names { get; set; }

                    public int? Age { get; set; }
                }
                """;

            var test = AnalyzerTestHelper.CreateAnalyzerTest<ChunkMemberAttributeAnalyzer>(testCode);

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NonStaticMemberRule.Id, DiagnosticSeverity.Error)
                    .WithMessage("ChunkMemberAtribute cannot be applied to a static member")
                    .WithSpan(4, 22, 4, 31));

            await test.RunAsync();
        }

        [Fact]
        public async Task ChunkMemberAttributeAnalyzer_WhenMemberIsAbstract_NonAbstractMemberRuleIsFired()
        {
            const string testCode =
                """
                #nullable enable 
                using Gord0.ChunkMonkey.Attributes;

                public abstract class TestClass
                {
                    public string? Name { get; set; }

                    [ChunkMember]
                    public abstract int[] Numbers { get; set; }
                }
                """;

            var test = AnalyzerTestHelper.CreateAnalyzerTest<ChunkMemberAttributeAnalyzer>(testCode);

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NonAbstractMemberRule.Id, DiagnosticSeverity.Error)
                    .WithMessage("ChunkMemberAtribute cannot be applied to an abstract member")
                    .WithSpan(4, 23, 4, 32));

            await test.RunAsync();
        }

        [Fact]
        public async Task ChunkMemberAttributeAnalyzer_WhenMemberIsAbstractAndAnotherIsStatic_NonAbstractMemberRuleIsFired()
        {
            const string testCode =
                """
                #nullable enable 
                using Gord0.ChunkMonkey.Attributes;

                public abstract class TestClass
                {
                    [ChunkMember]
                    public static string[] Names { get; set; }

                    [ChunkMember]
                    public abstract int[] Numbers { get; set; }
                }
                """;

            var test = AnalyzerTestHelper.CreateAnalyzerTest<ChunkMemberAttributeAnalyzer>(testCode);

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NonStaticMemberRule.Id, DiagnosticSeverity.Error)
                    .WithMessage("ChunkMemberAtribute cannot be applied to a static member")
                    .WithSpan(4, 23, 4, 32));

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NonAbstractMemberRule.Id, DiagnosticSeverity.Error)
                    .WithMessage("ChunkMemberAtribute cannot be applied to an abstract member")
                    .WithSpan(4, 23, 4, 32));

            await test.RunAsync();
        }

        [Fact]
        public async Task ChunkMemberAttributeAnalyzer_WhenMemberIsOfTypeNotSupportedByChunkyMonkey_NonSupportedChunkingTypeWithChunkMemberRuleIsFired()
        {
            const string testCode =
                """
                #nullable enable 
                using Gord0.ChunkMonkey.Attributes;

                public abstract class TestClass
                {
                    [ChunkMember]
                    public string Name { get; set; }
                }
                """;

            var test = AnalyzerTestHelper.CreateAnalyzerTest<ChunkMemberAttributeAnalyzer>(testCode);

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NonSupportedChunkingTypeWithChunkMemberRule.Id, DiagnosticSeverity.Error)
                    .WithMessage("ChunkMemberAtribute cannot be applied to a member with a type that ChunkyMonkey cannot chunk. See https://github.com/andrew-gordon/Gord0.ChunkyMonkey.CodeGenerator for a list of supported types.")
                    .WithSpan(4, 23, 4, 32));

            await test.RunAsync();
        }
    }
}