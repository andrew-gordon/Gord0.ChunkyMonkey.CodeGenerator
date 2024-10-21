using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty
{
    public sealed partial class ChunkMember_ClassWithNullableArrayProperty
    {
        [ChunkMember]
        public string? Name { get; set; }

        [ChunkMember]
        public int? Age { get; set; }

        [ChunkMember]
        public int[]? Numbers { get; set; }
    }
}
