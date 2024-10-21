# ChunkyMonkey - C# Object Chunking Source Generator

## Introduction

![ChunkyMonkey](https://raw.githubusercontent.com/andrew-gordon/Gordo0.ChunkyMonkey.CodeGenerator/main/media/ChunkMonkey.png)
ChunkyMonkey is a C# code generator that generates code, at build time, to split an object containing collection properties into chunks. It also provides the ability to merge the chunks back into a single object instance.

## Supported collection types

- `System.Array`
- `System.Collections.Generic.Dictionary<K,V>` 
- `System.Collections.Generic.List<T>`
- `System.Collections.Generic.HashSet<T>`
- `System.Collections.Generic.SortedSet<T>` 
- `System.Collections.Immutable.ImmutableArray<T>`
- `System.Collections.Immutable.ImmutableList<T>`
- `System.Collections.ObjectModel.Collection<T>`
- `System.Collections.ObjectModel.ReadOnlyCollection<T>`
- `System.Collections.Specialized.StringCollection`

## Use Case

* You need to break down a large API request into smaller pieces (chunks) - to avoid breaking the maximum request body size limit of your web server.

<br>

## Walkthrough

Imagine you have a class like this:

```csharp
public class CreateUserRequest
{
	public string Username { get; set; }
	public DateTime DateOfBirth { get; set; }	
	public List<int> FavouriteNumbers { get; set; }
	public string[] FavouriteFilms { get; set; }
	public Dictionary<string, string> Attributes { get; set; }
}
```

If a CreateUserRequest object is for a user who has lot of favourite numbers, films and attributes, you may want to split the object into chunks to reduce the size of the request object. _[Yes, this is a very contrived example!]_

**ChunkyMonkey** generates two methods within partial classes, where _TInstance_ is the name of the class. 

> :memo: The method isn't generic; the generated code contains the actual class type rather than _TInstance_.

``` csharp 
public IEnumerable<TInstance> Chunk(int chunkSize)
```

``` csharp
public TInstance MergeChunks(IEnumerable<TInstance> chunks)
```

## ```IEnumerable<TInstance> Chunk(int chunkSize)```

* The chunk size is the maximum number of values held in chunked collection properties.
* Only partial and non-static classes can be chunked. This is because the code generates generates a partial class containing the methods above for each chunked class.
* Non-chunkable properties are repeated for each chunked instance.
* If a collection (list, collection, array or dictionary) property's values are exhausted, subsequent chunks will contain an empty collection for the property value.
* Only top-level collection properties are chunked. Nested properties are not chunked.


### Example of usage

- Let's examine the use of the `Chunk` method on the `CreateUserRequest` class with a chunk size of 3.

#### Input to `Chunks(3)`

| Property | Value |
|--------|----------------|
| Username | `'bob'` | 
| DateOfBirth | `1/2/1975` |
| FavouriteNumbers | `[1, 2, 3, 4, 5, 6, 7]` |
| FavouriteFilms | `['E.T.', 'Flight of the Navigator', 'Weird Science', 'Ferris Bueller’s Day Off', 'Ghostbusters']` |
| Attributes | ``` { ```<br> ```  'Eye Colour': 'Blue',```<br> ```  'Hair Colour': 'Brown',```<br> ```  'Height (cm)': '180',```<br> ```   'First Language': 'English' ```<br> ``` } ``` |

#### Output for `Chunks(3)`

_Chunk #1_

| Property | Value |
|--------|----------------|
| Username | `'bob'` | 
| DateOfBirth | `1/2/1975` |
| FavouriteNumbers | `[1, 2, 3]` |
| FavouriteFilms | `['E.T.', 'Flight of the Navigator', 'Weird Science']` |
| Attributes | ``` { ```<br> ```  'Eye Colour': 'Blue',```<br> ```  'Hair Colour': 'Brown',```<br> ```  'Height (cm)': '180',```<br> ``` } ``` |

_Chunk #2_

| Property | Value |
|--------|----------------|
| Username | `'bob'` | 
| DateOfBirth | `1/2/1975` |
| FavouriteNumbers | `[4, 5, 6]` |
| FavouriteFilms | `['Ferris Bueller’s Day Off', 'Ghostbusters']` |
| Attributes | ``` { ```<br> ```  'First Language': 'English' ```<br> ```} ``` |

_Chunk #3_

| Property | Value |
|--------|----------------|
| Username | `'bob'` | 
| DateOfBirth | `1/2/1975` |
| FavouriteNumbers | `[7]` |
| FavouriteFilms | `[]` || 
| Attributes | ``` {} ``` |

<br>

## ``TInstance MergeChunks(IEnumerable<TInstance> chunks)``

This generated method merges a set of chunks back into a single instance. 

<br>

## Using within a .NET project

1. Add the ChunkyMonkey NuGet package to your C# project containing classes for which you'd like to generate the `Chunk` and `MergeChunks` methods.

    | Environment | Command |
    |-------------|---------|
    | .NET CLI | `dotnet add package Gord0.ChunkyMonkey.CodeGenerator` |
    | VS Package Manager Console | `NuGet\Install-Package Gord0.ChunkyMonkey.CodeGenerator` |
    | VS Package Manager Console | `<PackageReference Include="Gord0.ChunkyMonkey.CodeGenerator" Version="x.y.z" />` |

2. There are two ways of indicating to ChunkyMonkey that you would like the `Chunk` and `MergeChunks` to be generated for a class:

    | Method | Description |
    |--------|-------------|
    | Class-level | Add the `[Chunk]` attribute to the class for which you'd like ChunkyMonkey to generate the `Chunk` and `MergeChunks` methods. Code will be generated to chunk all public properties for supported collection types. |
    | Property-level | Add the `[ChunkMember]` attribute to one or more properties of a class for which you'd like ChunkyMonkey to generate the `Chunk` and `MergeChunks` methods. Code will be generated to chunk the properties for supported collection types if they have been decorated with `ChunkMember`.|
 

    > :bulb: **Tip:** Be sure to define the class as a `partial` class - otherwise, you will receive a compiler error when you build your project.

    ```csharp
    using Gord0.ChunkMonkey.Attributes;

    namespace TestProject
    {
        [Chunk]
        public partial class Person
        {
            public string? Name { get; set; }

            public DateTime? DateOfBirth { get; set; }

            public string[]? PhoneNumbers { get; set; }

            public List<string>? RecentAddresses { get; set; }
        }
    }
    ```

    **OR**

    ```csharp
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
    ```

    > :memo: **Note:** If your classes/DTOs live in a separate project, you should add the `Gord0.ChunkyMonkey.Attributes` package to that project. This package provides the `ChunkAttribute` and `ChunkMemberAttribute` types.

3. Build your project.

4. To view the generated partial classes:
	- Expand the Dependencies nodes under your project in the Solution Explorer. 
	- Expand the Analyzers node.
	- Expand the Gord0.ChunkyMonkey.CodeGenerator node.
	- Expand the Gord0.ChunkyMonkey.CodeGenerator.ChunkyMonkeyGenerator node.
	- Now you will see the generated partial classes, each containing the `Chunk` and `MergeChunks` methods. The generated classes are called `<ClassName>_ChunkeyMonkey.g.cs`
	
5. The output for the above Person class would be a file called `Person_ChunkeyMonkey.g.cs`:

    ```csharp
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Immutable;
    using System.Linq;

    namespace TestProject
    {
        public partial class Person
        {
            /// <summary>
            /// Chunks the instance into multiple instances based on the specified chunk size.
            /// </summary>
            /// <param name="chunkSize">The size of each chunk.</param>
            /// <returns>An enumerable of chunked instances.</returns>
            public IEnumerable<Person> Chunk(int chunkSize)
            {
                // Find the length of the biggest collection.
                long biggestCollectionLength = new long[] {
                    (this.PhoneNumbers is not null) ? this.PhoneNumbers.Length : 0, 
                    (this.RecentAddresses is not null) ? this.RecentAddresses.Count : 0
                }.Max();

                for (int i = 0; i < biggestCollectionLength; i += chunkSize)
                {
                    var instance = new Person();
                    instance.Name = this.Name;
                    instance.DateOfBirth = this.DateOfBirth;
                    {
                        if (this.PhoneNumbers is not null)
                        {
                            instance.PhoneNumbers = this.PhoneNumbers.Skip(i).Take(chunkSize).ToArray();
                        }
                    }

                    if (this.RecentAddresses is not null)
                    {
                        instance.RecentAddresses = this.RecentAddresses.Skip(i).Take(chunkSize).ToList();
                    }

                    yield return instance;
                }
            }

            /// <summary>
            /// Merges the specified chunks into a single instance.
            /// </summary>
            /// <param name="chunks">The chunks to merge.</param>
            /// <returns>The merged instance.</returns>
            public static Person MergeChunks(IEnumerable<Person> chunks)
            {
                var instance = new Person();

                long chunkNumber = 0;
                string? lastValue_Name = default;
                global::System.DateTime? lastValue_DateOfBirth = default;
                List<string>? tempArrayList_PhoneNumbers = null;

                foreach(var chunk in chunks)
                {
                    if (chunkNumber > 0)
                    {
                        if (lastValue_Name != chunk.Name)
                        {
                            throw new InvalidDataException("Chunks contain different values for non-chunked property 'Name'");
                        }
                        if (lastValue_DateOfBirth != chunk.DateOfBirth)
                        {
                            throw new InvalidDataException("Chunks contain different values for non-chunked property 'DateOfBirth'");
                        }
                    }

                    instance.Name = chunk.Name;
                    lastValue_Name = chunk.Name;

                    instance.DateOfBirth = chunk.DateOfBirth;
                    lastValue_DateOfBirth = chunk.DateOfBirth;

                    if (chunk.PhoneNumbers is not null)
                    {
                        if (tempArrayList_PhoneNumbers is null)
                        {
                            tempArrayList_PhoneNumbers = [];
                        }

                        tempArrayList_PhoneNumbers.AddRange(chunk.PhoneNumbers);
                    }

                    if (chunk.RecentAddresses is not null)
                    {
                        if (instance.RecentAddresses is null)
                        {
                            instance.RecentAddresses = new List<string>();
                        }

                        if (chunk.RecentAddresses is not null)
                        {
                            foreach(var value in chunk.RecentAddresses)
                            {
                                instance.RecentAddresses.Add(value);
                            }
                        }
                    }

                    chunkNumber++;
                }

                if (tempArrayList_PhoneNumbers is not null)
                {
                    instance.PhoneNumbers = tempArrayList_PhoneNumbers.ToArray();
                }
                else
                {
                    instance.PhoneNumbers = null;
                }

                return instance;
            }
       }
    }
    ```

<br>

## Future Enhancements

- Support `System.Collections.Generic.SortedDictionary<TKey, TValue>`
- Support `System.Collections.Generic.Stack<T>`
- Support `System.Collections.Generic.Queue<T>`
- Support `System.Collections.Generic.PriorityQueue<T>`
- Support `System.Collections.Specialized.NameValueCollection`
- Support `System.Collections.Specialized.BitArray`
- Support `System.Collections.Immutable.ImmutableList`
- Support `System.Collections.Immutable.ImmutableHashSet`
- Support `System.Collections.ObjectModel.ObservableCollection<T>`
- Support `System.Collections.ObjectModel.ReadOnlyObservableCollection<TKey>`
- Support `Memory<T>`
- Support `ReadOnlyMemory<T>`
- Analyzer warning when `[Chunk]` is used on a class containing a property without a getter and setter.
- Analyzer warning when `[ChunkMember]` is applied to a property without a getter and setter.

## Versions

| Version | Description |
|---------|-------------|
| 1.0.29  | First decent release. Earlier versions unlisted. |
| 1.0.30  | Added a code analyser to check if `ChunkAttribute` is misplaced on an abstract class, a static class, or a class with parameterless constructor. |
| 1.0.31  | Documentation updates |
| 1.0.32  | Allow `ChunkAttribute` to be used on `sealed` classes |
| 1.0.33  | Minor fixes and documentation updates |
| 1.0.38  | Minor fixes and documentation updates |
| 1.0.45  | CodeGenerator rewritten to use INamedTypeSymbol and IpropertyRecord.Symbol, rather than ClassDeclaration. Support for nullable types. Improved generic type handling. Updated documentation. |
| 2.0.3   | Refactored |
| 2.0.7   | Check that non-chunked values do not change beween chunks to be merged. Improved performance when merging chunks for array collections. |
| 2.0.8   | Added ChunkMemberAttributeAnalyzer. Fixes for ChunkAttributeAnalyzer. |
| 2.0.9   | Fixes for ChunkMemberAttributeAnalyzer. |
| 2.0.10  | Updated description for NonSupportedChunkingTypeWithChunkMemberRule |
| 2.0.11  | Minor fixes | ChunkMemberAttributeAnalyzer |
| 2.0.12  | Fixed code analyser rules: NoAccessibleChunkablePropertiesRule, NoChunkablePropertiesRule & NonSupportedChunkingTypeWithChunkMemberRule.<br>Parameterised NonSupportedChunkingTypeWithChunkMemberRule<br>PropertyRecord now has a GenericTypeArguments property<br>PropertyRecord.IsChunkable-> PropertyRecord.HasSupportedTypeForChunking<br>Support for `ImmutableArray<T>` |
| 2.0.13  | Added support for `System.Collections.ObjectModel.ReadOnlyCollection<T>` |
| 2.0.14  | Added support for `System.ArraySegment<T>` |
| 2.0.15  | Added support for `System.Collections.Immutable.ImmutableList` & `System.Collections.Specialized.StringCollection` |
| 2.0.16  | Added support for `System.Collections.Generic.SortedList<TKey, TValue>` |