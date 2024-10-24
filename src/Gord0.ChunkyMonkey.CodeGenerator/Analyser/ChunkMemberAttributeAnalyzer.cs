using System.Collections.Immutable;
using System.Text;
using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gord0.ChunkyMonkey.CodeGenerator.Analyser
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ChunkMemberAttributeAnalyzer : DiagnosticAnalyzer
    {
        private readonly ClassPropertyEvaluator classPropertyEvaluator;

        public ChunkMemberAttributeAnalyzer()
        {
            this.classPropertyEvaluator = new ClassPropertyEvaluator();
        }

        /// <summary>
        /// Gets the supported diagnostics for the analyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [
            DiagnosticDescriptors.InvalidUseOfChunkAttributesRule,
            DiagnosticDescriptors.NonAbstractMemberRule,
            DiagnosticDescriptors.NonStaticMemberRule,
            DiagnosticDescriptors.NonSupportedChunkingTypeWithChunkMemberRule,
            DiagnosticDescriptors.GetterAndSetterMissingOnCollectionPropertyDecoratedWithChunkMemberAttributeRule,
        ];

        /// <summary>
        /// Initializes the analyzer.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            // Register a syntax node action for class declarations
            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        }

        /// <summary>
        /// Analyzes a class declaration.
        /// </summary>
        /// <param name="context">The syntax node analysis context.</param>
        private void AnalyzeClass(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            // Get the class's symbol
            var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

            if (classSymbol == null)
            {
                return;
            }

            var classHasAPropertyWithChunkMemberAttribute = classSymbol.IsAttributeAppliedToAnyProperty(attributeFullTypeName: AttributeFullTypeNames.ChunkMember);

            if (!classHasAPropertyWithChunkMemberAttribute)
            {
                return;
            }

            var classRecord = new ClassRecord(classSymbol);

            var properties = classPropertyEvaluator.GetProperties(classRecord, removeIgnoredProperties: false);

            if (properties.Any(p => p.IsClassDecoratedWithChunkAttribute && p.IsMemberDecoratedWithChunkMemberAttribute))
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.InvalidUseOfChunkAttributesRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }

            if (properties.Any(p => p.IsMemberDecoratedWithChunkMemberAttribute && p.Symbol.IsAbstract))
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.NonAbstractMemberRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }

            if (properties.Any(p => p.IsMemberDecoratedWithChunkMemberAttribute && p.Symbol.IsStatic))
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.NonStaticMemberRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }

            var propertiesWithUnsupportedChunkingTypes = properties.Where(p => p.IsMemberDecoratedWithChunkMemberAttribute && !p.HasSupportedTypeForChunking);

            foreach(var property in propertiesWithUnsupportedChunkingTypes)
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.NonSupportedChunkingTypeWithChunkMemberRule, classDeclaration.Identifier.GetLocation(), property.Symbol.Name, 
                    property.Symbol.Type.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat));
                context.ReportDiagnostic(diagnostic);
            }

            var chunkablePropertiesWithoutGetterAndSetter = properties.Where(p => p.IsMemberDecoratedWithChunkMemberAttribute && !(p.HasGetter && p.HasSetter));

            foreach (var property in chunkablePropertiesWithoutGetterAndSetter)
            {
                var sb = new StringBuilder();
                sb.Append($"Property '{property.Symbol.Name}' does not not have ");

                var parts = new List<string>();

                if (!property.HasGetter)
                {
                    parts.Add("a getter");
                }

                if (!property.HasSetter)
                {
                    parts.Add("a setter");
                }

                sb.Append(string.Join(" and ", parts));

                var diagnostic = Diagnostic.Create(
                    DiagnosticDescriptors.GetterAndSetterMissingOnCollectionPropertyDecoratedWithChunkMemberAttributeRule,
                    classDeclaration.Identifier.GetLocation(),
                    sb.ToString());

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
