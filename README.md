# Visus.BibTex

A library for parsing and manipulating BibTex entries in .NET 8+.


## Usage

### BibItems
The library provides a `Dictionary`-based implementation of a BibTex entry in the form of the [`Visus.BibTex.BibItem`](Visus.BibTex/BibItem.cs) class. However, callers can customise the parser to fill their own class by implementing [`Visus.BibTex.IBibItemBuilder`](Visus.BibTex/IBibItemBuilder.cs) for this class. [`Visus.BibTex.BibItemBuilder`](Visus.BibTex/BibItemBuilder.cs) is the reference implementation for the default item type.

### Authors and editors
The library parses people into structured objects of type [`Visus.BibTex.Name`](Visus.BibTex/Name.cs). You can use these object to process the details of a name like this:

```C#
var author = new Name("Ulbricht", "Walter");
Console.WriteLine(author.Surname);
Console.WriteLine(author.ChristianName);

// Expected console output:
// Ulbricht
// Walter
```

`Name`s also support decomposition:

```C#
var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
var (surname, christianName, middleNames) = author;
Console.WriteLine(author.Surname);
Console.WriteLine(author.ChristianName);
Console.WriteLine(string.Join(" ", author.MiddleNames));

// Expected console output:
// Ulbricht
// Walter
// Ernst Paul
```

`Name` implements `IFormattable` and thus supports formatting using the formats defined in [`Visus.BibTex.NameFormats`](Visus.BibTex/NameFormats.cs).

```C#
var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
Console.WriteLine(author.ToString(NameFormats.SurnameChristianNameMiddleNames));

// Expected console output:
// Ulbricht, Walter Ernst Paul
```

You can also combine the single-character formats to form custom ones:

```C#
var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
Console.WriteLine(author.ToString("SCm"));

// Expected console output:
// Ulbricht, Walter E. P.
```

The formats that can be combined are "S" for the surname, "C" for the Christian name, "M" for the middle names and "X" for a name suffix like "jr." All except for the suffix support abbreviation by specifying the lower-case variant.
