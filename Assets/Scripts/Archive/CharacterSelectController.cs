using UnityEngine;

namespace Filibusters
{
    public class CharacterSelectController : MonoBehaviour
    {
        CharacterSelectManager mCharacterSelectManager;
        void Start()
        {
            mCharacterSelectManager =
                GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelectManager>();
        }

        /*
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mCharacterSelectManager.ToggleLocalPlayerReady();
            }
        }
        */
    }
}
