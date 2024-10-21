using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Represents a record for a property.
    /// </summary>
    public class PropertyRecord(IPropertySymbol p, TypeRecord? typeRule, string declarationType, bool isArray, ITypeSymbol? arrayElementType, bool chunkProperty, string lastValueVariableName, string? temporaryListVariableNameForArrays)
    {

        /// <summary>
        /// Gets or sets the symbol of the property.
        /// </summary>
        public IPropertySymbol Symbol { get; } = p;

        /// <summary>
        /// Gets or sets the type rule for the property.
        /// </summary>
        public TypeRecord? TypeRule { get; } = typeRule;

        /// <summary>
        /// Gets or sets the declaration type of the property.
        /// </summary>
        public string DeclarationType { get; } = declarationType;

        /// <summary>
        /// Gets or sets a value indicating whether the property should be chunked.
        /// </summary>
        public bool ChunkProperty { get; set; } = chunkProperty;

        /// <summary>
        /// Gets or sets the variable name for the last value of the property.
        /// </summary>
        public string? LastValueVariableName { get; set; } = lastValueVariableName;

        /// <summary>
        /// Gets or sets the temporary variable name for arrays.
        /// </summary>
        public string? TemporaryListVariableNameForArray { get; set; } = temporaryListVariableNameForArrays;

        public bool IsArray { get; } = isArray;
        public ITypeSymbol? ArrayElementType { get; } = arrayElementType;
    }
}