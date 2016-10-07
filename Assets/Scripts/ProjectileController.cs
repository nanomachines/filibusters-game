using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace Filibusters
{
    public class ProjectileController : MonoBehaviour 
    {
        // TODO: Let PlayerAttack set mVel based on the bullet type
        [SerializeField]
        private float mVel;
        private PhotonView mPhotonView;

        void Start()
        {
            mPhotonView = GetComponent<PhotonView>();
        }
        
        void FixedUpdate() 
        {
            transform.Translate(mVel * Time.deltaTime, 0f, 0f);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            GameObject obj = col.transform.gameObject;

            // Player
            if (obj.tag == Tags.PLAYER)
            {
                if (obj.GetComponent<PhotonView>().owner.ID != mPhotonView.owner.ID)
                {
                    mPhotonView.RPC("DestroyBullet", PhotonTargets.Others, mPhotonView.viewID);
                    obj.GetComponent<LifeManager>().Die();
                    Destroy(gameObject);
                }
            }
            // Walls and floors
            else if (obj.layer == LayerMask.NameToLayer("Barrier") || obj.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Hit wall");
                Destroy(gameObject);
                mPhotonView.RPC("DestroyBullet", PhotonTargets.Others, mPhotonView.viewID);
            }
        }

        [PunRPC]
        public void DestroyBullet(int viewId)
        {
            Debug.Log("Hit player");
            Destroy(PhotonView.Find(viewId).gameObject);
        }
    }
}
