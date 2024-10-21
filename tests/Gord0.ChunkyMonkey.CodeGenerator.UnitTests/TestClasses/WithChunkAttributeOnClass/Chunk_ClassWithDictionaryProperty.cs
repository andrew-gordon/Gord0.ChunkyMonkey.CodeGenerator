using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk(Accessibility.Public)]
    public partial class Chunk_ClassWithDictionaryProperty
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
    }
}
