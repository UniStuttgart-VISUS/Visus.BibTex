﻿// <copyright file="BibItem.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Visus.BibTex {

    /// <summary>
    /// The default representation of a BibTex entry.
    /// </summary>
    public sealed class BibItem : IFormattable,
            IEnumerable<KeyValuePair<string, object>> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="entryType">The type of the item, which is typically one of
        /// the constants in <see cref="WellKnownTypes"/>.</param>
        /// <param name="key">A unique key used to reference the item.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="entryType"/> is <c>null</c>, or if
        /// <paramref name="key"/> is <c>null</c>.</exception>
        public BibItem(string entryType, string key) {
            this.Key = key
                ?? throw new ArgumentNullException(nameof(key));
            this.EntryType = entryType
                ?? throw new ArgumentNullException(nameof(entryType));
            this._fields = new();
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the address of the publisher or institution, typically
        /// the city.
        /// </summary>
        public string? Address {
            get => this.GetField(WellKnownFields.Address);
            set => this.SetField(WellKnownFields.Address, value);
        }

        /// <summary>
        /// Gets or sets annotations for the bibliography, providing additional
        /// context or remarks.
        /// </summary>
        public string? Annote {
            get => this.GetField(WellKnownFields.Annote);
            set => this.SetField(WellKnownFields.Annote, value);
        }

        /// <summary>
        /// Gets or sets the authors of the work, typically listed in order of
        /// contribution.
        /// </summary>
        public IEnumerable<Name>? Author {
            get => this.GetField<IEnumerable<Name>>(WellKnownFields.Author);
            set => this.SetField(WellKnownFields.Author, value);
        }

        /// <summary>
        /// Gets or sets the title of the book when referencing a part of it,
        /// such as a chapter.
        /// </summary>
        public string? BookTitle {
            get => this.GetField(WellKnownFields.BookTitle);
            set => this.SetField(WellKnownFields.BookTitle, value);
        }

        /// <summary>
        /// Gets or sets the specific chapter or section number within a larger
        /// work.
        /// </summary>
        public string? Chapter {
            get => this.GetField(WellKnownFields.Chapter);
            set => this.SetField(WellKnownFields.Chapter, value);
        } 

        /// <summary>
        /// Gets or sets cross-referencing another BibTeX entry, useful for
        /// connecting related works.
        /// </summary>
        public string? CrossReference {
            get => this.GetField(WellKnownFields.CrossReference);
            set => this.SetField(WellKnownFields.CrossReference, value);
        }

        /// <summary>
        /// Gets or sets the Digital Object Identifier, a unique code for
        /// electronic publications.
        /// </summary>
        public string? Doi {
            get => this.GetField(WellKnownFields.Doi);
            set => this.SetField(WellKnownFields.Doi, value);
        }

        /// <summary>
        /// Gets or sets the edition of the book or publication, often specified
        /// for later editions.
        /// </summary>
        public string? Edition {
            get => this.GetField(WellKnownFields.Edition);
            set => this.SetField(WellKnownFields.Edition, value);
        }

        /// <summary>
        /// Gets or sets  the editors of a book or collection, often in place of
        /// authors for edited volumes.
        /// </summary>
        public IEnumerable<Name>? Editor {
            get => this.GetField<IEnumerable<Name>>(WellKnownFields.Editor);
            set => this.SetField(WellKnownFields.Editor, value);
        }

        /// <summary>
        /// Gets or sets the email address associated with the work or the
        /// primary contact.
        /// </summary>
        public string? Email {
            get => this.GetField(WellKnownFields.Email);
            set => this.SetField(WellKnownFields.Email, value);
        }

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        public string EntryType { get; }

        /// <summary>
        /// Gets or sets the institution responsible for the work, often for
        /// technical reports.
        /// </summary>
        public string? Institution {
            get => this.GetField(WellKnownFields.Institution);
            set => this.SetField(WellKnownFields.Institution, value);
        }

        /// <summary>
        /// Gets or sets the academic journal in which the work was published.
        /// </summary>
        public string? Journal {
            get => this.GetField(WellKnownFields.Journal);
            set => this.SetField(WellKnownFields.Journal, value);
        }

        /// <summary>
        /// Gets the key of the entry.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the month of publication, which can be combined with
        /// the <see cref="Year"/> property.
        /// </summary>
        public string? Month {
            get => this.GetField(WellKnownFields.Month);
            set => this.SetField(WellKnownFields.Month, value);
        }

        /// <summary>
        /// Gets or sets a dditional notes or information related to the
        /// reference.
        /// </summary>
        public string? Note {
            get => this.GetField(WellKnownFields.Note);
            set => this.SetField(WellKnownFields.Note, value);
        }

        /// <summary>
        /// Gets or sets the number of a journal, magazine, technical report, or
        /// other serialised publication.
        /// </summary>
        public string? Number {
            get => this.GetField(WellKnownFields.Number);
            set => this.SetField(WellKnownFields.Number, value);
        }

        /// <summary>
        /// Gets or sets the organisation responsible for hosting a conference
        /// or publishing a manual.
        /// </summary>
        public string? Organisation {
            get => this.GetField(WellKnownFields.Organisation);
            set => this.SetField(WellKnownFields.Organisation, value);
        }

        /// <summary>
        /// Gets or sets the page range for the referenced material, usually
        /// denoted as start-end.
        /// </summary>
        public string? Pages {
            get => this.GetField(WellKnownFields.Pages);
            set => this.SetField(WellKnownFields.Pages, value);
        }

        /// <summary>
        /// Gets or sets the entity responsible for publishing the work, whether
        /// a company or institution.
        /// </summary>
        public string? Publisher {
            get => this.GetField(WellKnownFields.Publisher);
            set => this.SetField(WellKnownFields.Publisher, value);
        }

        /// <summary>
        /// Gets or sets the academic institution where a thesis or dissertation
        /// was presented.
        /// </summary>
        public string? School {
            get => this.GetField(WellKnownFields.School);
            set => this.SetField(WellKnownFields.School, value);
        }

        /// <summary>
        /// Gets or sets the series of books or journals in which the work was
        /// published.
        /// </summary>
        public string? Series {
            get => this.GetField(WellKnownFields.Series);
            set => this.SetField(WellKnownFields.Series, value);
        }

        /// <summary>
        /// Gets or sets the main title of the referenced work.
        /// </summary>
        public string? Title {
            get => this.GetField(WellKnownFields.Title);
            set => this.SetField(WellKnownFields.Title, value);
        }

        /// <summary>
        /// Gets or sets the specific type of a technical report or thesis.
        /// </summary>
        public string? Type {
            get => this.GetField(WellKnownFields.Type);
            set => this.SetField(WellKnownFields.Type, value);
        }

        /// <summary>
        /// Gets or sets the volume of a book or journal, especially for
        /// multi-volume works.
        /// </summary>
        public string? Volume {
            get => this.GetField(WellKnownFields.Volume);
            set => this.SetField(WellKnownFields.Volume, value);
        }

        /// <summary>
        /// Gets or sets the year of publication or presentation of the work.
        /// </summary>
        public string? Year {
            get => this.GetField(WellKnownFields.Year);
            set => this.SetField(WellKnownFields.Year, value);
        }
        #endregion

        #region Public indexers
        /// <summary>
        /// Gets or sets the value of the field with the specified name.
        /// </summary>
        /// <param name="field">The name of the field to be retrieved.</param>
        /// <returns>The value of the field or <c>null</c> if no such field
        /// exists.</returns>
        public object? this[string field] {
            get => this._fields.TryGetValue(field, out var v) ? v : null;
            set => this.SetField(field, value);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Answer whether the item contains a <paramref name="field"/> with the
        /// specified name that has a non-<c>null</c> value.
        /// </summary>
        /// <param name="field">The field to be checked.</param>
        /// <returns><c>true</c> if the field is set and its value is not
        /// <c>null</c>, <c>false</c> otherwise.</returns>
        public bool ContainsField(string field)
            => this._fields.ContainsKey(field)
            && (this._fields[field] != null);

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => this._fields.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this._fields.GetEnumerator();

        /// <inheritdoc />
        public string ToString(string? format,
                IFormatProvider? formatProvider = null) {
            var splitPos = format?.IndexOf('.');

            // Split the format for the entry from the format for the names.
            var entryFormat = ((splitPos != null) && (splitPos >= 0))
                ? format!.Substring(0, splitPos.Value)
                : format;
            var nameFormat = ((splitPos != null) && (splitPos >= 0))
                ? format!.Substring(splitPos.Value + 1)
                : NameFormats.SurnamesSuffixChristianNameMiddleNames;

            // Make sure that the formats are not empty.
            if (string.IsNullOrWhiteSpace(entryFormat)) {
                entryFormat = "s4";
            }
            if (string.IsNullOrEmpty(nameFormat)) {
                nameFormat = NameFormats.SurnamesSuffixChristianNameMiddleNames;
            }

            // Now we can parse the entry format.
            var indent = string.Empty;
            var multiplier = 0;
            var separator = string.Empty;
            var space = string.Empty;
            var quoted = false;

            foreach (var f in entryFormat) {
                switch (f) {
                    case 'c':
                        indent = string.Empty;
                        separator = string.Empty;
                        space = string.Empty;
                        break;

                    case 'C':
                        indent = string.Empty;
                        separator = " ";
                        space = " ";
                        break;

                    case 'q':
                        quoted = true;
                        break;

                    case 'l':
                        indent = string.Empty;
                        multiplier = 0;
                        separator = Environment.NewLine;
                        space = " ";
                        break;

                    case 's':
                    case 'S':
                        indent = " ";
                        multiplier = 0;
                        separator = Environment.NewLine;
                        space = " ";
                        break;

                    case 't':
                    case 'T':
                        indent = "\t";
                        multiplier = 0;
                        separator = Environment.NewLine;
                        space = " ";
                        break;

                    default:
                        if (char.IsDigit(f)) {
                            multiplier *= 10;
                            multiplier += (f - '0');
                        }
                        break;
                }
            }

            if (indent.Length > 0) {
                indent = new string(indent.Single(), Math.Max(1, multiplier));
            }

            var retval = new StringBuilder("@");
            retval.Append(this.EntryType).Append("{");
            retval.Append(this.Key);

            foreach (var f in this._fields.Keys.OrderBy(f => f)) {
                var value = this[f];
                if (value == null) {
                    // Skip fields without a value.
                    continue;
                }

                retval.Append(',').Append(separator);
                retval.Append(indent);
                retval.Append(f).Append(space).Append('=').Append(space);

                if (quoted) {
                    retval.Append('"');
                } else {
                    retval.Append("{");
                }

                if (value is IEnumerable<Name> names) {
                    var first = true;

                    foreach (var n in names) {
                        if (first) {
                            first = false;
                        } else {
                            retval.Append(" and ");
                        }

                        var s = EscapeField(n.ToString(nameFormat), quoted);
                        retval.Append(s);
                    }

                } else {
                    retval.Append(EscapeField(value.ToString()!, quoted));
                }

                if (quoted) {
                    retval.Append('"');
                } else {
                    retval.Append("}");
                }
            }

            if (separator == Environment.NewLine) {
                retval.Append(separator);
            }

            retval.Append("}");
            return retval.ToString();
        }

        /// <inheritdoc />
        public override string ToString() => this.ToString(null, null);
        #endregion

        #region Private class methods
        /// <summary>
        /// Escapes the field value <paramref name="str"/> for braced or quoted
        /// fields.
        /// </summary>
        /// <param name="str">The string to be escaped.</param>
        /// <param name="quoted"><c>true</c> to assume a quoted string,
        /// <c>false</c> for braced ones.</param>
        /// <returns>The escaped string.</returns>
        private static string EscapeField(string str, bool quoted) {
            if (string.IsNullOrWhiteSpace(str)) {
                return str;
            }

            var retval = new StringBuilder(str);

            if (quoted) {
                // If we are in a quoted string, quotes must be in braces.
                int braces = 0;

                for (int i = 0; i < retval.Length; ++i) {
                    switch (retval[i]) {
                        case '{':
                            ++braces;
                            break;

                        case '}':
                            if (braces > 0) {
                                --braces;
                            }
                            break;

                        case '"':
                            if (braces < 1) {
                                retval.Insert(i++, '{');
                                retval.Insert(++i, '}');
                            }
                            break;
                    }
                }

            } else {
                // If we are in a braced string, it is illegal to have a
                // non-escaped @ sign.
                for (int i = 0; i < retval.Length; ++i) {
                    if (retval[i] == '@') {
                        if ((i == 0) || (retval[i - 1] != '\\')) {
                            retval.Insert(i++, '\\');
                        }
                    }
                }
            }

            return (retval.Length == str.Length) ? str : retval.ToString();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets a <see cref="string"/>-valued field or the string
        /// representation of the field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private string? GetField(string field) {
            if (this._fields.TryGetValue(field, out var value)) {
                switch (value) {
                    case string s:
                        return s;

                    case IEnumerable<Name> n:
                        return string.Join(" and ", n);

                    default:
                        return value?.ToString();
                }

            } else {
                return null;
            }
        }

        /// <summary>
        /// Gets a <typeparamref name="TField"/>-valued field or <c>null</c> if
        /// the field has a different type.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="field"></param>
        /// <returns></returns>
        private TField? GetField<TField>(string field) where TField : class {
            if (this._fields.TryGetValue(field, out var value)) {
                return value as TField;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Sets or erases a field.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        private void SetField<TField>(string field, TField? value) {
            if (value != null) {
                this._fields[field] = value;

            } else if (this._fields.ContainsKey(field)) {
                this._fields.Remove(field);
            }
        }
        #endregion

        #region Private fields
        private Dictionary<string, object> _fields;
        #endregion
    }
}
