using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class PlayerAttack : MonoBehaviour
    {
        private WeaponInventory mWeaponInventory;
        private PhotonView mPhotonView;
        private int mViewId;

        void Start()
        {
            mWeaponInventory = GetComponent<WeaponInventory>();
            mPhotonView = GetComponent<PhotonView>();
            mViewId = gameObject.GetComponent<PhotonView>().viewID;
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
            projScript.VelX = 1f;
            projScript.VelY = 0f;
        }
    }
}
