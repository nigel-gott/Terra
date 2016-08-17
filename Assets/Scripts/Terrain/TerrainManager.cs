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
                    var tileIfReady = terrainLoader.TileIfReady();
                    if (tileIfReady != null)
                    {
                        var terrainObject = new TerrainTileRenderer(tileIfReady, terrainConfig).BuildGameObject();
                        terrainObject.transform.parent = gameObject.transform;
                        terrainObject.transform.position = player.transform.position - new Vector3(0, 10, 0);
                        terrainLoader = null;
                    }
                }
                catch (ApplicationException e)
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