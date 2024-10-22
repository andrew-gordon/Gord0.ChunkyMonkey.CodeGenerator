using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories
{
    /// <summary>
    /// Produces code to set the value of a property on an instance by merging the value of the property from a chunk.
    /// </summary>
    internal class MergePropertyValuesFromChunkFactory
    {
        internal string ForArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if ({propertyRecord.TemporaryListVariableName} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        {propertyRecord.TemporaryListVariableName} = [];");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForImmutableArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if ({propertyRecord.TemporaryListVariableName} is null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName} = [];");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            sb.AppendLine($"                {propertyRecord.TemporaryListVariableName}.AddRange(chunk.{propertyRecord.Symbol.Name});");
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

        internal string ForImmutableHashSetProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if ({propertyRecord.TemporaryListVariableName} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        {propertyRecord.TemporaryListVariableName} = [];");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName}.AddRange(chunk.{propertyRecord.Symbol.Name});");
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
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType1}, {genericType2}>();");
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

        internal string ForArraySegmentProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if ({propertyRecord.TemporaryListVariableName} is null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName} = new List<{genericType}>();");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            sb.AppendLine($"                {propertyRecord.TemporaryListVariableName}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            return sb.ToString();
        }

        internal string ForImmutableListProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if ({propertyRecord.TemporaryListVariableName} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        {propertyRecord.TemporaryListVariableName} = [];");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName}.AddRange(chunk.{propertyRecord.Symbol.Name});");
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

        internal string ForSortedListProperty(PropertyRecord propertyRecord)
        {
            var genericType1 = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var genericType2 = propertyRecord.GenericTypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType1}, {genericType2}>();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    foreach(var kvp in chunk.{propertyRecord.Symbol.Name})");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(kvp.Key, kvp.Value);");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForSortedDictionaryProperty(PropertyRecord propertyRecord)
        {
            var genericType1 = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var genericType2 = propertyRecord.GenericTypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType1}, {genericType2}>();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    foreach(var kvp in chunk.{propertyRecord.Symbol.Name})");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(kvp.Key, kvp.Value);");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForNameValueCollectionProperty(PropertyRecord propertyRecord)
        {
            const string elementType = "string";
            const string kvpType = "KeyValuePair<string, string>";

            var sb = new StringBuilder();
            sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    var keyValuePairs = chunk?.{propertyRecord.Symbol.Name}?.AllKeys?");
            sb.AppendLine($"                        .Where(key => key != null)");
            sb.AppendLine($"                        .SelectMany(key => (instance?.{propertyRecord.Symbol.Name}?.GetValues(key) ?? Array.Empty<{elementType}>())");
            sb.AppendLine($"                        .Where(value => value != null)");
            sb.AppendLine($"                        .Select(value => new {kvpType}(key!, value!)))");
            sb.AppendLine($"                        .ToArray() ?? Array.Empty<{kvpType}>();");
            sb.AppendLine($"");
            sb.AppendLine($"                    foreach(var kvp in keyValuePairs)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(kvp.Key, kvp.Value);");
            sb.AppendLine($"                    }}");
            return sb.ToString();
        }

        internal string ForObservableCollectionProperty(PropertyRecord propertyRecord)
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
            sb.AppendLine($"                    if (chunk is not null && chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        foreach(var value in chunk.{propertyRecord.Symbol.Name})");
            sb.AppendLine($"                        {{");
            sb.AppendLine($"                            instance.{propertyRecord.Symbol.Name}.Add(value);");
            sb.AppendLine($"                        }}");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForReadOnlyObservableCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if ({propertyRecord.TemporaryListVariableName} is null && chunk.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName} = [];");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null && {propertyRecord.TemporaryListVariableName} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForReadOnlyCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null && {propertyRecord.TemporaryListVariableName} is null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName} = [];");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null && {propertyRecord.TemporaryListVariableName} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    {propertyRecord.TemporaryListVariableName}.AddRange(chunk.{propertyRecord.Symbol.Name});");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }
    }
}