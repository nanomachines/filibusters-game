using UnityEngine;
using System;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public struct ProjectileFXPair
    {
        public ProjectileFXPair(string b, string f)
        {
            bullet = b;
            fx = f;
        }
        public string bullet;
        public string fx;

        public static ProjectileFXPair FISTS = new ProjectileFXPair(null, null);
        public static ProjectileFXPair VETO = new ProjectileFXPair("VetoBullet", "VetoFireFX");
        public static ProjectileFXPair MAGIC_BULLET = new ProjectileFXPair("MagicBulletBullet", null);
    }

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
                        ProjectileFXPair projectile = GetProjectilePair(weaponId);
                        Transform xform = GetComponent<AimingController>().GetWeaponPointTransform();
                        if (projectile.bullet != null && ProjectileSpawnIsntClipping(transform.position, xform.position))
                        {
                            PhotonNetwork.Instantiate(projectile.bullet, xform.position, xform.rotation, 0);
                        }
                        if (projectile.fx != null)
                        {
                            GameObject go = PhotonNetwork.Instantiate(projectile.fx, xform.position, Quaternion.identity, 0);
                            go.transform.parent = gameObject.transform;
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

        ProjectileFXPair GetProjectilePair(WeaponId weaponId)
        {
            switch (weaponId)
            {
                case WeaponId.VETO:
                    return ProjectileFXPair.VETO;
                case WeaponId.MAGIC_BULLET:
                    return ProjectileFXPair.MAGIC_BULLET;
                default:
                    return ProjectileFXPair.FISTS;
            }
        }

        static bool ProjectileSpawnIsntClipping(Vector3 weaponOrigin, Vector3 weaponEmissionPoint)
        {
            return true;
        }
    }
}
