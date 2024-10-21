using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories
{
    /// <summary>
    /// Produces code to set the value of a property on an instance by merging the value of the property from a chunk.
    /// </summary>
    internal class MergePopertyValuesFromChunkFactory
    {
        internal string ForArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if ({propertyRecord.TemporaryListVariableNameForArray} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        {propertyRecord.TemporaryListVariableNameForArray} = [];");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableNameForArray}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForImmutableArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if ({propertyRecord.TemporaryListVariableNameForArray} is null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableNameForArray} = [];");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            sb.AppendLine($"                {propertyRecord.TemporaryListVariableNameForArray}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            return sb.ToString();
        }

        internal string ForHashSetProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType}>();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    foreach(var value in chunk.{propertyRecord.Symbol.Name})");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(value);");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForSortedSetProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType}>();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    foreach(var value in chunk.{propertyRecord.Symbol.Name})");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(value);");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForDictionaryProperty(PropertyRecord propertyRecord)
        {
            var genericType1 = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var genericType2 = propertyRecord.GenericTypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType1},{genericType2}>();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    foreach(var kvp in chunk.{propertyRecord.Symbol.Name})");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(kvp.Key, kvp.Value);");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();

        }

        internal string ForCollectionProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType}>();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        foreach(var value in chunk.{propertyRecord.Symbol.Name})");
            sb.AppendLine($"                        {{");
            sb.AppendLine($"                            instance.{propertyRecord.Symbol.Name}.Add(value);");
            sb.AppendLine($"                        }}");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForListProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType}>();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                            instance.{propertyRecord.Symbol.Name}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForReadOnlyCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if ({propertyRecord.TemporaryListVariableNameForArray} is null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableNameForArray} = [];");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            sb.AppendLine($"                {propertyRecord.TemporaryListVariableNameForArray}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            return sb.ToString();
        }

        internal string ForArraySegmentProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if ({propertyRecord.TemporaryListVariableNameForArray} is null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableNameForArray} = new List<{genericType}>();");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            sb.AppendLine($"                {propertyRecord.TemporaryListVariableNameForArray}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            return sb.ToString();
        }

        internal string ForImmutableListProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if ({propertyRecord.TemporaryListVariableNameForArray} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        {propertyRecord.TemporaryListVariableNameForArray} = [];");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableNameForArray}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForStringCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.AddRange(chunk.{propertyRecord.Symbol.Name}.Cast<string>().ToArray());");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }
    }
}