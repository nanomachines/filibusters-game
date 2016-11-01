using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon;

namespace Filibusters
{
    public class ToggleVisualReady : UnityEngine.MonoBehaviour 
    {
        private PhotonView mPhotonView;
        private bool mIsMine;

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
            mPhotonView = GetComponent<PhotonView>();
            mIsMine = false;

            mJoinText = Utility.GetChildWithTag(gameObject, Tags.JOIN_TEXT);
            mUIButtonPrompt = Utility.GetChildWithTag(gameObject, Tags.BUTTON);
            mPlayer = Utility.GetChildWithTag(gameObject, Tags.PLAYER);

            mRectTransform = GetComponent<RectTransform>();
            mFullHeight = mRectTransform.rect.height;
            mHalfHeight = mFullHeight / 2;

            mPlayerReady = false;
            mTimeSinceUnReady = 0f;
        }

        public void TakeOwnership()
        {
            mIsMine = true;
        }
        
        void Update() 
        {
            if (!mIsMine)
            {
                return;
            }

            mTimeSinceUnReady += Time.deltaTime;
            if (InputWrapper.Instance.JumpPressed && !mPlayerReady)
            {
                ToggleReadyPlayer(true);
            }
            if (InputWrapper.Instance.CancelPressed)
            {
                if (mPlayerReady)
                {
                    ToggleReadyPlayer(false);
                    mTimeSinceUnReady = 0f;
                }
                /*
                 * This time check is needed, otherwise the player will be immediately returned
                 * to the main menu
                */
                else if (mTimeSinceUnReady > 0.3f)
                {
                    // TODO: Trigger an event to leave the room
                    PhotonNetwork.LeaveRoom();
                    SceneManager.LoadScene(Scenes.START_MENU);
                }
            }
        }

        void ToggleReadyPlayer(bool isReady)
        {
            var panelHeight = isReady ? mHalfHeight : mFullHeight;
            mPhotonView.RPC("ToggleReadyPlayerRPC", PhotonTargets.AllBuffered, isReady, panelHeight);
        }

        [PunRPC]
        void ToggleReadyPlayerRPC(bool isReady, float panelHeight)
        {
            mPlayerReady = isReady;
            mRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
            mJoinText.SetActive(!isReady);
            mUIButtonPrompt.SetActive(!isReady);
            mPlayer.SetActive(isReady);
        }
    }
}
