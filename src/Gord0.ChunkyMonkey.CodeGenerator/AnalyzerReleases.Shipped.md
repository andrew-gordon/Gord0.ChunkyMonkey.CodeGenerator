## Release 1.0.36

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
CMKY0001| Usage    | Error    | ChunkAttribute cannot be applied to an abstract class
CMKY0002| Usage    | Error    | ChunkAttribute cannot be applied to a static class
CMKY0003| Usage    | Error    | ChunkAttribute can only be applied to a class with a parameterless constructor
CMKY0004| Usage    | Warning  | ChunkAttribute should only be applied to a class with at least one chunkable collection property
CMKY0005| Usage    | Error    | Do not not specify ChunkAttribute on a class at the same time as ChunkMemberAtribute on its fields or properties. Use either ChunkAttribute or ChunkMemberAttribute.
CMKY0006| Usage    | Error    | ChunkMemberAtribute cannot be applied to an abstract member
CMKY0007| Usage    | Error    | ChunkMemberAtribute cannot be applied to a static member
CMKY0008| Usage    | Error    | ChunkMemberAtribute cannot be applied to a member with a type that ChunkyMonkey cannot chunk
