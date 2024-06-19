# Visus.BibTex

[![Build Status](https://visualisierungsinstitut.visualstudio.com/Visus.BibTex/_apis/build/status/UniStuttgart-VISUS.Visus.BibTex?branchName=master)](https://visualisierungsinstitut.visualstudio.com/Visus.BibTex/_build/latest?definitionId=9&branchName=master)

A library for parsing and manipulating BibTex entries in .NET for use in Project Tiger.


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
var item = BibTexParser.Parse(new StringReader(bibtex), BibTexParserOptions.Create()).Single();
```


### BibItems
The library provides a `Dictionary`-based implementation of a BibTex entry in the form of the [`Visus.BibTex.BibItem`](Visus.BibTex/BibItem.cs) class. However, callers can customise the parser to fill their own class by implementing [`Visus.BibTex.IBibItemBuilder`](Visus.BibTex/IBibItemBuilder.cs) for this class. [`Visus.BibTex.BibItemBuilder`](Visus.BibTex/BibItemBuilder.cs) is the reference implementation for the default item type.

The `BibItem` allows for conversion to custom strings via `IFormattable`. The format string is split in two parts, the general format for the entry and the format for names (see below). Both parts are separated by a '.'. Possible format strings for the item are:

| Format string | Description |
| ------------- | ----------- |
| c             | The compact format puts everything into one line with as little spacing as possible. |
| C             | Like the compact format, but adds spaces after commas. |
| l             | Places each field in a separate line. |
| sN            | Places each field in a separate line and indents it with N spaces if N is an integer number. If N is omitted or less than one, one space will be used. |
| tN            | Places each field in a separate line and indents it with N tabs if N is an integer number. If N is omitted or less than one, one tab will be used. |
| q             | Uses quotes instead of braces for enclosing the fields. This modifier can be combined with any format. |

Note that if multiple conflicting formats are specified, the last one will be applied, i.e. "ts4" will use four spaces for indentation. The following example illustrates several formatting options:

```C#
var item = new BibItem(WellKnownTypes.InProceedings, "mueller:2022:power") {
    Author = [ new("Müller", "Christoph"), new("Heinemann", "Moritz"), new("Weiskopf", "Daniel"), new("Ertl", "Thomas") ],
    Title = "Power Overwhelming: Quantifying the Energy Cost of Visualisation",
    BookTitle = "Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)",
    Month = "October",
    Year = "2022",
    Doi = "10.1109/BELIV57783.2022.00009",
    Pages = "38-46"
};


Console.WriteLine(item);

// Expected console output:
// @inproceedings{mueller:2022:power,
//    author = {Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas},
//    booktitle = {Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)},
//    doi = {10.1109/BELIV57783.2022.00009},
//    month = {October},
//    pages = {38-46},
//    title = {Power Overwhelming: Quantifying the Energy Cost of Visualisation},
//    year = {2022}
// }


Console.WriteLine(item.ToString("C"));

// Expected console output:
// @inproceedings{mueller:2022:power, author = {Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas}, booktitle = {Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)}, doi = {10.1109/BELIV57783.2022.00009}, month = {October}, pages = {38-46}, title = {Power Overwhelming: Quantifying the Energy Cost of Visualisation}, year = {2022}}


Console.WriteLine(item.ToString("cq"));

// Expected console output:
// @inproceedings{mueller:2022:power,author="Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas",booktitle="Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)",doi="10.1109/BELIV57783.2022.00009",month="October",pages="38-46",title="Power Overwhelming: Quantifying the Energy Cost of Visualisation",year="2022"}


Console.WriteLine(item.ToString("s2q.cS"));

// Expected console output:
// @inproceedings{mueller:2022:power,
//   author = "C. Müller and M. Heinemann and D. Weiskopf and T. Ertl",
//   booktitle = "Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)",
//   doi = "10.1109/BELIV57783.2022.00009",
//   month = "October",
//   pages = "38-46",
//   title = "Power Overwhelming: Quantifying the Energy Cost of Visualisation",
//   year = "2022"
//}
```

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
var authors = Name.Parse("{Visualisierungsinstitut der Universität Suttgart}");
Console.Write(string.Join("; ", authors));

// Expected console output:
// {Visualisierungsinstitut der Universität Suttgart}
```

However, it will not remove the braces nor process any Latex instructions:
```C#
var authors = Name.Parse("{Visualisierungsinstitut der Universit\\\"at Suttgart}");
Console.Write(string.Join("; ", authors));

// Expected console output:
// {Visualisierungsinstitut der Universit\"at Suttgart}
```