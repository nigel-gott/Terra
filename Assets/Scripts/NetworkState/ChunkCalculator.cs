using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Google.Protobuf;
using NigelGott.Terra.Protobufs;
using NigelGott.Terra.Terrain;
using UnityEngine;

namespace NigelGott.Terra.NetworkState
{
    public class ChunkCalculator
    {
        private static int size = 6;

        private static HashSet<Vector2> Directions = new HashSet<Vector2> {
            Vector2.zero,
            Vector2.up,
            Vector2.up + Vector2.right,
            Vector2.right,
            Vector2.right + Vector2.down,
            Vector2.down,
            Vector2.left + Vector2.down,
            Vector2.left,
            Vector2.left + Vector2.up
        };

        static ChunkCalculator()
        {
            HashSet<Vector2> updatedDirections = new HashSet<Vector2>(Directions);
            foreach (var direction in Directions)
            {
                foreach (var direction2 in Directions)
                {
                    updatedDirections.Add(direction + direction2);
                }
            }
            Directions = updatedDirections;
        }
        

        public static List<Vector2> CalculateImmediatelySurroundingChunks(Vector2 playerCoord, int worldSize)
        {
            var numChunks = worldSize / TerrainConfig.ChunkSize;
            var playerChunkLoc = WorldPositionToChunkPosition(playerCoord);
            

            return (from direction in Directions
                let chunks = new Rect(0, 0, numChunks, numChunks)
                let surroundingChunk = direction + playerChunkLoc
                where chunks.Contains(surroundingChunk)
                select surroundingChunk).ToList();
        }

        public static Vector2 WorldPositionToChunkPosition(Vector2 playerCoord)
        {
            return new Vector2((int) (playerCoord.x/TerrainConfig.ChunkWorldUnitSize),
                (int) (playerCoord.y/TerrainConfig.ChunkWorldUnitSize));
        }
    }
}