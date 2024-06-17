// <copyright file="WellKnownTypes.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Provides well-known types of BibTex items.
    /// </summary>
    public static class WellKnownTypes {

        /// <summary>
        /// Citations for articles published in journals, magazines, or other
        /// periodic publications.
        /// </summary>
        public const string Article = "article";

        /// <summary>
        /// Citations for standalone books by one or more authors, including
        /// specifics like the publisher.
        /// </summary>
        public const string Book = "book";

        /// <summary>
        /// For printed works that are bound but lack a formal publisher or
        /// sponsoring institution.
        /// </summary>
        public const string Booklet = "booklet";

        /// <summary>
        /// References for papers presented at conferences, often
        /// interchangeable with inproceedings.
        /// </summary>
        public const string Conference = "conference";

        /// <summary>
        /// Citations for specific portions of a book, such as chapters or
        /// defined sections.
        /// </summary>
        public const string InBook = "inbook";

        /// <summary>
        /// Citations for parts of a book with their distinct title, typically
        /// housed within a larger collection.
        /// </summary>
        public const string InCollection = "incollection";

        /// <summary>
        /// References for articles or papers presented within the proceeding
        /// s of a conference.
        /// </summary>
        public const string InProceedings = "inproceedings";

        /// <summary>
        /// Technical documentation or manuals, often detailing software or
        /// hardware specifics.
        /// </summary>
        public const string Manual = "manual";

        /// <summary>
        /// Citations for Master's level theses detailing academic research at
        /// postgraduate levels.
        /// </summary>
        public const string MastersThesis = "mastersthesis";

        /// <summary>
        /// A catch-all entry type for references that do not neatly fit into
        /// standard categories.
        /// </summary>
        public const string Miscelleanous = "misc";

        /// <summary>
        /// Citations for doctoral dissertations, showcasing in-depth academic
        /// research and contributions.
        /// </summary>
        public const string PhdThesis = "phdthesis";

        /// <summary>
        /// For collections of academic papers presented at professional
        /// conferences or symposia.
        /// </summary>
        public const string Proceedings = "proceedings";

        /// <summary>
        /// Reports detailing technical aspects, often published by institutions
        /// or organistions.
        /// </summary>
        public const string TechnicalReport = "techreport";

        /// <summary>
        /// Works that have been draughted or completed but remain unpublished.
        /// </summary>
        public const string Unpublished = "unpublished";
    }
}
