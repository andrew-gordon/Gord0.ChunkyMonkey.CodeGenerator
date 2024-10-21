using Gord0.ChunkMonkey.Attributes;
using System.Collections.ObjectModel;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public partial class Chunk_ClassWithMultipleCollectionProperties
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
        public Collection<int>? FavouriteNumbers { get; set; }
        public List<int>? LotteryNumbers { get; set; }
        public string[]? FavouriteFilms { get; set; }
    }
}
