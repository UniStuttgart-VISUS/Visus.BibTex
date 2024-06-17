// <copyright file="BibItemBuilder.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Linq;


namespace Visus.BibTex {

    /// <summary>
    /// Implementation of <see cref="IBibItemBuilder{TBibItem}"/> for
    /// <see cref="BibItem"/>s.
    /// </summary>
    public sealed class BibItemBuilder : IBibItemBuilder<BibItem> {

        /// <inheritdoc />
        public IBibItemBuilder<BibItem> AddField(BibItem item,
                string name,
                string value) {
            ArgumentNullException.ThrowIfNull(item);
            ArgumentNullException.ThrowIfNull(name);
            item[name.ToLowerInvariant()] = value;
            return this;
        }

        /// <inheritdoc />
        public IBibItemBuilder<BibItem> AddField(BibItem item,
                string name,
                IEnumerable<Name> value) {
            ArgumentNullException.ThrowIfNull(item);
            ArgumentNullException.ThrowIfNull(name);

            if (value.Any()) {
                item[name.ToLowerInvariant()] = value;
            } else {
                item[name.ToLowerInvariant()] = null;
            }

            return this;
        }

        /// <inheritdoc />
        public BibItem Create(string type, string key) => new(type, key);
    }
}
