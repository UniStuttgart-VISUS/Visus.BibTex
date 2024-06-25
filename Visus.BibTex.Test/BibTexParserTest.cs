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
                Assert.AreEqual("130–140", item.Pages);
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

        [TestMethod]
        [DeploymentItem("becker2024alertsets.bib")]
        public void TestBecker2024AlertSets() {
            var options = BibTexParserOptions.Create();
            options.ProcessLatex = true;

            Assert.IsTrue(File.Exists("becker2024alertsets.bib"));
            var items = BibTexParser<BibItem>.Parse(new StreamReader("becker2024alertsets.bib"), options).ToList();
            Assert.AreEqual(63, items.Count);

            {
                var item = items.SingleOrDefault(i => i.Key == "Marchionini:2006:ExploratorySearch");
                Assert.IsNotNull(item);
                Assert.AreEqual(WellKnownTypes.Article, item.EntryType);
                Assert.IsNotNull(item.Author);
                Assert.IsTrue(item.Author.Any(a => a.Surname == "Marchionini"));
                Assert.AreEqual("Commun. ACM", item.Journal);
                Assert.AreEqual("4", item.Month);
                Assert.AreEqual("4", item.Number);
                Assert.AreEqual("41–46", item.Pages);
                Assert.AreEqual("Exploratory Search: From Finding to Understanding", item.Title);
                Assert.AreEqual("49", item.Volume);
                Assert.AreEqual("2006", item.Year);
            }

            Assert.IsTrue(items.Any(i => i.Key == "Bohara:2020:Intrusion"));
            Assert.IsTrue(items.Any(i => i.Key == "Silva:2018:Hierarchical"));
            Assert.IsTrue(items.Any(i => i.Key == "Raj:2020:ClusteringBasedIncident"));
            Assert.IsTrue(items.Any(i => i.Key == "Guizani:2016:KMeans"));
            Assert.IsTrue(items.Any(i => i.Key == "Landauer:2020:SystemLog"));
            Assert.IsTrue(items.Any(i => i.Key == "RiddleWorkman:2021:Multitype"));
            Assert.IsTrue(items.Any(i => i.Key == "Pavelenko:2022:ComputerNetworkClustering"));
            Assert.IsTrue(items.Any(i => i.Key == "Bar:2014:DBStream"));
            Assert.IsTrue(items.Any(i => i.Key == "Ester:1996:DensityBased"));
            Assert.IsTrue(items.Any(i => i.Key == "Sedlmair:2012:DSM"));
            Assert.IsTrue(items.Any(i => i.Key == "Gleicher:2020:Boxer"));
            Assert.IsTrue(items.Any(i => i.Key == "Agarwal:2020:SetStreams"));
            Assert.IsTrue(items.Any(i => i.Key == "Arias:2011:PairAnalytics"));
            Assert.IsTrue(items.Any(i => i.Key == "Yi:2007:Interaction"));
            Assert.IsTrue(items.Any(i => i.Key == "venn:1880:diagrammatic"));
            Assert.IsTrue(items.Any(i => i.Key == "keim:2008:visual"));
            Assert.IsTrue(items.Any(i => i.Key == "Fails:2003:InteractiveML"));
            Assert.IsTrue(items.Any(i => i.Key == "Wenskovitch:2021:HMT"));
            Assert.IsTrue(items.Any(i => i.Key == "Komadina:2022:VizSecDesignSpace"));
            Assert.IsTrue(items.Any(i => i.Key == "Beran:2020:FIMETIS"));
            Assert.IsTrue(items.Any(i => i.Key == "Guerra:2019:InteractiveLabeling"));
            Assert.IsTrue(items.Any(i => i.Key == "Theron:2017:IntrusionDetection"));
            Assert.IsTrue(items.Any(i => i.Key == "Camacho:2017:GPCA"));
            Assert.IsTrue(items.Any(i => i.Key == "Ulmer:2019:NetCapVis"));
            Assert.IsTrue(items.Any(i => i.Key == "Gove:2021:IncidentReports"));
            Assert.IsTrue(items.Any(i => i.Key == "Bakirtzis:2018:CPSAnalysis"));
            Assert.IsTrue(items.Any(i => i.Key == "Becker:2020:DGAVis"));
            Assert.IsTrue(items.Any(i => i.Key == "Sopan:2021:AITotal"));
            Assert.IsTrue(items.Any(i => i.Key == "Sopan:2018:MLforSOC"));
            Assert.IsTrue(items.Any(i => i.Key == "Angelini:2021:Bucephalus"));
            Assert.IsTrue(items.Any(i => i.Key == "Gates:2013:VisForSec"));
            Assert.IsTrue(items.Any(i => i.Key == "McInnes:2018:UMAP"));
            Assert.IsTrue(items.Any(i => i.Key == "Lavigne:2014:VAforSec"));
            Assert.IsTrue(items.Any(i => i.Key == "Jiang:2022:SysLitReviwe"));
            Assert.IsTrue(items.Any(i => i.Key == "grottel:2007:cluster"));
            Assert.IsTrue(items.Any(i => i.Key == "vanMaaten:2008:tsne"));
            Assert.IsTrue(items.Any(i => i.Key == "Koch:2011:Patents"));
            Assert.IsTrue(items.Any(i => i.Key == "Verleysen:2005:Curse"));
            Assert.IsTrue(items.Any(i => i.Key == "Seo:2002:GeneIdent"));
            Assert.IsTrue(items.Any(i => i.Key == "Eder:2017:Stylometry"));
            Assert.IsTrue(items.Any(i => i.Key == "HanMing:2010:GAP"));
            Assert.IsTrue(items.Any(i => i.Key == "Michaels:1998:GenomeExpr"));
            Assert.IsTrue(items.Any(i => i.Key == "Zhou:2009:ClusterComp"));
            Assert.IsTrue(items.Any(i => i.Key == "Das:2021:GeonoCluster"));
            Assert.IsTrue(items.Any(i => i.Key == "Xia:2023:CDR"));
            Assert.IsTrue(items.Any(i => i.Key == "Kwon:2018:Clustervision"));
            Assert.IsTrue(items.Any(i => i.Key == "Yang:2021:IntSteering"));
            Assert.IsTrue(items.Any(i => i.Key == "Lekschas:2023:reglscatterplot"));
            Assert.IsTrue(items.Any(i => i.Key == "Pocco:2022:DRIFT"));
            Assert.IsTrue(items.Any(i => i.Key == "Steed:2013:BDVA"));
            Assert.IsTrue(items.Any(i => i.Key == "Wang:2011:EHR"));
            Assert.IsTrue(items.Any(i => i.Key == "Röhling:2015:ActRec"));
            Assert.IsTrue(items.Any(i => i.Key == "Eichmann:2019:MetroViz"));
            Assert.IsTrue(items.Any(i => i.Key == "Adams:2018:Effective"));
            Assert.IsTrue(items.Any(i => i.Key == "DAmico:2016:Cyber"));
            Assert.IsTrue(items.Any(i => i.Key == "Shi:2018:RadialIDS"));
            Assert.IsTrue(items.Any(i => i.Key == "Gove:2022:Narrative"));
            Assert.IsTrue(items.Any(i => i.Key == "Hong:2019:AlertVision"));
            Assert.IsTrue(items.Any(i => i.Key == "Macedo:2021:Tool"));
            Assert.IsTrue(items.Any(i => i.Key == "Carvalho:2016:OwlSight"));
            Assert.IsTrue(items.Any(i => i.Key == "Shneiderman:2003:eyes"));
            Assert.IsTrue(items.Any(i => i.Key == "White:2009:ExploratorySearch"));
            Assert.IsTrue(items.Any(i => i.Key == "Marchionini:2006:ExploratorySearch"));
        }

        [TestMethod]
        [DeploymentItem("gralka2024power.bib")]
        public void TestGralka2024Power() {
            var options = BibTexParserOptions.Create();
            options.ProcessLatex = true;

            Assert.IsTrue(File.Exists("gralka2024power.bib"));
            var items = BibTexParser<BibItem>.Parse(new StreamReader("gralka2024power.bib"), options).ToList();
            Assert.AreEqual(74, items.Count);

            Assert.IsTrue(items.Any(i => i.Key == "Grinstein:2002:IVV"));
            Assert.IsTrue(items.Any(i => i.Key == "Isenberg:2017:VMC"));
            Assert.IsTrue(items.Any(i => i.Key == "Kindlmann:1999:SAG"));
            Assert.IsTrue(items.Any(i => i.Key == "Kitware:2003"));
            Assert.IsTrue(items.Any(i => i.Key == "Levoy:1989:DSV"));
            Assert.IsTrue(items.Any(i => i.Key == "Lorensen:1987:MCA"));
            Assert.IsTrue(items.Any(i => i.Key == "Max:1995:OMF"));
            Assert.IsTrue(items.Any(i => i.Key == "Nielson:1991:TAD"));
            Assert.IsTrue(items.Any(i => i.Key == "Ware:2004:IVP"));
            Assert.IsTrue(items.Any(i => i.Key == "Wyvill:1986:DSS"));
            Assert.IsTrue(items.Any(i => i.Key == "Chuang:2009:EAC"));
            Assert.IsTrue(items.Any(i => i.Key == "Wang:2016:RTR"));
            Assert.IsTrue(items.Any(i => i.Key == "bridges:2016:understanding"));
            Assert.IsTrue(items.Any(i => i.Key == "hong:2010:model"));
            Assert.IsTrue(items.Any(i => i.Key == "burtscher:2014:k20"));
            Assert.IsTrue(items.Any(i => i.Key == "abe:2012:power"));
            Assert.IsTrue(items.Any(i => i.Key == "johnsson:2012:efficiency"));
            Assert.IsTrue(items.Any(i => i.Key == "johnsson:2014:measuring"));
            Assert.IsTrue(items.Any(i => i.Key == "luo:2011:model"));
            Assert.IsTrue(items.Any(i => i.Key == "mittal:2014:survey"));
            Assert.IsTrue(items.Any(i => i.Key == "arafa:2020:verified"));
            Assert.IsTrue(items.Any(i => i.Key == "mei:2013:dvfs"));
            Assert.IsTrue(items.Any(i => i.Key == "weaver:2012:papi"));
            Assert.IsTrue(items.Any(i => i.Key == "fahad:2019:survey"));
            Assert.IsTrue(items.Any(i => i.Key == "jahanshahi:2020:inference"));
            Assert.IsTrue(items.Any(i => i.Key == "rethinagiri:2015:fpga"));
            Assert.IsTrue(items.Any(i => i.Key == "qasaimeh:2019:fpga"));
            Assert.IsTrue(items.Any(i => i.Key == "collange:2009:software"));
            Assert.IsTrue(items.Any(i => i.Key == "iea:2021:datacentres"));
            Assert.IsTrue(items.Any(i => i.Key == "holmes:2012:ecovis"));
            Assert.IsTrue(items.Any(i => i.Key == "cerf:2021:space"));
            Assert.IsTrue(items.Any(i => i.Key == "wallossek:2022:readout"));
            Assert.IsTrue(items.Any(i => i.Key == "amberger:2021:shunt"));
            Assert.IsTrue(items.Any(i => i.Key == "borkar:2011:future"));
            Assert.IsTrue(items.Any(i => i.Key == "jones:2018:factories"));
            Assert.IsTrue(items.Any(i => i.Key == "heinemann:2017:mobile"));
            Assert.IsTrue(items.Any(i => i.Key == "andrae:2015:electricity"));
            Assert.IsTrue(items.Any(i => i.Key == "ma:2009:statistical"));
            Assert.IsTrue(items.Any(i => i.Key == "nagasaka:2010:statistical"));
            Assert.IsTrue(items.Any(i => i.Key == "sheaffer:2004:simulation"));
            Assert.IsTrue(items.Any(i => i.Key == "ramani:2007:powerred"));
            Assert.IsTrue(items.Any(i => i.Key == "lim:2014:mcpat"));
            Assert.IsTrue(items.Any(i => i.Key == "hu:2021:dl"));
            Assert.IsTrue(items.Any(i => i.Key == "bruder:2019:evaluating"));
            Assert.IsTrue(items.Any(i => i.Key == "grottel:2015:megamol"));
            Assert.IsTrue(items.Any(i => i.Key == "gumhold:2003:splatting"));
            Assert.IsTrue(items.Any(i => i.Key == "krueger:2003:acceleration"));
            Assert.IsTrue(items.Any(i => i.Key == "heinrich:2009:parco"));
            Assert.IsTrue(items.Any(i => i.Key == "schulz:2016:generative"));
            Assert.IsTrue(items.Any(i => i.Key == "hall:1879:action"));
            Assert.IsTrue(items.Any(i => i.Key == "schneider:2020:pcat"));
            Assert.IsTrue(items.Any(i => i.Key == "ramsden:2006:hall"));
            Assert.IsTrue(items.Any(i => i.Key == "nvidia:nvml"));
            Assert.IsTrue(items.Any(i => i.Key == "amd:adl"));
            Assert.IsTrue(items.Any(i => i.Key == "darus-3044_2022"));
            Assert.IsTrue(items.Any(i => i.Key == "mueller:2022:overwhelming"));
            Assert.IsTrue(items.Any(i => i.Key == "microsoft:emi"));
            Assert.IsTrue(items.Any(i => i.Key == "microsoft:stable"));
            Assert.IsTrue(items.Any(i => i.Key == "ni:lpt"));
            Assert.IsTrue(items.Any(i => i.Key == "intel:rapl"));
            Assert.IsTrue(items.Any(i => i.Key == "rus:rta4004"));
            Assert.IsTrue(items.Any(i => i.Key == "rus:rtb2004"));
            Assert.IsTrue(items.Any(i => i.Key == "ivi:visa"));
            Assert.IsTrue(items.Any(i => i.Key == "intel:atx"));
            Assert.IsTrue(items.Any(i => i.Key == "shannon1949communication"));
            Assert.IsTrue(items.Any(i => i.Key == "khan:2018:rapl"));
            Assert.IsTrue(items.Any(i => i.Key == "haehnel:2012:rapl"));
            Assert.IsTrue(items.Any(i => i.Key == "hackenberg:2013:quantitative"));
            Assert.IsTrue(items.Any(i => i.Key == "lipp:2021:platypus"));
            Assert.IsTrue(items.Any(i => i.Key == "Gralka2019MegaMol"));
            Assert.IsTrue(items.Any(i => i.Key == "visus:pwrowg"));
            Assert.IsTrue(items.Any(i => i.Key == "apache:parquet"));
            Assert.IsTrue(items.Any(i => i.Key == "Parker_OptiX_2010"));
            Assert.IsTrue(items.Any(i => i.Key == "Sinha2022NotAllGPUs"));

            {
                var item = items.Where(i => i.Key == "Grinstein:2002:IVV").Single();
                Assert.AreEqual("IEEE Visualization Course #1 Notes", item["howpublished"]);
            }

            {
                var item = items.Where(i => i.Key == "Isenberg:2017:VMC").Single();
                Assert.AreEqual("vispubdata.org: A Metadata Collection about IEEE Visualization (VIS) Publications", item.Title);
            }

            {
                var item = items.Where(i => i.Key == "Kindlmann:1999:SAG").Single();
                Assert.AreEqual("Semi-Automatic Generation of Transfer Functions for Direct Volume Rendering", item.Title);
            }

            {
                var item = items.Where(i => i.Key == "Lorensen:1987:MCA").Single();
                Assert.AreEqual("Marching Cubes: A High Resolution 3D Surface Construction Algorithm", item.Title);
                Assert.AreEqual("163–169", item.Pages);
            }

            {
                var item = items.Where(i => i.Key == "Nielson:1991:TAD").Single();
                Assert.AreEqual("Proc. Visualization", item.BookTitle);
            }
        }
    }
}
