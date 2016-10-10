using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class PlayerAttack : MonoBehaviour
    {
        private WeaponInventory mWeaponInventory;
        private int mActorId;

        void Start()
        {
            mWeaponInventory = GetComponent<WeaponInventory>();
            mActorId = GetComponent<PhotonView>().owner.ID;
        }

        void Update()
        {
            if (InputWrapper.Instance.FirePressed)
            {
                if (mWeaponInventory.CooledDown())
                {
                    // Grab weapon id to trigger fire button effects
                    WeaponId weaponId = mWeaponInventory.mWeaponId;

                    // Has ammo
                    if (mWeaponInventory.GetRound())
                    {
                        string projectileName = GetProjectileName(weaponId);
                        if (projectileName.Length != 0)
                        {
                            Transform xform = GetComponent<AimingController>().GetWeaponPointTransform();
                            InstantiateProjectile(projectileName, xform.position, xform.rotation);
                        }
                        EventSystem.OnWeaponFired(mActorId, weaponId);
                    }
                    // Weapon is out of ammo
                    else
                    {
                        EventSystem.OnWeaponMisfired(mActorId, weaponId);
                    }
                }
            }
        }

        string GetProjectileName(WeaponId weaponId)
        {
            // TODO: replace with mWeaponId indexed array
            // Return string resource name to instantiate networked projectile
            switch (weaponId)
            {
                case WeaponId.VETO:
                    return "VetoBullet";
                case WeaponId.MAGIC_BULLET:
                    return "MagicBulletBullet";
                default:
                    return "";
            }
        }

        void InstantiateProjectile(string projectileName, Vector3 origin, Quaternion rotation)
        {
            GameObject projectile = PhotonNetwork.Instantiate(projectileName, origin, rotation, 0);
        }
    }
}
