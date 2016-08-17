using System;
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
            asyncResult = aSyncLoadTerrainTilesFromRemote.BeginInvoke( null, null);
        }

        public TerrainTile TileIfReady()
        {
            return asyncResult.IsCompleted ?
                aSyncLoadTerrainTilesFromRemote.EndInvoke(asyncResult) : null;
        }

        private delegate TerrainTile ASyncLoadTerrainTilesFromRemote();
     
        private TerrainTile LoadTerrainTilesFromRemote()
        {
            using (var tcpClient = new TcpClient("127.0.0.1", 9023))
            {
                tcpClient.Client.NoDelay = true;
                using (var stream = tcpClient.GetStream())
                {
                    var requestMessage = new RequestMessage
                    {
                        Type = RequestMessage.Types.RequestType.InitialWorldState
                    };

                    Debug.Log("Sending :" + requestMessage);

                    requestMessage.WriteDelimitedTo(stream);

                    var loadedHeightMapMessage = HeightMapMessage.Parser.ParseDelimitedFrom(stream);
                

                    var count = loadedHeightMapMessage.Height.Count;

                    Debug.Log("Recieved " + count + " heights...");
                    var heightmapSize = Convert.ToInt32(Math.Sqrt(count));

                    if (heightmapSize*heightmapSize > count)
                    {
                        throw new ApplicationException("Received a non square heightmap from server");
                    }

                    var heights = new float[heightmapSize,heightmapSize];

                    for (var y = 0; y < heightmapSize; y++)
                    {
                        for (var x = 0; x < heightmapSize; x++)
                        {
                            heights[y, x] = (float)loadedHeightMapMessage.Height[x + y*heightmapSize] / heightmapSize;
                        }
                    }

                    return new TerrainTile(heights);
                }
            }
        }
    }
}