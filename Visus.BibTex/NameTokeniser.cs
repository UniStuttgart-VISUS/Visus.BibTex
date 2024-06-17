// <copyright file="NameTokeniser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace Visus.BibTex {

    /// <summary>
    /// Performs tokenisation of lists of names.
    /// </summary>
    /// <param name="input">The input to be processed.</param>
    internal ref struct NameTokeniser(ReadOnlySpan<char> input) {

        #region Public methods
        /// <summary>
        /// Scans and returns the next token.
        /// </summary>
        /// <remarks>
        /// <para>We use this API instead of
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/>, because
        /// the token contains a <see cref="Span{T}"/> and therefore cannot be
        /// as type parameter for the enumerator.</para>
        /// </remarks>
        /// <returns>The next token in the string.</returns>
        public NameToken Next() {
            if (!this.ScanWhiteSpaces()) {
                // If the input is empty after processing all leading white
                // spaces, we are at the end. As '_input' itself is empty, we
                // can use it as the representation of the empty string.
                return new(NameTokenType.End, this._input);
            }

            Debug.Assert(this._input.Length >= 1);
            if (this._input[0] == ',') {
                // If we found a comma, the caller needs to decide whether this
                // is a separator between names or the separator between surname
                // and Christian name.
                return new(NameTokenType.Comma, this.Consume(1));
            }

            // Check whether we have a separator between two names.
            {
                var length = this.CheckSeparators(0);
                if (length > 0) {
                    return new(NameTokenType.Separator, this.Consume(length));
                }
            }

            // At this point, we have a string literal.
            var braces = 0;         // The depth of braces.
            var escaped = false;    // Remember the last character being '\'.

            for (int i = 0; i < this._input.Length; ++i) {
                switch (this._input[i]) {
                    case '{':
                        // If the brace is not escaped, count it on the brace
                        // depth.
                        if (!escaped) {
                            ++braces;
                        }
                        break;

                    case '}':
                        // If we are not escaped and have at least one brace,
                        // decrement the bracing depth.
                        if (!escaped && (braces > 0)) {
                            --braces;
                        }
                        break;

                    case ',':
                        if (braces == 0) {
                            // If we are not in a braced literal, a comma
                            // signals the end of the literal token, because
                            // the comma itself is a new token.
                            return new(NameTokenType.Literal,
                                this.Consume(i, true));
                        }
                        break;

                    default:
                        if (braces == 0) {
                            if (char.IsWhiteSpace(this._input[i])) {
                                // We found a white space and are not in a braced
                                // expression, which indicates the end of the token.
                                return new(NameTokenType.Literal,
                                    this.Consume(i, true));
                            }

                            if (this.CheckSeparators(i) > 0) {
                                // We found a separator and are not in a braced
                                // expression, which also marks a new token.
                                return new(NameTokenType.Literal,
                                    this.Consume(i, true));
                            }
                        }

                        break;
                }

                // Remember whether the next character is escaped.
                escaped = (this._input[i] == '\\');
            }

            return new(NameTokenType.Literal, this.Consume(this._input.Length, true));
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Checks whether any of <<see cref="Separators"/> matches at
        /// <paramref name="offset"/> and, if so, return the matched length.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns>The length of the match or zero if none of the
        /// <see cref="Separators"/> matched.</returns>
        private int CheckSeparators(int offset) {
            var range = this._input.Slice(offset);

            foreach (var s in Separators) {
                if (s.IsMatch(range)) {
                    // We need the match group here, so we need to copy the
                    // input to a string because the API for spans does not
                    // suport groups.
                    var m = s.Match(range.ToString());
                    return m.Groups[1].Length; // Only return non-white spaces.
                }
            }

            return 0;
        }

        /// <summary>
        /// &quot;Consumes&quot; <paramref name="length"/> characters by
        /// creating a new span as return value and setting the remainder as the
        /// new value of <see cref="_input"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private ReadOnlySpan<char> Consume(int length) {
            Debug.Assert(this._input.Length >= length);
            var retval = this._input.Slice(0, length);
            this._input = this._input.Slice(length);
            return retval;
        }

        /// <summary>
        /// &quot;Consumes&quot; <paramref name="length"/> characters by
        /// creating a new span as return value and setting the remainder as the
        /// new value of <see cref="_input"/>. If the whole range defined
        /// by <paramref name="length"/> is braced, remove the braces from the
        /// output.
        /// </summary>
        /// <param name="length">The number of characters to consume.</param>
        /// <param name="removeBraces">If <c>true</c>, braces around the whole
        /// range will be removed. This does not affect inner braces, though.
        /// </param>
        /// <returns></returns>
        private ReadOnlySpan<char> Consume(int length, bool removeBraces) {
            var retval = this.Consume(length);

            if (removeBraces
                    && (retval[0] == '{')
                    && (retval[retval.Length - 1] == '}')) {
                retval = retval.Slice(1, retval.Length - 2);
            }

            return retval;
        }

        /// <summary>
        /// Skips all white-space characters at the begin of
        /// <see cref="_input"/>.
        /// </summary>
        /// <returns><c>true</c> if there is still content in
        /// <see cref="_input"/> once the white spaces have been skipped,
        /// <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ScanWhiteSpaces() {
            this._input = this._input.TrimStart();
            return (this._input.Length > 0);
        }
        #endregion

        #region Private constants
        /// <summary>
        /// Shortcut to
        /// <see cref="StringComparison.InvariantCultureIgnoreCase"/>.
        /// </summary>
        private const StringComparison CaseInsensitive
            = StringComparison.InvariantCultureIgnoreCase;

        /// <summary>
        /// These tokens represent the concatenation of authors in a list
        /// using some kind of &quot;and&quot;. It is essential that the
        /// expressions only match the begin as we apply it on a moving
        /// window over the input.
        /// </summary>
        /// <remarks>
        /// The part to be consumed must be in the first match group of each
        /// expression.
        /// </remarks>
        private static readonly Regex[] Separators = [
            new(@"^(and)\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new("^(;)", RegexOptions.Compiled),
            new(@"^(\\&|&)", RegexOptions.Compiled)
        ];
        #endregion

        #region Private fields
        private ReadOnlySpan<char> _input = input;
        #endregion
    }
}
