// <copyright file="StringTests.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests our extension methods for strings.
    /// </summary>
    [TestClass]
    public class StringTests {

        [TestMethod]
        public void TestToInitial() {
            Assert.IsNotNull(((string?) null).ToInitial());
            Assert.AreEqual(string.Empty, ((string?) null).ToInitial());
            Assert.IsNotNull(string.Empty.ToInitial());
            Assert.AreEqual(string.Empty, string.Empty.ToInitial());
            Assert.AreEqual("x.", "x".ToInitial());
            Assert.AreEqual("X.", "X".ToInitial());
            Assert.AreEqual("x.", "xy".ToInitial());
            Assert.AreEqual("X.", "Xy".ToInitial());
        }

        [TestMethod]
        public void TestToInitials() {
            Assert.IsNotNull(((IEnumerable<string>?) null).ToInitials());
            Assert.AreEqual(string.Empty, ((IEnumerable<string>?) null).ToInitials());
            Assert.IsNotNull((new string[0]).ToInitials());
            Assert.AreEqual(string.Empty, (new string[0]).ToInitials());
            Assert.IsNotNull((new[] { string.Empty}).ToInitials());
            Assert.AreEqual(string.Empty, (new[] { string.Empty }).ToInitials());
            Assert.AreEqual("x.", (new[] { "x" }).ToInitials());
            Assert.AreEqual("X.", (new[] { "X" }).ToInitials());
            Assert.AreEqual("X.", (new[] { "Xy" }).ToInitials());
            Assert.AreEqual("X.", (new[] { "Xy", null }).ToInitials());
            Assert.AreEqual("X.", (new[] { "Xy", string.Empty }).ToInitials());
            Assert.AreEqual("X. Y.", (new[] { "Xy", string.Empty, null, "Yz" }).ToInitials());
        }
    }
}
