﻿using Gord0.ChunkMonkey.Attributes;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass
{
    [Chunk]
    public sealed partial class Chunk_ClassWithNullableArrayProperty
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public int[]? Numbers { get; set; }
    }
}
