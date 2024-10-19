using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gord0.ChunkyMonkey.CodeGenerator.Extensions
{
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
    }
}

