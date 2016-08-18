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
            terrainData.baseMapResolution = Mathf.Clamp(tile.Size, 16, 2048);
            terrainData.SetDetailResolution(tile.Size, 8);
            terrainData.SetHeights(0, 0, tile.heights);

            return UnityEngine.Terrain.CreateTerrainGameObject(terrainData);
        }
    }
}