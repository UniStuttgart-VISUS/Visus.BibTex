// <copyright file="NameTokenBuffer.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace Visus.BibTex {

    /// <summary>
    /// A buffer holding a fixed number of <see cref="NameToken"/>s for
    /// back tracking in <see cref="Name.Parse(ReadOnlySpan{char})"/>.
    /// </summary>
    /// <remarks>
    /// This is hacking around the problem that we cannot <c>stackalloc</c>
    /// <see cref="NameToken"/>, which behaves according to the specification,
    /// but I still do not understand why the compiler cannot derive the
    /// size of the <c>struct</c> in this case while it can for individual
    /// variables. Anyway, this structure provides an array-like access to
    /// a fixed number of tokens such that we do not switch on the index in
    /// the parser itself.
    /// </remarks>
    internal ref struct NameTokenBuffer {

        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        public int Length => 8;

        /// <summary>
        /// Gets or sets the token at the specified position.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public NameToken this[int index] {
            get => index switch {
                0 => this._t0,
                1 => this._t1,
                2 => this._t2,
                3 => this._t3,
                4 => this._t4,
                5 => this._t5,
                6 => this._t6,
                7 => this._t7,

                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };
            set {
                switch (index) {
                    case 0: this._t0 = value; break;
                    case 1: this._t1 = value; break;
                    case 2: this._t2 = value; break;
                    case 3: this._t3 = value; break;
                    case 4: this._t4 = value; break;
                    case 5: this._t5 = value; break;
                    case 6: this._t6 = value; break;
                    case 7: this._t7 = value; break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        #region Private fields
        private NameToken _t0, _t1, _t2, _t3, _t4, _t5, _t6, _t7;
        #endregion
    }
}
