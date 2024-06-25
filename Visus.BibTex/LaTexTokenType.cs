// <copyright file="LatexTokenType.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Specifies the tokens used for partial analysis of LaTex control
    /// sequences in BibTex fields.
    /// </summary>
    internal enum LatexTokenType {

        /// <summary>
        /// The backslash, which is the escape character and the begin of a
        /// LaTex control sequence.
        /// </summary>
        Backslash,

        /// <summary>
        /// A left brace.
        /// </summary>
        BraceLeft,

        /// <summary>
        /// A right brace.
        /// </summary>
        BraceRight,

        /// <summary>
        /// The dollar sign, which signals the boundaries of math mode.
        /// </summary>
        Dollar,

        /// <summary>
        /// Signals that the end of the input was reached.
        /// </summary>
        End,

        /// <summary>
        /// A hyphen, which may be part of a dash or long dash.
        /// </summary>
        Hyphen,

        /// <summary>
        /// A string literal.
        /// </summary>
        Literal,

        /// <summary>
        /// The tilde character, which is a protected space.
        /// </summary>
        Tilde,

        /// <summary>
        /// A sequence of one or more white spaces.
        /// </summary>
        WhiteSpace,
    }
}
