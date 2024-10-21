using Gord0.ChunkMonkey.Attributes;

namespace TestProject
{
    public partial class Person
    {
        public string? Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [ChunkMember]
        public string[]? PhoneNumbers { get; set; }

        [ChunkMember]
        public List<string>? RecentAddresses { get; set; }
    }
}