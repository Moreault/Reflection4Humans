![Reflection4Humans](https://github.com/Moreault/Reflection4Humans/blob/master/reflection4humans.png)

# Reflection4Humans.TypeGenerator
Generates temporary types at runtime based on interfaces and classes.

## Getting started

You can use the `TypeGenerator` static class to generate new types. It works with interfaces, abstract and non-abstract classes (excluding sealed classes). Types generated are empty and return default values only.

```cs
public interface IBogus
{
	int Id { get; }
	string Name { get; set; }
}
```

```cs
var newType = TypeGenerator.From<IBogus>();
```