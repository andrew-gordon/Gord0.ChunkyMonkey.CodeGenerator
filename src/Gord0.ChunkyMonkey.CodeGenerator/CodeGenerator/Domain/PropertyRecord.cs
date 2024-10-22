using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Represents a record for a property.
    /// </summary>
    public class PropertyRecord(
        IPropertySymbol propertySymbol,
        TypeRecord? typeRule,
        bool isValueType,
        string declarationType,
        bool isArray,
        bool isClassChunked,
        bool isMemberChunked,
        bool accessibilityRequirementFulfilled,
        ImmutableArray<ITypeSymbol> genericTypeArguments,
        ITypeSymbol? standardArrayElementType,
        bool ignoreProperty,
        string lastValueVariableName,
        string? temporaryListVariableName,
        bool hasGetter,
        bool hasSetter)
    {
        private static readonly SymbolDisplayFormat typeNameOnlyFormat = new(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
                genericsOptions: SymbolDisplayGenericsOptions.None);

        /// <summary>
        /// Gets or sets the symbol of the property.
        /// </summary>
        /// <value>The symbol of the property.</value>
        public IPropertySymbol Symbol { get; } = propertySymbol;

        /// <summary>
        /// Gets or sets the type rule for the property.
        /// </summary>
        /// <value>The type rule for the property.</value>
        public TypeRecord? TypeRecord { get; } = typeRule;

        /// <summary>
        /// Gets a value indicating whether the property is a value type.
        /// </summary>
        /// <value><c>true</c> if the property is a value type; otherwise, <c>false</c>.</value>
        public bool IsValueType { get; } = isValueType;

        /// <summary>
        /// Gets a value indicating whether the property has a getter.
        /// </summary>
        /// <value><c>true</c> if the property has a getter; otherwise, <c>false</c>.</value>
        public bool HasGetter { get; } = hasGetter;

        /// <summary>
        /// Gets a value indicating whether the property has a setter.
        /// </summary>
        /// <value><c>true</c> if the property has a setter; otherwise, <c>false</c>.</value>
        public bool HasSetter { get; } = hasSetter;

        /// <summary>
        /// Gets or sets the declaration type of the property.
        /// </summary>
        /// <value>The declaration type of the property.</value>
        /// 
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
        /// Gets or sets the temporary variable name for the array.
        /// </summary>
        /// <value>The temporary variable name for the array.</value>
        public string? TemporaryListVariableName { get; set; } = temporaryListVariableName;

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
        public ITypeSymbol? ArrayElementType { get; } = standardArrayElementType;

        /// <summary>
        /// Gets a value indicating whether the accessibility requirement is fulfilled.
        /// </summary>
        /// <value><c>true</c> if the accessibility requirement is fulfilled; otherwise, <c>false</c>.</value>
        public bool AccessibilityRequirementFulfilled { get; } = accessibilityRequirementFulfilled;

        /// <summary>
        /// Gets a value indicating whether the property has a supported type for chunking.
        /// </summary>
        /// <value><c>true</c> if the property has a supported type for chunking; otherwise, <c>false</c>.</value>
        public bool HasSupportedTypeForChunking => TypeRecord != null;

        /// <summary>
        /// Gets the generic type arguments.
        /// </summary>
        /// <value>The generic type arguments.</value>
        public ImmutableArray<ITypeSymbol> GenericTypeArguments => genericTypeArguments;

        /// <summary>
        /// Gets the type name of the property without qualification.
        /// </summary>
        /// <value>The type name of the property without qualification.</value>
        public string TypeNameOnly => Symbol.Type.ToDisplayString(typeNameOnlyFormat);

        /// <summary>
        /// Gets the type arguments as a comma-separated string.
        /// </summary>
        /// <value>The type arguments as a comma-separated string.</value>
        public string TypeArgsCommaSeparatedString = string.Join(", ", genericTypeArguments.Select(x => x.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
    }
}