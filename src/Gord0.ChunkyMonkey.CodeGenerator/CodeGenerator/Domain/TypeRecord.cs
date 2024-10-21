using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Represents a record that defines the properties of a type.
    /// </summary>
    public record TypeRecord(
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        string Name,

        /// <summary>
        /// Gets the function that determines if a property matches the type.
        /// </summary>
        Func<IPropertySymbol, bool> TypeMatcher,

        /// <summary>
        /// Gets the name of the property that represents the length of the type.
        /// </summary>
        string LengthPropertyName,

        /// <summary>
        /// Gets the function that generates the chunk code for a property.
        /// </summary>
        Func<PropertyRecord, string> ChunkCodeFactory,

        /// <summary>
        /// Gets the function that generates the code to merge property values from a chunk.
        /// </summary>
        Func<PropertyRecord, string> MergePopertyValuesFromChunkFactory,

        /// <summary>
        /// Gets the function that generates the code that needs to run for the property before chunks are processed.
        /// </summary>
        Func<PropertyRecord, string>? PreMergeChunksCodeFactory,

        /// <summary>
        /// Gets the function that generates the code that needs to run for the property after chunks have been processed.
        /// </summary>
        Func<PropertyRecord, string>? PostMergeChunksCodeFactory,

        /// <summary>
        /// Gets a value indicating whether the type requires a temporary list when merging chunks.
        /// </summary>
        bool? RequiresTemporaryListForMergingChunks = false
    );
}
