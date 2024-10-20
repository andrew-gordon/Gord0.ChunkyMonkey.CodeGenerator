using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator
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
        Func<IPropertySymbol, string> ChunkCodeFactory,
        
        /// <summary>
        /// Gets the function that generates the merge chunks code for a property.
        /// </summary>
        Func<IPropertySymbol, string> MergeChunksCodeFactory)
    {
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; } = Name;
        
        /// <summary>
        /// Gets the function that determines if a property matches the type.
        /// </summary>
        public Func<IPropertySymbol, bool> TypeMatcher { get; } = TypeMatcher;
        
        /// <summary>
        /// Gets the name of the property that represents the length of the type.
        /// </summary>
        public string LengthPropertyName { get; } = LengthPropertyName;
        
        /// <summary>
        /// Gets the function that generates the chunk code for a property.
        /// </summary>
        public Func<IPropertySymbol, string> ChunkCodeFactory { get; } = ChunkCodeFactory;
        
        /// <summary>
        /// Gets the function that generates the merge chunks code for a property.
        /// </summary>
        public Func<IPropertySymbol, string> MergeChunksCodeFactory { get; } = MergeChunksCodeFactory;
    }
}
