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
        public void TestSingleSurnameFirst() {
            {
                var input = "Ulbricht, Walter";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Ulbricht", names.Single().Surname);
                Assert.AreEqual("Walter", names.Single().ChristianName);
            }

            {
                var input = "Ulbricht, Walter Ernst Paul";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Ulbricht", names.Single().Surname);
                Assert.AreEqual("Walter", names.Single().ChristianName);
                Assert.AreEqual(2, names.Single().MiddleNames.Count());
                Assert.AreEqual("Ernst", names.Single().MiddleNames.First());
                Assert.AreEqual("Paul", names.Single().MiddleNames.Last());
            }

            {
                var input = "Ulbricht, Walter {Ernst Paul}";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Ulbricht", names.Single().Surname);
                Assert.AreEqual("Walter", names.Single().ChristianName);
                Assert.AreEqual(1, names.Single().MiddleNames.Count());
                Assert.AreEqual("{Ernst Paul}", names.Single().MiddleNames.First());
            }

            {
                var input = "von Berlichingen zu Hornberg, Gottfried";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("von Berlichingen zu Hornberg", names.Single().Surname);
                Assert.AreEqual("Gottfried", names.Single().ChristianName);
                Assert.IsFalse(names.Single().MiddleNames.Any());
            }

            {
                var input = "{von Berlichingen zu Hornberg}, Gottfried";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("{von Berlichingen zu Hornberg}", names.Single().Surname);
                Assert.AreEqual("Gottfried", names.Single().ChristianName);
                Assert.IsFalse(names.Single().MiddleNames.Any());
            }

            {
                var input = "Gates, William Henry III";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Gates", names.Single().Surname);
                Assert.AreEqual("William", names.Single().ChristianName);
                Assert.AreEqual(1, names.Single().MiddleNames.Count());
                Assert.AreEqual("Henry", names.Single().MiddleNames.First());
                Assert.AreEqual("III", names.Single().Suffix);
            }

            {
                var input = "Gates, William Henry III.";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Gates", names.Single().Surname);
                Assert.AreEqual("William", names.Single().ChristianName);
                Assert.AreEqual(1, names.Single().MiddleNames.Count());
                Assert.AreEqual("Henry", names.Single().MiddleNames.First());
                Assert.AreEqual("III.", names.Single().Suffix);
            }
        }

        [TestMethod]
        public void TestSingleChristianNameFirst() {
            {
                var input = "Walter Ulbricht";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Ulbricht", names.Single().Surname);
                Assert.AreEqual("Walter", names.Single().ChristianName);
            }

            {
                var input = "Walter Ernst Paul Ulbricht";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Ulbricht", names.Single().Surname);
                Assert.AreEqual("Walter", names.Single().ChristianName);
                Assert.AreEqual(2, names.Single().MiddleNames.Count());
                Assert.AreEqual("Ernst", names.Single().MiddleNames.First());
                Assert.AreEqual("Paul", names.Single().MiddleNames.Last());
            }

            {
                var input = "Walter {Ernst Paul} Ulbricht";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Ulbricht", names.Single().Surname);
                Assert.AreEqual("Walter", names.Single().ChristianName);
                Assert.AreEqual(1, names.Single().MiddleNames.Count());
                Assert.AreEqual("{Ernst Paul}", names.Single().MiddleNames.First());
            }

            {
                var input = "Gottfried von Berlichingen zu Hornberg";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("von Berlichingen zu Hornberg", names.Single().Surname);
                Assert.AreEqual("Gottfried", names.Single().ChristianName);
                Assert.IsFalse(names.Single().MiddleNames.Any());
            }

            {
                var input = "Gottfried {von Berlichingen zu Hornberg}";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("{von Berlichingen zu Hornberg}", names.Single().Surname);
                Assert.AreEqual("Gottfried", names.Single().ChristianName);
                Assert.IsFalse(names.Single().MiddleNames.Any());
            }

            {
                var input = "William Henry Gates III";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Gates", names.Single().Surname);
                Assert.AreEqual("William", names.Single().ChristianName);
                Assert.AreEqual(1, names.Single().MiddleNames.Count());
                Assert.AreEqual("Henry", names.Single().MiddleNames.First());
                Assert.AreEqual("III", names.Single().Suffix);
            }

            {
                var input = "William Henry Gates III.";
                var names = NameParser.ParseList(input).ToList();
                Assert.IsNotNull(names);
                Assert.IsTrue(names.Any());
                Assert.AreEqual("Gates", names.Single().Surname);
                Assert.AreEqual("William", names.Single().ChristianName);
                Assert.AreEqual(1, names.Single().MiddleNames.Count());
                Assert.AreEqual("Henry", names.Single().MiddleNames.First());
                Assert.AreEqual("III.", names.Single().Suffix);
            }
        }
    }
}
