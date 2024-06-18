﻿// <copyright file="BibTexParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

        public static IEnumerable<TBibItem> Parse(TextReader reader,
                BibTexParserOptions<TBibItem> options) {
            var state = new State(BibTexLexer.Tokenise(reader), options);
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

                    case BibTexTokenType.Percent:
                        // This is a comment until the next line.
                        ScanLine(state);
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
                options = new();
            }

            if (options.Builder == null) {
                options.Builder = new TBuilder();
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
            }

            /// <summary>
            /// Initialises a new instance.
            /// </summary>
            /// <param name="tokens"></param>
            public State(IEnumerable<BibTexToken> tokens,
                    BibTexParserOptions<TBibItem>? options)
                : this(tokens.GetEnumerator(), options ?? new()) { }

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
            /// Gets the parser options.
            /// </summary>
            public BibTexParserOptions<TBibItem> Options { get; }

            /// <summary>
            /// Gets the current offset into the input.
            /// </summary>
            public int Offset { get; private set; }

            /// <summary>
            /// Moves the iterator forward and updates the state accordingly.
            /// </summary>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
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
        /// Makes sure that <paramref name="state"/> has not reached the end or
        /// throws a <see cref="FormatException"/>.
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="FormatException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckValid(State state) {
            if (!state.IsValid) {
                throw new FormatException(Resources.ErrorPrematureEnd);
            }
        }

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
            int depth = 0;
            var retval = new StringBuilder();

            while (state.MoveNext()) {
                switch (state.CurrentTokenType) {
                    case BibTexTokenType.BraceLeft:
                        ++depth;
                        retval.Append(state.CurrentCharacter);
                        break;

                    case BibTexTokenType.BraceRight:
                        if (depth > 0) {
                            --depth;
                            retval.Append(state.CurrentCharacter);
                        } else {
                            state.MoveNext();
                            return retval.ToString();
                        }
                        break;

                    default:
                        retval.Append(state.CurrentCharacter);
                        break;
                }
            }

            throw new FormatException(Resources.ErrorPrematureEnd);
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
        /// <returns></returns>
        private static bool ParseEntry(State state,
                [NotNullWhen(true)] out TBibItem? entry) {
            Debug.Assert(state != null);
            Debug.Assert(state.CurrentTokenType == BibTexTokenType.At);

            if (!state.MoveNext()) {
                throw new FormatException(Resources.ErrorPrematureEnd);
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
            Debug.WriteLine($"BibTex type is \"{type}\".");

            // The first non-white space character must be the opening brace.
            if (!ScanWhiteSpacesAndThen(state, BibTexTokenType.BraceLeft, true)) {
                throw new FormatException(string.Format(
                    Resources.ErrorInvalidEntryBegin,
                    state.CurrentCharacter));
            }

            CheckValid(state);

            // We normalise the type to be lower case to facilitate the
            // subsequent processing of comment and preabmble blocks and to
            // match our pre-defined constants used by clients of the library.
            type = type.ToLowerInvariant();

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

            CheckValid(state);
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

                // Skip trailing spaces after comma such that the parser is on
                // the next field or closing brace.
                ScanWhiteSpaces(state, true);

                // Technically, we would need to check here that we are
                // at the end of the entry if the current token is not a
                // comma. However, not doing so makes the parser more lenient,
                // which is what we want.
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

            CheckValid(state);

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
            Debug.WriteLine($"Field \"{name}\" was found.");

            // The next non-white space must be the equals sign.
            if (!ScanWhiteSpacesAndThen(state, BibTexTokenType.Equals, true)) {
                throw new FormatException(string.Format(
                    Resources.ErrorMissingEquals,
                    state.CurrentCharacter));
            }

            ScanWhiteSpaces(state, true);

            // There are three valid kinds of fields: literal numbers, which may
            // be without quotes, strings in quotes and strings in braces.
            var value = state.CurrentTokenType switch {
                BibTexTokenType.Digit => ParseDigits(state),
                BibTexTokenType.Quote => ParseQuotedString(state),
                BibTexTokenType.BraceLeft => ParseBracedString(state),
                _ => throw new FormatException(string.Format(
                    Resources.ErrorInvalidFieldValueBegin,
                    state.CurrentCharacter))
            };
            Debug.Write($"Field value is \"{value}\".");

            // Skip trailing spaces, such that the caller is on the comma
            // separating two fields or on the brace ending the entry.
            ScanWhiteSpaces(state, true);

            return (name, value);
        }

        /// <summary>
        /// Parses a string in quotes.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static string ParseQuotedString(State state) {
            Debug.Assert(state != null);
            Debug.Assert(state.IsValid);
            Debug.Assert(state.CurrentTokenType == BibTexTokenType.Quote);
            bool escaped = false;
            var retval = new StringBuilder();

            while (state.MoveNext()) {
                switch (state.CurrentTokenType) {
                    case BibTexTokenType.Backslash:
                        escaped = true;
                        break;

                    case BibTexTokenType.Quote:
                        if (escaped) {
                            escaped = false;
                            retval.Append(state.CurrentCharacter);
                        } else {
                            state.MoveNext();
                            return retval.ToString();
                        }
                        break;

                    default:
                        escaped = false;
                        retval.Append(state.CurrentCharacter);
                        break;
                }
            }

            throw new FormatException(Resources.ErrorPrematureEnd);
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

            while ((state.CurrentTokenType != token) && state.MoveNext());

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
                && state.MoveNext());
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
}
