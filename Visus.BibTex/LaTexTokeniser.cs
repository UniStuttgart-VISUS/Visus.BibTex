﻿// <copyright file="LatexTokeniser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace Visus.BibTex {

    /// <summary>
    /// Lexer for LaTex tokens.
    /// </summary>
    /// <param name="input">The input to be processed.</param>
    [DebuggerDisplay("{_input}")]
    internal ref struct LatexTokeniser(ReadOnlySpan<char> input) {

        #region Public methods
        /// <summary>
        /// Produces the next token.
        /// </summary>
        /// <returns>The next token from the input.</returns>
        public LatexToken Next() {
            if (this._input.Length < 1) {
                return new LatexToken(LatexTokenType.End, new());
            }

            var type = Classify(this._input[0]);
            var length = type switch {
                LatexTokenType.WhiteSpace => this.ScanWhile(
                    char.IsWhiteSpace),
                LatexTokenType.Literal => this.ScanWhile(
                    c => Classify(c) == LatexTokenType.Literal),
                _ => 1
            };

            Debug.Assert(length > 0);
            return new(type, this.Consume(length));
        }
        #endregion

        #region Private class methods
        /// <summary>
        /// Classify <paramref name="c"/> as one of the
        /// <see cref="LatexTokenType"/>s.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static LatexTokenType Classify(char c) => c switch {
            '\\' => LatexTokenType.Backslash,
            '{' => LatexTokenType.BraceLeft,
            '}' => LatexTokenType.BraceRight,
            '$' => LatexTokenType.Dollar,
            '-' => LatexTokenType.Hyphen,
            '~' => LatexTokenType.Tilde,
            _ => char.IsWhiteSpace(c)
                ? LatexTokenType.WhiteSpace
                : LatexTokenType.Literal
        };
        #endregion

        #region Private methods
        /// <summary>
        /// &quot;Consumes&quot; <paramref name="length"/> characters by
        /// creating a new span as return value and setting the remainder as the
        /// new value of <see cref="_input"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<char> Consume(int length) {
            Debug.Assert(this._input.Length >= length);
            var retval = this._input.Slice(0, length);
            this._input = this._input.Slice(length);
            return retval;
        }

        /// <summary>
        /// Scan while <paramref name="predicate"/> holds for the input and
        /// return the length of the matching substring.
        /// </summary>
        /// <param name="predicate">The predicate for which the characters will
        /// be included in the token.</param>
        /// <returns>The number of continguous characters matching
        /// <paramref name="predicate"/>.</returns>
        private int ScanWhile(Func<char, bool> predicate) {
            Debug.Assert(predicate != null);

            int retval = 0;

            foreach (var c in this._input) {
                if (predicate(c)) {
                    ++retval;
                } else {
                    break;
                }
            }

            return retval;
        }
        #endregion

        #region Private fields
        private ReadOnlySpan<char> _input = input;
        #endregion
    }
}
