// <copyright file="Name.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


using System;
using System.IO;
using System.Linq;

namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests the demo code in the README on GitHub.
    /// </summary>
    [TestClass]
    public class Demos {

        [TestMethod]
        public void NameObject() {
            {
                var author = new Name("Ulbricht", "Walter");
                Console.WriteLine(author.Surname);
                Console.WriteLine(author.ChristianName);
                Assert.AreEqual("Ulbricht", author.Surname);
                Assert.AreEqual("Walter", author.ChristianName);
            }

            {
                var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
                var (surname, christianName, middleNames) = author;
                Console.WriteLine(author.Surname);
                Console.WriteLine(author.ChristianName);
                Console.WriteLine(string.Join(", ", author.MiddleNames));
                Assert.AreEqual("Ulbricht", author.Surname);
                Assert.AreEqual("Walter", author.ChristianName);
                Assert.AreEqual("Ernst Paul", string.Join(" ", author.MiddleNames));
            }

            {
                var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
                Console.WriteLine(author.ToString(NameFormats.SurnameChristianNameMiddleNames));
                Assert.AreEqual("Ulbricht, Walter Ernst Paul", author.ToString(NameFormats.SurnameChristianNameMiddleNames));
            }

            {
                var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
                Console.WriteLine(author.ToString("SCm"));
                Assert.AreEqual("Ulbricht, Walter E. P.", author.ToString("SCm"));
            }

            {
                var authors = Name.Parse("Ulbricht, Walter and Honecker, Erich");
                Console.Write(string.Join("; ", authors));
                Assert.AreEqual("Ulbricht, Walter; Honecker, Erich", string.Join("; ", authors));
            }

            {
                var authors = Name.Parse("Walter Ulbricht and Erich Honecker");
                Console.Write(string.Join("; ", authors));
                Assert.AreEqual("Ulbricht, Walter; Honecker, Erich", string.Join("; ", authors));
            }

            {
                var authors = Name.Parse("Walter Ulbricht, Erich Honecker");
                Console.Write(string.Join("; ", authors));
                Assert.AreEqual("Ulbricht, Walter; Honecker, Erich", string.Join("; ", authors));
            }

            {
                var authors = Name.Parse("{Visualisierungsinstitut der Universität Suttgart}");
                Console.Write(string.Join("; ", authors));
                Assert.AreEqual("{Visualisierungsinstitut der Universität Suttgart}", string.Join("; ", authors));
            }

            {
                var authors = Name.Parse("{Visualisierungsinstitut der Universit\\\"at Suttgart}");
                Console.Write(string.Join("; ", authors));
                Assert.AreEqual("{Visualisierungsinstitut der Universit\\\"at Suttgart}", string.Join("; ", authors));
            }
        }

        [TestMethod]
        public void BibTexParsing() {
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

            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Author);
            Assert.AreEqual(1, item.Author.Count());
            Assert.AreEqual("P.", item.Author.Single().GivenName);
            Assert.AreEqual("Hershkovitz", item.Author.Single().Surname);
            Assert.AreEqual("1962", item.Year);
            Assert.AreEqual("Evolution of {Neotropical} cricetine rodents ({Muridae}) with special reference to the phyllotine group", item.Title);
            Assert.AreEqual("Fieldiana: Zoology", item.Series);
            Assert.AreEqual("46", item.Volume);
            Assert.AreEqual("Chicago", item.Address);
            Assert.AreEqual("Field Museum of Natural History", item.Publisher);

        }
    }
}
