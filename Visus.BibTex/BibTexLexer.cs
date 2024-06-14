// <copyright file="BibTexLexer.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.IO;


namespace Visus.BibTex {

    /// <summary>
    /// Lexer for BibTex tokens.
    /// </summary>
    internal sealed class BibTexLexer : IDisposable {

        /// <summary>
        /// Initialises a new instance parsing the given text.
        /// </summary>
        /// <param name="reader">A reader of the BibTex text.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/>
        /// is <c>null</c>.</exception>
        public BibTexLexer(TextReader reader) {
            this._reader = reader
                ?? throw new ArgumentNullException(nameof(reader));
        }

        /// <summary>
        /// Disposes the underlying <see cref="StreamReader"/>.
        /// </summary>
        public void Dispose() => this._reader.Dispose();

        /// <summary>
        /// Scan the input and return the tokens it comprises of.
        /// </summary>
        /// <returns>The type of the token and its value.</returns>
        public IEnumerable<BibTexToken> Tokenise() {
            int code = 0;

            while ((code = this._reader.Read()) != -1) {
                var c = (char) code;

                yield return new BibTexToken(c switch {
                    '@' => BibTexTokenType.At,
                    '{' => BibTexTokenType.BraceLeft,
                    '}' => BibTexTokenType.BraceRight,
                    ',' => BibTexTokenType.Comma,
                    '=' => BibTexTokenType.Equals,
                    '#' => BibTexTokenType.Hash,
                    '\n' => BibTexTokenType.NewLine,
                    '%' => BibTexTokenType.Percent,
                    '"' => BibTexTokenType.Quote,
                    _ => char.IsLetter(c)
                        ? BibTexTokenType.Letter
                        : char.IsDigit(c)
                        ? BibTexTokenType.Digit
                        : char.IsWhiteSpace(c)
                        ? BibTexTokenType.WhiteSpace
                        : BibTexTokenType.Character,
                }, c);
            }
        }

        #region Private fields
        private readonly TextReader _reader;
        #endregion
    }
}
