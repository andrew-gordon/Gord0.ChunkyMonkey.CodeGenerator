using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain
{
    /// <summary>
    /// Represents a record for a class declaration.
    /// </summary>
    public record ClassRecord(INamedTypeSymbol ClassSymbol)
    {
        ///// <summary>
        ///// Gets the syntax node representing the class declaration.
        ///// </summary>
        //public ClassDeclarationSyntax ClassDeclaration { get; } = ClassDeclaration;

        /// <summary>
        /// Gets the named type symbol representing the class.
        /// </summary>
        public INamedTypeSymbol ClassSymbol { get; } = ClassSymbol;

        ///// <summary>
        ///// Gets a value indicating whether the class requires chunking.
        ///// </summary>
        //public bool RequiresChunking { get; } = RequiresChunking;

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name => ClassSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        /// <summary>
        /// Gets the namespace of the class.
        /// </summary>
        public string? Namespace => ClassSymbol.ContainingNamespace?.ToDisplayString();

        /// <summary>
        /// Gets a value indicating whether the class is abstract.
        /// </summary>
        public bool IsAbstract => ClassSymbol.TypeKind == TypeKind.Class && ClassSymbol.IsAbstract;

        /// <summary>
        /// Gets a value indicating whether the class is public.
        /// </summary>
        public bool IsPublic => ClassSymbol.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public;

        /// <summary>
        /// Gets a value indicating whether the class is sealed.
        /// </summary>
        public bool IsSealed => ClassSymbol.TypeKind == TypeKind.Class && ClassSymbol.IsSealed;

        /// <summary>
        /// Gets the property symbols of the class.
        /// </summary>
        /// <returns>An enumerable of property symbols of the class.</returns>
        public IEnumerable<IPropertySymbol> Properties
        {
            get
            {
                if (ClassSymbol is null)
                {
                    return [];
                }

                var members = ClassSymbol.GetMembers();
                var properies = members.OfType<IPropertySymbol>();
                return properies;
            }
        }
    }
}