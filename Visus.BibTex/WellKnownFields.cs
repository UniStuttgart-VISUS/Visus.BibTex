﻿// <copyright file="WellKnownFields.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Visus.BibTex {

    /// <summary>
    /// Enumerates the names of well-known BibTex fields, which can be accessed
    /// via strongly typed properties of the default <see cref="BibItem"/>.
    /// </summary>
    public static class WellKnownFields {

        /// <summary>
        /// Gets all fields required for the specified <paramref name="type"/>
        /// of BibTex entry.
        /// </summary>
        /// <param name="type">The type to search the fields for.</param>
        /// <returns>The names of all fields required for
        /// <paramref name="type"/>.</returns>
        public static IEnumerable<string> GetRequired(string type) {
            var fields = typeof(WellKnownFields)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(string));

            foreach (var f in fields) {
                if (RequiredBibTexFieldAttribute.IsRequired(f, type)) {
                    var retval = f.GetValue(null) as string;
                    if (retval != null) {
                        yield return retval;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all fields supported for the specified <paramref name="type"/>
        /// of BibTex entry.
        /// </summary>
        /// <param name="type">The type to search the fields for.</param>
        /// <returns>The names of all fields supported for
        /// <paramref name="type"/>.</returns>
        public static IEnumerable<string> GetSupported(string type) {
            var fields = typeof(WellKnownFields)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(string));

            foreach (var f in fields) {
                if (SupportedBibTexFieldAttribute.IsSupported(f, type)) {
                    var retval = f.GetValue(null) as string;
                    if (retval != null) {
                        yield return retval;
                    }
                }
            }
        }


        /// <summary>
        /// The address of the publisher or institution, typically the city.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Book)]
        [RequiredBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Manual)]
        [SupportedBibTexField(WellKnownTypes.MastersThesis)]
        [SupportedBibTexField(WellKnownTypes.PhdThesis)]
        [SupportedBibTexField(WellKnownTypes.Proceedings)]
        public const string Address = "address";

        /// <summary>
        /// Annotations for the bibliography, providing additional context or
        /// remarks.
        /// </summary>
        [SupportedBibTexField]
        public const string Annote = "annote";

        /// <summary>
        /// The authors of the work, typically listed in order of contribution.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.Book)]
        [RequiredBibTexField(WellKnownTypes.Booklet)]
        [RequiredBibTexField(WellKnownTypes.Conference)]
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.InCollection)]
        [RequiredBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Manual)]
        [RequiredBibTexField(WellKnownTypes.MastersThesis)]
        [RequiredBibTexField(WellKnownTypes.PhdThesis)]
        [RequiredBibTexField(WellKnownTypes.TechnicalReport)]
        [RequiredBibTexField(WellKnownTypes.Unpublished)]
        public const string Author = "author";

        /// <summary>
        /// The title of the book when referencing a part of it, such as a
        /// chapter.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Conference)]
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.InCollection)]
        [RequiredBibTexField(WellKnownTypes.InProceedings)]
        public const string BookTitle = "booktitle";

        /// <summary>
        /// The specific chapter or section number within a larger work.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Book)]
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        public const string Chapter = "chapter";

        /// <summary>
        /// Cross-referencing another BibTeX entry, useful for connecting
        /// related works.
        /// </summary>
        [SupportedBibTexField]
        public const string CrossReference = "crossref";

        /// <summary>
        /// The Digital Object Identifier, a unique code for electronic
        /// publications.
        /// </summary>
        [SupportedBibTexField]
        public const string Doi = "doi";

        /// <summary>
        /// The edition of the book or publication, often specified for later
        /// editions.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.Manual)]
        public const string Edition = "edition";

        /// <summary>
        /// The editors of a book or collection, often in place of authors for
        /// edited volumes.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Proceedings)]
        public const string Editor = "editor";

        /// <summary>
        /// The email address associated with the work or the primary contact.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// The way how a boolket was published.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Miscelleanous)]
        public const string HowPublished = "howpublished";

        /// <summary>
        /// The institution responsible for the work, often for technical
        /// reports.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.TechnicalReport)]
        [RequiredBibTexField(WellKnownTypes.Unpublished)]
        public const string Institution = "institution";

        /// <summary>
        /// The academic journal in which the work was published.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Article)]
        public const string Journal = "journal";

        /// <summary>
        /// The month of publication, which can be combined with the
        /// year field.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Manual)]
        [SupportedBibTexField(WellKnownTypes.MastersThesis)]
        [SupportedBibTexField(WellKnownTypes.PhdThesis)]
        [SupportedBibTexField(WellKnownTypes.Proceedings)]
        public const string Month = "month";

        /// <summary>
        /// Additional notes or information related to the reference.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.Manual)]
        [SupportedBibTexField(WellKnownTypes.MastersThesis)]
        [SupportedBibTexField(WellKnownTypes.Miscelleanous)]
        [SupportedBibTexField(WellKnownTypes.PhdThesis)]
        public const string Note = "note";

        /// <summary>
        /// The number of a journal, magazine, technical report, or other
        /// serialised publication.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Proceedings)]
        [RequiredBibTexField(WellKnownTypes.TechnicalReport)]
        public const string Number = "number";

        /// <summary>
        /// The organisation responsible for hosting a conference or publishing
        /// a manual.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Manual)]
        public const string Organisation = "organization";

        /// <summary>
        /// The page range for the referenced material, usually denoted
        /// as start-end.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        public const string Pages = "pages";

        /// <summary>
        /// The entity responsible for publishing the work, whether a company
        /// or institution.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Book)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Proceedings)]
        public const string Publisher = "publisher";

        /// <summary>
        /// The academic institution where a thesis or dissertation was
        /// presented.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.MastersThesis)]
        [RequiredBibTexField(WellKnownTypes.PhdThesis)]
        public const string School = "school";

        /// <summary>
        /// The series of books or journals in which the work was published.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Proceedings)]
        public const string Series = "series";

        /// <summary>
        /// The main title of the referenced work.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.Book)]
        [RequiredBibTexField(WellKnownTypes.Booklet)]
        [RequiredBibTexField(WellKnownTypes.Conference)]
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.InCollection)]
        [RequiredBibTexField(WellKnownTypes.InProceedings)]
        [RequiredBibTexField(WellKnownTypes.Manual)]
        [RequiredBibTexField(WellKnownTypes.MastersThesis)]
        [RequiredBibTexField(WellKnownTypes.PhdThesis)]
        [RequiredBibTexField(WellKnownTypes.Proceedings)]
        [RequiredBibTexField(WellKnownTypes.TechnicalReport)]
        [RequiredBibTexField(WellKnownTypes.Unpublished)]
        public const string Title = "title";

        /// <summary>
        /// The specific type of a technical report or thesis.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.MastersThesis)]
        [SupportedBibTexField(WellKnownTypes.PhdThesis)]
        public const string Type = "type";

        /// <summary>
        /// The volume of a book or journal, especially for multi-volume works.
        /// </summary>
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.Booklet)]
        [SupportedBibTexField(WellKnownTypes.Conference)]
        [SupportedBibTexField(WellKnownTypes.Article)]
        [SupportedBibTexField(WellKnownTypes.InCollection)]
        [SupportedBibTexField(WellKnownTypes.InProceedings)]
        [SupportedBibTexField(WellKnownTypes.Proceedings)]
        public const string Volume = "volume";

        /// <summary>
        /// The year of publication or presentation of the work.
        /// </summary>
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.Book)]
        [RequiredBibTexField(WellKnownTypes.Booklet)]
        [RequiredBibTexField(WellKnownTypes.Conference)]
        [RequiredBibTexField(WellKnownTypes.Article)]
        [RequiredBibTexField(WellKnownTypes.InCollection)]
        [RequiredBibTexField(WellKnownTypes.InProceedings)]
        [RequiredBibTexField(WellKnownTypes.Manual)]
        [RequiredBibTexField(WellKnownTypes.MastersThesis)]
        [RequiredBibTexField(WellKnownTypes.PhdThesis)]
        [RequiredBibTexField(WellKnownTypes.Proceedings)]
        [RequiredBibTexField(WellKnownTypes.TechnicalReport)]
        [RequiredBibTexField(WellKnownTypes.Unpublished)]
        public const string Year = "year";
    }
}
