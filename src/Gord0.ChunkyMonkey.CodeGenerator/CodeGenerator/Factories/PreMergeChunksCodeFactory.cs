using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories
{
    internal class PreMergeChunksCodeFactory
    {
        internal string ForArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"            List<{propertyRecord.ArrayElementType!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>? {propertyRecord.TemporaryListVariableNameForArray} = null;");
            return sb.ToString();
        }
    }
}