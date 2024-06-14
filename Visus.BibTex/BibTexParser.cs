// <copyright file="BibTexParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;


namespace Visus.BibTex {

    public static class BibTexParser<TBibItem> where TBibItem : new() {

        public static IEnumerable<TBibItem> Parse(TextReader reader) {
            using var lexer = new BibTexLexer(reader);
            var state = State.List;
            var sb = new StringBuilder();

            foreach (var l in lexer.Tokenise()) {
                if (!Transitions.TryGetValue((state, l.Type), out var xi)) {
                    throw new FormatException("TODO");
                }

                // If the transition said that we should remember the character
                // as part of the content, we do so.
                if (xi.Accumulate) {
                    sb.Append(l.Character);
                }

                switch (xi.To) {
                    case State.EntryKey:
                        if (state != xi.To) {
                            Debug.WriteLine($"type: {sb}");
                            sb.Clear();
                        }
                        break;

                    case State.FieldNameHead:
                        if (state == State.EntryKey) {
                            Debug.WriteLine($"key: {sb}");
                            sb.Clear();
                        }
                        break;

                    case State.FieldHead:
                        Debug.WriteLine($"field: {sb}");
                        sb.Clear();
                        break;
                }

                // Apply the transition.
                state = xi;
            }


            yield return new();
        }

        /// <summary>
        /// Enumerates all possible states of the parser.
        /// </summary>
        private enum State {
            /// <summary>
            /// In list, expecting the start of an entry.
            /// </summary>
            List = 0,

            /// <summary>
            /// The start of an entry indicated by the @.
            /// </summary>
            EntryStart,

            /// <summary>
            /// In the entry type.
            /// </summary>
            EntryType,

            /// <summary>
            /// In white space after the entry type.
            /// </summary>
            EntryTypeTail,

            /// <summary>
            /// In the entry key.
            /// </summary>
            EntryKey,

            /// <summary>
            /// In white space after the entry key.
            /// </summary>
            EntryKeyTail,

            FieldNameHead,
            FieldName,
            FieldNameTail,
            FieldHead,
            FieldQuoted,
            FieldBraced,
            EndEntry,

            CommentLine
        }

        #region Nested class Transition
        /// <summary>
        /// Specifies the target of a state transition.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="accumulate"></param>
        private class Transition(State to, bool accumulate = false) {
            public static implicit operator Transition(State to)
                => new Transition(to);
            public static implicit operator Transition((State, bool) xi)
                => new Transition(xi.Item1, xi.Item2);
            public static implicit operator State(Transition xi) => xi.To;

            /// <summary>
            /// Indicates whether the character that was read should be
            /// accumulated in the string buffer.
            /// </summary>
            public bool Accumulate { get; } = accumulate;

            /// <summary>
            /// Gets the new state.
            /// </summary>
            public State To { get; } = to;
        }
        #endregion

        private static readonly Dictionary<(State, BibTexTokenType), Transition> Transitions
            = new() {
                // Transition from list to begin of entry when we find an @.
                { (State.List, BibTexTokenType.At), State.EntryStart },
                { (State.List, BibTexTokenType.Quote), State.CommentLine },

                // The entry type starts at the first letter after the @.
                { (State.EntryStart, BibTexTokenType.WhiteSpace), State.EntryStart },
                { (State.EntryStart, BibTexTokenType.Letter), (State.EntryType, true) },

                // The entry type is only comprised of letters. If we reach {,
                // we are in the entry and accumulate the key.
                { (State.EntryType, BibTexTokenType.Letter), (State.EntryType, true) },
                { (State.EntryType, BibTexTokenType.WhiteSpace), State.EntryTypeTail },
                { (State.EntryType, BibTexTokenType.BraceLeft), State.EntryKey },

                // Eat white spaces after the type.
                { (State.EntryTypeTail, BibTexTokenType.WhiteSpace), State.EntryTypeTail },
                { (State.EntryTypeTail, BibTexTokenType.BraceLeft), State.EntryKey },

                // We accept almost anything for the key, but as for the entry
                // type, we bail out after the first white space.
                { (State.EntryKey, BibTexTokenType.At), (State.EntryKey, true) },
                { (State.EntryKey, BibTexTokenType.Character), (State.EntryKey, true) },
                { (State.EntryKey, BibTexTokenType.Digit), (State.EntryKey, true) },
                { (State.EntryKey, BibTexTokenType.Equals), (State.EntryKey, true) },
                { (State.EntryKey, BibTexTokenType.Hash), (State.EntryKey, true) },
                { (State.EntryKey, BibTexTokenType.Letter), (State.EntryKey, true) },

                { (State.EntryKey, BibTexTokenType.WhiteSpace), State.EntryKeyTail },
                { (State.EntryKey, BibTexTokenType.Comma), State.FieldNameHead },

                // Eat white spaces after the key.
                { (State.EntryKeyTail, BibTexTokenType.WhiteSpace), State.EntryKeyTail },
                { (State.EntryKeyTail, BibTexTokenType.Comma), State.FieldNameHead },

                // In white space before the field name.
                { (State.FieldNameHead, BibTexTokenType.NewLine), State.FieldNameHead },
                { (State.FieldNameHead, BibTexTokenType.WhiteSpace), State.FieldNameHead },

                { (State.FieldNameHead, BibTexTokenType.Letter), (State.FieldName, true) },

                // Parsing the field name. The end of the name is reached at the
                // first white space or the equals sign.
                { (State.FieldName, BibTexTokenType.Character), (State.FieldName, true) },
                { (State.FieldName, BibTexTokenType.Letter), (State.FieldName, true) },
                { (State.FieldName, BibTexTokenType.Digit), (State.FieldName, true) },

                { (State.FieldName, BibTexTokenType.WhiteSpace), State.FieldNameTail },
                { (State.FieldName, BibTexTokenType.Equals), State.FieldHead },

                // Eat white spaces after the name of the field.
                { (State.FieldNameTail, BibTexTokenType.WhiteSpace), State.FieldHead },
                { (State.FieldNameTail, BibTexTokenType.Equals), State.FieldHead },
            };
    }
}
