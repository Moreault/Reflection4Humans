![Reflection4Humans](https://github.com/Moreault/Reflection4Humans/blob/master/reflection4humans.png)

# Reflection4Humans
Reflection extension methods meant to be used by humans.

## Type extensions

### Type.GetHumanReadableName

Have you ever wanted to output a type name in an exception message before only for it to read like this?

```c#
"Reflection4Humans.Extensions.Tests.Dummy`1[System.String] did something weird!"
```

Wouldn't it have been easier on the eyes if it had said this instead?

```c#
"Dummy<String> did something weird!"
```

This is what this method helps you achieve.

```c#
//Instead of this
typeof(Dummy<string>).Name

//Use this
typeof(Dummy<string>).GetHumanReadableName();
```

### Type.IsAttribute
```c#
//Isn't it weird that you have the following in C#
var isInterface = type.IsInterface;
var isAbstract = type.IsAbstract;
var isClass = type.IsClass;
var isGenericType = type.IsGenericType;

//But not this?
var isAttribute = type.IsAttribute();
```

Well now you have it.

### Type.GetPropertyPath

```c#
//Say you have a multi-level object like this
public record Person
{
	public string Name { get; init; }
	public Job Job { get; init; }
}

//Maybe you would like to have code looking at the Salary from a Person object
public record Job
{
	public string Title { get; init; }
	public decimal Salary { get; init; }
}

public void DoStuff()
{
	//The propertyPath variable now holds the PropertyInfo objects for Person's Job property as well as Job's Salary property, 
	//including the types for Person and Job. Essentially looking something like this : 
	//{ { Property = [Job PropertyInfo], Owner = [Person type object] }, { Property = [Salary PropertyInfo], Owner = [Job type object] } }
	var propertyPath = typeof(Person).GetPropertyPath("Job.Salary");

	...
}
```

## TypeFetcher

Wouldn't it be awesome if there was a way to get all your type metadata in all your assemblies in a straightforward way like this?

```c#
var allMyAbstractClasses = TypeFetcher.Query().IsClass().IsAbstract().ToList();
```

Well now you can!

Currently, TypeFetcher only supports the following criterion :
- IsClass / IsNotClass
- IsAbstract / IsNotAbstract (applies to both classes and interfaces unless also searching for classes)
- IsStruct / IsNotStruct
- IsInterface / IsNotInterface
- IsAttribute / IsNotAttribute
- IsGeneric / IsNotGeneric
- IsGenericTypeDefinition / IsNotGenericTypeDefinition
- IsEnum /IsNotEnum
- Implements
- HasAttributes

It is still being actively tested and may (very probably) change a lot in its syntax and usability.

### Why should I use this when I can do it myself?

You certainly can use the following (and might have) :

```c#
AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())[...]
```

But if you're like me then you probably don't like remembering that whole trainwreck and might even have to look up how you did it before in order to use it.

Not only that but I have also encountered problems with the above line in some cases because assemblies are not always automatically loaded and some types got left out. TypeFetcher does that for you. I won't hide that there's a bigger performance cost to it but getting types is always something that should be used sparingly.