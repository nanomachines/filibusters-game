using UnityEngine;
using System.Collections;
using Photon;

namespace Filibusters
{
    public class ToggleVisualReady : UnityEngine.MonoBehaviour 
    {
        private PhotonView mPhotonView;

        private GameObject mJoinText;
        private GameObject mUIButtonPrompt;
        private GameObject mPlayer;

        private RectTransform mRectTransform;
        private float mFullHeight;
        private float mHalfHeight;


        void Start() 
        {
            mPhotonView = GetComponent<PhotonView>();

            mJoinText = Utility.GetChildWithTag(gameObject, Tags.JOIN_TEXT);
            mUIButtonPrompt = Utility.GetChildWithTag(gameObject, Tags.BUTTON);
            mPlayer = Utility.GetChildWithTag(gameObject, Tags.PLAYER);

            mRectTransform = GetComponent<RectTransform>();
            mFullHeight = mRectTransform.rect.height;
            mHalfHeight = mFullHeight / 2;
        }

        public void ToggleReadyPlayer(bool isReady)
        {
            var panelHeight = isReady ? mHalfHeight : mFullHeight;
            mPhotonView.RPC("ToggleReadyPlayerRPC", PhotonTargets.AllBuffered, isReady, panelHeight);
        }

        [PunRPC]
        void ToggleReadyPlayerRPC(bool isReady, float panelHeight)
        {
            mRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
            mJoinText.SetActive(!isReady);
            mUIButtonPrompt.SetActive(!isReady);
            mPlayer.SetActive(isReady);
        }
    }
}
