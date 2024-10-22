using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.Analyser
{
    /// <summary>
    /// Provides diagnostic descriptors for ChunkyMonkey code analysis.
    /// </summary>
    public static class DiagnosticDescriptors
    {
        /// <summary>
        /// Represents the diagnostic descriptor for the rule that prohibits applying ChunkAttribute to an abstract class.
        /// </summary>
        public static readonly DiagnosticDescriptor NonAbstractClassRule = new(
          "CMKY0001",
          "Invalid use of ChunkAttribute on an abstract class",
          "ChunkAttribute cannot be applied to an abstract class",
          "Usage",
          DiagnosticSeverity.Error,
          isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that prohibits applying ChunkAttribute to a static class.
        /// </summary>
        public static readonly DiagnosticDescriptor NonStaticClassRule = new(
            "CMKY0002",
            "Invalid use of ChunkAttribute on a static class",
            "ChunkAttribute cannot be applied to a static class",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that prohibits applying ChunkAttribute to a class without a parameterless constructor.
        /// </summary>
        public static readonly DiagnosticDescriptor ClassWithParameterlessContructorRule = new(
            "CMKY0003",
            "Invalid use of ChunkAttribute on class without parameterless constructor",
            "ChunkAttribute can only be applied to a class with a parameterless constructor",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that warns about applying ChunkAttribute to a class without chunkable collection properties.
        /// </summary>
        public static readonly DiagnosticDescriptor NoChunkablePropertiesRule = new(
            "CMKY0004",
            "Invalid use of ChunkAttribute on class without chunkable collection properties",
            "ChunkAttribute should only be applied to a class with at least one chunkable collection property",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that prohibits specifying both ChunkAttribute and ChunkMemberAttribute on the same class.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidUseOfChunkAttributesRule = new(
            "CMKY0005",
            "Invalid use of ChunkAttribute and ChunkMemberAtribute on the same class",
            "Do not specify ChunkAttribute on a class at the same time as ChunkMemberAttribute on its fields or properties. Use either ChunkAttribute or ChunkMemberAttribute.",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that prohibits applying ChunkMemberAttribute to an abstract member.
        /// </summary>
        public static readonly DiagnosticDescriptor NonAbstractMemberRule = new(
            "CMKY0006",
            "Invalid use of ChunkMemberAtribute on an abstract member",
            "ChunkMemberAtribute cannot be applied to an abstract member",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that prohibits applying ChunkMemberAttribute to a static member.
        /// </summary>
        public static readonly DiagnosticDescriptor NonStaticMemberRule = new(
            "CMKY0007",
            "Invalid use of ChunkMemberAtribute on a static member",
            "ChunkMemberAtribute cannot be applied to a static member",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that prohibits applying ChunkMemberAttribute to a member with an unsupported chunking type.
        /// </summary>
        public static readonly DiagnosticDescriptor NonSupportedChunkingTypeWithChunkMemberRule = new(
            "CMKY0008",
            "Invalid use of ChunkMemberAtribute on a member with a type that ChunkyMonkey cannot chunk",
            "ChunkMemberAtribute cannot be applied to member '{0}' with a type that ChunkyMonkey cannot chunk ('{1}'). See https://github.com/andrew-gordon/Gord0.ChunkyMonkey.CodeGenerator for a list of supported types.",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that warns about applying ChunkAttribute to a class without a chunkable collection property that meets the member accessibility criteria.
        /// </summary>
        public static readonly DiagnosticDescriptor NoAccessibleChunkablePropertiesRule = new(
            "CMKY0009",
            "Invalid use of ChunkAttribute on class without a chunkable collection property that meets the member accessibility criteria",
            "ChunkAttribute must only be applied to a class with at least one chunkable collection property that meets the member accessibility criteria",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that warns about applying ChunkMemberAttribute to a property without a getter.
        /// </summary>
        public static readonly DiagnosticDescriptor NoGetterOnPropertyDecoratedWithChunkMemberAttributeRule = new(
            "CMKY0010",
            "Invalid use of ChunkMemberAttribute on a property without a getter",
            "ChunkAttribute must only be applied to a property with a getter",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        /// <summary>
        /// Represents the diagnostic descriptor for the rule that warns about applying ChunkMemberAttribute to a property without a setter.
        /// </summary>
        public static readonly DiagnosticDescriptor NoSetterOnPropertyDecoratedWithChunkMemberAttributeRule = new(
            "CMKY0011",
            "Invalid use of ChunkMemberAttribute on a property without a setter",
            "ChunkAttribute must only be applied to a property with a setter",
            "Usage",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

    }
}
