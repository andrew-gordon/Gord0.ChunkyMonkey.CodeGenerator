using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty
{
    public sealed partial class ChunkMember_ClassWithSortedSetProperty
    {
        [ChunkMember]
        public string? Name { get; set; }

        [ChunkMember]
        public int? Age { get; set; }

        [ChunkMember]
        public SortedSet<int>? Numbers { get; set; }
    }
}