// <copyright file="NameFormats.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Enumerates possible format strings for <see cref="Name"/>.
    /// </summary>
    public static class NameFormats {

        /// <summary>
        /// The first letter of the surname.
        /// </summary>
        /// <remarks>
        /// The value of this constant must be a string of exactly one
        /// character. Furthermore, the abbreviated version must be the
        /// lower-case equivalent of the full version for any part.
        /// </remarks>
        public const string AbbreviatedSurname = "s";

        /// <summary>
        /// The inital of the christian name.
        /// </summary>
        /// <remarks>
        /// The value of this constant must be a string of exactly one
        /// character. Furthermore, the abbreviated version must be the
        /// lower-case equivalent of the full version for any part.
        /// </remarks>
        public const string ChristianInital = "c";

        /// <summary>
        /// The full Christian name.
        /// </summary>
        /// <remarks>
        /// The value of this constant must be a string of exactly one
        /// character. Furthermore, the abbreviated version must be the
        /// lower-case equivalent of the full version for any part.
        /// </remarks>
        public const string ChristianName = "C";

        /// <summary>
        /// The initials of the middle names.
        /// </summary>
        /// <remarks>
        /// The value of this constant must be a string of exactly one
        /// character. Furthermore, the abbreviated version must be the
        /// lower-case equivalent of the full version for any part.
        /// </remarks>
        public const string MiddleInitials = "m";

        /// <summary>
        /// The full middle names.
        /// </summary>
        /// <remarks>
        /// The value of this constant must be a string of exactly one
        /// character. Furthermore, the abbreviated version must be the
        /// lower-case equivalent of the full version for any part.
        /// </remarks>
        public const string MiddleNames = "M";

        /// <summary>
        /// Any suffix to the name.
        /// </summary>
        /// <remarks>
        /// The value of this constant must be a string of exactly one
        /// character.
        /// </remarks>
        public const string Suffix = "X";

        /// <summary>
        /// The full surname.
        /// </summary>
        /// <remarks>
        /// The value of this constant must be a string of exactly one
        /// character. Furthermore, the abbreviated version must be the
        /// lower-case equivalent of the full version for any part.
        /// </remarks>
        public const string Surname = "S";

        /// <summary>
        /// The surname followed by the christian name and the
        /// initials of the middle names, if any.
        /// </summary>
        public const string SurnameChristianNameMiddleInitials
            = $"{Surname}{ChristianName}{MiddleInitials}";

        /// <summary>
        /// The surname followed by the christian name and the
        /// initials of the middle names, if any.
        /// </summary>
        public const string SurnameChristianNameMiddleInitialsSuffix
            = $"{Surname}{ChristianName}{MiddleInitials}{Suffix}";

        /// <summary>
        /// The surname followed by suffix, the christian name and the
        /// initials of the middle names, if any.
        /// </summary>
        public const string SurnameSuffixChristianNameMiddleInitials
            = $"{Surname}{Suffix}{ChristianName}{MiddleInitials}";

        /// <summary>
        /// The surname followed by all Christian names.
        /// </summary>
        public const string SurnameChristianNameMiddleNames
            = $"{Surname}{ChristianName}{MiddleNames}";

        /// <summary>
        /// The surname followed by all Christian names and the suffix.
        /// </summary>
        public const string SurnameChristianNameMiddleNamesSuffix
            = $"{Surname}{ChristianName}{MiddleNames}{Suffix}";

        /// <summary>
        /// The surname followed by the suffix and all Christian names.
        /// </summary>
        public const string SurnamesSuffixChristianNameMiddleNames
            = $"{Surname}{Suffix}{ChristianName}{MiddleNames}";
    }
}
