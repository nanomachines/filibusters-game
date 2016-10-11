using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class UIController : MonoBehaviour
	{
        [SerializeField]
        private UnityEngine.UI.Button mStartButton;
        private UnityEngine.EventSystems.StandaloneInputModule mInputModule;

        public void Start()
        {
            mInputModule = GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            mStartButton.gameObject.SetActive(false);
            mStartButton.interactable = false;
            mInputModule.horizontalAxis = InputWrapper.LeftXInputName;
            mInputModule.verticalAxis = InputWrapper.LeftYInputName;
            EventSystem.OnAllPlayersReadyEvent += () =>
            {
                mStartButton.gameObject.SetActive(true);
                mStartButton.interactable = true;
            };
            EventSystem.OnAllPlayersNotReadyEvent += () =>
            {
                mStartButton.gameObject.SetActive(false);
                mStartButton.interactable = false;
            };
        }
        public void Update()
        {
            mInputModule.submitButton = InputWrapper.Instance.AnyJoysticksConnected() ?
                InputWrapper.Xbox360SubmitAxis : InputWrapper.SubmitAxis;
        }
	}

}
