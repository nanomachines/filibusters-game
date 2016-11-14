using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class SwapButtonToggle : MonoBehaviour 
    {
        public GameObject mSwapPrompt;
        private PhotonView mPhotonView;
        private bool mIsLocalPlayer;

        void Start()
        {
            mPhotonView = gameObject.GetPhotonView();

            EventSystem.OnDeathEvent += DisableOnDeath;
            mIsLocalPlayer = gameObject.GetPhotonView().isMine;
        }

        void OnDestroy()
        {
            EventSystem.OnDeathEvent -= DisableOnDeath;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Tags.WEAPON && mIsLocalPlayer)
            {
                mSwapPrompt.SetActive(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == Tags.WEAPON && mIsLocalPlayer)
            {
                mSwapPrompt.SetActive(false);
            }
        }

        void DisableOnDeath(int playerViewId, Vector3 pos)
        {
            if (playerViewId == mPhotonView.viewID)
            {
                mSwapPrompt.SetActive(false);
            }
        }

        public void DisableOnOtherPlayerEquip(int viewId)
        {
            if (PhotonView.Find(viewId).isMine)
            {
                mSwapPrompt.SetActive(false);
            }
        }
    }
}
