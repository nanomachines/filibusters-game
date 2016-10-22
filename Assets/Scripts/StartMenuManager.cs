using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Filibusters
{
    public class StartMenuManager : Photon.PunBehaviour 
    {
        public static readonly string SESSION_NAME = "Default Session Scott";

        [SerializeField]
        private Button HostButton;

        [SerializeField]
        private Button JoinButton;

        [SerializeField]
        private Button HowToPlayButton;

        void Start()
        {
            HostButton.onClick.AddListener(HostGame);
            JoinButton.onClick.AddListener(JoinGame);
            HowToPlayButton.onClick.AddListener(DisplayHowToPlay);
        }

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

        public void DisplayHowToPlay()
        {
            SceneManager.LoadScene(Scenes.HOW_TO_PLAY);
        }
    
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(Scenes.READY_MENU);
        }
    }
}
