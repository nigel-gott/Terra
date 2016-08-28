using System;
using System.Collections;
using NigelGott.Terra.NetworkState;
using NigelGott.Terra.Terrain;
using UnityEngine;


namespace NigelGott.Terra
{
    public class GameManager : MonoBehaviour {

        public GameObject Player;

        private NetworkManager networkManager;
        private TerrainManager terrainManager;

        public GameState.GameState GameState;
        
        void Awake ()
        {
            var terrainLoader = new TerrainLoader();
            networkManager = new NetworkManager(terrainLoader);
            terrainManager = new TerrainManager(gameObject, terrainLoader, networkManager);
        }

        void Start()
        {
            Player.SetActive(false);
            var worldState = networkManager.LoadWorldState();
            Debug.Log("Received world message of " + worldState.PlayerLocation.X + " , " + worldState.PlayerLocation.Y);
            GameState = new GameState.GameState(worldState.WorldSize, new Vector2(worldState.PlayerLocation.X, worldState.PlayerLocation.Y));
            Player.transform.position = new Vector3(worldState.PlayerLocation.X, 0, worldState.PlayerLocation.Y);
        }

        void LateUpdate ()
        {
            UpdatePlayerState();
            terrainManager.Update(GameState);
        }

        private void UpdatePlayerState()
        {
            GameState.PlayerPosition = new Vector2(Player.transform.position.x, Player.transform.position.z);
            var terrainUnderPlayerReady = terrainManager.TerrainAtPositionIsReady(GameState.PlayerPosition);

            if (Player.activeInHierarchy || terrainUnderPlayerReady)
            {
                var heightAtPlayer = terrainManager.GetWorldHeightAt(Player.transform.position);
                if (Player.transform.position.y < heightAtPlayer - 10)
                {
                    Player.transform.position += new Vector3(0, 5 + heightAtPlayer - Player.transform.position.y, 0);
                }
            }

            Player.SetActive(terrainUnderPlayerReady);
        }


        void OnApplicationQuit()
        {
            networkManager.Dispose();
        }
    }
}