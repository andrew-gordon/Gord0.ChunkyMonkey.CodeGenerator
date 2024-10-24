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
    /// <summary>
    /// Analyzes classes with the ChunkAttribute and reports diagnostics based on specific rules.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ChunkAttributeAnalyzer : DiagnosticAnalyzer
    {
        private readonly ClassPropertyEvaluator classPropertyEvaluator;

        public ChunkAttributeAnalyzer()
        {
            this.classPropertyEvaluator = new ClassPropertyEvaluator();
        }

        /// <summary>
        /// Gets the supported diagnostics for the ChunkAttributeAnalyzer.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [
            DiagnosticDescriptors.NonAbstractClassRule,
            DiagnosticDescriptors.NonStaticClassRule,
            DiagnosticDescriptors.ClassWithParameterlessContructorRule,
            DiagnosticDescriptors.NoChunkablePropertiesRule,
            DiagnosticDescriptors.GetterAndSetterMissingForChunkableCollectionPropertyInClassDecoratedWithChunkAttributeRule
        ];

        /// <summary>
        /// Initializes the ChunkAttributeAnalyzer.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        }

        /// <summary>
        /// Analyzes a class declaration and reports diagnostics based on specific rules.
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
            
            var hasChunkAttribute = classSymbol.GetAttribute(attributeFullTypeName: AttributeFullTypeNames.Chunk) is not null;

            var classRecord = new ClassRecord(classSymbol);
            var properties = classPropertyEvaluator.GetProperties(classRecord, removeIgnoredProperties: false);
            var hasChunkMemberAttributeOnAtLeastOneProperty = properties.Any(x => x.IsMemberDecoratedWithChunkMemberAttribute);


            if (!hasChunkAttribute && !hasChunkMemberAttributeOnAtLeastOneProperty)
            {
                return;
            }

            bool isAbstract = classSymbol.IsAbstract;
            bool isStatic = classSymbol.IsStatic;

            if (isAbstract)
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.NonAbstractClassRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
            if (isStatic)
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.NonStaticClassRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }

            bool hasParameterlessConstructor = classSymbol.Constructors
                .Any(constructor => constructor.Parameters.IsEmpty && !constructor.IsStatic);

            if (!hasParameterlessConstructor)
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.ClassWithParameterlessContructorRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }


            if (hasChunkMemberAttributeOnAtLeastOneProperty)
            {
                var inaccessibleChunkableProperties = properties
                    .Where(p => p.IsMemberDecoratedWithChunkMemberAttribute &&
                                p.HasSupportedTypeForChunking && 
                                !p.AccessibilityRequirementFulfilled);

                CheckPropertiesHaveAccessibleGetterAndSetter(context, classDeclaration, classSymbol, inaccessibleChunkableProperties);
            }

            if (hasChunkAttribute)
            {
                var inaccessibleChunkableProperties = properties
                    .Where(p => p.IsClassDecoratedWithChunkAttribute &&
                                p.HasSupportedTypeForChunking &&
                                !p.AccessibilityRequirementFulfilled);

                CheckPropertiesHaveAccessibleGetterAndSetter(context, classDeclaration, classSymbol, inaccessibleChunkableProperties);
            }

            var chunkableMembers = properties.Count(p => (p.IsClassDecoratedWithChunkAttribute || p.IsMemberDecoratedWithChunkMemberAttribute) && p.HasSupportedTypeForChunking);

            if (chunkableMembers == 0)
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.NoChunkablePropertiesRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void CheckPropertiesHaveAccessibleGetterAndSetter(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclaration, INamedTypeSymbol classSymbol, IEnumerable<PropertyRecord> inaccessibleChunkableProperties)
        {
            foreach (var property in inaccessibleChunkableProperties)
            {
                var sb = new StringBuilder();
                sb.Append($"Property '{property.Symbol.Name}' does not not have ");

                var parts = new List<string>();

                if (property.HasGetter && !property.GetterAccessibilityFulfilled)
                {
                    parts.Add("a public getter (required by class ChunkAttribute)");
                }

                if (property.HasSetter && !property.SetterAccessibilityFulfilled)
                {
                    parts.Add("a public setter (required by class ChunkAttribute)");
                }

                sb.Append(string.Join(" and ", parts));

                var diagnostic = Diagnostic.Create(
                    DiagnosticDescriptors.GetterAndSetterMissingForChunkableCollectionPropertyInClassDecoratedWithChunkAttributeRule,
                    classDeclaration.Identifier.GetLocation(),
                    classSymbol.Name,
                    property.Symbol.Name,
                    sb.ToString());

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}