using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Filibusters
{
    public class BackToMainMenuButton : MonoBehaviour
    {
        void Start()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                () => {  SceneManager.LoadScene(Scenes.START_MENU); }
            );
        }
    }
}
