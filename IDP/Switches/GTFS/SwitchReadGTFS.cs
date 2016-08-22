﻿// The MIT License (MIT)

// Copyright (c) 2016 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using GTFS;
using GTFS.IO;
using IDP.Processors;
using System;
using System.Collections.Generic;
using System.IO;

namespace IDP.Switches.GTFS
{
    /// <summary>
    /// Represents a switch to read a GTFS feed.
    /// </summary>
    class SwitchReadGTFS : Switch
    {
        /// <summary>
        /// Creates a new switch.
        /// </summary>
        public SwitchReadGTFS(string[] arguments)
            : base(arguments)
        {

        }

        /// <summary>
        /// Gets the names.
        /// </summary>
        public static string[] Names
        {
            get
            {
                return new string[] { "--rg", "--read-gtfs" };
            }
        }

        /// <summary>
        /// Parses this command into a processor given the arguments for this switch. Consumes the previous processors and returns how many it consumes.
        /// </summary>
        public override int Parse(List<Processor> previous, out Processor processor)
        {
            if (this.Arguments.Length != 1) { throw new ArgumentException("Exactly one argument is expected."); }

            var directory = new DirectoryInfo(this.Arguments[0]);
            if (!directory.Exists)
            {
                throw new FileNotFoundException("Directory not found.", directory.FullName);
            }

            // create the reader.
            var reader = new GTFSReader<GTFSFeed>(false);

            // build the get GTFS function.
            Func<global::GTFS.GTFSFeed> getGTFS = () =>
            {
                return reader.Read(new GTFSDirectorySource(directory));
            };
            processor = new IDP.Processors.GTFS.ProcessorGTFSSource(getGTFS);
            return 0;
        }
    }
}
