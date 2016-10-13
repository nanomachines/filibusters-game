using UnityEngine;
using UnityEngine.SceneManagement;

namespace Filibusters
{
    public class NetworkManager : MonoBehaviour
    {

        static bool NetworkInitialized = false;

        // Use this for initialization
        void Start()
        {
            if (!NetworkInitialized)
            {
                //Object.DontDestroyOnLoad(this);
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
                NetworkInitialized = true;
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
    }
}
