using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Gord0.ChunkyMonkey.CodeGenerator.Extensions;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator
{
    /// <summary>
    /// Represents a record for a class declaration.
    /// </summary>
    public record ClassRecord(ClassDeclarationSyntax ClassDeclaration, INamedTypeSymbol ClassSymbol, bool RequiresChunking)
    {
        /// <summary>
        /// Gets the syntax node representing the class declaration.
        /// </summary>
        public ClassDeclarationSyntax ClassDeclaration { get; } = ClassDeclaration;

        /// <summary>
        /// Gets the named type symbol representing the class.
        /// </summary>
        public INamedTypeSymbol ClassSymbol { get; } = ClassSymbol;

        /// <summary>
        /// Gets a value indicating whether the class requires chunking.
        /// </summary>
        public bool RequiresChunking { get; } = RequiresChunking;

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name => ClassDeclaration.Identifier.Text;

        /// <summary>
        /// Gets the namespace of the class.
        /// </summary>
        public string? Namespace => ClassDeclaration.GetNamespace();

        /// <summary>
        /// Gets a value indicating whether the class is abstract.
        /// </summary>
        public bool IsAbstract => ClassDeclaration.Modifiers.Any(SyntaxKind.AbstractKeyword);

        /// <summary>
        /// Gets a value indicating whether the class is public.
        /// </summary>
        public bool IsPublic => ClassDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword);

        /// <summary>
        /// Gets a value indicating whether the class is sealed.
        /// </summary>
        public bool IsSealed => ClassDeclaration.Modifiers.Any(SyntaxKind.SealedKeyword);

        /// <summary>
        /// Gets a value indicating whether the class is static.
        /// </summary>
        public bool IsStatic => ClassDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword);

        /// <summary>
        /// Gets a value indicating whether the class is partial.
        /// </summary>
        public bool IsPartial => ClassDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);

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