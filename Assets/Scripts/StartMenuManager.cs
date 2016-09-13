using UnityEngine;
using System.Collections;

public class StartMenuManager : MonoBehaviour
{
    // Use this for initialization
    void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        GUILayout.Label("Filibusters");
        if (GUILayout.Button("Host Game"))
        {
            NetworkManager.Instance.CreateAndJoinGameSession("Default Session");
        }
        if (GUILayout.Button("Join Game"))
        {
            NetworkManager.Instance.JoinGameSession("Default Session");
        }
    }
}
