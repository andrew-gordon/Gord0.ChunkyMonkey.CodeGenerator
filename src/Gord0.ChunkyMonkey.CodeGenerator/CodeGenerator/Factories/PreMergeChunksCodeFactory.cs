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

        internal string ForArraySegmentProperty(PropertyRecord propertyRecord)
        {
            var typeArg = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{typeArg}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }

        internal string ForImmutableArrayProperty(PropertyRecord propertyRecord)
        {
            var typeArg = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{typeArg}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }

        internal string ForImmutableHashSetProperty(PropertyRecord propertyRecord)
        {
            var typeArg = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{typeArg}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }

        internal string ForImmutableListProperty(PropertyRecord propertyRecord)
        {
            var typeArg = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{typeArg}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }

        internal string ForReadOnlyCollectionProperty(PropertyRecord propertyRecord)
        {
            var typeArg = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{typeArg}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }
    }
}