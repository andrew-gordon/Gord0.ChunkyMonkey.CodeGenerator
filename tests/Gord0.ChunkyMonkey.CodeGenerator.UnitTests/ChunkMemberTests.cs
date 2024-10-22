using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Helpers;
using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass;
using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnClass;
using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkMemberAttributeOnProperty;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests
{
    public partial class ChunkMemberTests
    {
        [Fact]
        public void ChunkMember_ArrayProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new ChunkMember_ClassWithArrayProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };
            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            Assert.Equal([1, 2, 3], result[0].Numbers);

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            Assert.Equal([4, 5, 6], result[1].Numbers);

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            Assert.Equal([7, 8, 9], result[2].Numbers);

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            Assert.Equal([10], result[3].Numbers);

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void ChunkMember_ArraySegmentProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new ChunkMember_ClassWithArraySegmentProperty
            {
                Name = "John",
                Age = 25,
                Numbers = new ArraySegment<int>([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
            };
            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            Assert.Equal([1, 2, 3], result[0].Numbers);

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            Assert.Equal([4, 5, 6], result[1].Numbers);

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            Assert.Equal([7, 8, 9], result[2].Numbers);

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            Assert.Equal([10], result[3].Numbers);

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void ChunkMember_HashSetProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new ChunkMember_ClassWithHashSetProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };
            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            Assert.Equal([1, 2, 3], result[0].Numbers);

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            Assert.Equal([4, 5, 6], result[1].Numbers);

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            Assert.Equal([7, 8, 9], result[2].Numbers);

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            Assert.Equal([10], result[3].Numbers);

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void ChunkMember_SortedSetProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new ChunkMember_ClassWithSortedSetProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };
            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            Assert.Equal([1, 2, 3], result[0].Numbers);

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            Assert.Equal([4, 5, 6], result[1].Numbers);

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            Assert.Equal([7, 8, 9], result[2].Numbers);

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            Assert.Equal([10], result[3].Numbers);

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void ChunkMember_ListProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new ChunkMember_ClassWithListProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };
            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            Assert.Equal([1, 2, 3], result[0].Numbers);

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            Assert.Equal([4, 5, 6], result[1].Numbers);

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            Assert.Equal([7, 8, 9], result[2].Numbers);

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            Assert.Equal([10], result[3].Numbers);

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void ChunkMember_CollectionProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new ChunkMember_ClassWithCollectionProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };
            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();


            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            Assert.Equal([1, 2, 3], result[0].Numbers);

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            Assert.Equal([4, 5, 6], result[1].Numbers);

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            Assert.Equal([7, 8, 9], result[2].Numbers);

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            Assert.Equal([10], result[3].Numbers);

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void ChunkMember_MultipleCollectionProperties_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new ChunkMember_ClassWithMultipleCollectionProperties
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

            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();


            // Assert
            Assert.Equal(8, result.Count);
            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());


            //Chunk 1

            var chunk1 = result[0];
            Assert.Equal(instance.Name, chunk1.Name);
            Assert.Equal(instance.Age, chunk1.Age);
            Assert.Equal(3, chunk1.Attributes!.Count);

            {
                var expectedAttributes = new Dictionary<string, string>
                {
                    { "Occupation", "Developer" },
                    { "Location", "USA" },
                    { "Hobbies", "Programming" }
                };

                Assert.True(DictionaryComparer.Compare(chunk1.Attributes, expectedAttributes));
                {
                    var expectedFilms = new string[] { "Reservoir Dogs", "Pulp Fiction", "Inception" };
                    Assert.True(expectedFilms.SequenceEqual(chunk1.FavouriteFilms!));
                }

                // TODO: Test LotteryNumbers
                // TODO: Test FavouriteNumbers
                // TODO: Test FavouriteFilms
            }



            //Chunk 2

            var chunk2 = result[1];
            Assert.Equal(instance.Name, chunk2.Name);
            Assert.Equal(instance.Age, chunk2.Age);
            Assert.Equal(2, chunk2.Attributes!.Count);
            {
                var expectedAttributes = new Dictionary<string, string>
                {
                   { "Favourite Colour", "Red" },
                   { "Favourite Biscuit", "Custard Cream" }
                };

                Assert.True(DictionaryComparer.Compare(chunk2.Attributes, expectedAttributes));

                var expectedFilms = new string[] { "The Matrix", "The Shawshank Redemption", "The Godfather" };
                Assert.True(expectedFilms.SequenceEqual(chunk2.FavouriteFilms!));
                // TODO: Test LotteryNumbers
                // TODO: Test FavouriteNumbers
                // TODO: Test FavouriteFilms

            }




            //Chunk 3

            var chunk3 = result[2];
            Assert.Equal(instance.Name, chunk3.Name);
            Assert.Equal(instance.Age, chunk3.Age);
            Assert.Empty(chunk3.Attributes!);
            {
                var expectedFilms = new string[] { "The Dark Knight", "Fight Club" };
                Assert.True(expectedFilms.SequenceEqual(chunk3.FavouriteFilms!));
            }
            // TODO: Test LotteryNumbers
            // TODO: Test FavouriteNumbers
            // TODO: Test FavouriteFilms
        }

        [Fact]
        public void ChunkMember_ObservableCollectionProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithObservableCollectionProperty
            {
                Name = "John",
                Age = 25,
                Numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
            };
            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();


            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            Assert.Equal([1, 2, 3], result[0].Numbers);

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            Assert.Equal([4, 5, 6], result[1].Numbers);

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            Assert.Equal([7, 8, 9], result[2].Numbers);

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            Assert.Equal([10], result[3].Numbers);

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }
    }
}