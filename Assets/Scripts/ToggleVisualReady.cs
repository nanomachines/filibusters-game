using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Filibusters
{
    public class ToggleVisualReady : MonoBehaviour 
    {
        private GameObject mJoinText;
        private GameObject mUIButtonPrompt;
        private GameObject mPlayer;

        private RectTransform mRectTransform;
        private float mFullHeight;
        private float mHalfHeight;

        private bool mPlayerReady;
        private float mTimeSinceUnReady;

        void Start() 
        {
            mJoinText = Utility.GetChildWithTag(gameObject, Tags.JOIN_TEXT);
            mUIButtonPrompt = Utility.GetChildWithTag(gameObject, Tags.BUTTON);
            mPlayer = Utility.GetChildWithTag(gameObject, Tags.PLAYER);

            mRectTransform = GetComponent<RectTransform>();
            mFullHeight = mRectTransform.rect.height;
            mHalfHeight = mFullHeight / 2;

            mPlayerReady = false;
            mTimeSinceUnReady = 0f;
        }
        
        void Update() 
        {
            mTimeSinceUnReady += Time.deltaTime;
            if (InputWrapper.Instance.JumpPressed && !mPlayerReady)
            {
                ToggleReadyPlayer(true, mHalfHeight);
            }
            if (InputWrapper.Instance.CancelPressed)
            {
                if (mPlayerReady)
                {
                    ToggleReadyPlayer(false, mFullHeight);
                    mTimeSinceUnReady = 0f;
                }
                /*
                 * This time check is needed, otherwise the player will be immediately returned
                 * to the main menu
                */
                else if (mTimeSinceUnReady > 0.3f)
                {
                    // TODO: Trigger an event to leave the room
                    SceneManager.LoadScene(Scenes.START_MENU);
                }
            }
        }

        void ToggleReadyPlayer(bool isReady, float panelHeight)
        {
            mPlayerReady = isReady;
            mRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
            mJoinText.SetActive(!isReady);
            mUIButtonPrompt.SetActive(!isReady);
            mPlayer.SetActive(isReady);
        }
    }
}
