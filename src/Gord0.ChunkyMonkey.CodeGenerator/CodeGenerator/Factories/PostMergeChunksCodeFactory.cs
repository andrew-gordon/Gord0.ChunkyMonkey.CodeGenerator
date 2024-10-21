using System.Text;
using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories
{
    internal class PostMergeChunksCodeFactory
    {
        internal string ForArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"            if ({propertyRecord.TemporaryListVariableNameForArray} is not null)");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                instance.{propertyRecord.Symbol.Name} = {propertyRecord.TemporaryListVariableNameForArray}.ToArray();");
            sb.AppendLine($"            }}");

            sb.AppendLine($"            else");

            var value = (propertyRecord.Symbol.Type.NullableAnnotation == Microsoft.CodeAnalysis.NullableAnnotation.Annotated) ? "null" : "[]";
            {
                sb.AppendLine($"            {{");
                sb.AppendLine($"                instance.{propertyRecord.Symbol.Name} = {value};");
                sb.AppendLine($"            }}");
            }
            return sb.ToString();
        }
    }
}