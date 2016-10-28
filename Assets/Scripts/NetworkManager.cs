using Photon;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

namespace Filibusters
{
    public class NetworkManager : PunBehaviour
    {

        public static NetworkManager Instance = null;

        public static readonly string PLAYER_NUMBER_KEY = "PlayerNumber";
        public static readonly string PLAYER_ACTIVE_KEY = "PlayerNumberActive";

        public static int LocalPlayerNumber
        {
            get { return (int)PhotonNetwork.player.customProperties[PLAYER_NUMBER_KEY]; }
        }


        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                Object.DontDestroyOnLoad(gameObject);
                PhotonNetwork.automaticallySyncScene = true;
                var activeSceneName = SceneManager.GetActiveScene().name;
                if (Utility.AreSceneNamesEqual(Scenes.READY_MENU, activeSceneName) &&
                    Utility.AreSceneNamesEqual(Scenes.MAIN, activeSceneName))
                {
                    PhotonNetwork.offlineMode = true;
                    PhotonNetwork.CreateRoom("OfflineRoom");
                    if (SceneManager.GetSceneByName("Main").isLoaded)
                    {
                        OnPhotonPlayerConnected(PhotonNetwork.player);
                    }
                }
                else
                {
                    PhotonNetwork.ConnectUsingSettings(GameConstants.VERSION_STRING);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        public static void CreateAndJoinGameSession(string sessionName)
        {
            if (!PhotonNetwork.insideLobby)
            {
                return;
            }

            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte)4;
            options.IsOpen = true;
            options.IsVisible = true;
            PhotonNetwork.CreateRoom(sessionName, options, TypedLobby.Default);
        }

        public static void JoinGameSession(string sessionName)
        {
            PhotonNetwork.JoinRoom(sessionName);
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.isMasterClient)
            {
                InitPlayerAllocator();
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            // Only allocate Player numbers on the master client to avoid redundancy and ensure consistency
            if (PhotonNetwork.isMasterClient)
            {
                int nextOpenPlayerNumber = FindNextPlayerNumber(PhotonNetwork.room.customProperties);
                Assert.IsTrue(nextOpenPlayerNumber != -1);

                // Set this player's custom properties so that if they leave the room we will have a record
                // of what player number they had
                newPlayer.SetCustomProperties(new PhotonHashtable{ { PLAYER_NUMBER_KEY, nextOpenPlayerNumber } });

                // Alert that the player number is taken so that the room can be activated and no one else
                // will take the same number
                PhotonNetwork.room.SetCustomProperties(
                    new PhotonHashtable { { PLAYER_NUMBER_KEY + nextOpenPlayerNumber, true } });
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int playerNumberToFree = GetPlayerNumber(otherPlayer); 
                PhotonNetwork.room.SetCustomProperties(
                    new PhotonHashtable { { PLAYER_NUMBER_KEY + playerNumberToFree, false} });
            }
        }

        public static bool HasPlayerNumberPropertyChangedForPlayer(PhotonHashtable playerPropertyMap)
        {
            return playerPropertyMap.ContainsKey(PLAYER_NUMBER_KEY);
        }

        public static bool HasPlayerNumberPropertyChangedInRoom(PhotonHashtable roomPropertyMap)
        {
            bool changed = false;
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                changed = changed || roomPropertyMap.ContainsKey(PLAYER_NUMBER_KEY + i);
            }
            return changed;
        }

        public static bool[] GetActivePlayerNumbers()
        {
            var activePlayers = new bool[GameConstants.MAX_ONLINE_PLAYERS_IN_GAME];
            var roomProperties = PhotonNetwork.room.customProperties;
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                activePlayers[i] = (bool)roomProperties[PLAYER_NUMBER_KEY + i];
            }
            return activePlayers;
        }

        public static int GetPlayerNumber(PhotonPlayer p)
        {
            return (int)p.customProperties[PLAYER_NUMBER_KEY];
        }

        private void InitPlayerAllocator()
        {
            var playerNumberAllocator = new PhotonHashtable();
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                playerNumberAllocator.Add(PLAYER_NUMBER_KEY + i, false);
            }
            PhotonNetwork.room.SetCustomProperties(playerNumberAllocator);
        }

        private int FindNextPlayerNumber(PhotonHashtable playerNumberMap)
        {
            for (int i = 0; i < GameConstants.MAX_ONLINE_PLAYERS_IN_GAME; ++i)
            {
                bool playerNumberTaken = (bool)playerNumberMap[PLAYER_NUMBER_KEY + i];
                if (!playerNumberTaken)
                {
                    return i;
                }
            }
            return -1;
        }
        
    }
}
