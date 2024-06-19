// <copyright file="BibItemTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Linq;


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests for the default <see cref="BibItem"/>.
    /// </summary>
    [TestClass]
    public sealed class BibItemTest {

        [TestMethod]
        public void TestConstruction() {
            {
                var item = new BibItem("article", "mueller:2024:test");
                Assert.AreEqual("article", item.EntryType);
                Assert.AreEqual("mueller:2024:test", item.Key);
            }

            Assert.ThrowsException<ArgumentNullException>(() => {
                var item = new BibItem(null!, "mueller:2024:test");
            });

            Assert.ThrowsException<ArgumentNullException>(() => {
                var item = new BibItem("article", null!);
            });
        }

        [TestMethod]
        public void TestFields() {
            var item = new BibItem("article", "mueller:2024:test");

            Assert.IsFalse(item.ContainsField(WellKnownFields.Address));
            item[WellKnownFields.Address] = WellKnownFields.Address;
            Assert.AreEqual(WellKnownFields.Address, item[WellKnownFields.Address]);
            Assert.AreEqual(WellKnownFields.Address, item.Address);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Address));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Address));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Annote));
            item[WellKnownFields.Annote] = WellKnownFields.Annote;
            Assert.AreEqual(WellKnownFields.Annote, item[WellKnownFields.Annote]);
            Assert.AreEqual(WellKnownFields.Annote, item.Annote);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Annote));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Annote));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Author));
            item[WellKnownFields.Author] = new[] { new Name(WellKnownFields.Author) };
            Assert.IsNotNull(item.Author);
            Assert.AreEqual(1, item.Author.Count());
            Assert.AreEqual(WellKnownFields.Author, item.Author.First().ToString());
            Assert.IsTrue(item.ContainsField(WellKnownFields.Author));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Author));

            Assert.IsFalse(item.ContainsField(WellKnownFields.BookTitle));
            item[WellKnownFields.BookTitle] = WellKnownFields.BookTitle;
            Assert.AreEqual(WellKnownFields.BookTitle, item[WellKnownFields.BookTitle]);
            Assert.AreEqual(WellKnownFields.BookTitle, item.BookTitle);
            Assert.IsTrue(item.ContainsField(WellKnownFields.BookTitle));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.BookTitle));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Chapter));
            item[WellKnownFields.Chapter] = WellKnownFields.Chapter;
            Assert.AreEqual(WellKnownFields.Chapter, item[WellKnownFields.Chapter]);
            Assert.AreEqual(WellKnownFields.Chapter, item.Chapter);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Chapter));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Chapter));

            Assert.IsFalse(item.ContainsField(WellKnownFields.CrossReference));
            item[WellKnownFields.CrossReference] = WellKnownFields.CrossReference;
            Assert.AreEqual(WellKnownFields.CrossReference, item[WellKnownFields.CrossReference]);
            Assert.AreEqual(WellKnownFields.CrossReference, item.CrossReference);
            Assert.IsTrue(item.ContainsField(WellKnownFields.CrossReference));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.CrossReference));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Doi));
            item[WellKnownFields.Doi] = WellKnownFields.Doi;
            Assert.AreEqual(WellKnownFields.Doi, item[WellKnownFields.Doi]);
            Assert.AreEqual(WellKnownFields.Doi, item.Doi);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Doi));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Doi));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Edition));
            item[WellKnownFields.Edition] = WellKnownFields.Edition;
            Assert.AreEqual(WellKnownFields.Edition, item[WellKnownFields.Edition]);
            Assert.AreEqual(WellKnownFields.Edition, item.Edition);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Edition));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Edition));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Editor));
            item[WellKnownFields.Editor] = new[] { new Name(WellKnownFields.Editor) };
            Assert.IsNotNull(item.Editor);
            Assert.AreEqual(1, item.Editor.Count());
            Assert.AreEqual(WellKnownFields.Editor, item.Editor.First().ToString());
            Assert.IsTrue(item.ContainsField(WellKnownFields.Editor));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Editor));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Email));
            item[WellKnownFields.Email] = WellKnownFields.Email;
            Assert.AreEqual(WellKnownFields.Email, item[WellKnownFields.Email]);
            Assert.AreEqual(WellKnownFields.Email, item.Email);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Email));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Email));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Institution));
            item[WellKnownFields.Institution] = WellKnownFields.Institution;
            Assert.AreEqual(WellKnownFields.Institution, item[WellKnownFields.Institution]);
            Assert.AreEqual(WellKnownFields.Institution, item.Institution);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Institution));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Institution));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Journal));
            item[WellKnownFields.Journal] = WellKnownFields.Journal;
            Assert.AreEqual(WellKnownFields.Journal, item[WellKnownFields.Journal]);
            Assert.AreEqual(WellKnownFields.Journal, item.Journal);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Journal));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Journal));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Month));
            item[WellKnownFields.Month] = WellKnownFields.Month;
            Assert.AreEqual(WellKnownFields.Month, item[WellKnownFields.Month]);
            Assert.AreEqual(WellKnownFields.Month, item.Month);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Month));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Month));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Note));
            item[WellKnownFields.Note] = WellKnownFields.Note;
            Assert.AreEqual(WellKnownFields.Note, item[WellKnownFields.Note]);
            Assert.AreEqual(WellKnownFields.Note, item.Note);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Note));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Note));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Number));
            item[WellKnownFields.Number] = WellKnownFields.Number;
            Assert.AreEqual(WellKnownFields.Number, item[WellKnownFields.Number]);
            Assert.AreEqual(WellKnownFields.Number, item.Number);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Number));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Number));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Organisation));
            item[WellKnownFields.Organisation] = WellKnownFields.Organisation;
            Assert.AreEqual(WellKnownFields.Organisation, item[WellKnownFields.Organisation]);
            Assert.AreEqual(WellKnownFields.Organisation, item.Organisation);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Organisation));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Organisation));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Pages));
            item[WellKnownFields.Pages] = WellKnownFields.Pages;
            Assert.AreEqual(WellKnownFields.Pages, item[WellKnownFields.Pages]);
            Assert.AreEqual(WellKnownFields.Pages, item.Pages);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Pages));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Pages));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Publisher));
            item[WellKnownFields.Publisher] = WellKnownFields.Publisher;
            Assert.AreEqual(WellKnownFields.Publisher, item[WellKnownFields.Publisher]);
            Assert.AreEqual(WellKnownFields.Publisher, item.Publisher);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Publisher));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Publisher));

            Assert.IsFalse(item.ContainsField(WellKnownFields.School));
            item[WellKnownFields.School] = WellKnownFields.School;
            Assert.AreEqual(WellKnownFields.School, item[WellKnownFields.School]);
            Assert.AreEqual(WellKnownFields.School, item.School);
            Assert.IsTrue(item.ContainsField(WellKnownFields.School));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.School));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Series));
            item[WellKnownFields.Series] = WellKnownFields.Series;
            Assert.AreEqual(WellKnownFields.Series, item[WellKnownFields.Series]);
            Assert.AreEqual(WellKnownFields.Series, item.Series);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Series));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Series));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Title));
            item[WellKnownFields.Title] = WellKnownFields.Title;
            Assert.AreEqual(WellKnownFields.Title, item[WellKnownFields.Title]);
            Assert.AreEqual(WellKnownFields.Title, item.Title);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Title));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Title));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Volume));
            item[WellKnownFields.Volume] = WellKnownFields.Volume;
            Assert.AreEqual(WellKnownFields.Volume, item[WellKnownFields.Volume]);
            Assert.AreEqual(WellKnownFields.Volume, item.Volume);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Volume));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Volume));

            Assert.IsFalse(item.ContainsField(WellKnownFields.Year));
            item[WellKnownFields.Year] = WellKnownFields.Year;
            Assert.AreEqual(WellKnownFields.Year, item[WellKnownFields.Year]);
            Assert.AreEqual(WellKnownFields.Year, item.Year);
            Assert.IsTrue(item.ContainsField(WellKnownFields.Year));
            Assert.IsTrue(item.Any(f => f.Key == WellKnownFields.Year));
        }

        [TestMethod]
        public void TestFormat() {
            var item = new BibItem(WellKnownTypes.InProceedings, "mueller:2022:power");
            item.Author = [ new Name("Müller", "Christoph"), new Name("Heinemann", "Moritz"), new Name("Weiskopf", "Daniel"), new Name("Ertl", "Thomas") ];
            item.Title = "Power Overwhelming: Quantifying the Energy Cost of Visualisation";
            item.BookTitle = "Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)";
            item.Month = "October";
            item.Year = "2022";
            item.Doi = "10.1109/BELIV57783.2022.00009";
            item.Pages = "38-46";

            {
                var formatted = item.ToString("c.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual("@inproceedings{mueller:2022:power,author={Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas},booktitle={Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)},doi={10.1109/BELIV57783.2022.00009},month={October},pages={38-46},title={Power Overwhelming: Quantifying the Energy Cost of Visualisation},year={2022}}", formatted);
            }

            {
                var formatted = item.ToString("C.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual("@inproceedings{mueller:2022:power, author = {Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas}, booktitle = {Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)}, doi = {10.1109/BELIV57783.2022.00009}, month = {October}, pages = {38-46}, title = {Power Overwhelming: Quantifying the Energy Cost of Visualisation}, year = {2022}}", formatted);
            }

            {
                var formatted = item.ToString("Cq.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual("@inproceedings{mueller:2022:power, author = \"Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas\", booktitle = \"Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)\", doi = \"10.1109/BELIV57783.2022.00009\", month = \"October\", pages = \"38-46\", title = \"Power Overwhelming: Quantifying the Energy Cost of Visualisation\", year = \"2022\"}", formatted);
            }

            {
                var formatted = item.ToString("l.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual($"@inproceedings{{mueller:2022:power,{Environment.NewLine}author = {{Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas}},{Environment.NewLine}booktitle = {{Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)}},{Environment.NewLine}doi = {{10.1109/BELIV57783.2022.00009}},{Environment.NewLine}month = {{October}},{Environment.NewLine}pages = {{38-46}},{Environment.NewLine}title = {{Power Overwhelming: Quantifying the Energy Cost of Visualisation}},{Environment.NewLine}year = {{2022}}{Environment.NewLine}}}", formatted);
            }

            {
                var formatted = item.ToString("ql.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual($"@inproceedings{{mueller:2022:power,{Environment.NewLine}author = \"Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas\",{Environment.NewLine}booktitle = \"Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)\",{Environment.NewLine}doi = \"10.1109/BELIV57783.2022.00009\",{Environment.NewLine}month = \"October\",{Environment.NewLine}pages = \"38-46\",{Environment.NewLine}title = \"Power Overwhelming: Quantifying the Energy Cost of Visualisation\",{Environment.NewLine}year = \"2022\"{Environment.NewLine}}}", formatted);
            }

            {
                var formatted = item.ToString("s1q.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual($"@inproceedings{{mueller:2022:power,{Environment.NewLine} author = \"Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas\",{Environment.NewLine} booktitle = \"Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)\",{Environment.NewLine} doi = \"10.1109/BELIV57783.2022.00009\",{Environment.NewLine} month = \"October\",{Environment.NewLine} pages = \"38-46\",{Environment.NewLine} title = \"Power Overwhelming: Quantifying the Energy Cost of Visualisation\",{Environment.NewLine} year = \"2022\"{Environment.NewLine}}}", formatted);
            }

            {
                var formatted = item.ToString("s2q.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual($"@inproceedings{{mueller:2022:power,{Environment.NewLine}  author = \"Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas\",{Environment.NewLine}  booktitle = \"Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)\",{Environment.NewLine}  doi = \"10.1109/BELIV57783.2022.00009\",{Environment.NewLine}  month = \"October\",{Environment.NewLine}  pages = \"38-46\",{Environment.NewLine}  title = \"Power Overwhelming: Quantifying the Energy Cost of Visualisation\",{Environment.NewLine}  year = \"2022\"{Environment.NewLine}}}", formatted);
            }

            {
                var formatted = item.ToString("s4q.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual($"@inproceedings{{mueller:2022:power,{Environment.NewLine}    author = \"Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas\",{Environment.NewLine}    booktitle = \"Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)\",{Environment.NewLine}    doi = \"10.1109/BELIV57783.2022.00009\",{Environment.NewLine}    month = \"October\",{Environment.NewLine}    pages = \"38-46\",{Environment.NewLine}    title = \"Power Overwhelming: Quantifying the Energy Cost of Visualisation\",{Environment.NewLine}    year = \"2022\"{Environment.NewLine}}}", formatted);
            }

            {
                var formatted = item.ToString("tq.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual($"@inproceedings{{mueller:2022:power,{Environment.NewLine}\tauthor = \"Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas\",{Environment.NewLine}\tbooktitle = \"Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)\",{Environment.NewLine}\tdoi = \"10.1109/BELIV57783.2022.00009\",{Environment.NewLine}\tmonth = \"October\",{Environment.NewLine}\tpages = \"38-46\",{Environment.NewLine}\ttitle = \"Power Overwhelming: Quantifying the Energy Cost of Visualisation\",{Environment.NewLine}\tyear = \"2022\"{Environment.NewLine}}}", formatted);
            }

            {
                var formatted = item.ToString(".SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual(item.ToString("s4.SC"), formatted);
            }

            {
                var formatted = item.ToString("s0q.SC");
                Assert.IsNotNull(formatted);
                Assert.AreEqual(item.ToString("s1q.SC"), formatted);
            }
        }
    }
}
