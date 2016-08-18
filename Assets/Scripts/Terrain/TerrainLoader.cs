using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Google.Protobuf;
using NigelGott.Terra.Protobufs;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class TerrainLoader
    {
        private readonly IAsyncResult asyncResult;
        private readonly ASyncLoadTerrainTilesFromRemote aSyncLoadTerrainTilesFromRemote;


        public TerrainLoader()
        {
            aSyncLoadTerrainTilesFromRemote = this.LoadTerrainTilesFromRemote;
            asyncResult = aSyncLoadTerrainTilesFromRemote.BeginInvoke(null, null);
        }

        public List<TerrainTile> TilesIfReady()
        {
            return asyncResult.IsCompleted
                ? aSyncLoadTerrainTilesFromRemote.EndInvoke(asyncResult)
                : null;
        }

        private delegate List<TerrainTile> ASyncLoadTerrainTilesFromRemote();

        private List<TerrainTile> LoadTerrainTilesFromRemote()
        {
            using (var tcpClient = new TcpClient("127.0.0.1", 9023))
            {
                tcpClient.Client.NoDelay = true;
                using (var stream = tcpClient.GetStream())
                {
                    var requestMessage = new RequestMessage
                    {
                        Type = RequestMessage.Types.RequestType.InitialWorldState,
                        PlayerName = "Nigel"
                    };

                    Debug.Log("Sending :" + requestMessage);

                    requestMessage.WriteDelimitedTo(stream);

                    List<TerrainTile> terrainTiles = new List<TerrainTile>();

                    var responseMessage = ResponseMessage.Parser.ParseDelimitedFrom(stream);

                    for(int i = 0; i < responseMessage.NumOfResponses; i++) { 
                        var loadedHeightMapMessage = HeightMapMessage.Parser.ParseDelimitedFrom(stream);


                        var count = loadedHeightMapMessage.Height.Count;

                        Debug.Log("Received chunk " + i + " with " + count + " heights...");
                        var heightmapSize = Convert.ToInt32(Math.Sqrt(count));

                        if (heightmapSize*heightmapSize > count)
                        {
                            throw new ApplicationException("Received a non square heightmap from server");
                        }

                        var heights = new float[heightmapSize, heightmapSize];

                        for (var y = 0; y < heightmapSize; y++)
                        {
                            for (var x = 0; x < heightmapSize; x++)
                            {
                                heights[y, x] = (float) loadedHeightMapMessage.Height[x + y*heightmapSize]/ TerrainConfig.MAXIMUM_TERRAIN_HEIGHT_WORLD_UNITS;
                            }
                        }

                        terrainTiles.Add(new TerrainTile(heights, loadedHeightMapMessage.Y, loadedHeightMapMessage.X));
                    }

                    return terrainTiles;
                }
            }
        }
    }
}