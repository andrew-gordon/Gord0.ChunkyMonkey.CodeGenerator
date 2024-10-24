using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System;
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
                    bool isValueType = p.Type.IsValueType;
                    bool hasGetter = p.GetMethod is not null;
                    bool hasSetter = p.SetMethod is not null;

                    var propertyGetterAccessibility = p.GetMethod?.DeclaredAccessibility ?? Microsoft.CodeAnalysis.Accessibility.NotApplicable;
                    var propertySetterAccessibility = p.SetMethod?.DeclaredAccessibility ?? Microsoft.CodeAnalysis.Accessibility.NotApplicable;
                    bool getterAccessibilityRequirementFulfilled = false;
                    bool setterAccessibilityRequirementFulfilled = false;

                    Accessibility requiredGetterSetterAccessibility = Accessibility.Public; // default state

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

                        requiredGetterSetterAccessibility = Accessibility.All;

                        getterAccessibilityRequirementFulfilled = true;
                        setterAccessibilityRequirementFulfilled = true;
                    }
                    
                    if (isClassChunked && isChunkableCollectionProperty)
                    {
                        Microsoft.CodeAnalysis.Accessibility[] nonPublicAccessibilities = [
                            Microsoft.CodeAnalysis.Accessibility.Private,
                            Microsoft.CodeAnalysis.Accessibility.Protected,
                            Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal,
                            Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal
                        ];

                        if (hasGetter)
                        {
                            if (chunkAttribute!.ConstructorArguments.Any())
                            {
                                requiredGetterSetterAccessibility = ConvertNullableObjectToAccessibility(chunkAttribute.ConstructorArguments[0].Value);

                                if (nonPublicAccessibilities.Contains(propertyGetterAccessibility) && requiredGetterSetterAccessibility == Accessibility.Public)
                                {
                                    ignoreProperty = true;
                                    getterAccessibilityRequirementFulfilled = false;
                                } 
                                else
                                {
                                    getterAccessibilityRequirementFulfilled = true;
                                }
                            }
                        }

                        if (hasSetter)
                        {
                            if (chunkAttribute!.ConstructorArguments.Any())
                            {
                                requiredGetterSetterAccessibility = ConvertNullableObjectToAccessibility(chunkAttribute.ConstructorArguments[0].Value);

                                if (nonPublicAccessibilities.Contains(propertyGetterAccessibility) && requiredGetterSetterAccessibility == Accessibility.Public)
                                {
                                    ignoreProperty = true;
                                    setterAccessibilityRequirementFulfilled = false;
                                }
                                else
                                {
                                    setterAccessibilityRequirementFulfilled = true;
                                }
                            }
                        }
                    }

                    var lastValueVariableName = "lastValue_" + p.Name;
                    var requiresTemporaryListForMergingChunks = typeRule?.RequiresTemporaryListForMergingChunks ?? false;
                    var temporaryListVariableName = requiresTemporaryListForMergingChunks ? "list_" + p.Name : null;

                    var standardArrayElementType = isArray ? ((IArrayTypeSymbol)p.Type).ElementType : null;

                    return new PropertyRecord(
                        propertySymbol: p,
                        typeRule: typeRule,
                        isValueType: isValueType,
                        declarationType: declarationType,
                        isArray: isArray, 
                        isClassChunked: isClassChunked,
                        isMemberChunked: isMemberChunked, 
                        genericTypeArguments: genericTypeArguments, 
                        standardArrayElementType: standardArrayElementType, 
                        ignoreProperty: ignoreProperty,
                        lastValueVariableName: lastValueVariableName,
                        temporaryListVariableName: temporaryListVariableName,
                        hasGetter: hasGetter,
                        hasSetter: hasSetter,
                        propertyGetterAccessibility: propertyGetterAccessibility,
                        propertySetterAccessibility: propertySetterAccessibility,
                        requiredGetterSetterAccessibility: requiredGetterSetterAccessibility,
                        getterAccessibilityFulfilled: getterAccessibilityRequirementFulfilled,
                        setterAccessibilityFulfilled: setterAccessibilityRequirementFulfilled
                        );
                })
                .Where(x => !removeIgnoredProperties || !x.IgnoreProperty)
                .ToArray();

            return classProperties;
        }

        private Accessibility ConvertNullableObjectToAccessibility(object? value)
        {
            Accessibility accessibility = Accessibility.Public;

            if (value != null && Enum.IsDefined(typeof(Accessibility), value))
            {
                accessibility = (Accessibility)value;
            }

            return accessibility;
        }
    }
}
