using System;
using System.Net.Sockets;
using Google.Protobuf;
using UnityEngine;

namespace NigelGott.Terra.NetworkState
{
    public class ServerConnection : IDisposable
    {
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;

        private readonly object streamLock = new object();

        public ServerConnection(string ip, int port)
        {
            tcpClient = new TcpClient(ip, port) {Client = {NoDelay = true}};
            stream = tcpClient.GetStream();
        }

        public void SendMessage(IMessage message)
        {
            lock (streamLock)
            {
                Debug.Log("Sending message to server : " + message.GetType());
                message.WriteDelimitedTo(stream);
            }
        }


        public T ReadMessage<T>(MessageParser<T> parser) where T : IMessage<T>
        {
            lock (streamLock)
            {
                Debug.Log("Reading message from server using " + parser.GetType());
                return parser.ParseDelimitedFrom(stream);
            }
        }

        public void Dispose()
        {
            lock (streamLock)
            {
                stream.Close();
                tcpClient.Close();
            }
        }
    }
}