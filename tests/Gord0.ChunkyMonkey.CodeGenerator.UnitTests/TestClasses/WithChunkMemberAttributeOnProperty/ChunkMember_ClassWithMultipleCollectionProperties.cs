using Gord0.ChunkMonkey.Attributes;
using System.Collections.ObjectModel;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty
{
    public partial class ChunkMember_ClassWithMultipleCollectionProperties
    {
        [ChunkMember]
        public string? Name { get; set; }

        [ChunkMember]
        public int? Age { get; set; }

        [ChunkMember]
        public Dictionary<string, string>? Attributes { get; set; }

        [ChunkMember]
        public Collection<int>? FavouriteNumbers { get; set; }

        [ChunkMember]
        public List<int>? LotteryNumbers { get; set; }

        [ChunkMember]
        public string[]? FavouriteFilms { get; set; }
    }
}
