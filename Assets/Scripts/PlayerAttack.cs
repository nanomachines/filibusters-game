using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class PlayerAttack : MonoBehaviour
    {
        private WeaponInventory mWeaponInventory;
//        private PhotonView mPhotonView;
        private int mActorId;

        void Start()
        {
            mWeaponInventory = GetComponent<WeaponInventory>();
//            mPhotonView = GetComponent<PhotonView>();
            mActorId = GetComponent<PhotonView>().owner.ID;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (mWeaponInventory.CooledDown())
                {
                    // Grab weapon id to trigger fire button effects
                    WeaponId weaponId = mWeaponInventory.UseWeapon();

                    // Has ammo
                    if (mWeaponInventory.HasAmmo())
                    {
                        string projectileName = GetProjectileName(weaponId);
                        if (projectileName.Length != 0)
                        {
                            InstantiateProjectile(projectileName);
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
            // Return string resource name to instantiate networked projectile
            if (weaponId == WeaponId.VETO)
            {
                return "VetoBullet";
            }
            else
            {
                return "";
            }
        }

        void InstantiateProjectile(string projectileName)
        {
            GameObject projectile = PhotonNetwork.Instantiate(projectileName, transform.position, Quaternion.identity, 0);
            ProjectileController projScript = projectile.GetComponent<ProjectileController>();

            // Assign projectile velocity
            projScript.NewDirection = new Vector3(1f, 0f);

            // The direction depends on the rotation CHANGE
            //projScript.NewDirection = new Vector3(0f, 1f);
            //projectile.transform.Rotate(new Vector3(0f, 0f, 90f));
        }
    }
}
