using Gord0.ChunkMonkey.Attributes;
using System.Collections.ObjectModel;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty
{
    public partial class ChunkMember_ClassWithCollectionProperty
    {
        public string? Name { get; set; }

        public int? Age { get; set; }

        [ChunkMember]
        public Collection<int>? Numbers { get; set; }
    }
}