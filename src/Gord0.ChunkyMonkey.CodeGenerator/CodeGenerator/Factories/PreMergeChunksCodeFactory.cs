using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories
{
    internal class PreMergeChunksCodeFactory
    {
        /// <summary>
        /// Generates code to finalise array property merging.
        /// </summary>
        /// <param name="propertyRecord">The property record.</param>
        /// <returns>The generated code.</returns>
        internal string ForArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{propertyRecord.ArrayElementType!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }

        internal string ForImmutableArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }
    }
}