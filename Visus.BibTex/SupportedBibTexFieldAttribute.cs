// <copyright file="SupportedBibTexFieldAttribute.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Reflection;


namespace Visus.BibTex {

    /// <summary>
    /// Annotates a field as supported for the specified type of BibTex entry.
    /// </summary>
    /// <param name="forType">The type for which the field is supported or
    /// <c>null</c> if it is supported for any field.</param>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = true,
        Inherited = true)]
    public sealed class SupportedBibTexFieldAttribute(string? forType = null)
            : Attribute {

        #region Public methods
        /// <summary>
        /// Answer whether the field in <paramref name="mi"/> is supported (or
        /// required) for the given <paramref name="type"/> of BibTex entry.
        /// </summary>
        /// <remarks>
        /// If a field or property is marked with
        /// <see cref="RequiredBibTexFieldAttribute"/>, this method will assume
        /// it supported, too.
        /// </remarks>
        /// <param name="mi">The annotated property or field.</param>
        /// <param name="type">The type of the BibTex entry.</param>
        /// <returns><c>true</c> if the field is annotated, <c>false</c>
        /// otherwise.</returns>
        public static bool IsSupported(MemberInfo mi, string type) {
            if ((mi == null) || (type == null)) {
                return false;
            }

            type = type.ToLowerInvariant();

            var atts = mi.GetCustomAttributes<SupportedBibTexFieldAttribute>();
            foreach (var a in atts) {
                if (a.ForType == null) {
                    return true;
                } else if (a.ForType == type) {
                    return true;
                }
            }

            return RequiredBibTexFieldAttribute.IsRequired(mi, type);
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the type for which the field is required.
        /// </summary>
        /// <remarks>
        /// If this property is <c>null</c>, the field is supported for all
        /// types of entries.
        /// </remarks>
        public string? ForType { get; } = forType?.ToLowerInvariant();
        #endregion
    }
}
