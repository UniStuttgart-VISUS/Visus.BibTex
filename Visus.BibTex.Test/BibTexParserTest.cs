// <copyright file="BibTexParserTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.IO;
using System.Linq;


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
        public void TestProcessLatex() {
            {
                var bibtex = """
@ARTICLE{Fellner-Helmberg93,
	AUTHOR = {Fellner, Dieter W. and Helmberg, Christoph},
	TITLE = {Robust Rendering of General Ellipses and Elliptical Arcs},
	JOURNAL = tog,
	VOLUME = {12},	NUMBER = {3},
	MONTH = jul,	YEAR = {1993},	PAGES = {251-276},
  DOI= {10.1145/169711.169704}
  }
""";
                var options = BibTexParserOptions.Create();
                options.ProcessLatex = true;
                options.Variables.Add("tog", "ACM Transactions on Graphics");
                var item = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex), options).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("article", item.EntryType);
                Assert.AreEqual("Fellner-Helmberg93", item.Key);
                Assert.AreEqual("ACM Transactions on Graphics", item.Journal);
                Assert.AreEqual("12", item.Volume);
                Assert.AreEqual("3", item.Number);
                Assert.AreEqual("July", item.Month);
                Assert.AreEqual("1993", item.Year);
                Assert.AreEqual("251-276", item.Pages);
                Assert.AreEqual("10.1145/169711.169704", item.Doi);
            }

            {
                var bibtex = """
@inproceedings{Macedo:2021:Tool,
    author="Macedo, In{\^e}s and Wanous, Sinan and Oliveira, Nuno and Sousa, Orlando and Pra{\c{c}}a, Isabel",
    title={A Tool to Support the Investigation and Visualization of Cyber and/or Physical Incidents},
    booktitle={Proc. Trends Appl. Inf. Syst. Tech.},
    year={2021},
    pages={130--140},
    doi={10/mh45},
}
""";
                var options = BibTexParserOptions.Create();
                options.ProcessLatex = true;
                var item = BibTexParser<BibItem>.Parse<BibItemBuilder>(new StringReader(bibtex), options).SingleOrDefault();
                Assert.IsNotNull(item);
                Assert.AreEqual("inproceedings", item.EntryType);
                Assert.AreEqual("Macedo:2021:Tool", item.Key);
                Assert.IsNotNull(item.Author);
                Assert.AreEqual("Inês", item.Author.First().ChristianName);
                Assert.AreEqual("Praça", item.Author.Last().Surname);
                Assert.AreEqual("Proc. Trends Appl. Inf. Syst. Tech.", item.BookTitle);
                Assert.AreEqual("2021", item.Year);
                Assert.AreEqual("130--140", item.Pages);
                Assert.AreEqual("10/mh45", item.Doi);
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

            {
                var item = items.SingleOrDefault(i => i.Key == "hershkovitz-62");
                Assert.IsNotNull(item);
                Assert.AreEqual(WellKnownTypes.Book, item.EntryType);
                Assert.IsNotNull(item.Author);
                Assert.IsTrue(item.Author.Any(a => a.Surname == "Hershkovitz"));
                Assert.IsTrue(item.Author.Any(a => a.ChristianName == "P."));
                Assert.AreEqual("1962", item.Year);
                Assert.AreEqual("""
Evolution of {Neotropical} cricetine rodents
                 ({Muridae}) with special reference to the phyllotine
                 group
""", item.Title);
                Assert.AreEqual("Fieldiana: Zoology", item.Series);
                Assert.AreEqual("46", item.Volume);
                Assert.AreEqual("Chicago", item.Address);
                Assert.AreEqual("Field Museum of Natural History", item.Publisher);
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
