using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class StartMenuManager : Photon.PunBehaviour 
    {
        public static readonly string SESSION_NAME = "Default Session Scott";

        void OnGUI()
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            GUILayout.Label("Filibusters");
            if (GUILayout.Button("Host Game"))
            {
                NetworkManager.CreateAndJoinGameSession(SESSION_NAME);
            }
            if (GUILayout.Button("Join Game"))
            {
                NetworkManager.JoinGameSession(SESSION_NAME);
            }
        }
    
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Scenes/ReadyMenu");
        }
    }
}
