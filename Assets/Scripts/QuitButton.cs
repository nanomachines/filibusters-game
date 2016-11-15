using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class QuitButton : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener( () => { Application.Quit(); } );
        }
    }
}
