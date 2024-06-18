// <copyright file="NameParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;


namespace Visus.BibTex.Benchmark {

    /// <summary>
    /// Hosts the benchmarks for the name parser.
    /// </summary>
    public class NameParser {

        [Params("Honecker, Erich",
            "Erich Honecker",
            "Honecker, Erich and Ulbricht, Walter",
            "Erich Honekcer and Walter Ulbricht",
            "Ulbricht, Walter Ernst Paul and Honecker, Erich Ernst Paul and Maleuda, Günther and Mittag, Günter and Schabowski, Günter",
            "Walter Ernst Paul Ulbricht and Erich Ernst Paul Honecker and Günther Maleuda and Günter Mittag and Günter Schabowski"
            )]
        public string Input { get; set; } = null!;

        [Benchmark]
        public List<Name> ParseList() => Name.Parse(this.Input).ToList();
    }
}
