using Photon;
using UnityEngine;

namespace Filibusters
{
    public class CharacterSelectController : PunBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CharacterSelectManager.instance.ToggleLocalPlayerReady();
            }
        }
    }
}
