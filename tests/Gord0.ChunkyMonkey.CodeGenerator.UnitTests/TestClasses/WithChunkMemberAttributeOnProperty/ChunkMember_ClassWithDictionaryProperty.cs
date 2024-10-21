using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty
{
     public partial class ChunkMember_ClassWithDictionaryProperty
    {
        public string? Name { get; set; }

        public int? Age { get; set; }

        [ChunkMember]
        public Dictionary<string, string>? Attributes { get; set; }
    }
}
