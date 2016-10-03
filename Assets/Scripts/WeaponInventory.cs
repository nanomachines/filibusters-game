using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class WeaponInventory : MonoBehaviour
    {
        private PlayerState mPlayerState;
        private WeaponId mWeaponId
        {
            get { return mPlayerState.mWeaponId; }
            set { mPlayerState.mWeaponId = value; }
        }

        void Start()
        {
            mPlayerState = GetComponent<PlayerState>();
        }

        public bool CanEquip()
        {
            return mWeaponId == WeaponId.FISTS;
        }

        public void EquipWeapon(WeaponId weaponId) 
        {
            mWeaponId = weaponId;
            int actorId = GetComponent<PhotonView>().owner.ID;
            EventSystem.OnEquipWeapon(actorId, weaponId);
        }
    }
}