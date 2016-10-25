using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class PromptSwitcher : MonoBehaviour
    {
        [SerializeField]
        GameObject mGamepadPrompt;
        [SerializeField]
        GameObject mKeyboardMousePrompt;

        void Update()
        {
            bool usingGamepad = InputWrapper.AnyJoysticksConnected();
            mGamepadPrompt.SetActive(usingGamepad);
            mKeyboardMousePrompt.SetActive(!usingGamepad);
        }
    }
}
