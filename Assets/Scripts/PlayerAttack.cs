using UnityEngine;
using System;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public struct ProjectileFXPair
    {
        public ProjectileFXPair(string w, string f)
        {
            weapon = w;
            fx = f;
        }
        public string weapon;
        public string fx;
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
                        if (projectile.weapon != null)
                        {
                            PhotonNetwork.Instantiate(projectile.weapon, xform.position, xform.rotation, 0);
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
            // TODO: replace with mWeaponId indexed array
            // Return string resource name to instantiate networked projectile
            switch (weaponId)
            {
                case WeaponId.VETO:
                    return new ProjectileFXPair("VetoBullet", "VetoFireFX");
                case WeaponId.MAGIC_BULLET:
                    return new ProjectileFXPair("MagicBulletBullet", null);
                default:
                    return new ProjectileFXPair(null, null);
            }
        }
    }
}
