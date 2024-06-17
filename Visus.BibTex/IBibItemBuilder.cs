// <copyright file="IBibItemBuilder.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Visus.BibTex {

    /// <summary>
    /// The interface of a builder that allows for constructing custom
    /// objects from BibTex input by the <see cref="BibTexParser{TBibItem}"/>.
    /// </summary>
    /// <typeparam name="TBibItem"></typeparam>
    public interface IBibItemBuilder<TBibItem> {

        /// <summary>
        /// Adds a field named <paramref name="name"/> to the currently
        /// constructed BibTex entry.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="System.ArgumentNullException">If
        /// <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="System.InvalidOperationException">If no item has
        /// been created before trying to add a field.</exception>
        IBibItemBuilder<TBibItem> AddField(string name, string value);

        /// <summary>
        /// Adds a field named <paramref name="name"/> to the currently
        /// constructed BibTex entry.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="System.ArgumentNullException">If
        /// <paramref name="name"/> is <c>null</c>.</exception>
        /// <exception cref="System.InvalidOperationException">If no item has
        /// been created before trying to add a field.</exception>
        IBibItemBuilder<TBibItem> AddField(string name, IEnumerable<Name> value);

        /// <summary>
        /// Returns the item that has been built and resets the builder to its
        /// initial state.
        /// </summary>
        /// <returns>The item that has been constructed by previous calls.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">If no item has
        /// been created before building it.</exception>
        [return: NotNull] TBibItem Build();

        /// <summary>
        /// Resets the builder to start a new entry of the specified
        /// <paramref name="type"> and with the specified
        /// <paramref name="key"/>.
        /// </summary>
        /// <param name="type">The type of the BibTex entry.</param>
        /// <param name="key">The key of the entry.</param>
        /// <returns><c>this</c>.</returns>
        /// <exception cref="System.InvalidOperationException">If a previous
        /// item has not been committed by calling <see cref="Build"/>.
        /// </exception>
        IBibItemBuilder<TBibItem> Create(string type, string key);

    }
}
