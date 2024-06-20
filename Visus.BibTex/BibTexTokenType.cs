// <copyright file="BibTexTokenType.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Specifies the type of tokens the <see cref="BibTexTokeniser"/> can create.
    /// </summary>
    internal enum BibTexTokenType {

        /// <summary>
        /// The @ sign signifying the start of an entry.
        /// </summary>
        At,

        /// <summary>
        /// The \ escape character.
        /// </summary>
        Backslash,

        /// <summary>
        /// A left brace signifying the body of the entry or a string literal.
        /// </summary>
        BraceLeft,

        /// <summary>
        /// A right brace signifying the end of an entry or string literal.
        /// </summary>
        BraceRight,

        /// <summary>
        /// Any other character not covered by a different type.
        /// </summary>
        Character,

        /// <summary>
        /// A comma separating fields.
        /// </summary>
        Comma,

        /// <summary>
        /// A digit.
        /// </summary>
        Digit,

        /// <summary>
        /// The equals sign between field names and content.
        /// </summary>
        Equals,

        /// <summary>
        /// The # sign, which can be used to concatenate two string literals in
        /// double quotes with a string variable.
        /// </summary>
        Hash,

        /// <summary>
        /// A letter.
        /// </summary>
        Letter,

        /// <summary>
        /// A line break.
        /// </summary>
        NewLine,

        /// <summary>
        /// An opening parenthesis.
        /// </summary>
        ParenthesisLeft,

        /// <summary>
        /// A closing parenthesis.
        /// </summary>
        ParenthesisRight,

        /// <summary>
        /// The percent sign, which is comment until the end of the line.
        /// </summary>
        Percent,

        /// <summary>
        /// A double quote.
        /// </summary>
        Quote,

        /// <summary>
        /// Any white-space character.
        /// </summary>
        WhiteSpace
    }
}
