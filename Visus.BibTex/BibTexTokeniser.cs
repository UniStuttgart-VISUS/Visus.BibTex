// <copyright file="BibTexTokeniser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
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
    internal static class BibTexTokeniser {

        /// <summary>
        /// Scan the input and return the tokens it comprises of.
        /// </summary>
        /// <param name="reader">The reader for the BibTex file.</param>
        /// <returns>The type of the token and its value.</returns>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="reader"/> is <c>null</c>.</exception>
        public static IEnumerable<BibTexToken> Tokenise(TextReader reader) {
            _ = reader ?? throw new ArgumentNullException(nameof(reader));
            int code = 0;

            while ((code = reader.Read()) != -1) {
                var c = (char) code;

                yield return new BibTexToken(c switch {
                    '@' => BibTexTokenType.At,
                    '\\' => BibTexTokenType.Backslash,
                    '{' => BibTexTokenType.BraceLeft,
                    '}' => BibTexTokenType.BraceRight,
                    ',' => BibTexTokenType.Comma,
                    '=' => BibTexTokenType.Equals,
                    '#' => BibTexTokenType.Hash,
                    '\n' => BibTexTokenType.NewLine,
                    '(' => BibTexTokenType.ParenthesisLeft,
                    ')' => BibTexTokenType.ParenthesisRight,
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
    }
}
