using UnityEngine;
using System.Collections.Generic;
using Photon;
using UnityEngine.SceneManagement;

namespace Filibusters
{
    public class CharacterSelectManager : PunBehaviour
    {
        [HideInInspector]
        public static CharacterSelectManager instance = null;

        private int mPlayersReady;

        // Use this for initialization
        void Start()
        {
            if (instance == null)
            {
                instance = this;
                PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable{ { "IsReady", false}, {"IsNew", true } });
    
                // If we are in offline mode we need to explicitly call the properties changed callback
                if (PhotonNetwork.offlineMode)
                {
                    OfflineReadyUpdate(false, true);
                }
    
                mPlayersReady = 0;
                foreach (var player in PhotonNetwork.playerList)
                {
                    bool isReady = player.customProperties.ContainsKey("IsReady") ? (bool)player.customProperties["IsReady"] : false;
                    if (isReady)
                    {
                        ++mPlayersReady;
                    }
                }
            }
        }

        void OnGUI()
        {
            var labelBuilder = new System.Text.StringBuilder(90);
            if (PhotonNetwork.isMasterClient)
            {
                labelBuilder.AppendLine("Host/Master Client");
            }
            labelBuilder.AppendLine("PlayersReady: " + mPlayersReady);
            foreach (var player in PhotonNetwork.playerList)
            {
                var isReady = player.customProperties.ContainsKey("IsReady") ? player.customProperties["IsReady"] : false;
                labelBuilder.AppendLine(player.ID + ": " + isReady);
            }
            GUILayout.Label(labelBuilder.ToString());

            if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == mPlayersReady)
            {
                if (GUILayout.Button("Start Game"))
                {
                    OnStartGame();
                }
            }
        }

        public void OnStartGame()
        {
            GetComponent<PhotonView>().RPC("LaunchGame", PhotonTargets.All);
//            NetworkManager.Instance.CloseGameSession();
        }

        [PunRPC]
        public void LaunchGame()
        {
            PhotonNetwork.LoadLevel("Scenes/MainGame");
        }
    
        public void ToggleLocalPlayerReady()
        {
            bool isReady = !(bool)PhotonNetwork.player.customProperties["IsReady"];
            PhotonNetwork.SetPlayerCustomProperties(
                new ExitGames.Client.Photon.Hashtable{ { "IsReady", isReady}, {"IsNew", false} });
    
            // If we are in offline mode we need to explicitly call the properties changed callback
            if (PhotonNetwork.offlineMode)
            {
                OfflineReadyUpdate(isReady, false);
            }
        }
    
        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            var properties = playerAndUpdatedProps[1] as ExitGames.Client.Photon.Hashtable;
            if ((bool)properties["IsReady"])
            {
                ++mPlayersReady;
            }
            else if (!(bool)properties["IsNew"])
            {
                --mPlayersReady;
            }
        }
    
        private void OfflineReadyUpdate(bool isReady, bool isNew)
        {
            object[] playerAndUpdatedProps = new object[2];
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("IsReady", isReady);
            properties.Add("IsNew", isNew);
            playerAndUpdatedProps[1] = properties;
            OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);
        }
    }
}
