﻿using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Helpers;
using Gord0.ChunkyMonkey.CodeGenerator.UnitTests.TestClasses.WithChunkAttributeOnClass;
using System.Collections.Specialized;
using Gord0.ChunkyMonkey.CodeGenerator.Extensions;

namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests
{
    public partial class ChunkTests
    {
        [Fact]
        public void Chunk_ArrayProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithArrayProperty
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
        public void Chunk_ArraySegmentProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithArraySegmentProperty
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
            int[] expectedNumbers = [1, 2, 3];
            Assert.True(expectedNumbers.SequenceEqual(result[0].Numbers));

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            expectedNumbers = [4, 5, 6];
            Assert.True(expectedNumbers.SequenceEqual(result[1].Numbers));

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            expectedNumbers = [7, 8, 9];
            Assert.True(expectedNumbers.SequenceEqual(result[2].Numbers));

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            expectedNumbers = [10];
            Assert.True(expectedNumbers.SequenceEqual(result[3].Numbers));

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void Chunk_ImmutableListProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithImmutableListProperty
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
            int[] expectedNumbers = [1, 2, 3];
            Assert.True(expectedNumbers.SequenceEqual([.. result[0].Numbers!]));

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            expectedNumbers = [4, 5, 6];
            Assert.True(expectedNumbers.SequenceEqual([.. result[1].Numbers!]));

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            expectedNumbers = [7, 8, 9];
            Assert.True(expectedNumbers.SequenceEqual([.. result[2].Numbers!]));

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            expectedNumbers = [10];
            Assert.True(expectedNumbers.SequenceEqual([.. result[3].Numbers!]));

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void Chunk_ImmutableHashSetProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithImmutableHashSetProperty
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
            int[] expectedNumbers = [1, 2, 3];
            Assert.True(expectedNumbers.SequenceEqual([.. result[0].Numbers!]));

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            expectedNumbers = [4, 5, 6];
            Assert.True(expectedNumbers.SequenceEqual([.. result[1].Numbers!]));

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            expectedNumbers = [7, 8, 9];
            Assert.True(expectedNumbers.SequenceEqual([.. result[2].Numbers!]));

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            expectedNumbers = [10];
            Assert.True(expectedNumbers.SequenceEqual([.. result[3].Numbers!]));

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void Chunk_ImmutableArrayProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithImmutableArrayProperty
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
            int[] expectedNumbers = [1, 2, 3];
            Assert.True(expectedNumbers.SequenceEqual(result[0].Numbers));

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            expectedNumbers = [4, 5, 6];
            Assert.True(expectedNumbers.SequenceEqual(result[1].Numbers));

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            expectedNumbers = [7, 8, 9];
            Assert.True(expectedNumbers.SequenceEqual(result[2].Numbers));

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            expectedNumbers = [10];
            Assert.True(expectedNumbers.SequenceEqual(result[3].Numbers));

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void Chunk_ReadOnlyCollectionProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithReadOnlyCollectionProperty
            {
                Name = "John",
                Age = 25,
                Numbers = new System.Collections.ObjectModel.ReadOnlyCollection<int>([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
            };
            int chunkSize = 3;


            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            int[] expectedNumbers = [1, 2, 3];
            Assert.True(expectedNumbers.SequenceEqual(result[0].Numbers));

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            expectedNumbers = [4, 5, 6];
            Assert.True(expectedNumbers.SequenceEqual(result[1].Numbers));

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            expectedNumbers = [7, 8, 9];
            Assert.True(expectedNumbers.SequenceEqual(result[2].Numbers));

            Assert.Equal(instance.Name, result[3].Name);
            Assert.Equal(instance.Age, result[3].Age);
            expectedNumbers = [10];
            Assert.True(expectedNumbers.SequenceEqual(result[3].Numbers));

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }

        [Fact]
        public void Chunk_StringCollectionProperty_ReturnsChunkedInstances()
        {
            string[] films = ["Reservoir Dogs", "Pulp Fiction", "Inception", "The Matrix", "The Shawshank Redemption", "The Godfather", "The Dark Knight", "Fight Club"];
            var sc = new StringCollection();
            sc.AddRange(films);

            // Arrange
            var instance = new Chunk_ClassWithStringCollectionProperty
            {
                Name = "John",
                Age = 25,
                FavouriteFilms = sc,
            };

            int chunkSize = 3;


            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(3, result.Count);

            Assert.Equal(instance.Name, result[0].Name);
            Assert.Equal(instance.Age, result[0].Age);
            string[] expectedFilms1 = ["Reservoir Dogs", "Pulp Fiction", "Inception"];
            Assert.True(expectedFilms1.SequenceEqual(result[0].FavouriteFilms!.ToEnumerable()));

            Assert.Equal(instance.Name, result[1].Name);
            Assert.Equal(instance.Age, result[1].Age);
            string[] expectedFilms2 = ["The Matrix", "The Shawshank Redemption", "The Godfather"];
            Assert.True(expectedFilms2.SequenceEqual(result[1].FavouriteFilms!.ToEnumerable()));

            Assert.Equal(instance.Name, result[2].Name);
            Assert.Equal(instance.Age, result[2].Age);
            string[] expectedFilms3 = ["The Dark Knight", "Fight Club"];
            Assert.True(expectedFilms3.SequenceEqual(result[2].FavouriteFilms!.ToEnumerable()));

            Assert.Single(result.Select(x => x.Name).Distinct());
            Assert.Single(result.Select(x => x.Age).Distinct());
        }


        [Fact]
        public void Chunk_HashSetProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithHashSetProperty
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
        public void Chunk_SortedSetProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithSortedSetProperty
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
        public void Chunk_ListProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithListProperty
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
        public void Chunk_CollectionProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithCollectionProperty
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
        public void Chunk_MultipleCollectionProperties_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithMultipleCollectionProperties
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
        public void ChunkSortedListProperty_ReturnsChunkedInstances()
        {
            var sortedList = new SortedList<string, int>
            {
                { "One", 1 },
                { "Two", 2 },
                { "Three", 3 },
                { "Four", 4 },
                { "Five", 5 },
                { "Six", 6 },
                { "Seven", 7 },
                { "Eight", 8 },
                { "Nine", 9 },
                { "Ten", 10 }
            };

            // Arrange
            var instance = new Chunk_ClassWithSortedListProperty
            {
                Numbers = sortedList
            };

            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            // NB: The keys are sorted so will appear in alphabetical order

            var expected1 = new SortedList<string, int>
            {
                { "Eight", 8 },
                { "Five", 5 },
                { "Four", 4 },
            };

            var expected2 = new SortedList<string, int>
            {
                { "Nine", 9 },
                { "One", 1 },
                { "Seven", 7 },
            };

            var expected3 = new SortedList<string, int>
            {
                { "Six", 6 },
                { "Ten", 10 },
                { "Three", 3 }
            };

            var expected4 = new SortedList<string, int>
            {
                { "Two", 2 }
            };

            Assert.Equal(expected1, result[0].Numbers);
            Assert.Equal(expected2, result[1].Numbers);
            Assert.Equal(expected3, result[2].Numbers);
            Assert.Equal(expected4, result[3].Numbers);
        }

        [Fact]
        public void ChunkSortedDictionaryProperty_ReturnsChunkedInstances()
        {
            var sortedDictionary = new SortedDictionary<string, int>
            {
                { "One", 1 },
                { "Two", 2 },
                { "Three", 3 },
                { "Four", 4 },
                { "Five", 5 },
                { "Six", 6 },
                { "Seven", 7 },
                { "Eight", 8 },
                { "Nine", 9 },
                { "Ten", 10 }
            };

            // Arrange
            var instance = new Chunk_ClassWithSortedDictionaryProperty
            {
                Numbers = sortedDictionary
            };

            int chunkSize = 3;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(4, result.Count);

            // NB: The keys are sorted so will appear in alphabetical order

            var expected1 = new SortedDictionary<string, int>
            {
                { "Eight", 8 },
                { "Five", 5 },
                { "Four", 4 },
            };

            var expected2 = new SortedDictionary<string, int>
            {
                { "Nine", 9 },
                { "One", 1 },
                { "Seven", 7 },
            };

            var expected3 = new SortedDictionary<string, int>
            {
                { "Six", 6 },
                { "Ten", 10 },
                { "Three", 3 }
            };

            var expected4 = new SortedDictionary<string, int>
            {
                { "Two", 2 }
            };

            Assert.Equal(expected1, result[0].Numbers);
            Assert.Equal(expected2, result[1].Numbers);
            Assert.Equal(expected3, result[2].Numbers);
            Assert.Equal(expected4, result[3].Numbers);
        }

        [Fact]
        public void ChunkNameValueCollectionProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithNameValueCollectionProperty
            {
                Values = new NameValueCollection
                {
                    { "One", "Un"},
                    { "Two", "Deux"},
                    { "Three", "Trois"},
                    { "Four", "Quatre"},
                    { "Five", "Cinq"},
                    { "Six", "Six"},
                    { "Seven", "Sept"},
                    { "Eight", "Huit"},
                    { "Nine", "Neuf"},
                    { "Ten", "Dix" }
                }
            };

            int chunkSize = 6;

            // Act
            var result = instance.Chunk(chunkSize).ToList();

            // Assert
            Assert.Equal(2, result.Count);

            // NB: The keys are sorted so will appear in alphabetical order

            var expected1 = new NameValueCollection
            {
                { "One", "Un"},
                { "Two", "Deux"},
                { "Three", "Trois"},
                { "Four", "Quatre"},
                { "Five", "Cinq"},
                { "Six", "Six"},
            };

            var expected2 = new NameValueCollection
            {
                { "Seven", "Sept"},
                { "Eight", "Huit"},
                { "Nine", "Neuf"},
                { "Ten", "Dix" }
            };

            Assert.Equal(expected1, result[0].Values);
            Assert.Equal(expected2, result[1].Values);
        }

        [Fact]
        public void Chunk_ObservableCollectionProperty_ReturnsChunkedInstances()
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

        [Fact]
        public void Chunk_ReadOnlyObservableCollectionProperty_ReturnsChunkedInstances()
        {
            // Arrange
            var instance = new Chunk_ClassWithReadOnlyObservableCollectionProperty
            {
                Name = "John",
                Age = 25,
                Numbers = new System.Collections.ObjectModel.ReadOnlyObservableCollection<int>([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
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