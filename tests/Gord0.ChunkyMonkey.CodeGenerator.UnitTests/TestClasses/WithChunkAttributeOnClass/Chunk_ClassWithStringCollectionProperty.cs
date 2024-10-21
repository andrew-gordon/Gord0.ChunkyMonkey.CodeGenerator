using Gord0.ChunkMonkey.Attributes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public partial class Chunk_ClassWithStringCollectionProperty
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public StringCollection? FavouriteFilms { get; set; }
    }
}