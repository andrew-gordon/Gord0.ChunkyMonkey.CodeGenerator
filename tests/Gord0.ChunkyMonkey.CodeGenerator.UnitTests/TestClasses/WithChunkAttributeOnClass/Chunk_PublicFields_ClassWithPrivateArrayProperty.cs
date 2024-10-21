using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    /// <summary>
    /// Represents a class with public fields and a private array property.
    /// </summary>
    [Chunk(Accessibility.All)]
    public sealed partial class Chunk_PublicFields_ClassWithPrivateArrayProperty
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the numbers.
        /// </summary>
        private int[]? Numbers { get; set; }
    }
}