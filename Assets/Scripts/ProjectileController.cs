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
        private float mRaycastDistance;
        private PhotonView mPhotonView;
        private Vector3 mLocalColliderOffset;
        [SerializeField]
        private LayerMask mColLayers;
        private RaycastHit2D mLastHit;
        [SerializeField]
        private float mSkin = 0.005f;

        void Start()
        {
            mPhotonView = GetComponent<PhotonView>();
            CircleCollider2D mCollider = GetComponent<CircleCollider2D>();
            mLocalColliderOffset = new Vector3(mCollider.offset.x, mCollider.offset.y);
            mRaycastDistance = mVel * Time.fixedDeltaTime + 0.001f;
        }
        
        void FixedUpdate() 
        {
            if (mLastHit.collider != null)
            {
                HandleCollisions(mLastHit);
            }

            mLastHit = DetectCollisions();
            if (mLastHit.collider == null)
            {
                transform.Translate(mVel * Time.fixedDeltaTime, 0f, 0f);
            }
        }

        RaycastHit2D DetectCollisions()
        {
            Vector2 worldSpaceColliderPos = transform.localPosition + mLocalColliderOffset;
            Vector2 direction = transform.TransformDirection(Vector2.right);

            Ray2D ray = new Ray2D(worldSpaceColliderPos, direction);

            var raydir = ray.direction;
            raydir.Normalize();
            raydir *= mRaycastDistance;
            Debug.DrawRay(ray.origin, raydir, Color.cyan);

            return Physics2D.Raycast(ray.origin, ray.direction, mRaycastDistance, mColLayers);
        }

        void HandleCollisions(RaycastHit2D hit)
        {
            GameObject obj = hit.transform.gameObject;
            // Player
            if (obj.tag == "Player")
            {
                if (obj.GetComponent<PhotonView>().owner.ID != mPhotonView.owner.ID)
                {
                    mPhotonView.RPC("DestroyBullet", PhotonTargets.Others, mPhotonView.viewID);
                    obj.GetComponent<LifeManager>().Die();
                    Destroy(gameObject);
                }
            }
            // Walls and floors
            if (obj.layer != LayerMask.NameToLayer("Player"))
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
