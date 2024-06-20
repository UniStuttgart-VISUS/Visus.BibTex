// <copyright file="LaTexTokeniserTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests LaTex tokenisation.
    /// </summary>
    [TestClass]
    public class LaTexTokeniserTest {

        [TestMethod]
        public void TestSingleToken() {
            {
                var input = "\\";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.Backslash, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "{";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.BraceLeft, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "}";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.BraceRight, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "$";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.Dollar, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "b";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.Literal, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "bla";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.Literal, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "\n";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.NewLine, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "\"";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.Quote, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = " ";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "\t";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "\r";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = " \r\t\r ";
                var tokeniser = new LaTexTokeniser(input);
                Assert.AreEqual(LaTexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LaTexTokenType.End, tokeniser.Next().Type);
            }
        }
    }
}
