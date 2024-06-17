// <copyright file="NameTokenType.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Specifies the tokens the <see cref="NameTokeniser"/> can create.
    /// </summary>
    internal enum NameTokenType {

        /// <summary>
        /// A comma, which could be a separator or signalling the place where
        /// the surname(s) end and the Christian names start.
        /// </summary>
        Comma,

        /// <summary>
        /// Signals that the end of the input was reached.
        /// </summary>
        End,

        /// <summary>
        /// A separator between two names, for instance the literal string
        /// &quot;and&quot; or a semicolon.
        /// </summary>
        Separator,

        /// <summary>
        /// A string literal, which may or may not comprise multiple words.
        /// </summary>
        Literal,
    }
}
