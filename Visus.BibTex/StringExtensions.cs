// <copyright file="StringExtensions.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;


namespace Visus.BibTex {

    /// <summary>
    /// Extension methods for strings.
    /// </summary>
    public static class StringExtensions {

        #region Public methods
        /// <summary>
        /// Convert the given string to an initial, ie return the first
        /// character.
        /// </summary>
        /// <param name="that">The string to be transformed.</param>
        /// <returns>The initial or an empty string if the string is
        /// <c>null</c>, empty or only comprising white spaces.</returns>
        public static string ToInitial(this string? that) {
            if (string.IsNullOrWhiteSpace(that)) {
                return string.Empty;
            } else {
                return $"{that[0]}.";
            }
        }

        /// <summary>
        /// Convert the given set of strings into a string of initials.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static string ToInitials(this IEnumerable<string?>? that) {
            var retval = new StringBuilder();

            if (that != null) {
                foreach (var t in that) {
                    if (!string.IsNullOrWhiteSpace(t)) {
                        retval.AppendSpaceIfNotEmpty();
                        retval.Append(t[0]);
                        retval.Append('.');
                    }
                }
            }

            return retval.ToString();
        }
        #endregion

        #region Internal methods
        /// <summary>
        /// If <paramref name="that"/> is a non-empty
        /// <see cref="StringBuilder"/>, append a space to it.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        internal static StringBuilder? AppendSpaceIfNotEmpty(
                this StringBuilder? that) {
            if ((that != null) && (that.Length > 1)) {
                that.Append(' ');
            }

            return that;
        }

        /// <summary>
        /// Answer whether the (case-insensitive) name part identifier
        /// <paramref name="part"/> starts at position <paramref name="index"/>
        /// in <paramref name="that"/>.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="index"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="part"/>
        /// is <c>null</c>.</exception>
        internal static bool IsNamePartAt(this string? that,
                int index,
                string part) {
            ArgumentNullException.ThrowIfNull(part);

            if (that == null) {
                return false;
            }

            if (part.Length == 0) {
                return false;
            }

            if (that.Length < index + part.Length) {
                return false;
            }

            return that.AsSpan(index, part.Length).Equals(part,
                StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Conditionally return the initial of <paramref name="that"/> if
        /// <paramref name="initial"/> is <c>true</c> or <paramref name="that"/>
        /// itself otherwise.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="initial"></param>
        /// <returns></returns>
        internal static string ToInitial(this string? that, bool initial) {
            return initial
                ? that.ToInitial()
                : string.IsNullOrWhiteSpace(that)
                ? string.Empty
                : that;
        }

        /// <summary>
        /// Trim characters from the end of a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        internal static StringBuilder TrimEnd(this StringBuilder that,
                Func<char, bool> predicate) {
            if ((that == null) || (predicate == null)) {
                return that!;
            }

            var index = that.Length - 1;
            while ((index > 0) && predicate(that[index])) {
                that.Remove(index, 1);
                --index;
            }

            return that;
        }

        /// <summary>
        /// Trims white-space characters from the end of a
        /// <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static StringBuilder TrimEnd(this StringBuilder that)
            => that.TrimEnd(char.IsWhiteSpace);
        #endregion
    }
}
