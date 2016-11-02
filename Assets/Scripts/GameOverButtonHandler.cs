using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Filibusters
{
    public class GameOverButtonHandler : MonoBehaviour
    {
        void Start()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClicked);
        }

        public void OnClicked()
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(Scenes.START_MENU);
        }
    }
}
