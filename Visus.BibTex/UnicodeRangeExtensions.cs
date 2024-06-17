// <copyright file="UnicodeRangeExtensions.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System.Text.Unicode;


namespace Visus.BibTex {

    /// <summary>
    /// Internal extension methods for <see cref="UnicodeRange"/>.
    /// </summary>
    internal static class UnicodeRangeExtensions {

        /// <summary>
        /// Answer whether the specified code point is in the specified
        /// <see cref="UnicodeRange"/>.
        /// </summary>
        /// <param name="that">The range to check against.</param>
        /// <param name="codePoint">The code point (character) to be checked.
        /// </param>
        /// <returns><c>true</c> if the code point is in the range, <c>false</c>
        /// otherwise.</returns>
        public static bool Contains(this UnicodeRange that, int codePoint) {
            return (that != null)
                && (codePoint >= that.FirstCodePoint)
                && (codePoint < that.FirstCodePoint + that.Length);
        }
    }
}
