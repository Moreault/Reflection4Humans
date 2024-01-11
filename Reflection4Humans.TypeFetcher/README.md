![Reflection4Humans](https://github.com/Moreault/Reflection4Humans/blob/master/reflection4humans.png)

# Reflection4Humans.TypeFetcher
Provides an easy way for humans to fetch types from any and all loaded assemblies.

## Getting started

You start by using the `Types` static class and querying what types interest you in largely the same way as you would perform regular LINQ queries.

```cs
//Returns all types from all assemblies, you animal
var allTypesInAllAssemblies = Types.ToList();

var myHighLevelTypes = Types.Where(x => x.HasAttribute<MineAttribute>(y => y.Level > 30)).ToArray()

var someType = Types.FirstOrDefault(x => x.Name == "Seb");
```

## Breaking changes

### 2.2.0 -> 3.0.0
* `TypeFetcher` is no longer available. Use `Types` instead.