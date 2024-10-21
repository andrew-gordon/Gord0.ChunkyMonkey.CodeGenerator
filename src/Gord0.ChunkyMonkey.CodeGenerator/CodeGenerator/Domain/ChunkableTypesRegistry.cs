using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Factories;
using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Represents a registry of chunkable types.
    /// </summary>
    internal class ChunkableTypesRegistry
    {
        private static ReadOnlyCollection<TypeRecord>? typeRecords = null;

        /// <summary>
        /// Gets the collection of type records.
        /// </summary>
        public ReadOnlyCollection<TypeRecord> TypeRecords => typeRecords!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkableTypesRegistry"/> class.
        /// </summary>
        public ChunkableTypesRegistry()
        {
            var chunkCodeFactory = new ChunkCodeFactory();
            var mergeChunksCodeFactory = new MergePopertyValuesFromChunkFactory();
            var preMergeChunksCodeFactory = new PreMergeChunksCodeFactory();
            var postMergeChunksCodeFactory = new PostMergeChunksCodeFactory();

            if (typeRecords == null)
            {
                var list = new List<TypeRecord>();
                list.AddRange(
                    [
                        new TypeRecord(
                        Name: "List<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.List<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForListProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForListProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Collection<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.Collection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Dictionary<K,V>",
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
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForArrayProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForArrayProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForArrayProperty),

                    new TypeRecord(
                        Name: "ReadOnlyCollection<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.ReadOnlyCollection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForReadOnlyCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForReadOnlyCollectionProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForReadOnlyCollectionProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForReadOnlyCollectionyProperty),

                    new TypeRecord(
                        Name: "ImmutableArray<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Immutable.ImmutableArray<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Length",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForImmutableArrayProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForImmutableArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForImmutableArrayProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForImmutableArrayProperty),

                    new TypeRecord(
                        Name: "ImmutableList<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Immutable.ImmutableList<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForImmutableListProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForImmutableListProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForImmutableListProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForImmutableListProperty),

                    new TypeRecord(
                        Name: "HashSet<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.HashSet<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForHashSetProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForHashSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "SortedSet<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.SortedSet<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForSortedSetProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForSortedSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "ArraySegment<T>",
                        TypeMatcher: x => x.IsGenericType("System.ArraySegment<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForArraySegmentProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForArraySegmentProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForArraySegmentProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForArraySegmentProperty),

                    new TypeRecord(
                        Name: "StringCollection",
                        TypeMatcher: x => x.IsType("System.Collections.Specialized.StringCollection") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: false,
                        ChunkCodeFactory: chunkCodeFactory.ForStringCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForStringCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "SortedList<TKey, TValue>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.SortedList<TKey, TValue>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForSortedListProperty,
                        MergePopertyValuesFromChunkFactory: mergeChunksCodeFactory.ForSortedListProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),


                ]);

                typeRecords = new ReadOnlyCollection<TypeRecord>(list);
            }
        }

        /// <summary>
        /// Gets the collection of type records.
        /// </summary>
        /// <returns>The collection of type records.</returns>
        public IReadOnlyCollection<TypeRecord> GetTypeRecords() => typeRecords!;

        /// <summary>
        /// Gets the type record for the specified property symbol.
        /// </summary>
        /// <param name="propertySymbol">The property symbol.</param>
        /// <returns>The type record for the specified property symbol.</returns>
        public TypeRecord GetTypeRecord(IPropertySymbol propertySymbol) => typeRecords.FirstOrDefault(x => x.TypeMatcher(propertySymbol));
    }
}
