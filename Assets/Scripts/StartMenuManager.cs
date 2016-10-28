using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Filibusters
{
    public class StartMenuManager : Photon.PunBehaviour 
    {
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
            SceneManager.LoadScene(Scenes.HOST_GAME);
        }

        public void JoinGame()
        {
            SceneManager.LoadScene(Scenes.JOIN_GAME);
        }

        public void DisplayHowToPlay()
        {
            SceneManager.LoadScene(Scenes.HOW_TO_PLAY);
        }
    }
}
