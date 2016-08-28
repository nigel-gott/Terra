using System.Collections.Generic;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    internal class RenderedChunk
    {
        private readonly GameObject terrainGameObject;
        private Chunk loadedChunk;
        private readonly GameObject parent;
        private readonly ChunkRenderer chunkRenderer = new ChunkRenderer();
        public readonly UnityEngine.Terrain Terrain;

        private readonly Dictionary<Vector2, RenderedChunk> neighbors;

        public RenderedChunk(Chunk loadedChunk, GameObject parent)
        {
            Debug.Log("Rendering chunk " + loadedChunk.Coord);

            this.loadedChunk = loadedChunk;
            this.parent = parent;
            terrainGameObject = chunkRenderer.CreateAndSpawnChunk(loadedChunk, parent);
            Terrain = terrainGameObject.GetComponent<UnityEngine.Terrain>();
            neighbors = new Dictionary<Vector2, RenderedChunk>();
        }

        public Vector2 ChunkPosition
        {
            get { return loadedChunk.Coord; }
        }

        public void RerenderAs(Chunk loadedChunk)
        {
            this.loadedChunk = loadedChunk;
            Debug.Log("Re-Rendering chunk " + loadedChunk.Coord);

            Terrain.terrainData.SetHeights(0,0,loadedChunk.heights);
            
            chunkRenderer.SpawnChunk(loadedChunk, parent, terrainGameObject);
        }

        public float GetWorldHeight(Vector3 worldPosition)
        {
            return Terrain.SampleHeight(worldPosition);
        }

        public void UpdateNeighbor(Vector2 direction, RenderedChunk neighbor, bool added)
        {
            if (added)
            {
                AddNeighbor(direction, neighbor);
            }
            else
            {
                neighbors.Remove(direction);
            }

            SetNeighbors();
        }

        public void SetNeighbors()
        {
            Terrain.SetNeighbors(GetNeighborTerrainOrNull(Vector2.left), GetNeighborTerrainOrNull(Vector2.up),
                GetNeighborTerrainOrNull(Vector2.right), GetNeighborTerrainOrNull(Vector2.down));
        }

        private UnityEngine.Terrain GetNeighborTerrainOrNull(Vector2 direction)
        {
            RenderedChunk result;
            neighbors.TryGetValue(direction, out result);
            return result == null ? null : result.Terrain;
        }

        public void AddNeighbor(Vector2 direction, RenderedChunk neighbor)
        {
            neighbors[direction] = neighbor;
        }
    }
}