using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Filibusters
{
    public class DynamicUIInputAxis : MonoBehaviour
    {
        private UnityEngine.EventSystems.EventSystem mUIEventSystem;
        private StandaloneInputModule mInputModule;
        private bool joystickWasConnected;

        private bool mFirstFrame;

        public void Start()
        {
            mFirstFrame = true;
            mUIEventSystem = GetComponent<UnityEngine.EventSystems.EventSystem>();

            if (InputWrapper.AnyJoysticksConnected())
            {
                joystickWasConnected = true;
                mUIEventSystem.SetSelectedGameObject(mUIEventSystem.firstSelectedGameObject);
            }
            else
            {
                joystickWasConnected = false;
                mUIEventSystem.SetSelectedGameObject(null);
            }

            mInputModule = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            mInputModule.horizontalAxis = InputWrapper.LeftXInputName;
            mInputModule.verticalAxis = InputWrapper.LeftYInputName;
        }

        public void Update()
        {
            bool anyJoysticksConnected = InputWrapper.AnyJoysticksConnected();
            mInputModule.submitButton = anyJoysticksConnected ?
                InputWrapper.Xbox360SubmitAxis : InputWrapper.SubmitAxis;

            if (!anyJoysticksConnected && (joystickWasConnected || mFirstFrame))
            {
                mUIEventSystem.SetSelectedGameObject(null);
                joystickWasConnected = false;
            }
            else if (anyJoysticksConnected && !joystickWasConnected )
            {
                mUIEventSystem.SetSelectedGameObject(mUIEventSystem.firstSelectedGameObject);
                joystickWasConnected = true;
            }
            mFirstFrame = false;
        }
    }

}
