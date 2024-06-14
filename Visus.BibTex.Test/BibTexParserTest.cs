// <copyright file="BibTexParserTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

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
            var bibtex = "@article{hugo, year=2024}";
            var items = BibTexParser<BibItem>.Parse(new StringReader(bibtex));
            items.ToList();
        }
    }
}