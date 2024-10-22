using System.Text;
using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Microsoft.CodeAnalysis;

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
            sb.AppendLine($"            if ({propertyRecord.TemporaryListVariableName} is not null)");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                instance.{propertyRecord.Symbol.Name} = {propertyRecord.TemporaryListVariableName}.ToArray();");
            sb.AppendLine($"            }}");
            sb.AppendLine($"            else");
            var value = (propertyRecord.Symbol.Type.NullableAnnotation == NullableAnnotation.Annotated) ? "null" : "[]";
            sb.AppendLine($"            {{");
            sb.AppendLine($"                instance.{propertyRecord.Symbol.Name} = {value};");
            sb.AppendLine($"            }}");
            return sb.ToString();
        }

        internal string ForArraySegmentProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"            if ({propertyRecord.TemporaryListVariableName} is not null)");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                instance.{propertyRecord.Symbol.Name} = new ArraySegment<{genericType}>({propertyRecord.TemporaryListVariableName}.ToArray());");
            sb.AppendLine($"            }}");
            sb.AppendLine($"            else");
            var value = (propertyRecord.Symbol.Type.NullableAnnotation == NullableAnnotation.Annotated) ? "null" : "[]";
            sb.AppendLine($"            {{");
            sb.AppendLine($"                instance.{propertyRecord.Symbol.Name} = {value};");
            sb.AppendLine($"            }}");
            return sb.ToString();
        }

        internal string ForGenericImmutableCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"            instance.{propertyRecord.Symbol.Name} = ({propertyRecord.TemporaryListVariableName} ?? []).To{propertyRecord.TypeNameOnly}();");
            return sb.ToString();
        }

        internal string ForGenericCollectionProperty(PropertyRecord propertyRecord)
        { 
            var sb = new StringBuilder();
            sb.AppendLine($"            instance.{propertyRecord.Symbol.Name} = new {propertyRecord.TypeNameOnly}<{propertyRecord.TypeArgsCommaSeparatedString}>({propertyRecord.TemporaryListVariableName} ?? []);");
            return sb.ToString();
        }

        internal string ForReadOnlyObservableCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"            var observableCollection = {propertyRecord.TemporaryListVariableName} is null ? null : new ObservableCollection<{propertyRecord.TypeArgsCommaSeparatedString}>({propertyRecord.TemporaryListVariableName});");
            sb.AppendLine($"            var readOnlyObservableCollection = observableCollection is not null ? new ReadOnlyObservableCollection<{propertyRecord.TypeArgsCommaSeparatedString}>(observableCollection) : null;");
            sb.AppendLine($"            instance.{propertyRecord.Symbol.Name} = readOnlyObservableCollection;");
            return sb.ToString();
        }
    }
}