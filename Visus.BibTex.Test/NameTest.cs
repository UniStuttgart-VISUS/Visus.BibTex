// <copyright file="NameTest.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Linq;


namespace Visus.BibTex.Test {

    /// <summary>
    /// Tests for the <see cref="Name"/> abstraction.
    /// </summary>
    [TestClass]
    public sealed class NameTest {

        [TestMethod]
        public void TestCtor() {
            {
                var name = new Name("Honecker");
                Assert.AreEqual("Honecker", name.Surname);
                Assert.IsNull(name.ChristianName);
                Assert.IsNotNull(name.MiddleNames);
                Assert.IsFalse(name.MiddleNames.Any());
                Assert.IsNull(name.Suffix);
            }

            {
                var name = new Name("Honecker", "Erich");
                Assert.AreEqual("Honecker", name.Surname);
                Assert.AreEqual("Erich", name.ChristianName);
                Assert.IsNotNull(name.MiddleNames);
                Assert.IsFalse(name.MiddleNames.Any());
                Assert.IsNull(name.Suffix);
            }

            {
                var name = new Name("Honecker", "Erich", "Ernst", "Paul");
                Assert.AreEqual("Honecker", name.Surname);
                Assert.AreEqual("Erich", name.ChristianName);
                Assert.IsNotNull(name.MiddleNames);
                Assert.AreEqual(2, name.MiddleNames.Count());
                Assert.AreEqual("Ernst", name.MiddleNames.First());
                Assert.AreEqual("Paul", name.MiddleNames.Last());
                Assert.IsNull(name.Suffix);
            }

            {
                var name = new Name("Honecker", "Erich", ["Ernst", "Paul"]);
                Assert.AreEqual("Honecker", name.Surname);
                Assert.AreEqual("Erich", name.ChristianName);
                Assert.IsNotNull(name.MiddleNames);
                Assert.AreEqual(2, name.MiddleNames.Count());
                Assert.AreEqual("Ernst", name.MiddleNames.First());
                Assert.AreEqual("Paul", name.MiddleNames.Last());
                Assert.IsNull(name.Suffix);
            }

            {
                var name = new Name("Honecker", "Erich", ["Ernst", "Paul"], "d. Ä.");
                Assert.AreEqual("Honecker", name.Surname);
                Assert.AreEqual("Erich", name.ChristianName);
                Assert.IsNotNull(name.MiddleNames);
                Assert.AreEqual(2, name.MiddleNames.Count());
                Assert.AreEqual("Ernst", name.MiddleNames.First());
                Assert.AreEqual("Paul", name.MiddleNames.Last());
                Assert.AreEqual("d. Ä.", name.Suffix);
            }

            Assert.ThrowsException<ArgumentNullException>(() => {
                var name = new Name(null!);
            });
        }

        [TestMethod]
        public void TestDeconstruct() {
            {
                var name = new Name("Honecker");

                {
                    var (surname, christianName) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.IsNull(christianName);
                }

                {
                    var (surname, christianName, middleNames) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.IsNull(christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.IsFalse(middleNames.Any());
                }

                {
                    var (surname, christianName, middleNames, suffix) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.IsNull(christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.IsFalse(middleNames.Any());
                    Assert.IsNull(suffix);
                }
            }

            {
                var name = new Name("Honecker", "Erich");

                {
                    var (surname, christianName) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                }

                {
                    var (surname, christianName, middleNames) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.IsFalse(middleNames.Any());
                }

                {
                    var (surname, christianName, middleNames, suffix) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.IsFalse(middleNames.Any());
                    Assert.IsNull(suffix);
                }
            }

            {
                var name = new Name("Honecker", "Erich", "Ernst", "Paul");

                {
                    var (surname, christianName) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                }

                {
                    var (surname, christianName, middleNames) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.AreEqual(2, middleNames.Count());
                    Assert.AreEqual("Ernst", middleNames.First());
                    Assert.AreEqual("Paul", middleNames.Last());
                }

                {
                    var (surname, christianName, middleNames, suffix) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.AreEqual(2, middleNames.Count());
                    Assert.AreEqual("Ernst", middleNames.First());
                    Assert.AreEqual("Paul", middleNames.Last());
                    Assert.IsNull(suffix);
                }
            }

            {
                var name = new Name("Honecker", "Erich", ["Ernst", "Paul"], "d. Ä.");

                {
                    var (surname, christianName) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                }

                {
                    var (surname, christianName, middleNames) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.AreEqual(2, middleNames.Count());
                    Assert.AreEqual("Ernst", middleNames.First());
                    Assert.AreEqual("Paul", middleNames.Last());
                }

                {
                    var (surname, christianName, middleNames, suffix) = name;
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Honecker", surname);
                    Assert.AreEqual("Erich", christianName);
                    Assert.IsNotNull(middleNames);
                    Assert.AreEqual(2, middleNames.Count());
                    Assert.AreEqual("Ernst", middleNames.First());
                    Assert.AreEqual("Paul", middleNames.Last());
                    Assert.AreEqual("d. Ä.", suffix);
                }
            }
        }

        [TestMethod]
        public void TestToString() {
            {
                var name = new Name("Honecker");
                Assert.AreEqual("Honecker", name.ToString("S", null));
                Assert.AreEqual("H.", name.ToString("s", null));
                Assert.AreEqual("Honecker", name.ToString("SC", null));
                Assert.AreEqual("Honecker", name.ToString("SCM", null));
                Assert.AreEqual("Honecker", name.ToString("SCm", null));
                Assert.AreEqual("Honecker", name.ToString("Scm", null));
                Assert.AreEqual("Honecker", name.ToString("CMS", null));
                Assert.AreEqual("Honecker", name.ToString("CmS", null));
                Assert.AreEqual("Honecker", name.ToString("cmS", null));
                Assert.AreEqual("H.", name.ToString("cms", null));
                Assert.AreEqual("Honecker", name.ToString("CMSX", null));
            }

            {
                var name = new Name("Honecker", "Erich");
                Assert.AreEqual("Honecker", name.ToString("S", null));
                Assert.AreEqual("H.", name.ToString("s", null));
                Assert.AreEqual("Honecker, Erich", name.ToString("SC", null));
                Assert.AreEqual("Honecker, Erich", name.ToString("SCM", null));
                Assert.AreEqual("Honecker, Erich", name.ToString("SCm", null));
                Assert.AreEqual("Honecker, E.", name.ToString("Scm", null));
                Assert.AreEqual("Erich Honecker", name.ToString("CMS", null));
                Assert.AreEqual("Erich Honecker", name.ToString("CmS", null));
                Assert.AreEqual("E. Honecker", name.ToString("cmS", null));
                Assert.AreEqual("E. H.", name.ToString("cms", null));
                Assert.AreEqual("Erich Honecker", name.ToString("CMSX", null));
            }

            {
                var name = new Name("Honecker", "Erich", "Ernst", "Paul");
                Assert.AreEqual("Honecker", name.ToString("S", null));
                Assert.AreEqual("H.", name.ToString("s", null));
                Assert.AreEqual("Honecker, Erich", name.ToString("SC", null));
                Assert.AreEqual("Honecker, Erich Ernst Paul", name.ToString("SCM", null));
                Assert.AreEqual("Honecker, Erich E. P.", name.ToString("SCm", null));
                Assert.AreEqual("Honecker, E. E. P.", name.ToString("Scm", null));
                Assert.AreEqual("Erich Ernst Paul Honecker", name.ToString("CMS", null));
                Assert.AreEqual("Erich E. P. Honecker", name.ToString("CmS", null));
                Assert.AreEqual("E. E. P. Honecker", name.ToString("cmS", null));
                Assert.AreEqual("E. E. P. H.", name.ToString("cms", null));
                Assert.AreEqual("Erich Ernst Paul Honecker", name.ToString("CMSX", null));
            }

            {
                var name = new Name("Honecker", "Erich", ["Ernst", "Paul"], "d. Ä.");
                Assert.AreEqual("Honecker", name.ToString("S", null));
                Assert.AreEqual("H.", name.ToString("s", null));
                Assert.AreEqual("Honecker, Erich", name.ToString("SC", null));
                Assert.AreEqual("Honecker, Erich Ernst Paul", name.ToString("SCM", null));
                Assert.AreEqual("Honecker, Erich E. P.", name.ToString("SCm", null));
                Assert.AreEqual("Honecker, E. E. P.", name.ToString("Scm", null));
                Assert.AreEqual("Erich Ernst Paul Honecker", name.ToString("CMS", null));
                Assert.AreEqual("Erich E. P. Honecker", name.ToString("CmS", null));
                Assert.AreEqual("E. E. P. Honecker", name.ToString("cmS", null));
                Assert.AreEqual("E. E. P. H.", name.ToString("cms", null));
                Assert.AreEqual("Erich Ernst Paul Honecker d. Ä.", name.ToString("CMSX", null));
            }
        }

    }
}
