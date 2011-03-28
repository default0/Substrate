﻿using System;
using System.Collections.Generic;
using System.Text;
using NDesk.Options;
using NBT;

namespace NBToolkit
{
    public class PurgeOptions : TKOptions, IChunkFilterable
    {
        private OptionSet _filterOpt = null;
        private ChunkFilter _chunkFilter = null;

        public PurgeOptions ()
            : base()
        {
            _filterOpt = new OptionSet();
            _chunkFilter = new ChunkFilter();
        }

        public PurgeOptions (string[] args)
            : this()
        {
            Parse(args);
        }

        public override void Parse (string[] args)
        {
            base.Parse(args);

            _filterOpt.Parse(args);
            _chunkFilter.Parse(args);
        }

        public override void PrintUsage ()
        {
            Console.WriteLine("Usage: nbtoolkit purge [options]");
            Console.WriteLine();
            Console.WriteLine("Options for command 'purge':");

            _filterOpt.WriteOptionDescriptions(Console.Out);

            Console.WriteLine();
            _chunkFilter.PrintUsage();

            Console.WriteLine();
            base.PrintUsage();
        }

        public override void SetDefaults ()
        {
            base.SetDefaults();
        }

        public IChunkFilter GetChunkFilter ()
        {
            return _chunkFilter;
        }
    }

    class Purge : TKFilter
    {
        private PurgeOptions opt;

        public Purge (PurgeOptions o)
        {
            opt = o;
        }

        public override void Run ()
        {
            World world = new World(opt.OPT_WORLD);

            int affectedChunks = 0;
            foreach (Chunk chunk in new FilteredChunkList(world.GetChunkManager(), opt.GetChunkFilter())) {
                affectedChunks++;
                world.GetChunkManager().DeleteChunk(chunk.X, chunk.Z);
            }

            Console.WriteLine("Purged Chunks: " + affectedChunks);
        }
    }
}