// <copyright file="RequiredBibTexFieldAttribute.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Reflection;


namespace Visus.BibTex {

    /// <summary>
    /// Annotates a field as being required for the specified type of BibTex
    /// entry.
    /// </summary>
    /// <remarks>
    /// Note that annotating a field with this attribute also implies that it
    /// is supported as checked by
    /// <see cref="SupportedBibTexFieldAttribute.IsSupported(MemberInfo, string)"/>.
    /// </remarks>
    /// <param name="forType">The name of the type for which the field is
    /// required, or <c>null</c> if it is required for all types.</param>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = true,
        Inherited = true)]
    public sealed class RequiredBibTexFieldAttribute(string? forType)
            : Attribute {

        #region Public methods
        /// <summary>
        /// Answer whether the field in <paramref name="mi"/> is required for
        /// the given <paramref name="type"/> of BibTex entry.
        /// </summary>
        /// <param name="mi">The annotated property or field.</param>
        /// <param name="type">The type of the BibTex entry.</param>
        /// <returns><c>true</c> if the field is annotated, <c>false</c>
        /// otherwise.</returns>
        public static bool IsRequired(MemberInfo mi, string type) {
            if ((mi == null) || (type == null)) {
                return false;
            }

            type = type.ToLowerInvariant();

            var atts = mi.GetCustomAttributes<RequiredBibTexFieldAttribute>();
            foreach (var a in atts) {
                if (a.ForType == null) {
                    return true;
                } else if (a.ForType == type) {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the type for which the field is required.
        /// </summary>
        /// <remarks>
        /// If this property is <c>null</c>, the field is required for any kind
        /// of entry.
        /// </remarks>
        public string? ForType { get; } = forType?.ToLowerInvariant();
        #endregion
    }
}
