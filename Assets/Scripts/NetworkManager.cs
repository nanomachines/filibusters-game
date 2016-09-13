using UnityEngine;
using Photon;
using UnityEngine.SceneManagement;

public class NetworkManager : Photon.PunBehaviour
{
    public static NetworkManager Instance = null;

    // Use this for initialization
    void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
            Object.DontDestroyOnLoad(this);
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings(GameConstants.VERSION_STRING);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
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

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Scenes/CharacterSelect");
    }
}
