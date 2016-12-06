using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class MagicBulletProjectileController : ProjectileController
    {
        [SerializeField]
        private float mRicochetTime;

        public void Update()
        {
            mRicochetTime -= Time.deltaTime;
        }

        protected override void HandleCollisions(RaycastHit2D hit)
        {
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
                EventSystem.OnWallHit(hit.centroid, hit.normal);
                if (mRicochetTime > 0)
                {
                    // RICOCHET
                    var localMoveDir = transform.TransformDirection(Vector2.right);
                    var reflectionRotation = 2 * Vector2.Angle(hit.normal, localMoveDir) - 180;
                    var rotatedDir = Quaternion.Euler(0, 0, reflectionRotation) * localMoveDir;
                    Debug.DrawRay(hit.centroid, rotatedDir);
                    transform.Rotate(0, 0, reflectionRotation);
                    transform.Translate(mVel * Time.fixedDeltaTime, 0f, 0f);
                }
                else
                {
                    Debug.Log("Hit wall");
                    Destroy(gameObject);
                    mPhotonView.RPC("DestroyBullet", PhotonTargets.Others, mPhotonView.viewID);
                }
            }
        }
    }
}
