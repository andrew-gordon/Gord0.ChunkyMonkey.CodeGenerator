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

            var test = new CSharpAnalyzerTest<ChunkMemberAttributeAnalyzer, DefaultVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.All
            };        

            test.TestState.Sources.Add(testCode);   
            test.TestState.AdditionalReferences.Add(typeof(ChunkMemberAttribute).Assembly);
            test.TestState.ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            test.DisabledDiagnostics.Add("CS1591"); // xml comments
            test.DisabledDiagnostics.Add("CS8618"); // Non-nullable property is uninitialized.

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

            var test = new CSharpAnalyzerTest<ChunkMemberAttributeAnalyzer, DefaultVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.All
            };

            test.TestState.Sources.Add(testCode);
            test.TestState.AdditionalReferences.Add(typeof(ChunkMemberAttribute).Assembly);
            test.TestState.ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            test.DisabledDiagnostics.Add("CS1591"); // xml comments

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

            var test = new CSharpAnalyzerTest<ChunkMemberAttributeAnalyzer, DefaultVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.All
            };

            test.TestState.Sources.Add(testCode);
            test.TestState.AdditionalReferences.Add(typeof(ChunkMemberAttribute).Assembly);
            test.TestState.ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            test.DisabledDiagnostics.Add("CS1591"); // xml comments
            test.DisabledDiagnostics.Add("CS8618"); // Non-nullable property is uninitialized.

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

            var test = new CSharpAnalyzerTest<ChunkMemberAttributeAnalyzer, DefaultVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.All
            };

            test.TestState.Sources.Add(testCode);
            test.TestState.AdditionalReferences.Add(typeof(ChunkMemberAttribute).Assembly);
            test.TestState.ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            test.DisabledDiagnostics.Add("CS1591"); // xml comments
            test.DisabledDiagnostics.Add("CS8618"); // Non-nullable property is uninitialized.

            test.ExpectedDiagnostics.Add(
                new DiagnosticResult(DiagnosticDescriptors.NonSupportedChunkingTypeWithChunkMemberRule.Id, DiagnosticSeverity.Error)
                    .WithMessage("ChunkMemberAtribute cannot be applied to a member with a type that ChunkyMonkey cannot chunk")
                    .WithSpan(4, 23, 4, 32));

            await test.RunAsync();
        }
    }
}
