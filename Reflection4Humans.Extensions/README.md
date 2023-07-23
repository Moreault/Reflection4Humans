![Reflection4Humans](https://github.com/Moreault/Reflection4Humans/blob/master/reflection4humans.png)

# Reflection4Humans.Extensions
Reflection extension methods meant to be used by humans.

## Member search extensions

### GetAll[MEMBER]

* Type.GetAllMembers
* Type.GetAllProperties
* Type.GetAllFields
* Type.GetAllMethods
* Type.GetAllConstructors

The above extension methods will return all members of said kind on any .NET type. There is no confusing flag for which you need to specify whether or not you want private, protected or static members. It's all done via predicates.

```cs
//Returns all members including those on inherited types
var allMembers = typeof(Dummy).GetAllMembers();

//Returns all private members that start with "Johnny"
var members = typeof(Dummy).GetAllMembers(x => x.Name.StartsWith("Johnny") && x.IsPrivate);

//Returns all get-only static properties
var properties = typeof(Dummy).GetAllProperties(x => x.IsGet && !x.IsSet && x.IsStatic);

//Returns public fields only
var fields = typeof(Dummy).GetAllFields(x => x.IsPublic);

//Returns methods that have parameters of string, char and int (in that order)
var methods = typeof(Dummy).GetAllMethods(x => x.HasParameters<string, char, int>());

//You can use some of the same approach for constructors as you would regular methods
var constructors = typeof(Dummy).GetAllConstructors(x => x.HasParameters<int>() && !x.IsStatic);
```

### GetSingle[MEMBER] & GetSingle[MEMBER]OrDefault

* Type.GetSingleMember & Type.GetSingleMemberOrDefault
* Type.GetSingleProperty & Type.GetSinglePropertyOrDefault
* Type.GetSingleField & Type.GetSingleFieldOrDefault
* Type.GetSingleMethod & Type.GetSingleMethodOrDefault
* Type.GetSingleConstructor & Type.GetSingleConstructorOrDefault

As you probably expect, the overload without "Default" in it will throw an exception if there is no member found. That is the only difference between the two. They are used the exact same way as the above "GetAll" extensions with the exception that they take a name.



## Type.GetHumanReadableName

Have you ever wanted to output a type name in an exception message before only for it to read like this?

```cs
"Reflection4Humans.Extensions.Tests.Dummy`1[System.String] did something weird!"
```

Wouldn't it have been easier on the eyes if it had said this instead?

```cs
"Dummy<String> did something weird!"
```

This is what this method helps you achieve.

```cs
//Instead of this
typeof(Dummy<string>).Name

//Use this
typeof(Dummy<string>).GetHumanReadableName();
```

## Type.IsAttribute
```cs
//Isn't it weird that you have the following in cs
var isInterface = type.IsInterface;
var isAbstract = type.IsAbstract;
var isClass = type.IsClass;
var isGenericType = type.IsGenericType;

//But not this?
var isAttribute = type.IsAttribute();
```

Well now you have it.

## Type.GetPropertyPath

```cs
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

