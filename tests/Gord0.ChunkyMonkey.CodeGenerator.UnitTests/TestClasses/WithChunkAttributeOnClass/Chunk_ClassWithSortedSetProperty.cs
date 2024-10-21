using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public sealed partial class Chunk_ClassWithSortedSetProperty
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public SortedSet<int>? Numbers { get; set; }
    }
}