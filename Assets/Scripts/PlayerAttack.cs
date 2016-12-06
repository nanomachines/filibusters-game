using UnityEngine;
using System;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        LayerMask mWeaponClippingCheck;
        WeaponInventory mWeaponInventory;
        BarrelTip mBarrelTip;
        int ownerId;


        void Start()
        {
            mWeaponInventory = GetComponent<WeaponInventory>();
            mBarrelTip = GetComponent<BarrelTip>();
            ownerId = GetComponent<PhotonView>().ownerId;
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
                        string projectile = GetProjectileName(weaponId);
                        Transform xform = mBarrelTip.GetBarrelTransform();
                        if (ProjectileSpawnIsntClipping(transform.position, xform.position))
                        {
                            PhotonNetwork.Instantiate(projectile, xform.position, xform.rotation, 0);
                        }
                        EventSystem.OnWeaponFired(weaponId, transform.position, ownerId);
                    }
                    // Weapon is out of ammo
                    else
                    {
                        EventSystem.OnWeaponMisfired(weaponId);
                    }
                }
            }
        }

        string GetProjectileName(WeaponId weaponId)
        {
            switch (weaponId)
            {
                case WeaponId.DARK_HORSE:
                    return "DarkHorseBullet";
                case WeaponId.VETO:
                    return "VetoBullet";
                case WeaponId.MAGIC_BULLET:
                    return "MagicBulletBullet";
                default:
                    return "DarkHorseBullet";
            }
        }

        bool ProjectileSpawnIsntClipping(Vector3 weaponOrigin, Vector3 weaponEmissionPoint)
        {
            Vector2 emissionVector = weaponEmissionPoint - weaponOrigin;
            float barrelDistance = emissionVector.magnitude;
            var hit = Physics2D.Raycast(weaponOrigin, emissionVector, barrelDistance, mWeaponClippingCheck);
            return hit.collider == null;
        }
    }
}
