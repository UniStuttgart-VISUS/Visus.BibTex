// <copyright file="NameParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
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
    /// Parses <see cref="string"/>s into <see cref="Name"/>s.
    /// </summary>
    internal static class NameParser {

        /// <summary>
        /// Parses <paramref name="names"/> as a list of one or more
        /// <see cref="Name"/>s.
        /// </summary>
        /// <param name="names">The name string to parse.</param>
        /// <returns>The list of names parsed from the input.</returns>
        /// <exception cref="NotImplementedException">In case an internal
        /// parsing error occurred that requires fixing of the code itself.
        /// </exception>
        public static IEnumerable<Name> ParseList(ReadOnlySpan<char> names) {
            NameTokeniser tokeniser = new(names);
            List<Name> retval = new();

            var buffer = new NameTokenBuffer();     // Buffers for tokens.
            var cntBuffer = 0;                      // Used part of 'buffer'.

            // Buffer as many tokens as we need to find out whether commas
            // are used to separate people or parts of the name. Note that the
            // maximum number of tokens we can buffer is limited, so it might be
            // that we need to decide on incomplete data after this loop.
            while (cntBuffer < buffer.Length - 1) {
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

            var buffered = 0;                       // Current index in buffer.
            var current = (Name?) null;             // Current name.
            var literals = new List<string>();      // Potential names.
            var suffixes = new StringBuilder();     // Suffixes of current.
            var surname = new StringBuilder();      // Surname before comma.

            // Commits the 'current' name to 'retval' and resets all other state
            // variables to the begin of a new person.
            var commit = () => {
                if ((current == null) && (surname.Length > 0)) {
                    // We have a surname (with affixes), but no entry for it
                    // yet.
                    current = new(surname.ToString());
                }

                if ((current == null) && literals.Any()) {
                    // We have a bunch of names, but still no entry, so the last
                    // token is the surname.
                    current = new(literals.Last());
                    literals.RemoveAt(literals.Count - 1);
                }

                if (current != null) {
                    if (literals.Any()) {
                        // The first remaining literal must be the christian
                        // name.
                        current.ChristianName = literals.First();
                        literals.RemoveAt(0);
                    }

                    // Any remaining literal must be a middle name.
                    current.MiddleNames = literals;

                    if (suffixes.Length > 0) {
                        // If we have suffixes, apply them. Otherwise, we want
                        // the property to remain null rather than becoming an
                        // empty string.
                        current.Suffix = suffixes.ToString();
                    }

                    retval.Add(current);
                    current = null;
                }

                // Do not clear the 'literals', but recreate them as the list
                // now belongs to the entry we just emitted.
                literals = new List<string>();

                suffixes.Clear();
            };

            while (true) {
                // Obtain a new token, first by emptying the buffer and then by
                // processing additional input.
                var token = (buffered < cntBuffer)
                    ? buffer[buffered++]
                    : tokeniser.Next();

                switch (token.Type) {
                    case NameTokenType.Comma:
                        if (commaIsSeparator == true) {
                            // If the comma is the separator, we just commit as
                            // if we hit a separator.
                            commit();

                        } else if (surname.Length > 0) {
                            // Comma is used to separate surname and Christian
                            // name, but we have accumulated a surname with
                            // affixes. Note that is important to clear the
                            // 'surname' buffer to prevent further tokens being
                            // accumulated as surname.
                            current = new(surname.ToString());
                            surname.Clear();

                        } else {
                            // Comma is used to separate surname and Christian
                            // name and we reached the comma, so all tokens up
                            // to here form the surname.
                            current = new(string.Join(' ', literals));
                            literals.Clear();
                        }
                        break;

                    case NameTokenType.End:
                        commit();
                        return retval;

                    case NameTokenType.Literal:
                        if (Suffixes.IsMatch(token.Text)) {
                            // This is a known suffix, so do not include it as
                            // a name, but accumulate the suffixes separately.
                            suffixes.AppendSpaceIfNotEmpty();
                            suffixes.Append(token.ToString());

                        } else if ((commaIsSeparator == false)
                                && ((surname.Length > 0)
                                || Affixes.IsMatch(token.Text))) {
                            // If we have emitted anything to the surname, all
                            // the remainder except for the suffixes we tested
                            // before must be part of the surname as well.
                            // Furthermore, if we found any known affix used
                            // with surnames, we also know that this must be
                            // the start of the surname.
                            surname.AppendSpaceIfNotEmpty();
                            surname.Append(token.Text);

                        } else {
                            // In any other case, we collect the literal and
                            // decide later.
                            literals.Add(token.ToString());
                        }


                        //} else {
                        //    if (Suffixes.IsMatch(token.Text)) {
                        //        // This is a known suffix, so do not include it
                        //        // as part of the surname.
                        //        suffixes.AppendSpaceIfNotEmpty();
                        //        suffixes.Append(token.Text);

                        //    } else if (current == null) {
                        //        // We have no entry yet, so this token is part
                        //        // of the surname.
                        //        surname.AppendSpaceIfNotEmpty();
                        //        surname.Append(token.Text);

                        //    } else if (current.ChristianName == null) {
                        //        // We have an entry but the Christian name has
                        //        // not yet been set.
                        //        current.ChristianName = token.ToString();

                        //    } else {
                        //        // This must be a middle name as we have the
                        //        // surname and the Christian name already set
                        //        // and the input does not match a suffix.
                        //        literals.Add(token.ToString());
                        //    }
                        //}
                        break;

                    case NameTokenType.Separator:
                        commit();
                        break;

                    default:
                        throw new NotImplementedException("An unexpected token "
                            + $"{token.Type} was encountered while parsing "
                            + $"names. This is a bug in {nameof(NameParser)}."
                            + $"{nameof(ParseList)}() which was not updated "
                            + "to account for new tokens produced by the "
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
        private static readonly Regex Suffixes = new(
            @"^(jn?r\.?|sn?r\.?|[ivxlcdm]+\.?)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Private methods
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

                        if (literalsAfter > maxLiteralsAfter) {
                            maxLiteralsAfter = literalsAfter;
                        }
                        if (literalsBefore > maxLiteralsBefore) {
                            maxLiteralsBefore = literalsBefore;
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
