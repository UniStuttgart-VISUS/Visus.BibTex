// <copyright file="BibTexParser.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Visus.BibTex.Benchmark {

    /// <summary>
    /// Hosts the benchmarks for the BibTex parser.
    /// </summary>
    public class BibItemParser {

        [Params("@inproceedings{mueller:2022:power, author = {Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas}, booktitle = {Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)}, doi = {10.1109/BELIV57783.2022.00009}, month = {October}, pages = {38-46}, title = {Power Overwhelming: Quantifying the Energy Cost of Visualisation}, year = {2022}}",
            """@inproceedings{mueller:2022:power,author="Müller, Christoph and Heinemann, Moritz and Weiskopf, Daniel and Ertl, Thomas",booktitle="Proceedings of the 2022 IEEE Workshop on Evaluation and Beyond – Methodological Approaches for Visualization (BELIV)",doi="10.1109/BELIV57783.2022.00009",month="October",pages="38-46",title="Power Overwhelming: Quantifying the Energy Cost of Visualisation",year="2022"}"""
            )]
        public string Input { get; set; } = null!;

        [Benchmark]
        public List<BibItem> ParseString() => BibTexParser.Parse(new StringReader(this.Input)).ToList();
    }
}
