using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Microsoft.CodeAnalysis;
using System.Collections.Specialized;
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

        internal string ForArraySegmentProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    instance.{propertyRecord.Symbol.Name} = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToArray();");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForImmutableArrayProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            // Don't perform null check as ImmutableArray is a value type.
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToImmutableArray();");
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
            var genericType1 = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        var items = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize);");
            sb.AppendLine($"");
            sb.AppendLine($"                        if (instance.{propertyRecord.Symbol.Name} is null)");
            sb.AppendLine($"                        {{");
            sb.AppendLine($"                            instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType1}>();");
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

        internal string ForDictionaryProperty(PropertyRecord propertyRecord)
        {
            var genericType1 = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var genericType2 = propertyRecord.GenericTypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    var dict = new {propertyRecord.Symbol.Type.Name}<{genericType1}, {genericType2}>();");
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

        internal string ForCollectionProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    instance.{propertyRecord.Symbol.Name} = new {propertyRecord.Symbol.Type.Name}<{genericType}>(this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToList());");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForListProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);           
            var sb = new StringBuilder();
            sb.AppendLine($"                if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    instance.{propertyRecord.Symbol.Name} = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToList();");
            sb.AppendLine($"                }}");
            return sb.ToString();
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

        internal string ForImmutableListProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = ImmutableList.Create<{genericType}>(this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToArray());");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForStringCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    var tempValues = this.{propertyRecord.Symbol.Name}.Cast<string>().Skip(i).Take(chunkSize).ToArray();");
            sb.AppendLine($"                    var sc = new StringCollection();");
            sb.AppendLine($"                    sc.AddRange(tempValues);");
            sb.AppendLine($"                    instance.{propertyRecord.Symbol.Name} = sc;");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }
        
        internal string ForSortedListProperty(PropertyRecord propertyRecord)
        {
            var genericType1 = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var genericType2 = propertyRecord.GenericTypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat); 
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    var sortedList = new {propertyRecord.Symbol.Type.Name}<{genericType1}, {genericType2}>();");
            sb.AppendLine($"");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        var chunkPairs = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize);");
            sb.AppendLine($"                        foreach(var kvp in chunkPairs)");
            sb.AppendLine($"                        {{");
            sb.AppendLine($"                            sortedList.Add(kvp.Key, kvp.Value);");
            sb.AppendLine($"                        }}");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = sortedList;");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForSortedDictionaryProperty(PropertyRecord propertyRecord)
        {
            var genericType1 = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var genericType2 = propertyRecord.GenericTypeArguments[1].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    var sortedList = new {propertyRecord.Symbol.Type.Name}<{genericType1}, {genericType2}>();");
            sb.AppendLine($"");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        var chunkPairs = this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize);");
            sb.AppendLine($"                        foreach(var kvp in chunkPairs)");
            sb.AppendLine($"                        {{");
            sb.AppendLine($"                            sortedList.Add(kvp.Key, kvp.Value);");
            sb.AppendLine($"                        }}");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = sortedList;");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForNameValueCollectionProperty(PropertyRecord propertyRecord)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    var nameValueCollection = new {propertyRecord.Symbol.Type.Name}();");
            sb.AppendLine($"");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");

            // Use deferred execution for large NameValueCollection instances
            sb.AppendLine($"                        var keyValuePairs = (this.{propertyRecord.Symbol.Name}.{propertyRecord.TypeRecord!.LengthPropertyName} > 20000)");
            sb.AppendLine($"                            ? this.{propertyRecord.Symbol.Name}.AllKeys.SelectMany(key => this.{propertyRecord.Symbol.Name}.GetValues(key).Select(value => new {{ Key = key, Value = value }}))");
            sb.AppendLine($"                            : this.{propertyRecord.Symbol.Name}.AllKeys.SelectMany(key => this.{propertyRecord.Symbol.Name}.GetValues(key).Select(value => new {{ Key = key, Value = value }})).ToArray();");
            sb.AppendLine($"                        var chunkPairs = keyValuePairs.Skip(i).Take(chunkSize);");
            sb.AppendLine($"                        foreach(var kvp in chunkPairs)");
            sb.AppendLine($"                        {{");
            sb.AppendLine($"                            nameValueCollection.Add(kvp.Key, kvp.Value);");
            sb.AppendLine($"                        }}");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = nameValueCollection;");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }

        internal string ForImmutableHashSetProperty(PropertyRecord propertyRecord)
        {
            var genericType = propertyRecord.GenericTypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var sb = new StringBuilder();
            sb.AppendLine($"                {{");
            sb.AppendLine($"                    if (this.{propertyRecord.Symbol.Name} is not null)");
            sb.AppendLine($"                    {{");
            sb.AppendLine($"                        instance.{propertyRecord.Symbol.Name} = ImmutableHashSet.Create<{genericType}>(this.{propertyRecord.Symbol.Name}.Skip(i).Take(chunkSize).ToArray());");
            sb.AppendLine($"                    }}");
            sb.AppendLine($"                }}");
            return sb.ToString();
        }
    }
}