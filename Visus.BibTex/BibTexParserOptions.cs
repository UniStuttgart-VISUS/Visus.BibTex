// <copyright file="BibTexParserOptions.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;


namespace Visus.BibTex {

    /// <summary>
    /// Customises the behaviour of a
    /// <see cref="BibTexParserOptions{TBibItem}"/>.
    /// </summary>
    /// <typeparam name="TBibItem"></typeparam>
    public sealed class BibTexParserOptions<TBibItem> {

        #region Public factory methods
        /// <summary>
        /// Creates a new set of options using a new builder of type
        /// <typeparamref name="TBuilder"/>.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the builder to be used. The
        /// method will create a new instance of this type for each options
        /// created.</typeparam>
        /// <returns>A new options object.</returns>
        public static BibTexParserOptions<TBibItem> Create<TBuilder>()
                where TBuilder : IBibItemBuilder<TBibItem>, new()
            => new BibTexParserOptions<TBibItem>(new TBuilder());
        #endregion

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="builder">The builder used to fill the items. It is
        /// strongly discouraged to share builders between parsers as the
        /// builder object is stateful by design.
        /// </param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="builder"/> is <c>null</c>.</exception>
        public BibTexParserOptions(IBibItemBuilder<TBibItem> builder) {
            this._builder = builder
                ?? throw new ArgumentNullException(nameof(builder));
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the builder used for creating the entries.
        /// </summary>
        /// <remarks>
        /// The builder is allowed to be <c>null</c>. If this is the case, the
        /// parser will create a new builder on its own.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If <c>null</c> is set.
        /// </exception>
        public IBibItemBuilder<TBibItem> Builder { 
            get => this._builder;
            set => this._builder = value
                ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets whether the parser should put best effort into
        /// processing input that is syntactically not correct.
        /// </summary>
        public bool Lenient { get; set; } = false;

        /// <summary>
        /// Gets a set of string variables that are injected into the parser.
        /// </summary>
        public Dictionary<string, string> Variables {
            get => this._variables;
            set => this._variables = value ?? new();
        }
        #endregion

        #region Private fields
        private IBibItemBuilder<TBibItem> _builder;
        private Dictionary<string, string> _variables = new();
        #endregion
    }


    /// <summary>
    /// Provides factory methods for <see cref="BibTexParserOptions{TBibItem}"/>.
    /// </summary>
    public static class BibTexParserOptions {

        /// <summary>
        /// Creates a new set of parser options for the specified type of item.
        /// </summary>
        /// <typeparam name="TBibItem">The type of the BibTex items being
        /// created.</typeparam>
        /// <param name="builder">The builder creating and filling the items.
        /// </param>
        /// <returns>A new options object.</returns>
        public static BibTexParserOptions<TBibItem> Create<TBibItem>(
                IBibItemBuilder<TBibItem> builder)
            => new BibTexParserOptions<TBibItem>(builder);

        /// <summary>
        /// Creates a new set of parser options for the specified type of item
        /// and using a new instance of <typeparamref name="TBuilder"/>.
        /// </summary>
        /// <typeparam name="TBibItem">The type of the BibTex items being
        /// created.</typeparam>
        /// <typeparam name="TBuilder">The type of the builder being created for
        /// filling the items.</typeparam>
        /// <returns>A new options object.</returns>
        public static BibTexParserOptions<TBibItem> Create<TBibItem, TBuilder>()
                where TBuilder : IBibItemBuilder<TBibItem>, new()
            => BibTexParserOptions<TBibItem>.Create<TBuilder>();

        /// <summary>
        /// Creates a new set of parser options for the default item type
        /// <see cref="BibItem"/> using the given item
        /// <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The builder used to create new items. It is
        /// strongly discouraged to share this object between parsers.</param>
        /// <returns>A new options object.</returns>
        public static BibTexParserOptions<BibItem> Create(
                IBibItemBuilder<BibItem> builder)
            => new BibTexParserOptions<BibItem>(builder);

        /// <summary>
        /// Creates a new set of parser options for the default item type
        /// <see cref="BibItem"/> and its associated
        /// <see cref="BibItemBuilder"/>.
        /// </summary>
        /// <returns>A new options object.</returns>
        public static BibTexParserOptions<BibItem> Create()
            => Create<BibItem, BibItemBuilder>();
    }
}
