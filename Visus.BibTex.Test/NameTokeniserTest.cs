// <copyright file="Name.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex.Test {

    /// <summary>
    /// Standalone tests for the <see cref="NameTokeniser"/>.
    /// </summary>
    [TestClass]
    public sealed class NameTokeniserTest {

        [TestMethod]
        public void TestChristianNameSurname() {
            var input = "Erich Honecker";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Erich", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Honecker", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }

        [TestMethod]
        public void TestSurnameChristianName() {
            var input = "Honecker, Erich";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Honecker", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Comma, token.Type);
                Assert.AreEqual(",", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Erich", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }

        [TestMethod]
        public void TestBracedOrganisation() {
            var input = "{Visualisierungsinstitut der Universität Stuttgart}";
            var tokeniser = new NameTokeniser(input);

            {
                var token1 = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token1.Type);
                Assert.AreEqual("Visualisierungsinstitut der Universität Stuttgart", token1.ToString());
            }

            {
                var token2 = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token2.Type);
                Assert.AreEqual(string.Empty, token2.ToString());
            }
        }

        [TestMethod]
        public void TestEscapedChristianNameSurname() {
            var input = "Erich \\{Honecker\\}";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Erich", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("\\{Honecker\\}", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }

        [TestMethod]
        public void TestBracedSurname() {
            var input = "{von Berlichingen zu Hornberg}, Gottfried";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("von Berlichingen zu Hornberg", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Comma, token.Type);
                Assert.AreEqual(",", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Gottfried", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }

        [TestMethod]
        public void TestAffixedSurname() {
            var input = "von Berlichingen zu Hornberg, Gottfried";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("von", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Berlichingen", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("zu", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Hornberg", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Comma, token.Type);
                Assert.AreEqual(",", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Gottfried", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }

        [TestMethod]
        public void TestInnerBraces() {
            var input = "{M}{\"u}ller-{L}{\"u}denscheidt";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("{M}{\"u}ller-{L}{\"u}denscheidt", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }

        [TestMethod]
        public void TestNestedBraces() {
            var input = "{{M}{\"u}ller-{L}{\"u}denscheidt}";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("{M}{\"u}ller-{L}{\"u}denscheidt", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }


        [TestMethod]
        public void TestMultipleNames() {
            var input = "{{M}{\"u}ller-{L}{\"u}denscheidt} \t and \t Walter {Ulbricht}; Erich E. P. Honecker";
            var tokeniser = new NameTokeniser(input);

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("{M}{\"u}ller-{L}{\"u}denscheidt", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Separator, token.Type);
                Assert.AreEqual("and", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Walter", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Ulbricht", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Separator, token.Type);
                Assert.AreEqual(";", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Erich", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("E.", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("P.", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.Literal, token.Type);
                Assert.AreEqual("Honecker", token.ToString());
            }

            {
                var token = tokeniser.Next();
                Assert.AreEqual(NameTokenType.End, token.Type);
                Assert.AreEqual(string.Empty, token.ToString());
            }
        }
    }
}
