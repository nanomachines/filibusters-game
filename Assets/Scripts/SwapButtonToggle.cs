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
            EventSystem.OnEquipWeaponEvent += DisableOnEquip;
            mIsLocalPlayer = gameObject.GetPhotonView().isMine;
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

        void DisableOnEquip(int actorId, GameConstants.WeaponId weaponId)
        {
            if (actorId == PhotonNetwork.player.ID)
            {
                mSwapPrompt.SetActive(false);
            }
        }

        void DisableOnDeath(int playerViewId)
        {
            if (playerViewId == mPhotonView.viewID)
            {
                mSwapPrompt.SetActive(false);
            }
        }
    }
}
