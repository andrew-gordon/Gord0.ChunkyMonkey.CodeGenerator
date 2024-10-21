using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories;
using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
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
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.List<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForListProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForListProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Collection",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.Collection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Dictionary",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.Dictionary<TKey, TValue>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForDictionaryProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForDictionaryProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Array",
                        TypeMatcher:x => x.Type.Kind == SymbolKind.ArrayType && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Length",
                        ChunkCodeFactory: chunkCodeFactory.ForArrayProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForArrayProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForArrayProperty),

                    new TypeRecord(
                        Name: "ReadOnlyCollection",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.ReadOnlyCollection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForReadOnlyCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForReadOnlyCollectionProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForReadOnlyCollectionProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForReadOnlyCollectionyProperty),

                    new TypeRecord(
                        Name: "ImmutableArray",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Immutable.ImmutableArray<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Length",
                        ChunkCodeFactory: chunkCodeFactory.ForImmutableArrayProperty, 
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForImmutableArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForImmutableArrayProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForImmutableArrayProperty),

                    new TypeRecord(
                        Name: "HashSet",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.HashSet<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForHashSetProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForHashSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "SortedSet",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.SortedSet<T>") && x.GetMethod != null && x.SetMethod != null,
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
    }
}
