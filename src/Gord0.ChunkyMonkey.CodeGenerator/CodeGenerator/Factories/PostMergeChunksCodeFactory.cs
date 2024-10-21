using System.Text;
using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories
{
    internal class PostMergeChunksCodeFactory
    {
        /// <summary>
        /// Generates the code for assigning values to an array property.
        /// </summary>
        /// <param name="propertyRecord">The property record containing the information about the array property.</param>
        /// <returns>The generated code as a string.</returns>
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

        internal string ForImmutableArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"            instance.{propertyRecord.Symbol.Name} = ({propertyRecord.TemporaryListVariableNameForArray} ?? []).ToImmutableArray();");
            return sb.ToString();
        }
    }
}