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

        [SerializeField]
        private LayerMask mColLayers;
        private Vector2 mSize = Vector2.zero;
//        private Vector2 mOffset = Vector2.zero;

        void Start()
        {
            mPhotonView = GetComponent<PhotonView>();

            Vector3 scale = transform.localScale;
            BoxCollider2D bCol = GetComponent<BoxCollider2D>();
            Assert.IsNotNull(bCol, "BoxCollider2D component missing");

            mSize = new Vector2(bCol.size.x * scale.x, bCol.size.y * scale.y);
            //mOffset = new Vector2(bCol.offset.x * scale.x, bCol.offset.y * scale.y);
        }
        
        void Update() 
        {
            float movement = mVel * Time.deltaTime;
            transform.Translate(movement, 0f, 0f);

            DetectCollisions();
        }

        void DetectCollisions()
        {
            float castDistance = mSize.x / 2f + 0.1f;

            Vector2 origin = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = transform.TransformDirection(Vector2.right);

            Ray2D ray = new Ray2D(origin, direction);
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan);

            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(ray.origin, ray.direction, castDistance, mColLayers))
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
        }

        [PunRPC]
        public void DestroyBullet(int viewId)
        {
            Debug.Log("Hit player");
            Destroy(PhotonView.Find(viewId).gameObject);
        }
    }
}
