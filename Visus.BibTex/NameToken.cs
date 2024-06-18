﻿// <copyright file="NameToken.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.Text.Unicode;


namespace Visus.BibTex {

    /// <summary>
    /// Represents a token generated by the <see cref="NameTokeniser"/>.
    /// </summary>
    /// <param name="type">The type of the token.</param>
    /// <param name="text">The content of the token.</param>
    [DebuggerDisplay("{Type}: {Text}")]
    internal ref struct NameToken(NameTokenType type, ReadOnlySpan<char> text) {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance with an <see cref="NameTokenType.End" />
        /// token.
        /// </summary>
        public NameToken() : this(NameTokenType.End, new()) { }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the first letter in <see cref="Text"/> or <c>null</c> if
        /// <see cref="Text"/> is empty or does not contain any letter.
        /// </summary>
        public char? FirstLetter {
            get {
                foreach (var c in this.Text) {
                    if (char.IsLetter(c)) {
                        return c;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Answer whether the first letter in the token is a captial.
        /// </summary>
        public bool IsCapitalised {
            get {
                if (this.Text.Length < 1) {
                    return false;
                }

                if (char.IsDigit(this.Text[0])) {
                    return false;
                }

                var letter = this.FirstLetter;
                if (letter == null) {
                    return false;
                }

                var l = letter.Value;
                return char.IsUpper(l)
                    || UnicodeRanges.CjkUnifiedIdeographs.Contains(l);
            }
        }

        /// <summary>
        /// Gets the content of the token.
        /// </summary>
        public ReadOnlySpan<char> Text { get; } = text;

        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        public NameTokenType Type { get; } = type;
        #endregion

        #region Public methods
        /// <inheritdoc />
        public override string ToString() => this.Text.ToString();
        #endregion
    }
}
