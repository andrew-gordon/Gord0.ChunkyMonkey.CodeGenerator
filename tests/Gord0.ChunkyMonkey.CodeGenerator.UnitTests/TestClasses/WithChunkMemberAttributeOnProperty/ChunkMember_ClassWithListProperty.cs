using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty
{
    public partial class ChunkMember_ClassWithListProperty
    {
        [ChunkMember]
        public string? Name { get; set; }

        [ChunkMember]
        public int? Age { get; set; }

        [ChunkMember]
        public List<int>? Numbers { get; set; }
    }
}
