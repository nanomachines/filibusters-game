using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class MuzzleFlash : MonoBehaviour
    {
        private static readonly string MUZZLE_FLASH_TRIGGER = "MuzzleFlashTrigger";

        PhotonView mPhotonView;
        Animator mFlashAnimator;

        void Start()
        {
            mFlashAnimator = GetComponent<Animator>();
            mPhotonView = GetComponent<PhotonView>();
            EventSystem.OnWeaponFiredEvent += OnWeaponFired;
        }

        void OnDestroy()
        {
            EventSystem.OnWeaponFiredEvent -= OnWeaponFired;
        }

        void OnWeaponFired(WeaponId weaponId, Vector3 pos, int ownerId)
        {
            if (ownerId == mPhotonView.ownerId)
            {
                mPhotonView.RPC("OnWeaponFiredRPC", PhotonTargets.All, ownerId);
            }
        }

        [PunRPC]
        void OnWeaponFiredRPC(int ownerId)
        {
            mFlashAnimator.SetTrigger(MUZZLE_FLASH_TRIGGER);
        }

    }
}
