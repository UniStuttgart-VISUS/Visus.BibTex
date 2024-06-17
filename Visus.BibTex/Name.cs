// <copyright file="Name.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Visus.BibTex {

    /// <summary>
    /// Represents a structured name of an <see cref="BibItem.Author"/> or
    /// <see cref="BibItem.Editor"/>.
    /// </summary>
    public sealed class Name : IFormattable {

        #region Public class methods
        /// <summary>
        /// Parse a name <see cref="ReadOnlySpan{char}"/> into a list of
        /// <see cref="Name"/>s.
        /// </summary>
        /// <param name="name">The string to be parsed.</param>
        /// <returns>The names represented by the input string.</returns>
        public static IEnumerable<Name> Parse(ReadOnlySpan<char> name) {
            NameTokeniser tokeniser = new(name);
            List<Name> retval = new();

            var commaIsSeparator = false;
            var haveSeparator = false;
            var literalSequence = 0;            // # of subsequent literals.
            var surname = new StringBuilder();

            while (true) {
                var token = tokeniser.Next();

                switch (token.Type) {
                    case NameTokenType.Comma:
                        literalSequence = 0;
                        break;

                    case NameTokenType.End:
                        return retval;

                    case NameTokenType.Literal:
                        if (Affixes.IsMatch(token.Text)) {
                            surname.Append(token.Text);

                        } else if (Suffixes.IsMatch(token.Text)) {

                        }
                        break;

                    case NameTokenType.Separator:
                        haveSeparator = true;
                        literalSequence = 0;
                        break;

                    default:
                        throw new NotImplementedException("An unexpected token "
                            + "was encountered while parsing names. This is "
                            + "a bug in Name.Parse(), which was not updated "
                            + "to account for new tokens produced by the "
                            + "tokeniser.");
                }
            }
        }
        #endregion

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="surname">The surname.</param>
        /// <param name="christianName">An optional christian name.</param>
        /// <param name="middleNames">The optional list of middle names.
        /// </param>
        /// <param name="suffix">The optional name suffix like &quot;jr.&quot;.
        /// </param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="surname"/> is <c>null</c>.</exception>
        public Name(string surname,
                string? christianName = null,
                IEnumerable<string>? middleNames = null,
                string? suffix = null) {
            this._middleNames = (middleNames != null)
                ? middleNames.Where(n => !string.IsNullOrWhiteSpace(n))
                : Enumerable.Empty<string>();
            this._surname = surname
                ?? throw new ArgumentNullException(nameof(surname));

            this.ChristianName = christianName;
            this.Suffix = suffix;
        }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="surname">The surname.</param>
        /// <param name="christianName">An optional christian name.</param>
        /// <param name="middleNames">The optional list of middle names.
        /// </param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="surname"/> is <c>null</c>.</exception>
        public Name(string surname, string christianName,
                params string[] middleNames)
            : this(surname, christianName, middleNames, null) { }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the Christian name.
        /// </summary>
        public string? ChristianName {
            get;
            set;
        }

        /// <summary>
        /// Gets all Christian names.
        /// </summary>
        /// <remarks>
        /// Note that invalid names (<c>null</c> or empty ones) will not be
        /// emitted by this property.
        /// </remarks>
        public IEnumerable<string> ChristianNames {
            get {
                if (!string.IsNullOrWhiteSpace(this.ChristianName)) {
                    yield return this.ChristianName;
                }

                foreach (var n in this.MiddleNames) {
                    if (!string.IsNullOrWhiteSpace(n)) {
                        yield return n;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the christian name.
        /// </summary>
        public string? GivenName {
            get => this.ChristianName;
            set => this.ChristianName = value;
        }

        /// <summary>
        /// Gets all Christian names.
        /// </summary>
        public IEnumerable<string> GivenNames => this.ChristianNames;

        /// <summary>
        /// Gets or sets the middle names.
        /// </summary>
        public IEnumerable<string> MiddleNames {
            get => this._middleNames;
            set => this._middleNames = value ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets or sets any name suffix like &quot;jr.&quot;.
        /// </summary>
        public string? Suffix {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        public string Surname {
            get => this._surname;
            set => this._surname = value
                ?? throw new ArgumentNullException(nameof(value));
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        /// <exception cref="NotImplementedException"></exception>
        public string ToString(string? format,
                IFormatProvider? formatProvider) {
            Debug.Assert(NameFormats.AbbreviatedSurname.Length == 1);
            Debug.Assert(NameFormats.ChristianInital.Length == 1);
            Debug.Assert(NameFormats.ChristianName.Length == 1);
            Debug.Assert(NameFormats.MiddleInitials.Length == 1);
            Debug.Assert(NameFormats.MiddleNames.Length == 1);
            Debug.Assert(NameFormats.Suffix.Length == 1);
            Debug.Assert(NameFormats.Surname.Length == 1);

            if (string.IsNullOrWhiteSpace(format)) {
                // Coerce 'format' to the default format if not specified.
                format = NameFormats.SurnameChristianNameMiddleNamesSuffix;
            }

            var commaEmitted = false;
            var retval = new StringBuilder();
            var surnameEmitted = false;

            for (int i = 0; i < format.Length; ++i) {
                var initial = char.IsLower(format[i]);

                if (format.IsNamePartAt(i, NameFormats.ChristianName)) {
                    if (!string.IsNullOrWhiteSpace(this.ChristianName)) {
                        if (surnameEmitted && ! commaEmitted) {
                            commaEmitted = true;
                            retval.Append(',');
                        }

                        retval.AppendSpaceIfNotEmpty();
                        retval.Append(this.ChristianName.ToInitial(initial));
                    }

                } else if (format.IsNamePartAt(i, NameFormats.MiddleNames)) {
                    var names = from n in this.MiddleNames
                                where !string.IsNullOrWhiteSpace(n)
                                select n;

                    if (names.Any()) {
                        if (surnameEmitted && !commaEmitted) {
                            commaEmitted = true;
                            retval.Append(',');
                        }

                        if (initial) {
                            retval.AppendSpaceIfNotEmpty();
                            retval.Append(names.ToInitials());

                        } else {
                            foreach (var n in names) {
                                retval.AppendSpaceIfNotEmpty();
                                retval.Append(n);
                            }
                        }
                    }

                } else if (format.IsNamePartAt(i, NameFormats.Surname)) {
                    if (!string.IsNullOrWhiteSpace(this.Surname)) {
                        surnameEmitted = true;
                        retval.AppendSpaceIfNotEmpty();
                        retval.Append(this.Surname.ToInitial(initial));
                    }

                } else if (format.IsNamePartAt(i, NameFormats.Suffix)) {
                    if (!string.IsNullOrWhiteSpace(this.Suffix)) {
                        retval.AppendSpaceIfNotEmpty();
                        retval.Append(this.Suffix);
                    }
                }
            }

            return retval.ToString();
        }

        /// <inheritdoc />
        public override string ToString() => this.ToString(null, null);
        #endregion

        #region Public deconstructors
        /// <summary>
        /// Deconstructs the name into a tuple of <see cref="Surname"/>
        /// and <see cref="ChristianName"/>.
        /// </summary>
        /// <param name="surname">Receives the surname.</param>
        /// <param name="christianName">Receives, if any, the christian name.
        /// </param>
        public void Deconstruct(out string surname, out string? christianName) {
            surname = this._surname;
            christianName = this.ChristianName;
        }

        /// <summary>
        /// Deconstructs the name into a tuple of <see cref="Surname"/>,
        /// <see cref="ChristianName"/> and <see cref="MiddleNames"/>.
        /// </summary>
        /// <param name="surname">Receives the surname.</param>
        /// <param name="christianName">Receives, if any, the christian name.
        /// </param>
        /// <param name="middleNames">Receives the middle names.</param>
        public void Deconstruct(out string surname,
                out string? christianName, 
                out IEnumerable<string> middleNames) {
            surname = this._surname;
            christianName = this.ChristianName;
            middleNames = this.MiddleNames;
        }

        /// <summary>
        /// Deconstructs the name into a tuple of <see cref="Surname"/>,
        /// <see cref="ChristianName"/>, <see cref="MiddleNames"/> and the
        /// <see cref="Suffix"/>.
        /// </summary>
        /// <param name="surname">Receives the surname.</param>
        /// <param name="christianName">Receives, if any, the christian name.
        /// </param>
        /// <param name="middleNames">Receives the middle names.</param>
        /// <param name="suffix">Receives, if any, the suffix.</param>
        public void Deconstruct(out string surname,
                out string? christianName,
                out IEnumerable<string> middleNames,
                out string? suffix) {
            surname = this._surname;
            christianName = this.ChristianName;
            middleNames = this.MiddleNames;
            suffix = this.Suffix;
        }
        #endregion

        #region Private constants
        /// <summary>
        /// These tokens are not considered individual names.
        /// </summary>
        private static readonly Regex Affixes = new("^(von|zu|van|der|den)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// These tokens are recognised as suffixes.
        /// </summary>
        private static readonly Regex Suffixes = new(@"^(jn?r\.?|sn?r\.?)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Private fields
        private IEnumerable<string> _middleNames;
        private string _surname;
        #endregion
    }
}
