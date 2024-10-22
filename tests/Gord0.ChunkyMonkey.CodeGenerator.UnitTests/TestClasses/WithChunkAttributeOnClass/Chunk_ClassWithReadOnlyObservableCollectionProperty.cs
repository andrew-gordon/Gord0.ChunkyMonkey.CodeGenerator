using Gord0.ChunkMonkey.Attributes;
using System.Collections.ObjectModel;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public partial class Chunk_ClassWithReadOnlyObservableCollectionProperty
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public ReadOnlyObservableCollection<int>? Numbers { get; set; }
    }
}