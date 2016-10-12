using UnityEngine;
using System.Collections;

namespace Filibusters
{
	public class MagicBulletProjectileController : ProjectileController
	{
        [SerializeField]
        private int mRicochetCounter;

        protected override void HandleCollisions(RaycastHit2D hit)
        {
            GameObject obj = hit.transform.gameObject;
            // Player
            if (obj.tag == Tags.PLAYER)
            {
                mPhotonView.RPC("DestroyBullet", PhotonTargets.Others, mPhotonView.viewID);
                obj.GetComponent<LifeManager>().Die();
                Destroy(gameObject);
            }
            // Walls and floors
            if (obj.layer != LayerMask.NameToLayer(Layers.PLAYER))
            {
                if (mRicochetCounter-- > 0)
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
