using Gord0.ChunkMonkey.Attributes;
using System.Collections.Immutable;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public sealed partial class Chunk_ClassWithImmutableListProperty
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public ImmutableList<int>? Numbers { get; set; }
    }
}