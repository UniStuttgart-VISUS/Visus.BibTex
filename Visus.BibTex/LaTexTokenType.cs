// <copyright file="LaTexTokenType.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Specifies the tokens used for partial analysis of LaTex control
    /// sequences in BibTex fields.
    /// </summary>
    internal enum LaTexTokenType {

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
        /// A line break.
        /// </summary>
        NewLine,

        /// <summary>
        /// A string literal.
        /// </summary>
        Literal,

        /// <summary>
        /// A double quote.
        /// </summary>
        Quote,

        /// <summary>
        /// A sequence of one or more white spaces.
        /// </summary>
        WhiteSpace,
    }
}
