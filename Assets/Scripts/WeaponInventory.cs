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

        private int mAmmo;
        [SerializeField]
        private float mCoolDownSeconds;
        private bool mCoolingDown;

        void Start()
        {
            mPlayerState = GetComponent<PlayerState>();
            // Set ammo to -1 for infinite ammo
            mAmmo = -1;
            mCoolingDown = false;
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
            // TODO: add ammo counts and cooldown seconds for each weapon as a serialized private field
            // Set ammo to infinite for now
            switch (weaponId)
            {
                default:
                    mAmmo = -1;
                    break;
            }
        }

        public bool CanFire()
        {
            if (!mCoolingDown && mAmmo != 0)
            {
                StartCoroutine(CoolDownTimer());
                return true;
            }
            return false;
        }

        private IEnumerator CoolDownTimer()
        {
            mCoolingDown = true;
            yield return new WaitForSeconds(mCoolDownSeconds);
            mCoolingDown = false;
        }

        public string GetBulletType()
        {
            // Decrement ammo count
            if (mAmmo > 0)
            {
                --mAmmo;
            }
            // Return string resource name to instantiate
            if (mWeaponId == WeaponId.VETO)
            {
                return "VetoBullet";
            }
            else
            {
                return "";
            }
        }
    }
}