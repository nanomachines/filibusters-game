using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class PlayerAttack : MonoBehaviour
    {
        private WeaponInventory mWeaponInventory;

        void Start()
        {
            mWeaponInventory = GetComponent<WeaponInventory>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Fire keycode pressed");
                if (mWeaponInventory.CanFire())
                {
                    Debug.Log("This player CanFire");

                    string projectileName = mWeaponInventory.GetProjectileType();
                    if (projectileName.Length != 0)
                    {
                        InstantiateProjectile(projectileName);
                    }
                    // trigger succesful fire event
                }
                else
                {
                    // trigger failed fire event
                }
            }
        }

        void InstantiateProjectile(string projectileName)
        {
            Debug.Log("Projectile launched");
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
