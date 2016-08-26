using System;
using System.Collections.Generic;
using NigelGott.Terra.Protobufs;
using NigelGott.Terra.Terrain;
using UnityEngine;

namespace NigelGott.Terra.NetworkState
{
    public class NetworkManager : IDisposable
    {
        private readonly TerrainLoader terrainLoader;
        private readonly ServerConnection serverConnection;
        private readonly SingleThreadedTaskRunner singleThreadedTaskRunner;

        public NetworkManager(TerrainLoader terrainLoader)
        {
            this.terrainLoader = terrainLoader;
            serverConnection = new ServerConnection("127.0.0.1", 9023);
            singleThreadedTaskRunner = new SingleThreadedTaskRunner();
        }


        public WorldState LoadWorldState()
        {
            serverConnection.SendMessage(new RequestMessage
            {
                Type = RequestMessage.Types.RequestType.WorldState,
                PlayerName = "Nigel"
            });

            return serverConnection.ReadMessage(WorldState.Parser);
        }

        public void StartLoadingChunks(List<IntCoord> immediatelySurroundingChunks, int chunkSize)
        {
            singleThreadedTaskRunner.Submit(() =>
            {
                serverConnection.SendMessage(new RequestMessage
                {
                    PlayerName = "Nigel",
                    Type = RequestMessage.Types.RequestType.TerrainChunks
                });
                serverConnection.SendMessage(new ChunkRequest
                {
                    ChunkSize = chunkSize,
                    ChunkCoords = {immediatelySurroundingChunks}
                });

                for(var i = 0; i < immediatelySurroundingChunks.Count; i++)
                {
                    var chunkMessage = serverConnection.ReadMessage(ChunkMessage.Parser);

                    if (chunkMessage.Heights.Count != chunkSize*chunkSize)
                    {
                        throw new Exception("Asked for chunk of size " + chunkSize +
                                            " but instead received chunk with size " + chunkMessage.Heights.Count);
                    }

                    terrainLoader.QueueChunk(chunkMessage, chunkSize);
                }
            });
        }

        public void Dispose()
        {
            singleThreadedTaskRunner.Dispose();
            serverConnection.Dispose();
        }
    }
}