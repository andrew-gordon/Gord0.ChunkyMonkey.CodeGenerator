using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gord0.ChunkyMonkey.CodeGenerator.Extensions
{
    /// <summary>
    /// Roslyn extensions for syntax nodes.
    /// </summary>
    public static class RoslynExtensions
    {
        /// <summary>
        /// Gets the namespace that contains the given ClassDeclarationSyntax.
        /// </summary>
        /// <param name="classDeclaration">The class declaration node.</param>
        /// <returns>The namespace as a string, or null if no namespace is found.</returns>
        public static string? GetNamespace(this ClassDeclarationSyntax classDeclaration)
        {
            // Traverse the parent nodes to find the NamespaceDeclarationSyntax
            var namespaceDeclaration = classDeclaration
                .Ancestors()
                .OfType<NamespaceDeclarationSyntax>()
                .FirstOrDefault();

            if (namespaceDeclaration == null)
            {
                return null; // No namespace found (e.g., global namespace)
            }

            // Return the fully qualified namespace
            return namespaceDeclaration.Name.ToString();
        }

        /// <summary>
        /// Gets the fully qualified type name of the property from a PropertyDeclarationSyntax.
        /// </summary>
        /// <param name="propertyDeclaration">The property declaration syntax.</param>
        /// <param name="semanticModel">The semantic model of the syntax tree.</param>
        /// <returns>The fully qualified name of the property type.</returns>
        public static string? GetFullPropertyTypeName(this PropertyDeclarationSyntax propertyDeclaration, SemanticModel semanticModel)
        {
            // Get the TypeSyntax of the property
            var typeSyntax = propertyDeclaration.Type;

            // Get the symbol information for the type
            var typeSymbol = semanticModel.GetTypeInfo(typeSyntax).Type;

            // Return the fully qualified name
            return typeSymbol?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        /// <summary>
        /// Gets the fully qualified type name of an attribute from an AttributeSyntax.
        /// </summary>
        /// <param name="attributeSyntax">The attribute syntax.</param>
        /// <param name="semanticModel">The semantic model.</param>
        /// <returns>The fully qualified name of the attribute's type, or null if not found.</returns>
        public static string? GetFullAttributeTypeName(this AttributeSyntax attributeSyntax, SemanticModel semanticModel)
        {
            // Get the symbol information for the attribute
            var symbolInfo = semanticModel.GetSymbolInfo(attributeSyntax);

            // The symbol should be an IMethodSymbol for the constructor of the attribute
            var attributeConstructor = symbolInfo.Symbol as IMethodSymbol;

            // The containing type of the constructor is the attribute's type
            var attributeType = attributeConstructor?.ContainingType;

            // Return the fully qualified type name if available
            var fullAttributeName = attributeType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            return fullAttributeName;
        }

        /// <summary>
        /// Checks if the specified attribute is applied to any property of the class symbol.
        /// </summary>
        /// <param name="attributeFullTypeName">The full type name of the attribute.</param>
        /// <param name="classSymbol">The class symbol to check.</param>
        /// <returns>True if the attribute is applied to any property, otherwise false.</returns>
        public static bool IsAttributeAppliedToAnyProperty(this INamedTypeSymbol classSymbol, string attributeFullTypeName)
        {
            var symbols = classSymbol
                .GetMembers()
                .OfType<IPropertySymbol>();

            foreach (var symbol in symbols)
            {
                var hasChunkMemberAttribute = GetAttribute(symbol, attributeFullTypeName) is not null;
                if (hasChunkMemberAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the attribute data for the specified attribute full type name and symbol.
        /// </summary>
        /// <param name="attributeFullTypeName">The full type name of the attribute.</param>
        /// <param name="symbol">The symbol to check for the attribute.</param>
        /// <returns>The attribute data if the attribute is found, otherwise null.</returns>
        public static AttributeData GetAttribute(this ISymbol symbol, string attributeFullTypeName)
        {
            var result = symbol.GetAttributes()
                .Where(attr =>
                {
                    var fullAttributeName = attr.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                    bool hasChunkAttribute = $"global::{attributeFullTypeName}" == fullAttributeName;
                    return hasChunkAttribute;
                })
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Checks if the property symbol represents a generic type with the specified type name.
        /// </summary>
        /// <param name="symbol">The property symbol to check.</param>
        /// <param name="typeName">The type name to match.</param>
        /// <returns>True if the property symbol represents a generic type with the specified type name, otherwise false.</returns>
        public static bool IsGenericType(this IPropertySymbol symbol, string typeName)
        {
            if (symbol.Type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                var from = namedType.ConstructedFrom.ToString();
                return from == typeName;
            }

            return false;
        }
    }
}

