// <copyright file="Programme.cs" company="Visualisierungsinstitut der Universität Stuttgart">
// Copyright © 2024 Visualisierungsinstitut der Universität Stuttgart.
// Licensed under the MIT licence. See LICENCE file for details.
// </copyright>
// <author>Christoph Müller</author>

using BenchmarkDotNet.Running;
using Visus.BibTex.Benchmark;


BenchmarkRunner.Run<NameParser>();
