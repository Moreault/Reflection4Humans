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
var members = typeof(Dummy).GetAllMembers(x => x.Name.StartsWith("Johnny") && x.IsPrivate());

//Returns all get-only static properties
var properties = typeof(Dummy).GetAllProperties(x => x.IsGet() && !x.IsSet() && x.IsStatic());

//Returns public fields only
var fields = typeof(Dummy).GetAllFields(x => x.IsPublic());

//Returns methods that have parameters of string, char and int (in that order)
var methods = typeof(Dummy).GetAllMethods(x => x.HasParameters<string, char, int>());

//You can use some of the same approach for constructors as you would regular methods
var constructors = typeof(Dummy).GetAllConstructors(x => x.HasParameters<int>() && !x.IsStatic());
```

### GetSingle[MEMBER] & GetSingle[MEMBER]OrDefault

* Type.GetSingleMember & Type.GetSingleMemberOrDefault
* Type.GetSingleProperty & Type.GetSinglePropertyOrDefault
* Type.GetSingleField & Type.GetSingleFieldOrDefault
* Type.GetSingleMethod & Type.GetSingleMethodOrDefault
* Type.GetSingleConstructor & Type.GetSingleConstructorOrDefault

As you probably expect, the overload without "Default" in it will throw an exception if there is no member found. That is the only difference between the two. They are used the exact same way as the above "GetAll" extensions. There are additional overloads that do a simple search by name but they will throw an exception if there is more than one result to your query so make sure that there is indeed only one member by that name.

```cs
//Will return a single get-only property from the Dummy type or will throw if there is more than one public get-only property
var property = typeof(Dummy).GetSingleProperty(x => x.IsPublic() && !x.IsSet())

//It may be better to go by name in the case of properties if you can
var propertyByName = typeof(Dummy).GetSingleProperty("Level");

//But in the case of methods, multiple ones can share the same name and even some parameters. It can be difficult to find the right one in some cases
var method = typeof(Dummy).GetSingleMethodOrDefault(x => x.Name == "LevelUp" && x.HasParameters<int, string>() && x.IsGeneric());
```

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

## Type.IsAttribute
True if the the type is an attribute.

```cs
if (type.IsAttribute())
{
	...
}
```

## Type.GetDirectInterfaces
Returns all interfaces that the type implements. In other words, it excludes those that are implemented "indirectly" via a base class.

```cs
var directInterfaces = type.GetDirectInterfaces();
```

## Type.Implements
Provides an easy way to check whether or not a type implements an interface (directly OR indirectly).

```cs
if (type.Implements<IDisposable>)
{
	...
}

//Also comes with a non-generic overload
if (type.Implements(typeof(IDisposable))
{
	...
}
```

## Type.DirectlyImplements
Returns true when a type directly implements an interface (as opposed to the "regular" `Implements` method). It is also used the same as the `Implements` method.

```cs
if (type.DirectlyImplements<IDisposable>)
{
	...
}

//Also comes with a non-generic overload
if (type.DirectlyImplements(typeof(IDisposable))
{
	...
}
```

## Type.HasInterface
True if the type implements at least one interface.

```cs
if (type.HasInterface())
{
	...
}
```

## Type.HasAttribute
* HasAttribute()
```cs
//True if the type has at least one attribute of any type
if (type.HasAttribute())
{
	...
}
```
* HasAttribute<TAttribute>()
```cs
//Is true if the type has an attribute of type AutoInjectAttribute regardless of the attribute's properties
if (type.HasAttribute<AutoInjectAttribute>())
{
	...
}

//Can also be used to check the attribute's properties as well
if (type.HasAttribute<AutoInjectAttribute>(x => x.Interface == typeof(IDummy)))
{
	...
}

//There is also a non-generic overload but it can't be used to check the attributes' properties
if (type.HasAttribute(typeof(AutoInjectAttribute))
{
	...
}
```

## MemberInfo extensions
* IsStatic
* IsInstance
* IsPrivate
* IsProptected
* IsInternal
* IsPublic
* IsConstructor
* IsMethod
* IsField
* IsProperty

These are meant to make it easier to search `MemberInfo` but could be used in other scenarios as well.

## MethodInfo & MethodBase extensions
* HasParameters
```cs
//Returns true if the method has a signature of string, char and long (in this precise order)
if (method.HasParameters<string, char, long>()) 
{
	...
}

//It also comes with a non-generic overload
if (method.HasParameters(typeof(string), typeof(char), typeof(long))
{
	...
}
```
* HasParameters(count)
```cs
//True if the constructor has exactly two parameters
if (constructor.HasParameters(2))
{
	...
}
```
* HasNoParameter
```cs
//True if the method has no parameter
if (method.HasNoParameter())
{
	...
}
```

## PropertyInfo
* IsStatic
```cs
//Normally, you would need to access the get or set method to check whether or not the property is static. Now you can check out the property directly.
if (property.IsStatic())
{
	...
}
```
* IsGet
```cs
//True if the property has a get accessor
if (property.IsGet())
{
	...
}
```
* IsSet
```cs
//True if the property has a set accessor
if (property.IsSet())
{
	...
}
```