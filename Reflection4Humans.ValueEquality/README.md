![Reflection4Humans](https://github.com/Moreault/Reflection4Humans/blob/master/reflection4humans.png)

# Reflection4Humans.ValueEquality
Provides value equality between any two objects.

## Getting started
Adds a `ValueEquals` extension method to `System.Object` so it can basically be used from anywhere and on any type as long as this DLL is referenced.

```cs
var areEqual = object1.ValueEquals(object2);
```

By default, `ValueEquals` does a "shallow" comparison. That is, it compares all values on the current objects but doesn't go any further. This is the same behavior you would expect from .NET 5's `record` types right out of the box and will work for most use cases. Concretely, the object being compared is broken down into its public members one level deep, and each member is then compared using its own `Equals`. Strings and numbers are always compared by value (honoring `StringComparison` and treating equal numbers of different types as equal), but a member that is itself a complex object or a collection is left to its own equality, so two equivalent-but-distinct collections are *not* shallow-equal.

A value compares the same way whether it stands alone, is a member, or is an element of a collection.

If you need this comparison to be recursive (digging into nested objects and collection elements by value all the way down), you can do the following : 

```cs
var areEqual = object1.ValueEquals(object2, new ValueEqualityOptions { Depth = Depth.Recursive });
```

You can also provide a string comparison to be used for string properties or fields.

```cs
var areEqual = object1.ValueEquals(object2, new ValueEqualityOptions { StringComparison = StringComparison.InvariantCultureIgnoreCase });
```

## Specifics
All public properties and fields are compared so all private, protected and even internal values are ignored.