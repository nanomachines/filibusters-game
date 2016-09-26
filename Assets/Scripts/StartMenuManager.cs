using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class StartMenuManager : Photon.PunBehaviour 
    {
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
    
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Scenes/CharacterSelect");
        }
    }
}
