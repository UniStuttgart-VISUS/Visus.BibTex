# Visus.BibTex

A library for parsing and manipulating BibTex entries in .NET 8+.


## Usage

### BibItems

### Authors and editors
The library parses people into structured objects of type `Visus.BibTex.Name`. You can use these object to process the details of a name like this:

```C#
var author = new Name("Ulbricht", "Walter");
Console.WriteLine(author.Surname);
Console.WriteLine(author.ChristianName);
```

`Name`s also support decomposition:

```C#
var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
var (surname, christianName, middleNames) = author;
Console.WriteLine(author.Surname);
Console.WriteLine(author.ChristianName);
Console.WriteLine(string.Join(" ", author.MiddleNames));
```

`Name` implements `IFormattable` and thus supports formatting using the formats defined in `Visus.BibTex.NameFormats`.

```C#
var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
Console.WriteLine(author.ToString(NameFormats.SurnameChristianNameMiddleNames));
```

You can also combine the single-character formats to form custom ones:

```C#
var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
Console.WriteLine(author.ToString("SCm"));
```

The formats that can be combined are "S" for the surname, "C" for the Christian name, "M" for the middle names and "X" for a name suffix like "jr." All except for the suffix support abbreviation by specifying the lower-case variant.
