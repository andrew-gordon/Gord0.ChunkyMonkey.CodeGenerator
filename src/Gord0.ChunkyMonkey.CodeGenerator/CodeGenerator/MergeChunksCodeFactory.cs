using Microsoft.CodeAnalysis;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator
{
    /// <summary>
    /// Produces code to set the value of a property on an instance by merging the value of the property from a chunk.
    /// </summary>
    internal class MergeChunksCodeFactory
    {
        internal string ForArrayProperty(IPropertySymbol propertySymbol, Func<IPropertySymbol, string?> newInstanceCommand)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"");
            sb.AppendLine($"                if (chunk.{propertySymbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (instance.{propertySymbol.Name} is null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertySymbol.Name} = {newInstanceCommand(propertySymbol)};");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"");
            sb.AppendLine($"                    instance.{propertySymbol.Name} = instance.{propertySymbol.Name}.Concat(chunk.{propertySymbol.Name}).ToArray();");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForNullableArrayProperty(IPropertySymbol propertySymbol, Func<IPropertySymbol, string?> newInstanceCommand)
        {
            var propertyType = propertySymbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"");
                sb.AppendLine($"                if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertySymbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    instance.{propertySymbol.Name} = instance.{propertySymbol.Name}.Concat(chunk.{propertySymbol.Name}).ToArray();");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this SortedSet: " + propertySymbol);
            }
        }

        internal string ForHashSetProperty(IPropertySymbol propertySymbol)
        {
            var propertyType = propertySymbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"");
                sb.AppendLine($"                if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertySymbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    foreach(var value in chunk.{propertySymbol.Name})");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name}.Add(value);");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this SortedSet: " + propertySymbol);
            }
        }

        internal string ForSortedSetProperty(IPropertySymbol propertySymbol)
        {
            var propertyType = propertySymbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"");
                sb.AppendLine($"                if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertySymbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    foreach(var value in chunk.{propertySymbol.Name})");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name}.Add(value);");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this SortedSet: " + propertySymbol);
            }
        }

        internal string ForDictionaryProperty(IPropertySymbol propertySymbol)
        {
            var propertyType = propertySymbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var keyType = namedType.TypeArguments[0];
                var valueType = namedType.TypeArguments[1];

                var keyTypeString = keyType.ToDisplayString();
                var valueTypeString = valueType.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"");
                sb.AppendLine($"                if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertySymbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{keyTypeString},{valueTypeString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    foreach(var kvp in chunk.{propertySymbol.Name})");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name}.Add(kvp.Key, kvp.Value);");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this dictionary: " + propertySymbol);
            }
        }

        internal string ForCollectionProperty(IPropertySymbol propertySymbol)
        {
            var propertyType = propertySymbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"");
                sb.AppendLine($"                if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertySymbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        foreach(var value in chunk.{propertySymbol.Name})");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            instance.{propertySymbol.Name}.Add(value);");
                sb.AppendLine($"                        }}");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this collection: " + propertySymbol);
            }
        }

        internal string ForListProperty(IPropertySymbol propertySymbol)
        {
            var propertyType = propertySymbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"");
                sb.AppendLine($"                if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (instance.{propertySymbol.Name} is null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"");
                sb.AppendLine($"                    if (chunk.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        foreach(var value in chunk.{propertySymbol.Name})");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            instance.{propertySymbol.Name}.Add(value);");
                sb.AppendLine($"                        }}");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this collection: " + propertySymbol);
            }
        }
    }
}