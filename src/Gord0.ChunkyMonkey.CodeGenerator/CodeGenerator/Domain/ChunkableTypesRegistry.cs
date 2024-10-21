using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories;
using Microsoft.CodeAnalysis;
using System.Collections.ObjectModel;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Represents a registry of chunkable types.
    /// </summary>
    internal class ChunkableTypesRegistry
    {
        private readonly List<TypeRecord> typeRecords;

        /// <summary>
        /// Gets the collection of type records.
        /// </summary>
        public IReadOnlyCollection<TypeRecord> TypeRecords => typeRecords;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkableTypesRegistry"/> class.
        /// </summary>
        public ChunkableTypesRegistry()
        {
            this.typeRecords = [];
            var chunkCodeFactory = new ChunkCodeFactory();
            var mergeChunksCodeFactory = new MergePopertyValuesFromChunkFactory();
            var preMergeChunksCodeFactory = new PreMergeChunksCodeFactory();
            var postMergeChunksCodeFactory = new PostMergeChunksCodeFactory();

            typeRecords.AddRange(
                [
                    new TypeRecord(
                        Name: "List",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.List<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForListProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForListProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Collection",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.ObjectModel.Collection<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Dictionary",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.Dictionary<TKey, TValue>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForDictionaryProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForDictionaryProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Array",
                        TypeMatcher: x => x.Type.Kind == SymbolKind.ArrayType && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Length",
                        ChunkCodeFactory: chunkCodeFactory.ForArrayProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForArrayProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForArrayProperty),

                    new TypeRecord(
                        Name: "HashSet",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.HashSet<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForHashSetProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForHashSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "SortedSet",
                        TypeMatcher: x => IsGenericType(x, "System.Collections.Generic.SortedSet<T>"),
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForSortedSetProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForSortedSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),
                ]);
        }

        /// <summary>
        /// Gets the collection of type records.
        /// </summary>
        /// <returns>The collection of type records.</returns>
        public IReadOnlyCollection<TypeRecord> GetTypeRecords()
        {
            return new ReadOnlyCollection<TypeRecord>(typeRecords);
        }

        /// <summary>
        /// Gets the type record for the specified property symbol.
        /// </summary>
        /// <param name="propertySymbol">The property symbol.</param>
        /// <returns>The type record for the specified property symbol.</returns>
        public TypeRecord GetTypeRecord(IPropertySymbol propertySymbol)
        {
            return typeRecords.FirstOrDefault(x => x.TypeMatcher(propertySymbol));
        }

        /// <summary>
        /// Checks if the property symbol represents a generic type with the specified type name.
        /// </summary>
        /// <param name="symbol">The property symbol to check.</param>
        /// <param name="typeName">The type name to match.</param>
        /// <returns>True if the property symbol represents a generic type with the specified type name, otherwise false.</returns>
        private bool IsGenericType(IPropertySymbol symbol, string typeName)
        {
            if (symbol.Type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var from = namedType.ConstructedFrom.ToString();
                return from == typeName;
            }

            return false;
        }
    }
}
