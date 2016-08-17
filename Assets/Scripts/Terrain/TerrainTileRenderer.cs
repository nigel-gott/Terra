using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class TerrainTileRenderer
    {
        private readonly TerrainTile tile;
        private readonly TerrainConfig config;

        public TerrainTileRenderer(TerrainTile tile, TerrainConfig config)
        {
            this.tile = tile;
            this.config = config;
        }

        public GameObject BuildGameObject()
        {
            TerrainData terrainData = new TerrainData();

            terrainData.heightmapResolution = tile.Size;
            terrainData.size = config.CreateTerrainWorldSizeFromTile(tile);
            terrainData.baseMapResolution = tile.Size;
            terrainData.SetDetailResolution(tile.Size, 8);
            terrainData.SetHeights(0, 0, tile.heights);

            return UnityEngine.Terrain.CreateTerrainGameObject(terrainData);
        }
    }
}