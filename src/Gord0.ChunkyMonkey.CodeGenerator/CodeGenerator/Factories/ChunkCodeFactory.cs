using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories
{
    /// <summary>
    /// Produces code to split a collection property into chunks.
    /// </summary>
    internal class ChunkCodeFactory
    {
        internal string ForArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToArray();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForImmutableArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            //sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            //sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToImmutableArray();");
            //sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForHashSetProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToHashSet();");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForSortedSetProperty(PropertyRecord propertyRecord)
        {
            var propertyType = propertyRecord.Symbol.Type;
            if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var typeArg = namedType.TypeArguments[0];
                var typeArgString = typeArg.ToDisplayString();

                var sb = new StringBuilder();
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        var items = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize);");
                sb.AppendLine($"");
                sb.AppendLine($"                        if (instance.{propertyRecord.Symbol.Name} is null)");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{typeArgString}>();");
                sb.AppendLine($"                        }}");
                sb.AppendLine($"");
                sb.AppendLine($"                        foreach(var item in items)");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            instance.{propertyRecord.Symbol.Name}.Add(item);");
                sb.AppendLine($"                        }}");
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
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    var dict = new {propertyRecord.Symbol.Type.Name}<{keyTypeString}, {valueTypeString}>();");
                sb.AppendLine($"");
                sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                    {{");
                sb.AppendLine($"                        var chunkPairs = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize);");
                sb.AppendLine($"                        foreach(var kvp in chunkPairs)");
                sb.AppendLine($"                        {{");
                sb.AppendLine($"                            dict.Add(kvp.Key, kvp.Value);");
                sb.AppendLine($"                        }}");
                sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = dict;");
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
                sb.AppendLine($"                if (this.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{typeArgString}>(this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToList());");
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
                var sb = new StringBuilder();
                sb.AppendLine($"                if (this.{propertyRecord.Symbol.Name} is not null)");
                sb.AppendLine($"                {{");
                sb.AppendLine($"                    instance.{propertyRecord.Symbol.Name} = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToList();");
                sb.AppendLine($"                }}");
                return sb.ToString();
            }
            else
            {
                throw new NotSupportedException("ChunkyMonkey does not support this collection: " + propertyRecord.Symbol);
            }
        }

        internal string ForReadOnlyCollectionProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = new ReadOnlyCollection<{genericType}>(this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToList());");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }
    }
}