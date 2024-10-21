using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk(Accessibility.All)]
    public sealed partial class Chunk_AllFields_ClassWithPrivateArrayProperty
    {
        public string? Name { get; set; }

        public int? Age { get; set; }

        private int[]? Numbers { get; set; }
    }
}