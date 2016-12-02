using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace Filibusters
{
    public class ProjectileController : MonoBehaviour 
    {
        // TODO: Let PlayerAttack set mVel based on the bullet type
        [SerializeField]
        protected float mVel;
        private float mRaycastDistance;
        protected PhotonView mPhotonView;
        private Vector3 mLocalColliderOffset;
        [SerializeField]
        private LayerMask mColLayers;
        private RaycastHit2D mLastHit;
        [SerializeField]
        private float mSkin = 0.005f;

        protected bool mHitRegistered = false;

        [SerializeField]
        protected int mProjectileDamage;

        public virtual void Start()
        {
            mPhotonView = GetComponent<PhotonView>();
            CircleCollider2D mCollider = GetComponent<CircleCollider2D>();
            mLocalColliderOffset = new Vector3(mCollider.offset.x, mCollider.offset.y);
            mRaycastDistance = mVel * Time.fixedDeltaTime + mSkin;
        }
        
        void FixedUpdate() 
        {
            if (mLastHit.collider != null)
            {
                HandleCollisions(mLastHit);
            }

            mLastHit = DetectCollisions();
            if (mLastHit.collider == null || ShouldIgnoreHit(mLastHit))
            {
                transform.Translate(mVel * Time.fixedDeltaTime, 0f, 0f);
                mLastHit = new RaycastHit2D();
            }
        }

        bool ShouldIgnoreHit(RaycastHit2D hit)
        {
            var obj = hit.transform.gameObject;
            return obj.tag.Equals(Tags.PLAYER) &&
                obj.GetComponent<PhotonView>().owner.ID == mPhotonView.owner.ID;
        }

        RaycastHit2D DetectCollisions()
        {
            Vector2 worldSpaceColliderPos = transform.TransformPoint(mLocalColliderOffset);
            Vector2 direction = transform.TransformDirection(Vector2.right);

            Ray2D ray = new Ray2D(worldSpaceColliderPos, direction);

            var raydir = ray.direction;
            raydir.Normalize();
            raydir *= mRaycastDistance;
            Debug.DrawRay(ray.origin, raydir, Color.cyan);

            return Physics2D.Raycast(ray.origin, ray.direction, mRaycastDistance, mColLayers);
        }

        protected virtual void HandleCollisions(RaycastHit2D hit)
        {
            if (mHitRegistered)
            {
                return;
            }

            GameObject obj = hit.transform.gameObject;
            // Player
            if (obj.tag == Tags.PLAYER)
            {
                mHitRegistered = true;
                mPhotonView.RPC("HandleInconsistentPlayerHits", PhotonTargets.Others, obj.GetComponent<PhotonView>().viewID);
                bool playerKilled = obj.GetComponent<LifeManager>().InflictDamage(mProjectileDamage);
                if (playerKilled)
                {
                    EventSystem.OnPlayerKilled(mPhotonView.ownerId);
                }
                Destroy(gameObject);
            }
            // Walls and floors
            if (obj.layer != Layers.PLAYER)
            {
                Debug.Log("Hit wall");
                EventSystem.OnWallHit(hit.centroid, hit.normal);
                mPhotonView.RPC("DestroyBullet", PhotonTargets.Others, mPhotonView.viewID);
                Destroy(gameObject);
            }
        }

        [PunRPC]
        public void DestroyBullet(int viewId)
        {
            Debug.Log("Hit player");
            Destroy(PhotonView.Find(viewId).gameObject);
        }

        [PunRPC]
        public void HandleInconsistentPlayerHits(int playerViewId)
        {
            StartCoroutine(WaitToCorrectHit(playerViewId));
        }

        private IEnumerator WaitToCorrectHit(int playerViewId)
        {
            yield return new WaitForSeconds(0.1f);
            if (!mHitRegistered)
            {
                mHitRegistered = true;
                bool playerKilled = PhotonView.Find(playerViewId).gameObject.GetComponent<LifeManager>().InflictDamage(mProjectileDamage);
                if (playerKilled)
                {
                    EventSystem.OnPlayerKilled(mPhotonView.ownerId);
                }
                Destroy(gameObject);
            }
        }
    }
}
