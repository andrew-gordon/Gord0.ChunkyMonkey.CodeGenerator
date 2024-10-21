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
            var arrayTypeElement = propertyRecord.Symbol.Type is IArrayTypeSymbol arrayTypeSymbol
               ? arrayTypeSymbol.ElementType.Name
               : throw new InvalidOperationException("ForNullableArrayProperty called for a property that doesn't have an array type.");

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

        internal string ForHashSetProperty(PropertyRecord propertyRecord)
        {
            var propertyType = propertyRecord.Symbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    foreach(var value in chunk.{propertyRecord.Symbol.Name})");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(value);");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this SortedSet: " + propertyRecord.Symbol);
            }
        }

        internal string ForSortedSetProperty(PropertyRecord propertyRecord)
        {
            var propertyType = propertyRecord.Symbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    foreach(var value in chunk.{propertyRecord.Symbol.Name})");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(value);");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this SortedSet: " + propertyRecord.Symbol);
            }
        }

        internal string ForDictionaryProperty(PropertyRecord propertyRecord)
        {
            var propertyType = propertyRecord.Symbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var keyType = namedType.TypeArguments[0];
                var valueType = namedType.TypeArguments[1];

                var keyTypeString = keyType.ToDisplayString();
                var valueTypeString = valueType.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{keyTypeString},{valueTypeString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    foreach(var kvp in chunk.{propertyRecord.Symbol.Name})");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name}.Add(kvp.Key, kvp.Value);");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this dictionary: " + propertyRecord.Symbol);
            }
        }

        internal string ForCollectionProperty(PropertyRecord propertyRecord)
        {
            var propertyType = propertyRecord.Symbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{typeArgString}>();");
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
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this collection: " + propertyRecord.Symbol);
            }
        }

        internal string ForListProperty(PropertyRecord propertyRecord)
        {
            var propertyType = propertyRecord.Symbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"                if (chunk.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertyRecord.Symbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{typeArgString}>();");
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
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this collection: " + propertyRecord.Symbol);
            }
        }
    }
}