using Gord0.ChunkMonkey.Attributes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests
{
    /// <summary>
    /// Helper class for analyzer tests.
    /// </summary>
    internal class AnalyzerTestHelper
    {
        /// <summary>
        /// Creates an analyzer test.
        /// </summary>
        /// <typeparam name="TAnalyzer">The type of the analyzer.</typeparam>
        /// <param name="testCode">The test code.</param>
        /// <returns>The created analyzer test.</returns>
        internal static CSharpAnalyzerTest<TAnalyzer, DefaultVerifier> CreateAnalyzerTest<TAnalyzer>(string testCode) where TAnalyzer : DiagnosticAnalyzer, new()
        {
            var test = new CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
            {
                CompilerDiagnostics = CompilerDiagnostics.All
            };

            test.TestState.Sources.Add(testCode);
            test.TestState.AdditionalReferences.Add(typeof(ChunkMemberAttribute).Assembly);
            test.TestState.ReferenceAssemblies = ReferenceAssemblies.Net.Net80;
            test.DisabledDiagnostics.Add("CS1591"); // xml comments
            test.DisabledDiagnostics.Add("CS8618"); // Non-nullable property is uninitialized.        
            return test;
        }
    }
}
