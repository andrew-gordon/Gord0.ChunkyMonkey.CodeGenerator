using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Helpers;
using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests
{
    public partial class MergeChunksTests
    {
        [Fact]
        public void MergeChunks_ArrayProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var chunks = new List<Chunk_ClassWithArrayProperty>
            {
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [1, 2, 3]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [4, 5, 6]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [7, 8, 9]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [10]
                },

                // This chunk wouldn't be emitted by the generator, but checking it for completeness.
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = []
                }
            };

            // Act
            var actual = Chunk_ClassWithArrayProperty.MergeChunks(chunks);

            var expected = new Chunk_ClassWithArrayProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Age, actual.Age);
            Assert.True(expected.Numbers.SequenceEqual(actual.Numbers));
        }

        [Fact]
        public void MergeChunks_NullableArrayProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var chunks = new List<Chunk_ClassWithNullableArrayProperty>
            {
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [1, 2, 3]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [4, 5, 6]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [7, 8, 9]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [10]
                },

                // This chunk wouldn't be emitted by the generator, but checking it for completeness.
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = []
                }
            };

            // Act
            var actual = Chunk_ClassWithNullableArrayProperty.MergeChunks(chunks);

            var expected = new Chunk_ClassWithArrayProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Age, actual.Age);
            Assert.NotNull(actual.Numbers);
            Assert.True(expected.Numbers.SequenceEqual(actual.Numbers));
        }

        [Fact]
        public void MergeChunks_ImmutableArrayProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var chunks = new List<Chunk_ClassWithImmutableArrayProperty>
            {
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [1, 2, 3]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [4, 5, 6]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [7, 8, 9]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [10]
                },
            };

            // Act
            var actual = Chunk_ClassWithImmutableArrayProperty.MergeChunks(chunks);

            var expected = new Chunk_ClassWithArrayProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Age, actual.Age);
            Assert.True(expected.Numbers.SequenceEqual(actual.Numbers));
        }

        [Fact]
        public void MergeChunks_ImmutableHashSetProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var chunks = new List<Chunk_ClassWithImmutableHashSetProperty>
            {
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [1, 2, 3]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [4, 5, 6]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [7, 8, 9]
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = [10]
                },
            };

            // Act
            var actual = Chunk_ClassWithImmutableHashSetProperty.MergeChunks(chunks);

            var expected = new Chunk_ClassWithImmutableHashSetProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Age, actual.Age);
            Assert.True(expected.Numbers.SequenceEqual(actual.Numbers!));
        }

        [Fact]
        public void MergeChunks_ReadOnlyCollectionProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var chunks = new List<Chunk_ClassWithReadOnlyCollectionProperty>
            {
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = new System.Collections.ObjectModel.ReadOnlyCollection<int>([1, 2, 3])
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = new System.Collections.ObjectModel.ReadOnlyCollection<int>([4, 5, 6])
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = new System.Collections.ObjectModel.ReadOnlyCollection<int>([7, 8, 9])
                },
                new() {
                    Name = "John",
                    Age = 25,
                    Numbers = new System.Collections.ObjectModel.ReadOnlyCollection<int>([10])
                },
            };

            // Act
            var actual = Chunk_ClassWithReadOnlyCollectionProperty.MergeChunks(chunks);

            var expected = new Chunk_ClassWithReadOnlyCollectionProperty
            {
                Name = "John",
                Age = 25,
                Numbers = new System.Collections.ObjectModel.ReadOnlyCollection<int>([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
            };

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Age, actual.Age);
            Assert.True(expected.Numbers.SequenceEqual(actual.Numbers));
        }

        [Fact]
        public void MergeChunks_MulitpleCollectionProperties_ReturnsChunkedInstances()
        {
            // Arrange
            var chunks = new List<Chunk_ClassWithMultipleCollectionProperties>
            {
                new() {
                    Name = "John",
                    Age = 25,
                    FavouriteNumbers = [1, 2, 3],
                    FavouriteFilms = [ "Reservoir Dogs", "Pulp Fiction" ],
                    LotteryNumbers = [1,2,3,4,5,6],
                    Attributes = new Dictionary<string, string>
                    {
                        { "Occupation", "Developer" },
                        { "Location", "USA" },
                        { "Hobbies", "Programming" }
                    }
                },
                new() {
                    Name = "John",
                    Age = 25,
                    FavouriteNumbers = [4, 5, 6],
                    FavouriteFilms = [ "Inception", "The Matrix" ],
                    LotteryNumbers = [11,12,13,14,15,16],
                    Attributes = new Dictionary<string, string>
                    {
                        { "Favourite Colour", "Red" },
                    }
                },
                new() {
                    Name = "John",
                    Age = 25,
                    FavouriteNumbers = [7, 8, 9],
                    FavouriteFilms = [ "The Shawshank Redemption", "The Godfather" ],
                    LotteryNumbers = [21,22,23,24,25,26],
                    Attributes = new Dictionary<string, string>
                    {
                        { "Favourite Biscuit", "Custard Cream" }
                    }
                },
                new() {
                    Name = "John",
                    Age = 25,
                    FavouriteNumbers = [10],
                    FavouriteFilms = [ "The Dark Knight", "Fight Club"],
                    LotteryNumbers = [31,32,33,34,35,36],
                    Attributes = []
                },

                // This chunk wouldn't be emitted by the generator, but checking it for completeness.
                new() {
                    Name = "John",
                    Age = 25,
                    FavouriteNumbers = [],
                    FavouriteFilms = [],
                    LotteryNumbers = [],
                    Attributes = []
                }
            };

            // Act
            var actual = Chunk_ClassWithMultipleCollectionProperties.MergeChunks(chunks);

            var expected = new Chunk_ClassWithMultipleCollectionProperties
            {
                Name = "John",
                Age = 25,
                FavouriteNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                FavouriteFilms = ["Reservoir Dogs", "Pulp Fiction", "Inception", "The Matrix", "The Shawshank Redemption", "The Godfather", "The Dark Knight", "Fight Club"],
                LotteryNumbers = [1, 2, 3, 4, 5, 6, 11, 12, 13, 14, 15, 16, 21, 22, 23, 24, 25, 26, 31, 32, 33, 34, 35, 36],
                Attributes = new Dictionary<string, string>
                {
                    { "Occupation", "Developer" },
                    { "Location", "USA" },
                    { "Hobbies", "Programming" },
                    { "Favourite Colour", "Red" },
                    { "Favourite Biscuit", "Custard Cream" }
                }
            };

            // Assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Age, actual.Age);
            Assert.True(expected.FavouriteNumbers.SequenceEqual(actual.FavouriteNumbers!));
            Assert.True(expected.FavouriteFilms.SequenceEqual(actual.FavouriteFilms!));
            Assert.True(expected.LotteryNumbers.SequenceEqual(actual.LotteryNumbers!));
            Assert.True(DictionaryComparer.Compare(expected.Attributes, actual.Attributes!));
        }
    }
}
