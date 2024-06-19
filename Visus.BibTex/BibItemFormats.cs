// <copyright file="BibItemFormats.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Enumerates possible format strings for <see cref="BibItem"/>.
    /// </summary>
    public static class BibItemFormats {

        /// <summary>
        /// Renders the <see cref="BibItem"/> as compact as possible, in a
        /// single line.
        /// </summary>
        public const string Compact = "c";

        /// <summary>
        /// Works as <see cref="Compact"/>, but add spaces after commas like it
        /// is common in type setting.
        /// </summary>
        public const string CompactlySpaced = "C";

        /// <summary>
        /// The default format to be applied if no format string is given.
        /// </summary>
        public const string General = "s4.SXCM";

        /// <summary>
        /// Renders each field of the <see cref="BibItem"/> in a separate line,
        /// but does not indent it.
        /// </summary>
        public const string Lines = "l";

        /// <summary>
        /// Quote fields instead of using braces.
        /// </summary>
        /// <remarks>
        /// This format can be combined with any of the other formats.
        /// </remarks>
        public const string Quoted = "q";

        /// <summary>
        /// Renders each field of the <see cref="BibItem"/> in a separate line
        /// and indents it with spaces.
        /// </summary>
        /// <remarks>
        /// The format specified can be followed by a number indicating the
        /// number of spaces. Otherwise, this number will default to four.
        /// </remarks>
        public const string Spaced = "s";

        /// <summary>
        /// Renders each field of the <see cref="BibItem"/> in a separate line
        /// and indents it with tabs.
        /// </summary>
        /// <remarks>
        /// The format specified can be followed by a number indicating the
        /// number of tabs. Otherwise, this number will default to four.
        /// </remarks>
        public const string Tabbed = "t";

    }
}
