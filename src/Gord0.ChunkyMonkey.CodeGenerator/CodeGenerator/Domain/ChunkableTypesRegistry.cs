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
            var mergePropertyValuesFromChunkFactory = new MergePropertyValuesFromChunkFactory();
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
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForListProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Collection<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.Collection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Dictionary<K,V>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.Dictionary<TKey, TValue>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForDictionaryProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForDictionaryProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "Array",
                        TypeMatcher:x => x.Type.Kind == SymbolKind.ArrayType && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Length",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForArrayProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForArrayProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForArrayProperty),

                    new TypeRecord(
                        Name: "ReadOnlyCollection<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.ReadOnlyCollection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForReadOnlyCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForReadOnlyCollectionProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForGenericClassProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForGenericCollectionProperty),

                    new TypeRecord(
                        Name: "ImmutableArray<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Immutable.ImmutableArray<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Length",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForImmutableArrayProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForImmutableArrayProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForGenericClassProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForGenericImmutableCollectionProperty),

                    new TypeRecord(
                        Name: "ImmutableList<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Immutable.ImmutableList<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForImmutableListProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForImmutableListProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForGenericClassProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForGenericImmutableCollectionProperty),

                    new TypeRecord(
                        Name: "HashSet<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.HashSet<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForHashSetProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForHashSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "SortedSet<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.SortedSet<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        ChunkCodeFactory: chunkCodeFactory.ForSortedSetProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForSortedSetProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "ArraySegment<T>",
                        TypeMatcher: x => x.IsGenericType("System.ArraySegment<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForArraySegmentProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForArraySegmentProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForGenericClassProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForArraySegmentProperty),

                    new TypeRecord(
                        Name: "StringCollection",
                        TypeMatcher: x => x.IsType("System.Collections.Specialized.StringCollection") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: false,
                        ChunkCodeFactory: chunkCodeFactory.ForStringCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForStringCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "SortedList<TKey, TValue>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.SortedList<TKey, TValue>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: false,
                        ChunkCodeFactory: chunkCodeFactory.ForSortedListProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForSortedListProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "SortedDictionary<TKey, TValue>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Generic.SortedDictionary<TKey, TValue>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: false,
                        ChunkCodeFactory: chunkCodeFactory.ForSortedDictionaryProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForSortedDictionaryProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "System.Collections.Specialized.NameValueCollection",
                        TypeMatcher: x => x.IsType("System.Collections.Specialized.NameValueCollection") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: false,
                        ChunkCodeFactory: chunkCodeFactory.ForNameValueCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForNameValueCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "ImmutableHashSet<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.Immutable.ImmutableHashSet<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForImmutableHashSetProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForImmutableHashSetProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForGenericClassProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForGenericImmutableCollectionProperty),

                    new TypeRecord(
                        Name: "System.Collections.ObjectModel.ObservableCollection<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.ObservableCollection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForObservableCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForObservableCollectionProperty,
                        PreMergeChunksCodeFactory: null,
                        PostMergeChunksCodeFactory: null),

                    new TypeRecord(
                        Name: "System.Collections.ObjectModel.ReadOnlyObservableCollection<T>",
                        TypeMatcher: x => x.IsGenericType("System.Collections.ObjectModel.ReadOnlyObservableCollection<T>") && x.GetMethod != null && x.SetMethod != null,
                        LengthPropertyName: "Count",
                        RequiresTemporaryListForMergingChunks: true,
                        ChunkCodeFactory: chunkCodeFactory.ForReadOnlyObservableCollectionProperty,
                        MergePopertyValuesFromChunkFactory: mergePropertyValuesFromChunkFactory.ForReadOnlyObservableCollectionProperty,
                        PreMergeChunksCodeFactory: preMergeChunksCodeFactory.ForGenericClassProperty,
                        PostMergeChunksCodeFactory: postMergeChunksCodeFactory.ForReadOnlyObservableCollectionProperty),
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
