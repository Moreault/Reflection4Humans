![Reflection4Humans](https://github.com/Moreault/Reflection4Humans/blob/master/reflection4humans.png)

# Reflection4Humans
Reflection extension methods meant to be used by humans.

## Type extensions
Have you ever wanted to output a type name in an exception message before only for it to read like this?

### Type.GetHumanReadableName

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
