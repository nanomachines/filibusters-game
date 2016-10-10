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
        }

        public void HostGame()
        {
            NetworkManager.CreateAndJoinGameSession(SESSION_NAME);
        }

        public void JoinGame()
        {
            NetworkManager.JoinGameSession(SESSION_NAME);
        }
    
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }
    }
}
