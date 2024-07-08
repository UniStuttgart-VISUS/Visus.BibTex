// <copyright file="LatexConverter.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;


namespace Visus.BibTex {

    /// <summary>
    /// Processes LaTex commands in a string such that the output is a standard
    /// string (to the extent possible) for use in arbitraty applications.
    /// </summary>
    public static class LatexConverter {

        /// <summary>
        /// Converts braced parts in <paramref name="text"/> from LaTex, but
        /// keeps all other text.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <param name="options">A <see cref="LatexConverterOptions"/>
        /// customising the behaviour of the method.</param>
        /// <returns>The text which has as many Latex commands within braced
        /// ranges replaced with matching Unicode characters as possible.
        /// </returns>
        public static string ConvertBracedParts(ReadOnlySpan<char> text,
                LatexConverterOptions options) {
            var retval = new StringBuilder();
            var tokeniser = new LatexTokeniser(text);

            while (true) {
                var token = tokeniser.Next();

                switch (token.Type) {
                    case LatexTokenType.BraceLeft:
                        // If we find an opening brace, we are in Latex mode and
                        // hand over to the appropriate method.
                        ParseLatex(ref tokeniser, retval, options, true);
                        break;

                    case LatexTokenType.End:
                        // If we are at the end, we commit the output.
                        return retval.ToString().Normalize();

                    case LatexTokenType.WhiteSpace:
                        // Collapse all white spaces to one space.
                        retval.Append(' ');
                        break;

                    default:
                        retval.Append(token.Text);
                        break;
                }
            }
        }

        /// <summary>
        /// Converts braced parts in <paramref name="text"/> from LaTex, but
        /// keeps all other text.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>The text which has as many Latex commands within braced
        /// ranges replaced with matching Unicode characters as possible.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertBracedParts(ReadOnlySpan<char> text)
            => ConvertBracedParts(text, LatexConverterOptions.Default);

        /// <summary>
        /// Converts from LaTex to plain text.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <param name="options">A <see cref="LatexConverterOptions"/>
        /// customising the behaviour of the method.</param>
        /// <returns>The text which has as many Latex commands replaced with
        /// matching Unicode characters as possible.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertFrom(ReadOnlySpan<char> text,
                LatexConverterOptions options) {
            var tokeniser = new LatexTokeniser(text);
            var retval = ParseLatex(ref tokeniser, new(), options, false);
            return retval.ToString().Normalize();
        }

        /// <summary>
        /// Converts from LaTex to plain text.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <returns>The text which has as many Latex commands replaced with
        /// matching Unicode characters as possible.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ConvertFrom(ReadOnlySpan<char> text) {
            var tokeniser = new LatexTokeniser(text);
            var retval = ParseLatex(ref tokeniser, new(),
                LatexConverterOptions.Default, false);
            return retval.ToString().Normalize();
        }

        #region Private methods
        /// <summary>
        /// Make the first character of <paramref name="text"/> a combining
        /// diacritic if <paramref name="diacritic"/> is non-<c>null</c>. If
        /// <paramref name="text"/> is empty or <paramref name="diacritic"/> is
        /// <c>null</c>, the unmodified <paramref name="text"/> is appended to
        /// the output <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="text">The text to emit with a potential diacritic. If
        /// this text is empty, nothing will happen.</param>
        /// <param name="diacritic">The diacritic to (potentially) emit. This
        /// variable will be set <c>null</c> if the diacritic has been emitted.
        /// </param>
        /// <param name="retval">Receives the output.</param>
        /// <returns><paramref name="retval"/>.</returns>
        private static StringBuilder EmitDiacritic(ReadOnlySpan<char> text,
                ref Diacritic? diacritic, StringBuilder retval) {
            if (text.Length > 0) {
                if (diacritic != null) {
                    retval.Append(text[0]);
                    retval.Append(diacritic.Unicode);
                    text = text.Slice(1);
                    diacritic = null;
                }

                retval.Append(text);
            }

            return retval;
        }

        /// <summary>
        /// Match the start of <paramref name="token"/> to one of the elements
        /// of <paramref name="table"/> or of <see cref="TextDiacritics"/> if
        /// no table was specified.
        /// </summary>
        /// <remarks>
        /// Diacritics will be matched in order of occurrence in
        /// <paramref name="table"/>, so longer tokens must be at the begin of
        /// the table for this to work.
        /// </remarks>
        /// <param name="token">The token to match agains
        /// <see cref="Diacritic.Latex"/>.</param>
        /// <param name="table">The table to use. If <c>null</c>, which is the
        /// default, <see cref="TextDiacritics"/> is used.</param>
        /// <returns></returns>
        private static Diacritic? MatchDiacritic(LatexToken token,
                Diacritic[]? table = null) {
            foreach (var d in table ?? TextDiacritics) {
                if (token.Text.StartsWith(d.Latex)) {
                    return d;
                }
            }

            return null;
        }

        /// <summary>
        /// Parse the tokens produced by <paramref name="tokeniser"/> as Latex
        /// input and convert it to Unicode text to the extent possible.
        /// </summary>
        /// <param name="tokeniser"></param>
        /// <param name="options"></param>
        /// <param name="retval"></param>
        /// <param name="stopAtBrace">If <c>true</c>, stop parsing if the last
        /// (non-matching) closing brace was found. This brace will be consumed
        /// and not returned, which allows for parsing sub-parts of a BibTex
        /// field.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static StringBuilder ParseLatex(ref LatexTokeniser tokeniser,
                StringBuilder retval,
                LatexConverterOptions options,
                bool stopAtBrace) {
            var braces = 0;
            var command = false;
            var diacritic = (Diacritic?) null;
            var full = false;
            var hyphens = 0;
            var math = false;

            while (true) {
                var token = tokeniser.Next();

                // A series of hyphens as ended, so we emit them before we
                // continue.
                if (!token.Is(LatexTokenType.Hyphen)) {
                    while (hyphens > 0) {
                        switch (hyphens) {
                            case 1:
                                retval.Append('\u002d');
                                --hyphens;
                                break;

                            case 2:
                                retval.Append('\u2013');
                                hyphens -= 2;
                                break;

                            default:
                                retval.Append('\u2014');
                                hyphens -= 3;
                                break;
                        }
                    }
                }

                switch (token.Type) {
                    case LatexTokenType.Backslash:
                        if (command) {
                            // This is an escaped literal.
                            retval.Append(token.Text);
                            command = false;
                        } else {
                            command = true;
                        }
                        break;

                    case LatexTokenType.BraceLeft:
                        if (command) {
                            // This is an escaped literal.
                            retval.Append(token.Text);
                            command = false;

                        } else {
                            ++braces;
                            full = true;
                        }
                        break;

                    case LatexTokenType.BraceRight:
                        if (command) {
                            // This is an escaped literal.
                            retval.Append(token.Text);
                            command = false;

                        } else if (stopAtBrace && (braces == 0)) {
                            // This is the end of a sub-part.
                            return retval;

                        } else if (braces > 0) {
                            // This is a closing brace.
                            --braces;

                        } else {
                            // This is invalid syntax, which we handle
                            // gracefully as a literal.
                            retval.Append(token.Text);
                        }

                        full = false;
                        break;

                    case LatexTokenType.Dollar:
                        if (command) {
                            // This is an escaped literal.
                            retval.Append(token.Text);
                            command = false;

                        } else {
                            // Toggle math mode.
                            math = !math;
                        }

                        full = false;
                        break;

                    case LatexTokenType.End:
                        return retval;

                    case LatexTokenType.Hyphen:
                        // Accumulate to determine whether this is a dash or a
                        // single hyphen. The actual text will be emitted when
                        // the first non-hyphen is encountered.
                        ++hyphens;
                        command = false;
                        full = false;
                        break;

                    case LatexTokenType.Literal:
                        // Either interpret the following literal as a command
                        // or just emit it if it is not escaped.
                        if (command) {
                            diacritic = MatchDiacritic(token);
                            if (diacritic != null) {
                                if (diacritic.IsCombining) {
                                    // This is a combining diacritic mark in
                                    // Unicode, so we need to emit the "real"
                                    // character first.
                                    var rem = diacritic.Slice(token.Text);
                                    EmitDiacritic(rem, ref diacritic, retval);

                                } else {
                                    // The control sequence produces a
                                    // normalised diacritic.
                                    retval.Append(diacritic.Unicode);
                                    retval.Append(diacritic.Slice(token.Text));
                                    diacritic = null;
                                }

                            } else {
                                // If this is not a diacritic, check whether we
                                // have a known special character.
                                if (token.Text.Length == 1) {
                                    switch(token.Text[0]) {
                                        case '&':
                                        case '%':
                                        case '#':
                                        case '_':
                                        case '^':
                                            break;

                                        default:
                                            // This is something we do not
                                            // understand, so we emit it
                                            // literally as Latex code.
                                            retval.Append('\\');
                                            break;
                                    }
                                }

                                retval.Append(token.Text);
                            }

                            command = false;

                        } else {
                            // This is just normal text to emit. However, we
                            // need to consider a pending diacritic here.
                            EmitDiacritic(token.Text, ref diacritic, retval);
                        }

                        full = false;
                        break;

                    case LatexTokenType.Tilde:
                        if (command) {
                            // If escaped, this is potentially a diacritic.
                            diacritic = MatchDiacritic(token);
                            command = false;
                        } else {
                            // This is a protected space.
                            // TODO: we cannot collapse tildes atm.
                            retval.Append(' ');
                        }
                        break;

                    case LatexTokenType.WhiteSpace:
                        // Collapse any sequence of spaces to one.
                        command = false;
                        full = false;
                        retval.Append(' ');
                        break;

                    default:
                        throw new NotImplementedException("An unexpected token "
                            + $"{token.Type} was encountered while parsing "
                            + "names. This is a bug in "
                            + $"{nameof(LatexConverter)}.{nameof(ParseLatex)}()"
                            + "which was not updated to account for new tokens "
                            + $"produced by the {nameof(LatexTokeniser)}.");
                }
            }
        }
        #endregion

        #region Nested class Diacritic
        /// <summary>
        /// Describes how LaTex represents a diacritic and how it is replaced
        /// with either a combinding diacritical mark or with a character in
        /// Unicode.
        /// </summary>
        /// <remarks>
        /// Mappings are derived from
        /// https://en.wikibooks.org/wiki/LaTeX/Special_Characters and
        /// https://en.wikipedia.org/wiki/Combining_Diacritical_Marks.
        /// </remarks>
        /// <param name="latex"></param>
        /// <param name="unicode"></param>
        private sealed class Diacritic(string latex, char unicode) {

            #region Public Properties
            /// <summary>
            /// Gets wether <see cref="Unicode"/> is a combining diacritical
            /// mark or whether it is a direct replacement.
            /// </summary>
            public bool IsCombining => UnicodeRanges.CombiningDiacriticalMarks
                .Contains(this.Unicode);
            //public bool IsCombining => (this.Unicode >= '\u0300')
            //    && (this.Unicode <= '\u036F');
            //public bool IsCombining => (char.GetUnicodeCategory(this.Unicode)
            //    == UnicodeCategory.NonSpacingMark);

            /// <summary>
            /// Gets the LaTex control sequence to match.
            /// </summary>
            public string Latex { get; } = latex;

            /// <summary>
            /// Gets the Unicode replacement.
            /// </summary>
            public char Unicode { get; } = unicode;
            #endregion

            #region Public methods
            /// <summary>
            /// Answer whether <paramref name="text"/> starts with the given
            /// Latex control sequence.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public bool IsMatch(ReadOnlySpan<char> text) {
                return text.StartsWith(this.Latex);
            }

            /// <summary>
            /// Slice <paramref name="text"/> after the length of
            /// <see cref="Latex"/> or truncate it if <see cref="Latex"/> is
            /// longer than the input.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public ReadOnlySpan<char> Slice(ReadOnlySpan<char> text) {
                return text.Slice(Math.Min(text.Length, this.Latex.Length));
            }
            #endregion
        }
        #endregion

        #region Private constants
        private static readonly Diacritic[] MathDiacritics = [
            new("hat", '\u0302'),
            //new("widehat", '\u'),
            new("check", '\u030C'),
            new("tilde", '\u0303'),
            new("widetilde", '\u0360'),
            new("acute", '\u0301'),
            new("grave", '\u0300'),
            new("dot", '\u0307'),
            new("ddot", '\u0308'),
            new("breve", '\u0306'),
            new("bar", '\u0304'),
            new("vec", '\u0305'),
        ];

        private static readonly Diacritic[] TextDiacritics = [
            new("aa", 'å'),
            new("AA", 'Å'),
            new("oe", 'œ'),
            new("OE", 'Œ'),
            new("euro", '€'),
            new("EUR", '€'),
            // Longer matches must be before single characters.
            new("`", '\u0300'),     // grave accent
            new("'", '\u0301'),     // acute accent
            new("^", '\u0302'),     // circumflex
            new("\"", '\u0308'),    // dieresis
            new("H", '\u030B'),     // Hungarian umlaut
            new("~", '\u0303'),     // tilde
            new("c", '\u0327'),     // cedilla
            new("k", '\u0328'),     // ogonek
            new("l", 'ł'),
            new("L", 'Ł'),
            new("=", '\u0304'),     // macron
            new("b", '\u0331'),     // bar-under
            new(".", '\u0307'),     // dot accent
            new("d", '\u0323'),     // dot-under
            new("r", '\u030A'),     // 
            new("u", '\u0306'),     // breve accent
            new("v", '\u030C'),     // háček
            new("o", 'ø'),
            new("O", 'Ø'),
            new("t", '\u0361'),     // tie-after
            new("i", 'ı')
        ];

        // Other commands:
        //new("textdoublegrave", '\u030F'),
        // emph
        #endregion
    }
}
