// <copyright file="BibTexParserOptions.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System.Collections.Generic;


namespace Visus.BibTex {

    /// <summary>
    /// Customises the behaviour of a
    /// <see cref="BibTexParserOptions{TBibItem}"/>.
    /// </summary>
    /// <typeparam name="TBibItem"></typeparam>
    public sealed class BibTexParserOptions<TBibItem> {

        /// <summary>
        /// Gets or sets the builder used for creating the entries.
        /// </summary>
        /// <remarks>
        /// The builder is allowed to be <c>null</c>. If this is the case, the
        /// parser will create a new builder on its own.
        /// </remarks>
        public IBibItemBuilder<TBibItem>? Builder { get; set; }

        /// <summary>
        /// Gets a set of string variables that are injected into the parser.
        /// </summary>
        public Dictionary<string, string> Variables { get; } = new();
    }
}
