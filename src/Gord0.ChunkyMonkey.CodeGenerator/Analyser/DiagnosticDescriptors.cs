using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.Analyser
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor NonAbstractClassRule = new(
          "CMKY0001",
          "Invalid use of ChunkAttribute on an abstract class",
          "ChunkAttribute cannot be applied to an abstract class",
          "Usage",
          DiagnosticSeverity.Error,
          isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NonStaticClassRule = new(
            "CMKY0002",
            "Invalid use of ChunkAttribute on a static class",
            "ChunkAttribute cannot be applied to a static class",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ClassWithParameterlessContructorRule = new(
            "CMKY0003",
            "Invalid use of ChunkAttribute on class without parameterless constructor",
            "ChunkAttribute can only be applied to a class with a parameterless constructor",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NoChunkablePropertiesRule = new(
            "CMKY0004",
            "Invalid use of ChunkAttribute on class without chunkable collection properties",
            "ChunkAttribute should only be applied to a class with at least one chunkable collection property",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor InvalidUseOfChunkAttributesRule = new(
            "CMKY0005",
            "Invalid use of ChunkAttribute and ChunkMemberAtribute on the same class",
            "Do not not specify ChunkAttribute on a class at the same time as ChunkMemberAtribute on its fields or properties. Use either ChunkAttribute or ChunkMemberAttribute.",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NonAbstractMemberRule = new(
            "CMKY0006",
            "Invalid use of ChunkMemberAtribute on an abstract member",
            "ChunkMemberAtribute cannot be applied to an abstract member",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NonStaticMemberRule = new(
            "CMKY0007",
            "Invalid use of ChunkMemberAtribute on a static member",
            "ChunkMemberAtribute cannot be applied to a static member",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
