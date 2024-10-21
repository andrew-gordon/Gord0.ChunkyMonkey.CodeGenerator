using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    internal class ClassPropertyEvaluator
    {
        private readonly ChunkableTypesRegistry chunkableTypesRegistry;

        public ClassPropertyEvaluator()
        {
            chunkableTypesRegistry = new ChunkableTypesRegistry();
        }

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
                        }
                    }
                    

                    var lastValueVariableName = "lastValue_" + p.Name;
                    var temporaryListVariableNameForArrays = isArray ? "tempArrayList_" + p.Name : null;
                    var arrayElementType = isArray ? ((IArrayTypeSymbol)p.Type).ElementType : null;
                    return new PropertyRecord(p, typeRule, declarationType, isArray, isClassChunked, isMemberChunked, arrayElementType, ignoreProperty, lastValueVariableName, temporaryListVariableNameForArrays);
                })         
                .Where(x => !removeIgnoredProperties || !x.IgnoreProperty)
                .ToArray();

            return classProperties;
        }
    }
}
