// <copyright file="BibTexParserTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests for the <see cref="BibTexParser{TBibItem}"/>.
    /// </summary>
    [TestClass]
    public class BibTexParserTest {

        [TestMethod]
        public void TestSingle() {
            {
                var bibtex = "@article{hugo, year=2024}";
                var items = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).ToList();
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual("article", items.First().EntryType);
                Assert.AreEqual("hugo", items.First().Key);
                Assert.IsTrue(items.First().ContainsField("year"));
            }

            {
                var bibtex = "@ article   {   hugo  , year  =  2024   }";
                var items = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).ToList();
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual("article", items.First().EntryType);
                Assert.AreEqual("hugo", items.First().Key);
                Assert.IsTrue(items.First().ContainsField("year"));
            }

            {
                var bibtex = "@ article{hugo,year=\"2024\"}";
                var items = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).ToList();
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual("article", items.First().EntryType);
                Assert.AreEqual("hugo", items.First().Key);
                Assert.IsTrue(items.First().ContainsField("year"));
            }

            {
                var bibtex = "@ article   {   hugo  , year  =  \"2024\"   }";
                var items = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).ToList();
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual("article", items.First().EntryType);
                Assert.AreEqual("hugo", items.First().Key);
                Assert.IsTrue(items.First().ContainsField("year"));
            }


            {
                var bibtex = "@ article{hugo,year={2024}}";
                var items = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).ToList();
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual("article", items.First().EntryType);
                Assert.AreEqual("hugo", items.First().Key);
                Assert.IsTrue(items.First().ContainsField("year"));
            }

            {
                var bibtex = "@ article   {   hugo  , year  =  {2024}   }";
                var items = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).ToList();
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual("article", items.First().EntryType);
                Assert.AreEqual("hugo", items.First().Key);
                Assert.IsTrue(items.First().ContainsField("year"));
            }

            {
                var bibtex = "@ article   {   hugo  , year  =  {{2024}}   }";
                var items = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex)).ToList();
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual(1, items.Count());
                Assert.AreEqual("article", items.First().EntryType);
                Assert.AreEqual("hugo", items.First().Key);
                Assert.IsTrue(items.First().ContainsField("year"));
            }
        }

        [TestMethod]
        [DeploymentItem("ctan.bib")]
        public void TestCtanBibtexTestFile() {
            var options = BibTexParserOptions.Create();
            Assert.IsTrue(File.Exists("ctan.bib"));
            var items = BibTexParser<BibItem>.Parse(new StreamReader("ctan.bib"), options).ToList();
            Assert.AreEqual(85, items.Count);

            {
                var item = items.SingleOrDefault(i => i.Key == "bs-1629");
                Assert.IsNotNull(item);
                Assert.AreEqual(WellKnownTypes.TechnicalReport, item.EntryType);
                Assert.IsNotNull(item.Author);
                Assert.IsTrue(item.Author.Any(a => a.Surname == "BSI"));
                Assert.AreEqual("Bibliographic References", item.Title);
                Assert.AreEqual("British Standards Institution", item.Institution);
                Assert.AreEqual("1976", item.Year);
                Assert.AreEqual("BS", item.Type);
                Assert.AreEqual("1629", item.Number);
            }
        }

        [TestMethod]
        public void TestInriaQuotedStringSample() {
            // Test samples from https://maverick.inria.fr/~Xavier.Decoret/resources/xdkbibtex/bibtex_summary.html
            {
                var bibtex = """
@Article{key03,
  title = "The lonely { brace",
}
""";

                Assert.ThrowsException<FormatException>(() => {
                    BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                });
            }

            {
                var bibtex = """
@Article{key03,
  title = "The lonely } brace",
}
""";

                Assert.ThrowsException<FormatException>(() => {
                    BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                });

                {
                    var options = BibTexParserOptions.Create();
                    options.Lenient = true;

                    var item = BibTexParser.Parse(new StringReader(bibtex), options).SingleOrDefault();
                    Assert.IsNotNull(item);
                }
            }

            {
                var bibtex = """
@Article{key03,
  title = "A {bunch {of} braces {in}} title"
}
""";
                var item = BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("A {bunch {of} braces {in}} title", item.Title);
            }

            {
                var bibtex = """
@Article{key01,
  author = "Simon \"the saint\" Templar",
}
""";

                Assert.ThrowsException<FormatException>(() => {
                    BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                });
            }

            {
                var bibtex = """
@Article{key01,
  author = "Simon {"}the {saint"} Templar",
}
""";
                var item = BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("Simon {\"}the {saint\"} Templar", item.Author!.Single().ToString("CMS"));
            }

            {
                var bibtex = """
@Article{key01,
  title = "The history of @ sign"
}
""";
                var item = BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("The history of @ sign", item.Title);
            }
        }

        [TestMethod]
        public void TestInriaBracedStringSample() {
            // Test samples from https://maverick.inria.fr/~Xavier.Decoret/resources/xdkbibtex/bibtex_summary.html
            {
                var bibtex = """
@Article{key01,
  title = { The history of @ sign }
}
""";

                Assert.ThrowsException<FormatException>(() => {
                    BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                });

                {
                    var options = BibTexParserOptions.Create();
                    options.Lenient = true;

                    var item = BibTexParser.Parse(new StringReader(bibtex), options).SingleOrDefault();
                    Assert.IsNotNull(item);
                    Assert.AreEqual("The history of @ sign", item.Title);
                }
            }
        }

        [TestMethod]
        public void TestInriaStringSample() {
            {
                var bibtex = """
@String(mar = "march")

@Book{sweig42,
  Author = { Stefan Sweig },
  title = { The impossible book },
  publisher = { Dead Poet Society},
  year = 1942,
  month = mar
}
""";
                var item = BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("march", item.Month);
            }

            {
                var bibtex = """
@String{mar = "march"}

@Book{sweig42,
  Author = { Stefan Sweig },
  title = { The impossible book },
  publisher = { Dead Poet Society},
  year = 1942,
  month = mar
}
""";
                var item = BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("march", item.Month);
            }

            {
                var bibtex = """
@String {firstname = "Xavier"}
@String {lastname  = "Decoret"}
@String {email      = firstname # "." # lastname # "@imag.fr"}

@Misc{x,
  Author = lastname # "," # firstname,
  Email = email,
}
""";
                var item = BibTexParser.Parse(new StringReader(bibtex)).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("Decoret, Xavier", item.Author!.Single().ToString("SC"));
                Assert.AreEqual("Xavier.Decoret@imag.fr", item["email"]);
            }
        }
    }
}
