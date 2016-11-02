using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class LeaveGameManager : MonoBehaviour 
	{
        [SerializeField]
        private GameObject mLeaveOverlay;
        private bool mOverlayEnabled;

        [SerializeField]
        private UnityEngine.UI.Button mYesButton;
        [SerializeField]
        private UnityEngine.UI.Button mNoButton;

		void Start() 
		{
            mOverlayEnabled = false;
            mYesButton.onClick.AddListener(() => { Utility.BackToStartMenu(); });
            mNoButton.onClick.AddListener(() => { ToggleOverlay(false); });
        }
		
		void Update() 
		{
		    if (InputWrapper.Instance.CancelPressed)
            {
                ToggleOverlay(!mOverlayEnabled);
            }
		}

        void ToggleOverlay(bool isEnabled)
        {
            mOverlayEnabled = isEnabled;
            mLeaveOverlay.SetActive(isEnabled);
        }
	}
}
