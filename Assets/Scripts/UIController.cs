using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class UIController : MonoBehaviour
	{
        [SerializeField]
        private UnityEngine.EventSystems.StandaloneInputModule mKeyboardModule;
        [SerializeField]
        private UnityEngine.EventSystems.StandaloneInputModule mXbox360Windows;
        [SerializeField]
        private UnityEngine.EventSystems.StandaloneInputModule mXbox360OSX;
        private UnityEngine.EventSystems.StandaloneInputModule mXbox360Module;
        [SerializeField]
        private GameObject mReadyMenuCanvas;

        public void Start()
        {
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                mXbox360Windows.enabled = false;
                mXbox360Module = mXbox360OSX;
            }
            else
            {
                mXbox360OSX.enabled = false;
                mXbox360Module = mXbox360Windows;
            }
            EventSystem.OnAllPlayersReadyEvent += () =>
            {
                mReadyMenuCanvas.SetActive(true);
            };
            EventSystem.OnAllPlayersNotReadyEvent += () =>
            {
                mReadyMenuCanvas.SetActive(false);
            };
        }
        public void Update()
        {
            bool joysticksConnected = InputWrapper.Instance.AnyJoysticksConnected();
            mXbox360Module.forceModuleActive = joysticksConnected;
            mKeyboardModule.forceModuleActive = !joysticksConnected;
        }
	}

}
