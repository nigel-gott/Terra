using System;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class TerrainConfig
    {
        public static readonly int ChunkSize = 513;

        public static readonly int MaximumTerrainHeightWorldUnits = (int) Math.Pow(2, 16);
        public const int HeightmapGridSizeInWorldUnits = 1;

        public static readonly int ChunkWorldUnitSize = ChunkSize*HeightmapGridSizeInWorldUnits;

        public static Vector3 CreateTerrainWorldSizeFromTile(Chunk tile)
        {
            return new Vector3(tile.Size * HeightmapGridSizeInWorldUnits, 1000, tile.Size * HeightmapGridSizeInWorldUnits);
        }
    }
}