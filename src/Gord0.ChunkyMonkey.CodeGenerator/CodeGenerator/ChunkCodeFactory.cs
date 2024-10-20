using Microsoft.CodeAnalysis;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator
{
    /// <summary>
    /// Produces code to split a collection property into chunks.
    /// </summary>
    internal class ChunkCodeFactory
    {
        internal string ForArrayProperty(IPropertySymbol propertySymbol)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertySymbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertySymbol.Name} = this.{propertySymbol.Name}.Skip(i).Take(chunkSize).ToArray();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            return sb.ToString();
        }

        internal string ForNullableArrayProperty(IPropertySymbol propertySymbol)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertySymbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertySymbol.Name} = this.{propertySymbol.Name}.Skip(i).Take(chunkSize).ToArray();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            return sb.ToString();
        }

        internal string ForHashSetProperty(IPropertySymbol propertySymbol)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertySymbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertySymbol.Name} = this.{propertySymbol.Name}.Skip(i).Take(chunkSize).ToHashSet();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            sb.AppendLine($"");
            return sb.ToString();
        }

        internal string ForSortedSetProperty(IPropertySymbol propertySymbol)
        {
            var propertyType = propertySymbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (this.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        var items = this.{propertySymbol.Name}.Skip(i).Take(chunkSize);");
                sb.AppendLine($"");
                sb.AppendLine($"                        if (instance.{propertySymbol.Name} is null)");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                        }}");
                sb.AppendLine($"");
                sb.AppendLine($"                        foreach(var item in items)");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            instance.{propertySymbol.Name}.Add(item);");
                sb.AppendLine($"                        }}");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                sb.AppendLine($"");
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
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    var dict = new {propertySymbol.Type.Name}<{keyTypeString}, {valueTypeString}>();");
                sb.AppendLine($"");
                sb.AppendLine($"                    if (this.{propertySymbol.Name} is not null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        var chunkPairs = this.{propertySymbol.Name}.Skip(i).Take(chunkSize);");
                sb.AppendLine($"                        foreach(var kvp in chunkPairs)");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            dict.Add(kvp.Key, kvp.Value);");
                sb.AppendLine($"                        }}");
                sb.AppendLine($"                        instance.{propertySymbol.Name} = dict;");
                sb.AppendLine($"                    }}");
                sb.AppendLine($"                }}");
                sb.AppendLine($"");
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

                return $"                instance.{propertySymbol.Name} = new {propertySymbol.Type.Name}<{typeArgString}>(this.{propertySymbol.Name}.Skip(i).Take(chunkSize).ToList());";
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
                //var typeArg = namedType.TypeArguments[0];
                //var typeArgString = typeArg.ToDisplayString();
                return $"                instance.{propertySymbol.Name} = this.{propertySymbol.Name}.Skip(i).Take(chunkSize).ToList();";
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this collection: " + propertySymbol);
            }
        }
    }
}