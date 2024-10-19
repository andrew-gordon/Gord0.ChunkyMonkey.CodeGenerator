using Microsoft.CodeAnalysis;

namespace Gord0.ChunkyMonkey.CodeGenerator.CodeGenerator
{
    public record TypeRecord(
        string Name,
        Func<IPropertySymbol, bool> TypeMatcher,
        string LengthPropertyName,
        Func<IPropertySymbol, string> ChunkCodeFactory,
        Func<IPropertySymbol, string> MergeChunksCodeFactory)
    {
        public string Name { get; } = Name;
        public Func<IPropertySymbol, bool> TypeMatcher { get; } = TypeMatcher;
        public string LengthPropertyName { get; } = LengthPropertyName;
        public Func<IPropertySymbol, string> ChunkCodeFactory { get; } = ChunkCodeFactory;
        public Func<IPropertySymbol, string> MergeChunksCodeFactory { get; } = MergeChunksCodeFactory;
    }
}
