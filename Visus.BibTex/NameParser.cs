// <copyright file="NameParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var buffer = new NameTokenBuffer();     // Buffers for tokens.
            var cntBuffer = 0;                      // Used part of 'buffer'.

            // Buffer as many tokens as we need to find out whether commas
            // are used to separate people or parts of the name. Note that the
            // maximum number of tokens we can buffer is limited, so it might be
            // that we need to decide on incomplete data after this loop.
            while (cntBuffer < buffer.Length) {
                var token = tokeniser.Next();
                buffer[cntBuffer++] = token;

                if (token.Type == NameTokenType.End) {
                    // We do not have anything else to buffer, so bail out
                    // early here.
                    break;

                }

                if (buffer[cntBuffer].Type == NameTokenType.Separator) {
                    // We found a separator, so we can be sure at this point
                    // that the comma is not used to separate people without
                    // buffering more data.
                    break;
                }
            }

            // Find out whether the comma is used to separate people.
            var commaIsSeparator = IsCommaSeparator(ref buffer, cntBuffer);
            if (!commaIsSeparator.HasValue) {
                // As we cannot use more data to decide, we make the educated
                // guess that the comma is not used to separate people because
                // this is the less common approach in BibTex files.
                Debug.WriteLine("Forcing comma to not be a separator as "
                    + "buffering capacity or available data are exhausted.");
                commaIsSeparator = false;
            }

            var buffered = 0;               // Current token from buffer.
            var current = (Name?) null;     // Current name.
            var text = new StringBuilder(); // Accumulates all literal text.

            while (true) {
                // Obtain a new token, first by emptying the buffer and then by
                // processing additional input.
                var token = (buffered < cntBuffer)
                    ? buffer[buffered]
                    : tokeniser.Next();
                ++buffered;

                switch (token.Type) {
                    case NameTokenType.Comma:
                        if (commaIsSeparator == true) {
                            // If the comma is the separator, we commit the
                            // current name as if we had hit a separator.
                            if (current != null) {
                                retval.Add(current);
                                current = null;
                            }

                        } else if (commaIsSeparator == false) {
                            // If the comma is not the separator, all buffered
                            // stuff is now the surname of a new person.
                            current = new(Accumulate(ref buffer,
                                ref cntBuffer));
                        }
                        break;

                    case NameTokenType.End:
                        return retval;

                    case NameTokenType.Literal:
                        if (Affixes.IsMatch(token.Text)) {
                            //surname.Append(token.Text);

                        } else if (Suffixes.IsMatch(token.Text)) {
                            //suffixes.AppendSpaceIfNotEmpty();
                            //suffixes.Append(token.ToString());
                        }
                        break;

                    case NameTokenType.Separator:
                        // If we are at a separator, we commit the current name.
                        if (current != null) {
                            retval.Add(current);
                            current = null;
                        }
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

        #region Private methods
        /// <summary>
        /// Accumulate at most <paramref name="cnt"/> buffered literals.
        /// </summary>
        /// <param name="buffer">The buffer from which to accumulate.</param>
        /// <param name="cnt"> The number of valid tokens in
        /// <paramref name="buffer"/>. The method will adjust this variable
        /// (and the <paramref name="buffer"/>) to reflect all
        /// <see cref="NameTokenType.Literal"/>s that have been consumed for
        /// the return value.</param>
        /// <returns>A string holding the text of all tokens until the first
        /// non-literal.</returns>
        private static string Accumulate(
                ref NameTokenBuffer buffer,
                ref int cnt) {
            var retval = new StringBuilder();

            if (cnt > buffer.Length) {
                cnt = buffer.Length;
            }

            for (int i = 0; i < cnt; ++i) {
                var t = buffer[i];

                if (t.Type == NameTokenType.Literal) {
                    if (!t.IsEmpty) {
                        retval.AppendSpaceIfNotEmpty();
                        retval.Append(t.Text);
                    }

                } else {
                    // We found a non-literal token, so we shift the buffer
                    // to reflect the stuff we have used before and bail out
                    // early.
                    var rem = cnt - i;
                    Debug.Assert(rem > 0);

                    for (int j = 0; j < rem; ++i, ++j) {
                        buffer[j] = buffer[i];
                    }

                    cnt = rem;
                    return retval.ToString();
                }
            }

            cnt = 0;
            return retval.ToString();
        }

        /// <summary>
        /// Tries to find out based on the <paramref name="cnt"/> buffered
        /// <see cref="NameToken"/>s whether the comma is a separator between
        /// names or separates the surname and the Christian names.
        /// </summary>
        /// <remarks>
        /// <para>The decision by the method is performed as follows: If any
        /// <see cref="NameTokenType.Literal"/> is in the buffer, the return
        /// value will always be <c>false</c>. If we find more than one
        /// <see cref="NameTokenType.Comma"/>, we assume that the comma is
        /// used to separate people. If we have one
        /// <see cref="NameTokenType.Comma"/>, but there are literals to
        /// represent a Christian name and a surname before and after the
        /// comma, we assume that the comma separates people, too. In all
        /// other cases, we are too unsure and return <c>null</c>.</para>
        /// </remarks>
        /// <param name="buffer"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        private static bool? IsCommaSeparator(
                ref NameTokenBuffer buffer,
                int cnt) {
            if (cnt >  buffer.Length) {
                cnt = buffer.Length;
            }

            var commas = 0;             // # of commas found overall.
            var lastComma = -1;         // Position of the comma.
            var literalsAfter = 0;      // # of literals before the comma.
            var literalsBefore = 0;     // # of literals after the comma.
            var maxLiteralsAfter = 0;   // Max of 'literalsAfter' over commas.
            var maxLiteralsBefore = 0;  // Max of 'literalsBefore' over commas.

            for (int i = 0; i < cnt; ++i) {
                switch (buffer[i].Type) {
                    case NameTokenType.Comma:
                        if (lastComma >= 0) {
                            // If we had a comma before, check whether the
                            // maxima have changed and swap before and after
                            // for the new comma.
                            if (literalsAfter > maxLiteralsAfter) {
                                maxLiteralsAfter = literalsAfter;
                            }
                            if (literalsBefore > maxLiteralsBefore) {
                                maxLiteralsBefore = literalsBefore;
                            }

                            literalsBefore = literalsAfter;
                            literalsAfter = 0;
                        }

                        ++commas;
                        lastComma = i;
                        break;

                    case NameTokenType.End:
                        i = cnt;
                        break;

                    case NameTokenType.Literal:
                        if (lastComma < 0) {
                            ++literalsBefore;
                        } else if (lastComma >= 0) {
                            ++literalsAfter;
                        }
                        break;

                    case NameTokenType.Separator:
                        return false;
                }
            }

            if (commas > 1) {
                return true;
            }

            if ((maxLiteralsBefore >= 2) && (maxLiteralsAfter >= 2)) {
                return true;
            }

            return null;
        }
        #endregion
    }
}
