// <copyright file="LatexConverterTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex.Test {

    /// <summary>
    /// Test conversion from LaTex to Unicode.
    /// </summary>
    [TestClass]
    public sealed class LatexConverterTest {

        [TestMethod]
        public void TestDiacritics() {
            Assert.AreEqual("ò", LatexConverter.ConvertFrom(@"\`{o}"));
            Assert.AreEqual("ó", LatexConverter.ConvertFrom(@"\'{o}"));
            Assert.AreEqual("ô", LatexConverter.ConvertFrom(@"\^{o}"));
            Assert.AreEqual("ö", LatexConverter.ConvertFrom(@"\""{o}"));
            Assert.AreEqual("ő", LatexConverter.ConvertFrom(@"\H{o}"));
            Assert.AreEqual("õ", LatexConverter.ConvertFrom(@"\~{o}"));
            Assert.AreEqual("ç", LatexConverter.ConvertFrom(@"\c{c}"));
            Assert.AreEqual("ą", LatexConverter.ConvertFrom(@"\k{a}"));
            Assert.AreEqual("ł", LatexConverter.ConvertFrom(@"\l{}"));
            Assert.AreEqual("ō", LatexConverter.ConvertFrom(@"\={o}"));
            Assert.AreEqual("o̱", LatexConverter.ConvertFrom(@"\b{o}"));
            Assert.AreEqual("ȯ", LatexConverter.ConvertFrom(@"\.{o}"));
            Assert.AreEqual("ụ", LatexConverter.ConvertFrom(@"\d{u}"));
            Assert.AreEqual("å", LatexConverter.ConvertFrom(@"\r{a}"));
            Assert.AreEqual("ŏ", LatexConverter.ConvertFrom(@"\u{o}"));
            Assert.AreEqual("š", LatexConverter.ConvertFrom(@"\v{s}"));
            Assert.AreEqual("o͡o", LatexConverter.ConvertFrom(@"\t{oo}"));
            Assert.AreEqual("ø", LatexConverter.ConvertFrom(@"\o{}"));
            Assert.AreEqual("ı", LatexConverter.ConvertFrom(@"{\i}"));

            Assert.AreEqual("Ò", LatexConverter.ConvertFrom(@"\`{O}"));
            Assert.AreEqual("Ó", LatexConverter.ConvertFrom(@"\'{O}"));
            Assert.AreEqual("Ô", LatexConverter.ConvertFrom(@"\^{O}"));
            Assert.AreEqual("Ö", LatexConverter.ConvertFrom(@"\""{O}"));
            Assert.AreEqual("Ő", LatexConverter.ConvertFrom(@"\H{O}"));
            Assert.AreEqual("Õ", LatexConverter.ConvertFrom(@"\~{O}"));
            Assert.AreEqual("Ç", LatexConverter.ConvertFrom(@"\c{C}"));
            Assert.AreEqual("Ą", LatexConverter.ConvertFrom(@"\k{A}"));
            Assert.AreEqual("Ł", LatexConverter.ConvertFrom(@"\L{}"));
            Assert.AreEqual("Ō", LatexConverter.ConvertFrom(@"\={O}"));
            Assert.AreEqual("O̱", LatexConverter.ConvertFrom(@"\b{O}"));
            Assert.AreEqual("Ȯ", LatexConverter.ConvertFrom(@"\.{O}"));
            Assert.AreEqual("Ụ", LatexConverter.ConvertFrom(@"\d{U}"));
            Assert.AreEqual("Å", LatexConverter.ConvertFrom(@"\r{A}"));
            Assert.AreEqual("Ŏ", LatexConverter.ConvertFrom(@"\u{O}"));
            Assert.AreEqual("Š", LatexConverter.ConvertFrom(@"\v{S}"));
            Assert.AreEqual("O͡O", LatexConverter.ConvertFrom(@"\t{OO}"));
            Assert.AreEqual("Ø", LatexConverter.ConvertFrom(@"\O{}"));

            Assert.AreEqual("ä", LatexConverter.ConvertFrom(@"\""a"));
            Assert.AreEqual("Ä", LatexConverter.ConvertFrom(@"\""A"));
            Assert.AreEqual("ö", LatexConverter.ConvertFrom(@"\""o"));
            Assert.AreEqual("Ö", LatexConverter.ConvertFrom(@"\""O"));
            Assert.AreEqual("ü", LatexConverter.ConvertFrom(@"\""u"));
            Assert.AreEqual("Ü", LatexConverter.ConvertFrom(@"\""U"));
            Assert.AreEqual("ä", LatexConverter.ConvertFrom(@"\""a"));

            Assert.AreEqual("Ä", LatexConverter.ConvertFrom(@"{\""{A}}"));
            Assert.AreEqual("ö", LatexConverter.ConvertFrom(@"{\""{o}}"));
            Assert.AreEqual("Ö", LatexConverter.ConvertFrom(@"{\""{O}}"));
            Assert.AreEqual("ü", LatexConverter.ConvertFrom(@"{\""{u}}"));
            Assert.AreEqual("Ü", LatexConverter.ConvertFrom(@"{\""{U}}"));
        }

        [TestMethod]
        public void TestHyphens() {
            Assert.AreEqual("-", LatexConverter.ConvertFrom("-"));
            Assert.AreEqual("–", LatexConverter.ConvertFrom("--"));
            Assert.AreEqual("—", LatexConverter.ConvertFrom("---"));
            Assert.AreEqual("—-", LatexConverter.ConvertFrom("----"));
        }

        [TestMethod]
        public void TestJabRefNames() {
            // Names from JabRef unit test
            // https://github.com/JabRef/jabref/blob/e0333c44a7bbb3e1b0783e45c218e6cd7a6ff94c/src/test/java/org/jabref/logic/importer/AuthorListParserTest.java
            Assert.AreEqual("Kent-Boswell, E. S.", LatexConverter.ConvertFrom("{K}ent-{B}oswell, E. S."));
            Assert.AreEqual("Nuñez, Jose", LatexConverter.ConvertFrom("Nu{\\~{n}}ez, Jose"));
            Assert.AreEqual("Œrjan Umlauts", LatexConverter.ConvertFrom("{\\OE}rjan Umlauts"));
            Assert.AreEqual("Company Name, LLC", LatexConverter.ConvertFrom("{Company Name, LLC}"));
            Assert.AreEqual("Society of Automotive Engineers", LatexConverter.ConvertFrom("{Society of Automotive Engineers}"));
        }

        [TestMethod]
        public void TestBraced() {
            Assert.AreEqual(@"\""a", LatexConverter.ConvertBracedParts(@"\""a"));
            Assert.AreEqual(@"\""A", LatexConverter.ConvertBracedParts(@"\""A"));
            Assert.AreEqual(@"\""o", LatexConverter.ConvertBracedParts(@"\""o"));
            Assert.AreEqual(@"\""O", LatexConverter.ConvertBracedParts(@"\""O"));
            Assert.AreEqual(@"\""u", LatexConverter.ConvertBracedParts(@"\""u"));
            Assert.AreEqual(@"\""U", LatexConverter.ConvertBracedParts(@"\""U"));

            Assert.AreEqual("ä", LatexConverter.ConvertBracedParts(@"{\""{a}}"));
            Assert.AreEqual("Ä", LatexConverter.ConvertBracedParts(@"{\""{A}}"));
            Assert.AreEqual("ö", LatexConverter.ConvertBracedParts(@"{\""{o}}"));
            Assert.AreEqual("Ö", LatexConverter.ConvertBracedParts(@"{\""{O}}"));
            Assert.AreEqual("ü", LatexConverter.ConvertBracedParts(@"{\""{u}}"));
            Assert.AreEqual("Ü", LatexConverter.ConvertBracedParts(@"{\""{U}}"));

            Assert.AreEqual(@"\""aä", LatexConverter.ConvertBracedParts(@"\""a{\""a}"));
            Assert.AreEqual(@"\""AÄ", LatexConverter.ConvertBracedParts(@"\""A{\""A}"));
            Assert.AreEqual(@"\""oö", LatexConverter.ConvertBracedParts(@"\""o{\""o}"));
            Assert.AreEqual(@"\""OÖ", LatexConverter.ConvertBracedParts(@"\""O{\""O}"));
            Assert.AreEqual(@"\""uü", LatexConverter.ConvertBracedParts(@"\""u{\""u}"));
            Assert.AreEqual(@"\""UÜ", LatexConverter.ConvertBracedParts(@"\""U{\""U}"));
        }
    }
}
