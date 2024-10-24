using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk(Accessibility.All)] // this is to allow chunking of PrivateNumbers property
    public sealed partial class Chunk_ClassWithPrivateArrayProperty
    {
        public string? Name { get; set; }
        private int[]? PrivateNumbers { get; set; }
    }
}