// <copyright file="LatexTokeniserTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests LaTex tokenisation.
    /// </summary>
    [TestClass]
    public class LatexTokeniserTest {

        [TestMethod]
        public void TestSingleToken() {
            {
                var input = "\\";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.Backslash, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "{";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.BraceLeft, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "}";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.BraceRight, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "$";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.Dollar, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "-";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.Hyphen, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "b";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.Literal, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "bla";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.Literal, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "\n";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = " ";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "\t";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = "\r";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }

            {
                var input = " \r\t\r ";
                var tokeniser = new LatexTokeniser(input);
                Assert.AreEqual(LatexTokenType.WhiteSpace, tokeniser.Next().Type);
                Assert.AreEqual(LatexTokenType.End, tokeniser.Next().Type);
            }
        }
    }
}
