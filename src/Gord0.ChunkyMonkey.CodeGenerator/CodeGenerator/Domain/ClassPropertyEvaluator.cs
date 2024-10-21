using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
using Microsoft.CodeAnalysis;

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

                        if (propertyAccessor != Accessibility.Public && attributeMemberAccessorValue == 0)
                        {
                            ignoreProperty = true;
                            accessibilityRequirementFulfilled = false;
                        }
                    }


                    var lastValueVariableName = "lastValue_" + p.Name;
                    var temporaryListVariableNameForArrays = isArray ? "tempArrayList_" + p.Name : null;
                    var arrayElementType = isArray ? ((IArrayTypeSymbol)p.Type).ElementType : null;
                    return new PropertyRecord(p, typeRule, declarationType, isArray, isClassChunked, isMemberChunked, accessibilityRequirementFulfilled, arrayElementType, ignoreProperty, lastValueVariableName, temporaryListVariableNameForArrays);
                })
                .Where(x => !removeIgnoredProperties || !x.IgnoreProperty)
                .ToArray();

            return classProperties;
        }
    }
}
