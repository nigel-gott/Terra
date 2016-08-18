using System;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class TerrainConfig
    {
        public static readonly int MAXIMUM_TERRAIN_HEIGHT_WORLD_UNITS = (int) (Math.Pow(2, 16) / 2);
        public const int HEIGHTMAP_GRID_SIZE_IN_WORLD_UNITS = 1;
        public Vector3 CreateTerrainWorldSizeFromTile(TerrainTile tile)
        {
            return new Vector3(tile.Size * HEIGHTMAP_GRID_SIZE_IN_WORLD_UNITS, 10, tile.Size * HEIGHTMAP_GRID_SIZE_IN_WORLD_UNITS);
        }
    }
}