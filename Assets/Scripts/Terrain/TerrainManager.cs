using System;
using System.Collections.Generic;
using System.Linq;
using NigelGott.Terra.NetworkState;
using NigelGott.Terra.Protobufs;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class TerrainManager
    {
        private readonly GameObject terrainParent;
        private readonly TerrainLoader terrainLoader;
        private readonly Dictionary<Vector2, RenderedChunk> displayedChunks;

        private readonly HashSet<Vector2> chunksAlreadyLoading;

        private readonly List<RenderedChunk> freeChunks;
        private readonly NetworkManager networkManager;

        public TerrainManager(GameObject terrainParent, TerrainLoader terrainLoader, NetworkManager networkManager)
        {
            this.terrainParent = terrainParent;
            this.terrainLoader = terrainLoader;
            this.networkManager = networkManager;
            displayedChunks = new Dictionary<Vector2, RenderedChunk>();
            chunksAlreadyLoading = new HashSet<Vector2>();
            freeChunks = new List<RenderedChunk>();
        }

        public void Update()
        {
        }

        public bool TerrainAtPositionIsReady(Vector2 playerLocation)
        {
            return displayedChunks.ContainsKey(ChunkCalculator.WorldPositionToChunkPosition(playerLocation));
        }


        public void Update(GameState.GameState gameState)
        {
            var chunksSurroundingPlayer = ChunkCalculator.CalculateImmediatelySurroundingChunks(
                gameState.PlayerPosition, gameState.WorldSize);


            UnloadChunks(displayedChunks.Keys.Except(chunksSurroundingPlayer).ToList());

            var missingChunks = chunksSurroundingPlayer.Except(displayedChunks.Keys).ToList();

            var chunksToLoad = missingChunks.Except(chunksAlreadyLoading).ToList();

            chunksAlreadyLoading.UnionWith(chunksToLoad);

            var intCoords = chunksToLoad.Select(chunk => new IntCoord
            {
                X = (int) chunk.x,
                Y = (int) chunk.y
            }).ToList();

            if (intCoords.Count > 0)
            {
                networkManager.StartLoadingChunks(intCoords, TerrainConfig.ChunkSize);
            }

            var loadedChunk = terrainLoader.DequeueChunk();
            if (loadedChunk != null)
            {
                Debug.Log("Loaded chunk " + loadedChunk.Coord);
                chunksAlreadyLoading.Remove(loadedChunk.Coord);
                if (missingChunks.Contains(loadedChunk.Coord))
                {
                    RenderChunk(loadedChunk);
                }
            }
            
        }

        private void UnloadChunks(IEnumerable<Vector2> chunksToUnload)
        {
            foreach (var chunk in chunksToUnload)
            {
                freeChunks.Add(displayedChunks[chunk]);
                displayedChunks.Remove(chunk);
            }
        }

        private void RenderChunk(Chunk loadedChunk)
        {
            if (freeChunks.Count > 0)
            {
                var renderedChunk = freeChunks[0];
                UpdateNeighbors(renderedChunk, false);
                renderedChunk.RerenderAs(loadedChunk);
                displayedChunks[loadedChunk.Coord] = renderedChunk;
                UpdateNeighbors(renderedChunk, true);

                freeChunks.RemoveAt(0);
            }
            else
            {
                newRenderedChunk = new RenderedChunk(loadedChunk, terrainParent);
                displayedChunks[loadedChunk.Coord] = newRenderedChunk;
                UpdateNeighbors(newRenderedChunk, true);
            }
        }

        public static readonly List<Vector2> NeighborDirections = new List<Vector2>
        {
            Vector2.down, Vector2.left, Vector2.up, Vector2.right
        };

        private RenderedChunk newRenderedChunk;

        private void UpdateNeighbors(RenderedChunk renderedChunk, bool added)
        {
            foreach (var neighborDirection in NeighborDirections)
            {
                var neighboringChunk = renderedChunk.ChunkPosition + neighborDirection;
                if (displayedChunks.ContainsKey(neighboringChunk))
                {
                    var neighbor = displayedChunks[neighboringChunk];
                    neighbor.UpdateNeighbor(neighborDirection * -1, renderedChunk, added);
                    if (added)
                    {
                        renderedChunk.AddNeighbor(neighborDirection, neighbor);
                    }
                }
            }
            if (added)
            {
                renderedChunk.SetNeighbors();
            }
        }

        public float GetWorldHeightAt(Vector3 playerWorldPosition)
        {
            var playerChunkPosition = ChunkCalculator.WorldPositionToChunkPosition(new Vector2(playerWorldPosition.x, playerWorldPosition.z));
            return displayedChunks[playerChunkPosition].GetWorldHeight(playerWorldPosition);
        }
    }
}