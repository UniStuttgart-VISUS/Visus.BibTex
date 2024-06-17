// <copyright file="Name.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


using System;

namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests the demo code in the README on GitHub.
    /// </summary>
    [TestClass]
    public class Demos {

        [TestMethod]
        public void NameObject() {
            {
                var author = new Name("Ulbricht", "Walter");
                Console.WriteLine(author.Surname);
                Console.WriteLine(author.ChristianName);
                Assert.AreEqual("Ulbricht", author.Surname);
                Assert.AreEqual("Walter", author.ChristianName);
            }

            {
                var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
                var (surname, christianName, middleNames) = author;
                Console.WriteLine(author.Surname);
                Console.WriteLine(author.ChristianName);
                Console.WriteLine(string.Join(", ", author.MiddleNames));
                Assert.AreEqual("Ulbricht", author.Surname);
                Assert.AreEqual("Walter", author.ChristianName);
                Assert.AreEqual("Ernst Paul", string.Join(" ", author.MiddleNames));
            }

            {
                var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
                Console.WriteLine(author.ToString(NameFormats.SurnameChristianNameMiddleNames));
                Assert.AreEqual("Ulbricht, Walter Ernst Paul", author.ToString(NameFormats.SurnameChristianNameMiddleNames));
            }

            {
                var author = new Name("Ulbricht", "Walter", "Ernst", "Paul");
                Console.WriteLine(author.ToString("SCm"));
                Assert.AreEqual("Ulbricht, Walter E. P.", author.ToString("SCm"));
            }
        }
    }
}
