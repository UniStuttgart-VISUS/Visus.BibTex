// <copyright file="LatexConverterOptions.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Customises the behaviour of <see cref="LatexConverter"/>.
    /// </summary>
    public sealed class LatexConverterOptions {

        #region Public constants.
        /// <summary>
        /// A default-initialises instance of the converter options.
        /// </summary>
        public static readonly LatexConverterOptions Default = new();
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets whether the converter should ignore recoverable
        /// syntax errors.
        /// </summary>
        public bool Lenient { get; set; }

        /// <summary>
        /// Gets or sets how the converter should handle commands that it does
        /// not recognise.
        /// </summary>
        public UnknownLatexCommandHandling UnknownCommandHandling {
            get;
            set;
        } = UnknownLatexCommandHandling.EmitLiterally;
        #endregion
    }
}
