using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk(Accessibility.Public)]
    public sealed partial class Chunk_ClassWithPrivateArrayProperty
    {
        public string? Name { get; set; }
        private int? Age { get; set; }
        public int[]? Numbers { get; set; }
    }
}