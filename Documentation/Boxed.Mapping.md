![.NET Boxed Banner](https://github.com/Dotnet-Boxed/Templates/blob/main/Images/Banner.png)

## Boxed.Mapping

[![Boxed.Mapping](https://img.shields.io/nuget/v/Boxed.Mapping.svg)](https://www.nuget.org/packages/Boxed.Mapping/) [![Boxed.Mapping package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/5ed2eb60-9538-4890-b90e-5d4d4cbb2a7a/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=5ed2eb60-9538-4890-b90e-5d4d4cbb2a7a&preferRelease=true) [![Boxed.Mapping NuGet Package Downloads](https://img.shields.io/nuget/dt/Boxed.Mapping)](https://www.nuget.org/packages/Boxed.Mapping)

A simple and fast (fastest?) object to object mapper that does not use reflection. Read [A Simple and Fast Object Mapper](https://rehansaeed.com/a-simple-and-fast-object-mapper/) for more information.

```c#
public class MapFrom
{
    public bool BooleanFrom { get; set; }
    public int IntegerFrom { get; set; }
    public List<MapFromChild> ChildrenFrom { get; set; }
}
public class MapFromChild
{
    public DateTimeOffset DateTimeOffsetFrom { get; set; }
    public string StringFrom { get; set; }
}
 
public class MapTo
{
    public bool BooleanTo { get; set; }
    public int IntegerTo { get; set; }
    public List<MapToChild> ChildrenTo { get; set; }
}
public class MapToChild
{
    public DateTimeOffset DateTimeOffsetTo { get; set; }
    public string StringTo { get; set; }
}

public class DemoMapper : IMapper<MapFrom, MapTo>
{
    private readonly IMapper<MapFromChild, MapToChild> childMapper;
    
    public DemoMapper(IMapper<MapFromChild, MapToChild> childMapper) => this.childMapper = childMapper;
    
    public void Map(MapFrom source, MapTo destination)
    {
        destination.BooleanTo = source.BooleanFrom;
        destination.IntegerTo = source.IntegerFrom;
        destination.ChildrenTo = childMapper.MapList(source.ChildrenFrom);
    }
}

public class DemoChildMapper : IMapper<MapFromChild, MapToChild>
{
    public void Map(MapFromChild source, MapToChild destination)
    {
        destination.DateTimeOffsetTo = source.DateTimeOffsetFrom;
        destination.StringTo = source.StringFrom;
    }
}

public class UsageExample
{
    private readonly IMapper<MapFrom, MapTo> mapper = new DemoMapper();
    
    public MapTo MapOneObject(MapFrom source) => this.mapper.Map(source);
    
    public MapTo[] MapArray(List<MapFrom> source) => this.mapper.MapArray(source);
    
    public List<MapTo> MapList(List<MapFrom> source) => this.mapper.MapList(source);
    
    public IAsyncEnumerable<MapTo> MapAsyncEnumerable(IAsyncEnumerable<MapFrom> source) =>
        this.mapper.MapEnumerableAsync(source);
}
```

Also includes `IImmutableMapper<TSource, TDestination>` which is for mapping to immutable types like C# 9 `record`'s and can also be used for `enum` types.

```c#
public record MapFrom(bool BooleanFrom, int IntegerFrom);
public record MapTo(bool BooleanTo, int IntegerTo);

public class DemoImmutableMapper : IImmutableMapper<MapFrom, MapTo>
{
    public MapTo Map(MapFrom source) => 
        new MapTo(source.BooleanFrom, source.IntegerFrom);
}
```
