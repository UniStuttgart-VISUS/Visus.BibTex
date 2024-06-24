// <copyright file="BibTexParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using Visus.BibTex.Properties;


namespace Visus.BibTex {

    /// <summary>
    /// A parser for BibTex entries, which produces items of type
    /// <typeparamref name="TBibItem"/> using the given builder.
    /// </summary>
    /// <typeparam name="TBibItem">The type of the BibTex items begin
    /// created by the parser. The caller needs to provide an appropriate
    /// <see cref="IBibItemBuilder{TBibItem}"/> for the parser to create
    /// the items of this type.</typeparam>
    public static class BibTexParser<TBibItem> {

        // https://maverick.inria.fr/~Xavier.Decoret/resources/xdkbibtex/bibtex_summary.html
        // https://www.bibtex.org/Format/
        // https://bibtex.eu/

        /// <summary>
        /// Parses the text provided by <paramref name="reader"/> as BibTex.
        /// </summary>
        /// <remarks>
        /// <para>The <paramref name="options"/> provide the ability to
        /// configure <see cref="BibTexParserOptions{TBibItem}.Lenient"/>
        /// behaviour, which has the following effects: First, if the parser
        /// encounters a string variable that has not been defined, it will emit
        /// the name of the variable as a literal. Second, if the parser
        /// encounters an unmatched brace closing in a quoted string, it will
        /// ignore this error and treat the brace as a normal character. Note
        /// that the parser cannot easily ignore opening braces, because these
        /// escape the quote character itself.</para>
        /// </remarks>
        /// <param name="reader"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IEnumerable<TBibItem> Parse(TextReader reader,
                BibTexParserOptions<TBibItem> options) {
            var state = new State(BibTexTokeniser.Tokenise(reader), options);
            var sb = new StringBuilder();

            while (state.IsValid) {
                ScanWhiteSpaces(state, true);

                switch (state.CurrentTokenType) {
                    case BibTexTokenType.At:
                        // This is the begin of a BibTex entry or of a comment
                        // block. The latter will yield null and should be
                        // ignored.
                        if (ParseEntry(state, out var entry)) {
                            yield return entry;
                        }
                        break;

                    default:
                        // Everything outside an entry is ignored.
                        state.MoveNext();
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the text provided by <paramref name="reader"/> as a list of
        /// BibTexItems.
        /// </summary>
        /// <typeparam name="TBuilder">The builder that will be created if the
        /// <paramref name="options"/> do not provide one.</typeparam>
        /// <param name="reader">The reader providing the input to the parser.
        /// </param>
        /// <param name="options">The parser options. If this parameter is
        /// <c>null</c>, the parser will use the default options.</param>
        /// <returns>The list of items that have been parsed from the input.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="FormatException">If the input text is not valid
        /// BibTex.</exception>
        public static IEnumerable<TBibItem> Parse<TBuilder>(TextReader reader,
                BibTexParserOptions<TBibItem>? options = null)
                where TBuilder : IBibItemBuilder<TBibItem>, new() {
            if (options == null) {
                options = new(new TBuilder());
            }

            return Parse(reader, options);
        }

        #region Nested class State
        /// <summary>
        /// Encapsulates the parser state such that we can pass it on to methods
        /// we call.
        /// </summary>
        private class State {

            /// <summary>
            /// Initialises a new instance.
            /// </summary>
            /// <param name="tokens"></param>
            public State(IEnumerator<BibTexToken> tokens,
                    BibTexParserOptions<TBibItem> options) {
                this.Offset = 0;
                this.Options = options
                    ?? throw new ArgumentNullException(nameof(options));
                ArgumentNullException.ThrowIfNull(this.Options.Builder);
                this._tokens = tokens
                    ?? throw new ArgumentNullException(nameof(tokens));
                this.MoveNext();

                foreach (var v in this.Options.Variables) {
                    this.Variables.Add(v.Key, v.Value);
                }
            }

            /// <summary>
            /// Initialises a new instance.
            /// </summary>
            /// <param name="tokens"></param>
            public State(IEnumerable<BibTexToken> tokens,
                    BibTexParserOptions<TBibItem> options)
                : this(tokens.GetEnumerator(), options) { }

            /// <summary>
            /// Gets the item builder.
            /// </summary>
            public IBibItemBuilder<TBibItem> Builder => this.Options.Builder!;

            /// <summary>
            /// Gets the current token if <see cref="IsValid"/> is <c>true</c>.
            /// </summary>
            public BibTexToken CurrentToken => this._tokens.Current;

            /// <summary>
            /// Gets the current input character if <see cref="IsValid"/> is
            /// <c>true</c>.
            /// </summary>
            public char CurrentCharacter => this.CurrentToken.Character;

            /// <summary>
            /// Gets the type of the current token  if <see cref="IsValid"/> is
            /// <c>true</c>.
            /// </summary>
            public BibTexTokenType CurrentTokenType => this.CurrentToken.Type;

            /// <summary>
            /// Gets whether there is still a valid token.
            /// </summary>
            public bool IsValid { get; private set; }

            /// <summary>
            /// Gets the current line number, which is automatically tracked by
            /// <see cref="MoveNext"/>.
            /// </summary>
            public int Line { get; private set; } = 1;

            /// <summary>
            /// Gets the parser options.
            /// </summary>
            public BibTexParserOptions<TBibItem> Options { get; }

            /// <summary>
            /// Gets the current offset into the input.
            /// </summary>
            public int Offset { get; private set; }

            /// <summary>
            /// Gets the variables parsed from the file.
            /// </summary>
            public Dictionary<string, string> Variables { get; } = new();

            /// <summary>
            /// Makes sure that the end of the input was not yet reached or
            /// throws a <see cref="FormatException"/>.
            /// </summary>
            /// <exception cref="FormatException">If <see cref="IsValid"/> is
            /// not <c>true</c>.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void CheckValid() {
                if (!this.IsValid) {
                    throw new FormatException(string.Format(
                        Resources.ErrorPrematureEnd,
                        this.Line));
                }
            }

            /// <summary>
            /// Moves the iterator forward and updates the state accordingly.
            /// </summary>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
                if (this.IsValid && (this.CurrentTokenType
                        == BibTexTokenType.NewLine)) {
                    ++this.Line;
                }

                this.IsValid = this._tokens.MoveNext();

                if (this.IsValid) {
                    ++this.Offset;
                }

                return this.IsValid;
            }

            #region Private fields
            private IEnumerator<BibTexToken> _tokens;
            #endregion
        }
        #endregion

        /// <summary>
        /// Parses a braced string.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private static string ParseBracedString(State state) {
            Debug.Assert(state != null);
            Debug.Assert(state.IsValid);
            Debug.Assert(state.CurrentTokenType == BibTexTokenType.BraceLeft);
            int braces = 0;
            var escaped = false;
            var retval = new StringBuilder();

            while (state.MoveNext()) {
                switch (state.CurrentTokenType) {
                    case BibTexTokenType.At:
                        if (!escaped && !state.Options.Lenient) {
                            throw new FormatException(string.Format(
                                Resources.ErrorIllegalCharacter,
                                state.Line));
                        }

                        retval.Append(state.CurrentCharacter);
                        break;

                    case BibTexTokenType.BraceLeft:
                        if (!escaped) {
                            ++braces;
                        }

                        retval.Append(state.CurrentCharacter);
                        break;

                    case BibTexTokenType.BraceRight:
                        if (escaped) {
                            retval.Append(state.CurrentCharacter);

                        } else if (braces > 0) {
                            --braces;
                            retval.Append(state.CurrentCharacter);

                        } else {
                            state.MoveNext();
                            return retval.TrimEnd().ToString();
                        }
                        break;

                    case BibTexTokenType.WhiteSpace:
                        // If we encounter a white space and have not yet
                        // emitted anything, we can trivially trim the string.
                        if (retval.Length > 0) {
                            retval.Append(state.CurrentCharacter);
                        }
                        break;

                    default:
                        retval.Append(state.CurrentCharacter);
                        break;
                }

                // Remember whether we are escaped for the next iteration.
                escaped = !escaped
                    && (state.CurrentTokenType == BibTexTokenType.Backslash);
            }

            throw new FormatException(string.Format(Resources.ErrorPrematureEnd,
                state.Line));
        }

        /// <summary>
        /// Parses a sequence of digits.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static string ParseDigits(State state) {
            Debug.Assert(state != null);
            var retval = new StringBuilder();

            while (state.IsValid
                    && state.CurrentToken.Is(BibTexTokenType.Digit)) {
                retval.Append(state.CurrentCharacter);
                state.MoveNext();
            }

            return retval.ToString();
        }

        /// <summary>
        /// Parses a BibTex entry from its start indicated by the @ sign.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="entry"></param>
        /// <returns><c>true</c> if an <paramref name="entry"/> was emitted,
        /// <c>false</c> if the entry was a special one like a comment or a
        /// definition of a string variable.</returns>
        private static bool ParseEntry(State state,
                [NotNullWhen(true)] out TBibItem? entry) {
            Debug.Assert(state != null);
            Debug.Assert(state.CurrentTokenType == BibTexTokenType.At);

            if (!state.MoveNext()) {
                throw new FormatException(string.Format(
                    Resources.ErrorPrematureEnd,
                    state.Line));
            }

            // The type of an entry must start with a letter.
            if (!ScanWhiteSpaces(state, BibTexTokenType.Letter, false)) {
                throw new FormatException(string.Format(
                    Resources.ErrorInvalidTypeBegin,
                    state.CurrentCharacter));
            }

            // Parse all valid characters into the type string. We do not check
            // the type here to keep the parser extensible.
            var type = ParseString(state, BibTexTokenType.Character,
                BibTexTokenType.Digit,
                BibTexTokenType.Equals,
                BibTexTokenType.Letter);

            // We normalise the type to be lower case to facilitate the
            // subsequent processing of comment and preabmble blocks and to
            // match our pre-defined constants used by clients of the library.
            type = type.ToLowerInvariant();
            Debug.WriteLine($"BibTex type is \"{type}\".");

            // If the type is a string definition, we accept a brace or
            // parentheses, so we have a special case here.
            if (type == "string") {
                ParseVariable(state);
                entry = default;
                return false;
            }

            // The first non-white space character must be the opening brace.
            if (!ScanWhiteSpacesAndThen(state, BibTexTokenType.BraceLeft, true)) {
                throw new FormatException(string.Format(
                    Resources.ErrorInvalidEntryBegin,
                    state.CurrentCharacter,
                    state.Line));
            }

            state.CheckValid();

            if (type is "comment" or "preamble") {
                // This is a block to ignore, so we skip until its end.
                ScanUntil(state, BibTexTokenType.BraceRight);
                entry = default;
                return false;
            }

            ScanWhiteSpaces(state, true);

            // Next, we have the key, which must be a non-empty string.
            var key = ParseString(state, BibTexTokenType.Character,
                BibTexTokenType.Digit,
                BibTexTokenType.Equals,
                BibTexTokenType.Hash,
                BibTexTokenType.Letter);

            if (key.Length < 1) {
                throw new FormatException(Resources.ErrorInvalidKey);
            }

            // After the key, there must be a comma as the first non-space
            // character.
            if (!ScanWhiteSpacesAndThen(state, BibTexTokenType.Comma, true)) {
                throw new FormatException(string.Format(
                    Resources.ErrorInvalidKeyEnd,
                    state.CurrentCharacter));
            }

            state.CheckValid();
            ScanWhiteSpaces(state, true);

            // Next, we parse the fields, for which we need to construct the
            // return value.
            state.Builder.Create(type, key);

            while (state.CurrentTokenType != BibTexTokenType.BraceRight) {
                var field = ParseField(state);

                switch (field.Item1) {
                    case WellKnownFields.Author:
                    case WellKnownFields.Editor:
                        state.Builder.AddField(field.Item1,
                            Name.Parse(field.Item2));
                        break;

                    default:
                        state.Builder.AddField(field.Item1, field.Item2);
                        break;
                }

                if (state.CurrentTokenType == BibTexTokenType.Comma) {
                    // Consume the trailing comma at the end of the entry.
                    state.MoveNext();
                }

                // Technically, we would need to check here that we are
                // at the end of the entry if the current token is not a
                // comma. However, not doing so makes the parser more lenient,
                // which is what we want.

                // Skip trailing spaces after comma such that the parser is on
                // the next field or closing brace.
                ScanWhiteSpaces(state, true);
            }

            // Consume the closing brace.
            state.MoveNext();

            entry = state.Builder.Build();
            return true;
        }

        /// <summary>
        /// Parses a BibTex field and advance the state to the first
        /// non-white space after it, which must be either a comma for
        /// starting the next field or the closing brace at the end of
        /// the entry.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private static (string, string) ParseField(State state) {
            Debug.Assert(state != null);

            state.CheckValid();

            // The name of a field must start with a letter.
            if (!ScanWhiteSpaces(state, BibTexTokenType.Letter, false)) {
                throw new FormatException(string.Format(
                    Resources.ErrorInvalidFieldNameBegin,
                    state.CurrentCharacter));
            }

            // Parse all valid characters into the name.
            var name = ParseString(state, BibTexTokenType.Character,
                BibTexTokenType.Digit,
                BibTexTokenType.Letter);
            name = name.ToLowerInvariant();
            Debug.WriteLine($"Field \"{name}\" was found.");

            // The next non-white space must be the equals sign.
            if (!ScanWhiteSpacesAndThen(state, BibTexTokenType.Equals, true)) {
                throw new FormatException(string.Format(
                    Resources.ErrorMissingEquals,
                    state.CurrentCharacter));
            }

            ScanWhiteSpaces(state, true);

            // There are three valid kinds of fields: literal numbers, which may
            // be without quotes, strings in quotes and strings in braces. Note
            // that we interpret a letter as the begin of a variable name here,
            // which will be processed by the branch for quoted strings.
            var value = state.CurrentTokenType switch {
                BibTexTokenType.Digit => ParseDigits(state),
                BibTexTokenType.Quote => ParseQuotedStrings(state),
                BibTexTokenType.Letter => ParseQuotedStrings(state),
                BibTexTokenType.BraceLeft => ParseBracedString(state),
                _ => throw new FormatException(string.Format(
                    Resources.ErrorInvalidFieldValueBegin,
                    state.CurrentCharacter))
            };
            Debug.WriteLine($"Field value is \"{value}\".");

            // Skip trailing spaces, such that the caller is on the comma
            // separating two fields or on the brace ending the entry.
            ScanWhiteSpaces(state, true);

            return (name, value);
        }

        /// <summary>
        /// Parses an identifier that ends with any of the specified
        /// <paramref name="tokens"/>.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static string ParseIdentifier(State state,
                params BibTexTokenType[] tokens) {
            Debug.Assert(state != null);

            if ((tokens == null) || (tokens.Length < 1)) {
                tokens = [BibTexTokenType.WhiteSpace];
            }

            var retval = new StringBuilder();

            while (state.IsValid && !state.CurrentToken.IsAnyOf(tokens)) {
                retval.Append(state.CurrentCharacter);
                state.MoveNext();
            }

            return retval.ToString();
        }

        /// <summary>
        /// Parses a single string in quotes.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static StringBuilder ParseQuotedString(State state) {
            Debug.Assert(state != null);
            Debug.Assert(state.IsValid);
            Debug.Assert(state.CurrentTokenType == BibTexTokenType.Quote);
            var braces = 0;
            var escaped = false;
            var retval = new StringBuilder();

            while (state.MoveNext()) {
                switch (state.CurrentTokenType) {
                    case BibTexTokenType.BraceLeft:
                        retval.Append(state.CurrentCharacter);

                        if (!escaped) {
                            // If not escaped, count the quotes as they need to
                            // be balanced.
                            ++braces;
                        }
                        break;

                    case BibTexTokenType.BraceRight:
                        retval.Append(state.CurrentCharacter);

                        if (!escaped) {
                            // If not escaped, make sure that the quotes are
                            // balanced.
                            if (braces > 0) {
                                --braces;

                            } else if (!state.Options.Lenient) {
                                // Unmatched braces in a string are illegal.
                                throw new FormatException(string.Format(
                                    Resources.ErrorUnmatchedBraceInString,
                                    state.CurrentCharacter,
                                    state.Line));
                            }
                        }
                        break;

                    case BibTexTokenType.Quote:
                        if (braces > 0) {
                            // If we are in a braced expression, quotes are
                            // escaped in a quoted string.
                            retval.Append(state.CurrentCharacter);

                        } else {
                            // Otherwise, this marks the end of the string.
                            state.MoveNext();
                            return retval;
                        }
                        break;

                    default:
                        escaped = false;
                        retval.Append(state.CurrentCharacter);
                        break;
                }

                // Remember whether we are escaped for the next iteration.
                escaped = (state.CurrentTokenType == BibTexTokenType.Backslash);
            }

            throw new FormatException(string.Format(Resources.ErrorPrematureEnd,
                state.Line));
        }

        /// <summary>
        /// Parses a sequence of strings in quotes that contain
        /// (potentially) string variables and are (potentially)
        /// concatenated using the hash operator.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static string ParseQuotedStrings(State state) {
            var retval = new StringBuilder();

            while (true) {
                switch (state.CurrentTokenType) {
                    case BibTexTokenType.Quote:
                        // If we are on a quote, parse the string that follows.
                        retval.Append(ParseQuotedString(state));
                        ScanWhiteSpaces(state, true);
                        break;

                    case BibTexTokenType.Hash:
                        // If we are on a hash, we just eat it and continue to
                        // concatenate the following tokens.
                        state.MoveNext();
                        ScanWhiteSpaces(state, true);
                        break;

                    case BibTexTokenType.Letter: {
                        // If we are on a letter, this is the identifier of a
                        // string variable. We therefore try to replace it from
                        // the table of variables.
                        var name = ParseIdentifier(state,
                            BibTexTokenType.WhiteSpace,
                            BibTexTokenType.Comma);
                        Debug.WriteLine($"Requested variable \"{name}\".");

                        if (state.Variables.TryGetValue(name, out var value)) {
                            Debug.WriteLine($"Replace variable \"{name}\" with "
                                +$"value \"{value}\".");
                            retval.Append(value);

                        } else if (state.Options.Lenient) {
                            Debug.WriteLine("Emitting missing variable "
                                + $"name \"{name}\" as literal string.");
                            retval.Append(name);

                        } else {
                            throw new FormatException(string.Format(
                                Resources.ErrorUnknownVariable,
                                name,
                                state.Line));
                        }
                        } break;

                    case BibTexTokenType.WhiteSpace:
                        ScanWhiteSpaces(state, true);
                        break;

                    default:
                        return retval.ToString();
                }
            }
        }

        /// <summary>
        /// Collects all <paramref name="tokens"/> that are specified as valid
        /// and returns the string up to the first non-matching token.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static string ParseString(State state,
                params BibTexTokenType[] tokens) {
            Debug.Assert(state != null);
            var retval = new StringBuilder();

            while (state.IsValid && state.CurrentToken.IsAnyOf(tokens)) {
                retval.Append(state.CurrentCharacter);
                state.MoveNext();
            }

            return retval.ToString();
        }

        /// <summary>
        /// Parses a &quot;@string&quot; variable declaration and stores the
        /// variable in the <paramref name="state"/>.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (string, string) ParseVariable(State state) {
            Debug.Assert(state != null);

            ScanWhiteSpaces(state, true);

            var expectedEnd = state.CurrentTokenType switch {
                BibTexTokenType.BraceLeft => BibTexTokenType.BraceRight,
                BibTexTokenType.ParenthesisLeft => BibTexTokenType.ParenthesisRight,
                _ => throw new FormatException(string.Format(
                    Resources.ErrorInvalidStringBegin,
                    state.CurrentCharacter,
                    state.Line))
            };

            // Skip the opening brace or parenthesis and parse as field.
            state.MoveNext();
            var retval = ParseField(state);

            ScanWhiteSpaces(state, true);

            if (state.CurrentTokenType != expectedEnd) {
                throw new FormatException(string.Format(
                    Resources.ErrorInvalidStringEnd,
                    expectedEnd,
                    state.CurrentCharacter,
                    state.Line));
            }

            // As https://maverick.inria.fr/~Xavier.Decoret/resources/xdkbibtex/bibtex_summary.html
            // states that for duplicate definitions, the last is kept, we just
            // override the key here.
            state.Variables[retval.Item1] = retval.Item2;

            return retval;
        }

        #region Methods for skipping whole ranges of tokens
        /// <summary>
        /// Advances until the next line or to the end of the input.
        /// </summary>
        /// <param name="state">The state object to advance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ScanLine(State state)
            => ScanUntil(state, BibTexTokenType.NewLine);

        /// <summary>
        /// Advances until <paramref name="token"/> was found or the end of the
        /// input was reached. If <paramref name="token"/> was found, it will
        /// be consumed.
        /// </summary>
        /// <param name="state">The state object to advance.</param>
        /// <param name="token">The token up to which to advance.</param>

        private static void ScanUntil(State state, BibTexTokenType token) {
            Debug.Assert(state != null);
            Debug.Assert(state.IsValid);

            while ((state.CurrentTokenType != token) && state.MoveNext()) ;

            if (state.IsValid) {
                Debug.Assert(state.CurrentToken.Type == token);
                state.MoveNext();
            }
        }

        /// <summary>
        /// Advances the enumerator while the current token is of type
        /// <see cref="BibTexTokenType.WhiteSpace"/>.
        /// </summary>
        /// <remarks>
        /// This method has no effect if the current state is not a white-space
        /// (or new-line) token.
        /// </remarks>
        /// <param name="e"></param>
        /// <param name="includeNewLine"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ScanWhiteSpaces(State state,
                bool includeNewLine) {
            Debug.Assert(state != null);
            Debug.Assert(state.IsValid);
            while (state.CurrentToken.IsWhiteSpace(includeNewLine)
                && state.MoveNext()) ;
        }

        /// <summary>
        /// Advances to the first non-white space character and returns whether
        /// this character is <paramref name="token"/>. The following token will
        /// not be consumed.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="token"></param>
        /// <param name="includeNewLine"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ScanWhiteSpaces(State state,
                BibTexTokenType token,
                bool includeNewLine) {
            ScanWhiteSpaces(state, includeNewLine);
            return (state.CurrentTokenType == token);
        }

        /// <summary>
        /// Advances to first non-white space character and returns whether
        /// this character is <paramref name="token"/>, in which case the token
        /// will be consumed, too.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="token"></param>
        /// <param name="includeNewLine"></param>
        /// <returns></returns>
        private static bool ScanWhiteSpacesAndThen(State state,
                BibTexTokenType token,
                bool includeNewLine) {
            var retval = ScanWhiteSpaces(state, token, includeNewLine);

            if (retval) {
                state.MoveNext();
            }

            return retval;
        }
        #endregion
    }


    /// <summary>
    /// The parser for the default <see cref="BibItem"/>.
    /// </summary>
    public static class BibTexParser {

        /// <summary>
        /// Uses <see cref="BibTexParser{TBibItem}"/> to parse the given
        /// text as <see cref="BibItem"/>s.
        /// </summary>
        /// <param name="reader">The reader providing the input text.</param>
        /// <param name="options">The parser options to use.</param>
        /// <returns>The list of items parsed from the text.</returns>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="options"/> is <c>null</c>.</exception>
        public static IEnumerable<BibItem> Parse(TextReader reader,
                BibTexParserOptions<BibItem> options)
            => BibTexParser<BibItem>.Parse(reader, options);

        /// <summary>
        /// Uses <see cref="BibTexParser{TBibItem}"/> to parse the given text
        /// as <see cref="BibItem"/>s using default
        /// <see cref="BibTexParser{TBibItem}"/>.
        /// </summary>
        /// <param name="reader">The reader providing the input text.</param>
        /// <returns>The list of items parsed from the text.</returns>
        public static IEnumerable<BibItem> Parse(TextReader reader)
            => BibTexParser<BibItem>.Parse(reader,
                BibTexParserOptions.Create());
    }
}
