using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Represents a record for a property.
    /// </summary>
    public class PropertyRecord(IPropertySymbol p, TypeRecord? typeRule, string declarationType, bool isArray, bool isClassChunked, bool isMemberChunked, bool accessibilityRequirementFulfilled, ITypeSymbol? arrayElementType, bool ignoreProperty, string lastValueVariableName, string? temporaryListVariableNameForArrays)
    {

        /// <summary>
        /// Gets or sets the symbol of the property.
        /// </summary>
        /// <value>The symbol of the property.</value>
        public IPropertySymbol Symbol { get; } = p;

        /// <summary>
        /// Gets or sets the type rule for the property.
        /// </summary>
        /// <value>The type rule for the property.</value>
        public TypeRecord? TypeRecord { get; } = typeRule;

        /// <summary>
        /// Gets or sets the declaration type of the property.
        /// </summary>
        /// <value>The declaration type of the property.</value>
        public string DeclarationType { get; } = declarationType;

        /// <summary>
        /// Gets or sets a value indicating whether the property should be ignored.
        /// </summary>
        /// <value><c>true</c> if the property should be ignored; otherwise, <c>false</c>.</value>
        public bool IgnoreProperty { get; set; } = ignoreProperty;

        /// <summary>
        /// Gets or sets the variable name for the last value of the property.
        /// </summary>
        /// <value>The variable name for the last value of the property.</value>
        public string? LastValueVariableName { get; set; } = lastValueVariableName;

        /// <summary>
        /// Gets or sets the temporary variable name for arrays.
        /// </summary>
        /// <value>The temporary variable name for arrays.</value>
        public string? TemporaryListVariableNameForArray { get; set; } = temporaryListVariableNameForArrays;

        /// <summary>
        /// Gets a value indicating whether the property is an array.
        /// </summary>
        /// <value><c>true</c> if the property is an array; otherwise, <c>false</c>.</value>
        public bool IsArray { get; } = isArray;

        /// <summary>
        /// Gets a value indicating whether the class is decorated with the ChunkAttribute.
        /// </summary>
        /// <value><c>true</c> if the class is decorated with the ChunkAttribute; otherwise, <c>false</c>.</value>
        public bool IsClassDecoratedWithChunkAttribute { get; } = isClassChunked;

        /// <summary>
        /// Gets a value indicating whether the member is decorated with the ChunkMemberAttribute.
        /// </summary>
        /// <value><c>true</c> if the member is decorated with the ChunkMemberAttribute; otherwise, <c>false</c>.</value>
        public bool IsMemberDecoratedWithChunkMemberAttribute { get; } = isMemberChunked;

        /// <summary>
        /// Gets the array element type.
        /// </summary>
        /// <value>The array element type.</value>
        public ITypeSymbol? ArrayElementType { get; } = arrayElementType;

        /// <summary>
        /// Gets a value indicating whether the property is chunkable.
        /// </summary>
        /// <value><c>true</c> if the property is chunkable; otherwise, <c>false</c>.</value>
        public bool IsChunkable => TypeRecord != null;

        /// <summary>
        /// Gets a value indicating whether the accessibility requirement is fulfilled.
        /// </summary>
        /// <value><c>true</c> if the accessibility requirement is fulfilled; otherwise, <c>false</c>.</value>
        public bool AccessibilityRequirementFulfilled { get; } = accessibilityRequirementFulfilled;
    }
}