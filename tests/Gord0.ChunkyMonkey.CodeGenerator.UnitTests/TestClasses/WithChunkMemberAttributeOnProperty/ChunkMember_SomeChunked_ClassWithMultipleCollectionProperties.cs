using Gord0.ChunkMonkey.Attributes;
using System.Collections.ObjectModel;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty
{
    public partial class ChunkMember_SomeChunked_ClassWithMultipleCollectionProperties
    {
        public string? Name { get; set; }

        public int? Age { get; set; }

        [ChunkMember]
        public Dictionary<string, string>? Attributes { get; set; }

        public Collection<int>? FavouriteNumbers { get; set; }

        public List<int>? LotteryNumbers { get; set; }

        public string[]? FavouriteFilms { get; set; }
    }
}
