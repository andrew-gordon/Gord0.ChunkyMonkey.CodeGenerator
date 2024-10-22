using Gord0.ChunkMonkey.Attributes;
using System.Collections.Specialized;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public sealed partial class Chunk_ClassWithNameValueCollectionProperty
    {
        public NameValueCollection? Values{ get; set; }
    }
}