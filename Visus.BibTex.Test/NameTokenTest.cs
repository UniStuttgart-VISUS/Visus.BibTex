// <copyright file="NameTokenTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests of <see cref="NameToken"/>.
    /// </summary>
    [TestClass]
    public class NameTokenTest {

        [TestMethod]
        public void TestConstruction() {
            {
                var t = new NameToken();
                Assert.AreEqual(NameTokenType.End, t.Type);
                Assert.AreEqual(string.Empty, t.Text.ToString());
                Assert.AreEqual(string.Empty, t.ToString());
            }

            {
                var s = "Hugo";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual(NameTokenType.Literal, t.Type);
                Assert.AreEqual("Hugo", t.Text.ToString());
                Assert.AreEqual("Hugo", t.ToString());
            }

            {
                var s = "hugo";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual(NameTokenType.Literal, t.Type);
                Assert.AreEqual("hugo", t.Text.ToString());
                Assert.AreEqual("hugo", t.ToString());
            }

            {
                var s = "景色";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual(NameTokenType.Literal, t.Type);
                Assert.AreEqual("景色", t.Text.ToString());
                Assert.AreEqual("景色", t.ToString());
            }
        }

        [TestMethod]
        public void TestCapitalised() {
            {
                var t = new NameToken();
                Assert.IsNull(t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }

            {
                var t = new NameToken(NameTokenType.Comma, ",");
                Assert.IsNull(t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }

            {
                var s = "Hugo";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('H', t.FirstLetter);
                Assert.IsTrue(t.IsCapitalised);
            }

            {
                var s = "hugo";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('h', t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }

            {
                var s = "景色";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('景', t.FirstLetter);
                Assert.IsTrue(t.IsCapitalised);
            }

            {
                var s = "{Hugo}";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('H', t.FirstLetter);
                Assert.IsTrue(t.IsCapitalised);
            }

            {
                var s = "{hugo}";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('h', t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }

            {
                var s = "{景色}";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('景', t.FirstLetter);
                Assert.IsTrue(t.IsCapitalised);
            }

            {
                var s = " \t{Hugo}";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('H', t.FirstLetter);
                Assert.IsTrue(t.IsCapitalised);
            }

            {
                var s = " \t{hugo}";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('h', t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }

            {
                var s = " \t{景色}";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('景', t.FirstLetter);
                Assert.IsTrue(t.IsCapitalised);
            }

            {
                var s = "5Hugo";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('H', t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }

            {
                var s = "5hugo";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('h', t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }

            {
                var s = "5景色";
                var t = new NameToken(NameTokenType.Literal, s.AsSpan());
                Assert.AreEqual('景', t.FirstLetter);
                Assert.IsFalse(t.IsCapitalised);
            }
        }
    }
}
