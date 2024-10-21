using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnClass
{
    public sealed partial class ChunkMember_ClassWithArraySegmentProperty
    {
        public string? Name { get; set; }
        public int? Age { get; set; }

        [ChunkMember]
        public ArraySegment<int> Numbers { get; set; }
    }
}