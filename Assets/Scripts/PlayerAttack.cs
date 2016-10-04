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

                    string bulletName = mWeaponInventory.GetBulletType();
                    if (bulletName.Length != 0)
                    {
                        InstantiateBullet(bulletName);
                    }
                    // trigger succesful fire event
                }
                else
                {
                    // trigger failed fire event
                }
            }
        }

        void InstantiateBullet(string bulletName)
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletName, transform.position, Quaternion.identity, 0);
        }
    }
}
