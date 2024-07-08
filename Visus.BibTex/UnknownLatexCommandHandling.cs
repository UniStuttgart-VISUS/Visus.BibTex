// <copyright file="UnknownLatexCommandHandling.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>


namespace Visus.BibTex {

    /// <summary>
    /// Possible ways how <see cref="LatexConverter"/> can handle Latex commands
    /// it does not know.
    /// </summary>
    public enum UnknownLatexCommandHandling {

        /// <summary>
        /// Ignores the command and all of its content.
        /// </summary>
        Discard,

        /// <summary>
        /// Emits the argument that is passed to the command literally, but
        /// discards the command itself.
        /// </summary>
        EmitArgument,

        /// <summary>
        /// Emits the Latex command literally.
        /// </summary>
        EmitLiterally
    }
}
