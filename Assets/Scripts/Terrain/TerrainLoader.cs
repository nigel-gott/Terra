using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NigelGott.Terra.Protobufs;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class TerrainLoader
    {
        private readonly ConcurrentQueue<Chunk> unprocessedChunks;

        public TerrainLoader()
        {
            this.unprocessedChunks = new ConcurrentQueue<Chunk>();
        }


        public void QueueChunk(ChunkMessage chunkMessage, int chunkSize)
        {
            Debug.Log("Received chunk at" + chunkMessage.X + ", " + chunkMessage.Y + " with " +
                      chunkMessage.Heights.Count + " heights...");

            var heights = Build2DHeightMapArray(chunkMessage, chunkSize);

            unprocessedChunks.Enqueue(new Chunk(heights, chunkMessage.Y, chunkMessage.X));
        }

        private static float[,] Build2DHeightMapArray(ChunkMessage chunkMessage, int heightmapSize)
        {
            var heights = new float[heightmapSize, heightmapSize];

            for (var y = 0; y < heightmapSize; y++)
            {
                for (var x = 0; x < heightmapSize; x++)
                {
                    
                    
                    heights[y, x] = (float) ((ushort) chunkMessage.Heights[x + y*heightmapSize])/
                                    TerrainConfig.MaximumTerrainHeightWorldUnits;
                }
            }
            return heights;
        }

        public List<Chunk> DequeueAllChunks()
        {
            Chunk result;

            var chunks = new List<Chunk>();
            while (unprocessedChunks.TryDequeue(out result))
            {
                chunks.Add(result);
            }

            return chunks;
        }

        public Chunk DequeueChunk()
        {
            Chunk result;
            var success = unprocessedChunks.TryDequeue(out result);
            return success ? result : null;
        }
    }
}