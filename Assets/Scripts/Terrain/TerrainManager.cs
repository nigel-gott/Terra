using System;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class TerrainManager : MonoBehaviour
    {
        public GameObject player;

        private TerrainLoader terrainLoader;
        private TerrainConfig terrainConfig;

        void Start()
        {
            terrainConfig = new TerrainConfig();
            LoadTerrain();
        }

        void Update()
        {
            if (terrainLoader != null)
            {
                try
                {
                    var tilesIfReady = terrainLoader.TilesIfReady();
                    if (tilesIfReady != null)
                    {
                        foreach(TerrainTile tile in tilesIfReady)
                        {
                            var terrainObject = new TerrainTileRenderer(tile, terrainConfig).BuildGameObject();
                            terrainObject.transform.parent = gameObject.transform;
                            var spawnLoc = new Vector3(tile.Size * tile.X, -100, tile.Size * tile.Y);
                            Debug.Log("Spawning chunk at " + spawnLoc);
                            terrainObject.transform.position = player.transform.position + spawnLoc;
                        }

                        terrainLoader = null;
                    }
                }
                catch (Exception e)
                {
                    terrainLoader = null;
                    throw e;
                }
            
            }
        }

        public void LoadTerrain()
        {
            terrainLoader = new TerrainLoader();
        }
    }
}