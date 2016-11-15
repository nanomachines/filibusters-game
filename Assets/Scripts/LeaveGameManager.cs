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

        private bool mGameOver;

        void Start() 
        {
            mOverlayEnabled = false;
            mYesButton.onClick.AddListener(() => { Utility.BackToStartMenu(); });
            mNoButton.onClick.AddListener(() => { ToggleOverlay(false); });

            mGameOver = false;
            EventSystem.OnGameOverEvent += (int winningActorId) => { mGameOver = true; };
        }
        
        void Update() 
        {
            if (InputWrapper.Instance.CancelPressed && !mGameOver)
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
