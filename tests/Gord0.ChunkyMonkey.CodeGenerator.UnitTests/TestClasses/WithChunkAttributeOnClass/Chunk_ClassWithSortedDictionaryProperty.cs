using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public sealed partial class Chunk_ClassWithSortedDictionaryProperty
    {
        public SortedDictionary<string, int>? Numbers { get; set; }
    }
}