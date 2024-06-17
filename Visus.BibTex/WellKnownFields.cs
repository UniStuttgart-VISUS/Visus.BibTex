// <copyright file="WellKnownFields.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Enumerates the names of well-known BibTex fields, which can be accessed
    /// via strongly typed properties of the default <see cref="BibItem"/>.
    /// </summary>
    public static class WellKnownFields {

        /// <summary>
        /// The address of the publisher or institution, typically the city.
        /// </summary>
        public const string Address = "address";

        /// <summary>
        /// Annotations for the bibliography, providing additional context or
        /// remarks.
        /// </summary>
        public const string Annote = "annote";

        /// <summary>
        /// The authors of the work, typically listed in order of contribution.
        /// </summary>
        public const string Author = "author";

        /// <summary>
        /// The title of the book when referencing a part of it, such as a
        /// chapter.
        /// </summary>
        public const string BookTitle = "booktitle";

        /// <summary>
        /// The specific chapter or section number within a larger work.
        /// </summary>
        public const string Chapter = "chapter";

        /// <summary>
        /// Cross-referencing another BibTeX entry, useful for connecting
        /// related works.
        /// </summary>
        public const string CrossReference = "crossref";

        /// <summary>
        /// The Digital Object Identifier, a unique code for electronic
        /// publications.
        /// </summary>
        public const string Doi = "doi";

        /// <summary>
        /// The edition of the book or publication, often specified for later
        /// editions.
        /// </summary>
        public const string Edition = "edition";

        /// <summary>
        /// The editors of a book or collection, often in place of authors for
        /// edited volumes.
        /// </summary>
        public const string Editor = "editor";

        /// <summary>
        /// The email address associated with the work or the primary contact.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// The institution responsible for the work, often for technical
        /// reports.
        /// </summary>
        public const string Institution = "institution";

        /// <summary>
        /// The academic journal in which the work was published.
        /// </summary>
        public const string Journal = "journal";

        /// <summary>
        /// The month of publication, which can be combined with the
        /// year field.
        /// </summary>
        public const string Month = "month";

        /// <summary>
        /// Additional notes or information related to the reference.
        /// </summary>
        public const string Note = "note";

        /// <summary>
        /// The number of a journal, magazine, technical report, or other
        /// serialised publication.
        /// </summary>
        public const string Number = "number";

        /// <summary>
        /// The organisation responsible for hosting a conference or publishing
        /// a manual.
        /// </summary>
        public const string Organisation = "organization";

        /// <summary>
        /// The page range for the referenced material, usually denoted
        /// as start-end.
        /// </summary>
        public const string Pages = "pages";

        /// <summary>
        /// The entity responsible for publishing the work, whether a company
        /// or institution.
        /// </summary>
        public const string Publisher = "publisher";

        /// <summary>
        /// The academic institution where a thesis or dissertation was
        /// presented.
        /// </summary>
        public const string School = "school";

        /// <summary>
        /// The series of books or journals in which the work was published.
        /// </summary>
        public const string Series = "series";

        /// <summary>
        /// The main title of the referenced work.
        /// </summary>
        public const string Title = "title";

        /// <summary>
        /// The specific type of a technical report or thesis.
        /// </summary>
        public const string Type = "type";

        /// <summary>
        /// The volume of a book or journal, especially for multi-volume works.
        /// </summary>
        public const string Volume = "volume";

        /// <summary>
        /// The year of publication or presentation of the work.
        /// </summary>
        public const string Year = "year";
    }
}
