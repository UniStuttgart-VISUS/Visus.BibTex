// <copyright file="BibItemBuilder.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Visus.BibTex.Properties;


namespace Visus.BibTex {

    /// <summary>
    /// Implementation of <see cref="IBibItemBuilder{TBibItem}"/> for
    /// <see cref="BibItem"/>s.
    /// </summary>
    public sealed class BibItemBuilder : IBibItemBuilder<BibItem> {

        /// <inheritdoc />
        public IBibItemBuilder<BibItem> AddField(string name,
                string value) {
            ArgumentNullException.ThrowIfNull(name);
            this.ThrowIfNoItem();
            this._item![name.ToLowerInvariant()] = value;
            return this;
        }

        /// <inheritdoc />
        public IBibItemBuilder<BibItem> AddField(string name,
                IEnumerable<Name> value) {
            ArgumentNullException.ThrowIfNull(name);
            this.ThrowIfNoItem();

            if (value.Any()) {
                this._item![name.ToLowerInvariant()] = value;
            } else {
                this._item![name.ToLowerInvariant()] = null;
            }

            return this;
        }

        /// <inheritdoc />
        public BibItem Build() {
            if (this._item == null) {
                throw new InvalidOperationException(Resources.ErrorNoItem);
            }

            var retval = this._item;
            this._item = null;
            return retval;
        }

        /// <inheritdoc />
        public IBibItemBuilder<BibItem> Create(string type, string key) {
            if (this._item != null) {
                throw new InvalidOperationException(Resources.ErrorBuilderActive);
            }

            this._item = new(type, key);
            return this;
        }

        #region Private methods
        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if
        /// <see cref="_item"/> is <c>null</c>.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [MemberNotNull(nameof(_item))]
        private void ThrowIfNoItem() => _ = this._item
            ?? throw new InvalidOperationException(Resources.ErrorNoItem);
        #endregion

        #region Private fields
        private BibItem? _item;
        #endregion
    }
}
