// <copyright file="IBibItemBuilder.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


using System.Collections.Generic;

namespace Visus.BibTex {

    /// <summary>
    /// The interface of a builder that allows for constructing custom
    /// objects from BibTex input by the <see cref="BibTexParser{TBibItem}"/>.
    /// </summary>
    /// <typeparam name="TBibItem"></typeparam>
    public interface IBibItemBuilder<TBibItem> {

        /// <summary>
        /// Adds a field named <paramref name="name"/> to the given BibTex
        /// <paramref name="item"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><c>this</c>.</returns>
        IBibItemBuilder<TBibItem> AddField(TBibItem item, string name,
            string value);

        /// <summary>
        /// Adds a field named <paramref name="name"/> to the given BibTex
        /// <paramref name="item"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns><c>this</c>.</returns>
        IBibItemBuilder<BibItem> AddField(TBibItem item, string name,
            IEnumerable<Name> value);

        /// <summary>
        /// Creates a new entry of the specified <paramref name="type"> and with
        /// the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="type">The type of the BibTex entry.</param>
        /// <param name="key">The key of the entry.</param>
        /// <returns>A new BibTex entry.</returns>
        TBibItem Create(string type, string key);

    }
}
