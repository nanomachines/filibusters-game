using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class WeaponInventory : MonoBehaviour
    {
        private PlayerState mPlayerState;
        private PhotonView mPhotonView;
        public WeaponId mWeaponId
        {
            get { return mPlayerState.mWeaponId; }
            private set { mPlayerState.mWeaponId = value; }
        }

        private int mAmmo;
        [SerializeField]
        private int mVetoAmmo;
        [SerializeField]
        private int mMagicBulletAmmo;

        [SerializeField]
        private float mDefaultCoolDown;
        [SerializeField]
        private float mMagicBulletCoolDown;

        private float mCoolDownSeconds;
        private bool mCoolingDown;

        void Start()
        {
            mPlayerState = GetComponent<PlayerState>();
            mPhotonView = GetComponent<PhotonView>();

            // Set ammo to -1 for infinite ammo
            mAmmo = -1;
            mCoolingDown = false;
        }

        void Update()
        {
            if (mWeaponId != WeaponId.FISTS && InputWrapper.Instance.DropWeaponPressed)
            {
                EquipWeapon(WeaponId.FISTS);
                EventSystem.OnWeaponDrop(mPhotonView.owner.ID);
            }
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
                case WeaponId.VETO:
                    mAmmo = mVetoAmmo;
                    break;
                case WeaponId.MAGIC_BULLET:
                    mAmmo = mMagicBulletAmmo;
                    mCoolDownSeconds = mMagicBulletCoolDown;
                    break;
                default:
                    mAmmo = -1;
                    mCoolDownSeconds = mDefaultCoolDown;
                    break;
            }
        }

        public bool GetRound()
        {
            if (mAmmo > 0)
            {
                --mAmmo;
                return true;
            }
            else if (mAmmo == 0)
            {
                EquipWeapon(WeaponId.FISTS);
                EventSystem.OnWeaponDrop(mPhotonView.owner.ID);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CooledDown()
        {
            if (!mCoolingDown)
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
    }
}