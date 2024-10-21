﻿using System.Collections.Immutable;
using Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator.Domain;
using Gord0.ChunkyMonkey.CodeGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Gord0.ChunkyMonkey.CodeGenerator.Analyser
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ChunkAttributeAnalyzer : DiagnosticAnalyzer
    {      
        private readonly ClassPropertyEvaluator classPropertyEvaluator;

        public ChunkAttributeAnalyzer()
        {
            this.classPropertyEvaluator = new ClassPropertyEvaluator();
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [
            DiagnosticDescriptors.NonAbstractClassRule, 
            DiagnosticDescriptors.NonStaticClassRule, 
            DiagnosticDescriptors.ClassWithParameterlessContructorRule, 
            DiagnosticDescriptors.NoChunkablePropertiesRule];

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            // Register a syntax node action for class declarations
            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        }

        private void AnalyzeClass(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            // Get the class's symbol
            var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);

            if (classSymbol == null)
            {
                return;
            }

            // Check if the class has the ChunkAttribute
            var hasCustomAttribute = classSymbol.GetAttribute(attributeFullTypeName: AttributeFullTypeNames.Chunk) is not null;

            if (!hasCustomAttribute)
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

            var classRecord = new ClassRecord(classSymbol);

            var properties = classPropertyEvaluator.GetProperties(classRecord);

            if (!properties.Any(x => x.IsChunkable))
            {
                var diagnostic = Diagnostic.Create(DiagnosticDescriptors.NoChunkablePropertiesRule, classDeclaration.Identifier.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
