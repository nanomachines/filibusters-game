using UnityEngine;
using Photon;

public class NetworkManager : Photon.PunBehaviour
{
    #region Public Fields
    public static NetworkManager Instance = null;
    #endregion

    #region Public Methods
    void CreateGameSession(string sessionName)
    {
    }

    void JoinGameSession(string sessionName)
    {
    }
    #endregion

    #region Unity Methods
    // Use this for initialization
    void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
        }
        PhotonNetwork.ConnectUsingSettings(GameConstants.VERSION_STRING);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	#endregion

	#region Callbacks
	#endregion

	#region Private Methods
	#endregion
}
