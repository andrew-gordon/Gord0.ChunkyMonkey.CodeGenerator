using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Evaluates the properties of a class.
    /// </summary>
    internal class ClassPropertyEvaluator
    {
        private readonly ChunkableTypesRegistry chunkableTypesRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassPropertyEvaluator"/> class.
        /// </summary>
        public ClassPropertyEvaluator()
        {
            chunkableTypesRegistry = new ChunkableTypesRegistry();
        }

        /// <summary>
        /// Gets the properties of a class.
        /// </summary>
        /// <param name="classRecord">The class record.</param>
        /// <param name="removeIgnoredProperties">A flag indicating whether to remove ignored properties.</param>
        /// <returns>The properties of the class.</returns>
        public IEnumerable<PropertyRecord> GetProperties(ClassRecord classRecord, bool removeIgnoredProperties = true)
        {
            var classProperties = classRecord.Properties
                .Select(p =>
                {
                    var typeRule = chunkableTypesRegistry.GetTypeRecord(p);
                    var declarationType = p.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier | SymbolDisplayMiscellaneousOptions.UseSpecialTypes));
                    var isArray = p.Type.Kind == SymbolKind.ArrayType;
                    var chunkAttribute = classRecord.ClassSymbol.GetAttribute(AttributeFullTypeNames.Chunk);
                    var chunkMemberAttribute = p.GetAttribute(AttributeFullTypeNames.ChunkMember);
                    var isClassChunked = chunkAttribute is not null;
                    var isMemberChunked = chunkMemberAttribute is not null;
                    var isChunkableCollectionProperty = typeRule is not null;
                    bool ignoreProperty = false;
                    bool accessibilityRequirementFulfilled = true;
                    bool isValueType = p.Type.IsValueType;
                    ImmutableArray<ITypeSymbol> genericTypeArguments = [];

                    var propertyType = p.Type;
                    if (propertyType is INamedTypeSymbol namedType && namedType.IsGenericType)
                    {
                        genericTypeArguments = namedType.TypeArguments;
                    }

                    if (isMemberChunked)
                    {
                        ignoreProperty = false;

                        if (p.IsStatic || p.IsAbstract)
                        {
                            ignoreProperty = true;
                        }
                    }
                    else if (isClassChunked)
                    {
                        var propertyAccessor = p.DeclaredAccessibility;
                        int? attributeMemberAccessorValue = 0;

                        if (chunkAttribute!.ConstructorArguments.Any())
                        {
                            attributeMemberAccessorValue = chunkAttribute.ConstructorArguments[0].Value as int?;
                        }

                        Accessibility[] nonPublicAccessibilities = [Accessibility.Private, Accessibility.Protected, Accessibility.ProtectedAndInternal, Accessibility.ProtectedOrInternal];
                        if (nonPublicAccessibilities.Contains(propertyAccessor) && attributeMemberAccessorValue == 0)
                        {
                            ignoreProperty = true;
                            accessibilityRequirementFulfilled = false;
                        }
                    }

                    var lastValueVariableName = "lastValue_" + p.Name;
                    var temporaryListVariableNameForArray = isArray || p.IsGenericType("System.Collections.Immutable.ImmutableArray<T>") || p.IsGenericType("System.Collections.ObjectModel.ReadOnlyCollection<T>") 
                        ? "tempArrayList_" + p.Name 
                        : null;

                    var standardArrayElementType = isArray ? ((IArrayTypeSymbol)p.Type).ElementType : null;

                    return new PropertyRecord(
                        propertySymbol: p,
                        typeRule: typeRule,
                        isValueType: isValueType,
                        declarationType: declarationType,
                        isArray: isArray, 
                        isClassChunked: isClassChunked,
                        isMemberChunked: isMemberChunked, 
                        accessibilityRequirementFulfilled: accessibilityRequirementFulfilled,
                        genericTypeArguments: genericTypeArguments, 
                        standardArrayElementType: standardArrayElementType, 
                        ignoreProperty: ignoreProperty,
                        lastValueVariableName: lastValueVariableName,
                        temporaryListVariableNameForArray: temporaryListVariableNameForArray);
                })
                .Where(x => !removeIgnoredProperties || !x.IgnoreProperty)
                .ToArray();

            return classProperties;
        }
    }
}
