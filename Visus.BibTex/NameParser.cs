// <copyright file="NameParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace Visus.BibTex {

    /// <summary>
    /// Parses <see cref="string"/>s into <see cref="Name"/>s.
    /// </summary>
    internal static class NameParser {

        public static IEnumerable<Name> ParseList(ReadOnlySpan<char> name) {
            NameTokeniser tokeniser = new(name);
            List<Name> retval = new();

            var buffer = new NameTokenBuffer();
            var cntBuffer = 0;
            //var guesses = stackalloc[] { new NameToken() };
            var commaIsSeparator = false;       // The comma is a separator.
            var haveSeparator = false;          // Have seen a separator.
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
                            + "was encountered while parsing names. This is a "
                            + $"bug in {nameof(NameParser)}.{nameof(ParseList)}"
                            + "(), which was not updated to account for new "
                            + "tokens produced by the "
                            + $"{nameof(NameTokeniser)}.");
                }
            }
        }

        #region Private constants
        /// <summary>
        /// These tokens are not considered individual names.
        /// </summary>
        private static readonly Regex Affixes = new(
            "^(von|zu|van|der|den|del(la)?)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// These tokens are recognised as suffixes.
        /// </summary>
        private static readonly Regex Suffixes = new(@"^(jn?r\.?|sn?r\.?)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion
    }
}
