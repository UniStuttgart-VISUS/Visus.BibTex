// <copyright file="NameParserTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


using System.Linq;

namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests the <see cref="NameParser">.
    /// </summary>
    [TestClass]
    public sealed class NameParserTest {

        [TestMethod]
        public void TestParseEmptyList() {
            var input = "";
            var names = NameParser.ParseList(input).ToList();
            Assert.IsNotNull(names);
            Assert.IsFalse(names.Any());
        }

        [TestMethod]
        public void TestParseSingle() {
            var input = "Walter Ulbricht";
            var names = NameParser.ParseList(input).ToList();
            Assert.IsNotNull(names);
            Assert.IsTrue(names.Any());
        }
    }
}
