// <copyright file="WellKnownFields.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System.Linq;


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests for the built-in type and field name constants.
    /// </summary>
    [TestClass]
    public class TypesAndFieldsTest {

        [TestMethod]
        public void TestRequired() {
            // From https://bibtex.eu/types/article/
            var required = WellKnownFields.GetRequired(WellKnownTypes.Article);
            Assert.IsTrue(required.Contains(WellKnownFields.Author));
            Assert.IsTrue(required.Contains(WellKnownFields.Title));
            Assert.IsTrue(required.Contains(WellKnownFields.Journal));
            Assert.IsTrue(required.Contains(WellKnownFields.Year));
        }

        [TestMethod]
        public void TestSupported() {
            // From https://bibtex.eu/types/article/
            var supported = WellKnownFields.GetSupported(WellKnownTypes.Article);
            Assert.IsTrue(supported.Contains(WellKnownFields.Author));
            Assert.IsTrue(supported.Contains(WellKnownFields.Title));
            Assert.IsTrue(supported.Contains(WellKnownFields.Journal));
            Assert.IsTrue(supported.Contains(WellKnownFields.Year));

            Assert.IsTrue(supported.Contains(WellKnownFields.Volume));
            Assert.IsTrue(supported.Contains(WellKnownFields.Number));
            Assert.IsTrue(supported.Contains(WellKnownFields.Pages));
            Assert.IsTrue(supported.Contains(WellKnownFields.Month));
            Assert.IsTrue(supported.Contains(WellKnownFields.Note));
        }
    }
}
