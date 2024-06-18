# Visus.BibTex

[![Build Status](https://visualisierungsinstitut.visualstudio.com/Visus.BibTex/_apis/build/status/UniStuttgart-VISUS.Visus.BibTex?branchName=master)](https://visualisierungsinstitut.visualstudio.com/Visus.BibTex/_build/latest?definitionId=9&branchName=master)

A library for parsing and manipulating BibTex entries in .NET 8+.


## Usage

### Parsing
BibTex can be parsed from any `TextReader`, for instance a `StringReader` in the following example, and yields an enumeration of items of the requested type:

```C#
var bibtex = """
@Book{hershkovitz-62,
    author = "P. Hershkovitz",
    year = "1962",
    title = "Evolution of {Neotropical} cricetine rodents ({Muridae}) with special reference to the phyllotine group",
    series = "Fieldiana: Zoology",
    volume = "46",
    address = "Chicago",
    publisher = "Field Museum of Natural History"
}
""";
var item = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).SingleOrDefault();
```


### BibItems
The library provides a `Dictionary`-based implementation of a BibTex entry in the form of the [`Visus.BibTex.BibItem`](Visus.BibTex/BibItem.cs) class. However, callers can customise the parser to fill their own class by implementing [`Visus.BibTex.IBibItemBuilder`](Visus.BibTex/IBibItemBuilder.cs) for this class. [`Visus.BibTex.BibItemBuilder`](Visus.BibTex/BibItemBuilder.cs) is the reference implementation for the default item type.

### Authors and editors
The library parses people (`Visus.BibTex.WellKnownFields.Author` and `Visus.BibTex.WellKnownFields.Editor`) into structured objects of type [`Visus.BibTex.Name`](Visus.BibTex/Name.cs). You can use these object to process the details of a name like this:

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

`Visus.BibTex.Name.Parse` also supports the opposite direction, i.e. parsing a string into a list of names:

```C#
var authors = Name.Parse("Ulbricht, Walter and Honecker, Erich");
Console.Write(string.Join("; ", authors));

// Expected console output:
// Ulbricht, Walter; Honecker, Erich
```

The library makes best effort to recognise different formats of names, but it works best if authors are separated by the common "and":

```C#
var authors = Name.Parse("Walter Ulbricht and Erich Honecker");
Console.Write(string.Join("; ", authors));

// Expected console output:
// Ulbricht, Walter; Honecker, Erich

// The following should yield the same outputs, but is more error-prone for
// instance if surnames comprise of more than one token.
var authors = Name.Parse("Walter Ulbricht, Erich Honecker");
Console.Write(string.Join("; ", authors));
```

Note that the parser honours braces around names by not tokenising braced expressions:

```C#
var authors = Name.Parse("{Visualisierungsinstitut der Universitšt Suttgart}");
Console.Write(string.Join("; ", authors));

// Expected console output:
// {Visualisierungsinstitut der Universitšt Suttgart}
```

However, it will not remove the braces nor process any Latex instructions:
```C#
var authors = Name.Parse("{Visualisierungsinstitut der Universit\\\"at Suttgart}");
Console.Write(string.Join("; ", authors));

// Expected console output:
// {Visualisierungsinstitut der Universit\"at Suttgart}
```