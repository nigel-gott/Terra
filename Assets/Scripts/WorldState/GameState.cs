using UnityEngine;

namespace NigelGott.Terra.GameState
{
    public class GameState
    {
        private readonly object stateLock = new object();

        private Vector2 playerLocation;

        public Vector2 PlayerPosition
        {
            get
            {
                lock (stateLock)
                {
                    return playerLocation;
                }
            }
            set
            {
                lock (stateLock)
                {
                    playerLocation = value;
                }
            }
        }

        public readonly int WorldSize;


        public GameState(int worldSize, Vector2 playerPosition)
        {
            this.WorldSize = worldSize;
            this.PlayerPosition = playerPosition;
        }
    }
}
