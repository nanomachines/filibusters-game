using UnityEngine;
using Photon;
using UnityEngine.SceneManagement;

namespace Filibusters
{
    public class NetworkManager : Photon.PunBehaviour
    {
        public static NetworkManager Instance = null;

        // Use this for initialization
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                Object.DontDestroyOnLoad(this);
                PhotonNetwork.automaticallySyncScene = true;
                if (!SceneManager.GetSceneByName("StartMenu").isLoaded)
                {
                    PhotonNetwork.offlineMode = true;
                    PhotonNetwork.CreateRoom("OfflineRoom");
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
	
        public void CreateAndJoinGameSession(string sessionName)
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

        public void JoinGameSession(string sessionName)
        {
            PhotonNetwork.JoinRoom(sessionName);
        }
    }
}
