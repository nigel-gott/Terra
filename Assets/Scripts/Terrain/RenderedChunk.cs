using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    internal class RenderedChunk
    {
        private GameObject terrainGameObject;
        private readonly GameObject parent;

        public RenderedChunk(Chunk loadedChunk, GameObject parent)
        {
            Debug.Log("Rendering chunk " + loadedChunk.Coord);

            this.parent = parent;
            terrainGameObject = new ChunkRenderer().RenderChunk(loadedChunk, parent);
        }

        public void RerenderAs(Chunk loadedChunk)
        {
            Debug.Log("Re-Rendering chunk " + loadedChunk.Coord);

            Object.Destroy(terrainGameObject);
            terrainGameObject = new ChunkRenderer().RenderChunk(loadedChunk, parent);
        }

        public float GetWorldHeight(Vector2 worldPosition)
        {
            var terrainData = terrainGameObject.GetComponent<UnityEngine.Terrain>();
            return terrainData.SampleHeight(worldPosition);
        }
    }
}