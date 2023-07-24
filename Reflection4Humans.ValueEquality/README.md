![Reflection4Humans](https://github.com/Moreault/Reflection4Humans/blob/master/reflection4humans.png)

# Reflection4Humans.ValueEquality
Provides value equality between any two objects.

## Getting started
Adds a `ValueEquals` extension method to `System.Object` so it can basically be used from anywhere and on any type as long as this DLL is referenced.

```cs
var areEqual = object1.ValueEquals(object2);
```

By default, `ValueEquals` does a "shallow" comparison. That is, it compares all values on the current objects but doesn't go any further. This is the same behavior you would expect from .NET 5's `record` types right out of the box and will work for most use cases. If you need this comparison to be recursive, you can do the following : 

```cs
var areEqual = object1.ValueEquals(object2, new ValueEqualityOptions { Depth = EqualityDepth.Recursive });
```

You can also provide a string comparison to be used for string properties or fields.

```cs
var areEqual = object1.ValueEquals(object2, new ValueEqualityOptions { StringComparison = StringComparison.InvariantCultureIgnoreCase });
```

## Specifics
All public properties and fields are compared so all private, protected and even internal values are ignored.