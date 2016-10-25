using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class WeaponSpawner : Photon.PunBehaviour
    {
        private bool mHasBeenCollected;
        private SpriteRenderer mRenderer;
        private PhotonView mPhotonView;

        private BoxCollider2D mCollider;
        private Vector2 mTLCorner;
        private Vector2 mBRCorner;
        [SerializeField]
        private LayerMask mPlayerLayer;

        // If SecondsToRespawn is less than zero that means
        // that we don't want weapons to respawn (i.e. in the 
        // ready menu)
        [SerializeField]
        private float SecondsToRespawn;

        [SerializeField]
        private WeaponId mWeaponId;

        void Start()
        {
            mHasBeenCollected = false;
            mRenderer = GetComponent<SpriteRenderer>();
            mPhotonView = GetComponent<PhotonView>();

            mCollider = GetComponent<BoxCollider2D>();

            float halfX = mCollider.bounds.extents.x;
            float halfY = mCollider.bounds.extents.y;
            float offsetX = mCollider.offset.x;
            float offsetY = mCollider.offset.y;

            mTLCorner = transform.TransformPoint(offsetX - halfX, offsetY + halfY, 0f);
            mBRCorner = transform.TransformPoint(offsetX + halfX, offsetY - halfY, 0f);
        }

        void Update()
        {
            if (InputWrapper.Instance.EquipWeaponPressed)
            {
                CheckForPlayerCollisions();
            }
        }

        void CheckForPlayerCollisions()
        {
            Collider2D[] playersInBoundingBox = Physics2D.OverlapAreaAll(mTLCorner, mBRCorner, mPlayerLayer);
            Collider2D player = FindPlayerWhoPressedEquip(playersInBoundingBox);
            if (player != null)
            {
                Debug.Log("weapon");
                // this is an id that maps 1-to-1 with the player who collected the weapon
                int playerViewId = player.gameObject.GetComponent<PhotonView>().viewID;
                mPhotonView.RPC("OnWeaponTriggered", PhotonTargets.MasterClient, playerViewId);
                // disable swap prompt for each player currently in the weapon's bounding box
                for (int i = 0; i < playersInBoundingBox.Length; i++)
                {
                    int viewId = playersInBoundingBox[i].gameObject.GetPhotonView().viewID;
                    mPhotonView.RPC("DisableSwapPrompt", PhotonTargets.All, viewId);
                }
            }
        }  
        
        Collider2D FindPlayerWhoPressedEquip(Collider2D[] playersInBoundingBox)
        {
            for (int i = 0; i < playersInBoundingBox.Length; i++)
            {
                if (playersInBoundingBox[i].gameObject.GetPhotonView().isMine)
                {
                    return playersInBoundingBox[i];
                }
            }
            return null;
        }

        [PunRPC]
        public void DisableSwapPrompt(int viewId)
        {
            var swapToggleScript = PhotonView.Find(viewId).gameObject.GetComponent<SwapButtonToggle>();
            swapToggleScript.DisableOnOtherPlayerEquip(viewId);
        }

        [PunRPC]
        public void OnWeaponTriggered(int viewId)
        {
            if (!mHasBeenCollected)
            {
                mHasBeenCollected = true;
                Debug.Log("Master Verified Weapon Collected");
                mPhotonView.RPC("OnWeaponCollectionVerified", PhotonTargets.All, viewId);
            }
        }

        [PunRPC]
        public void OnWeaponCollectionVerified(int viewId)
        {
            Debug.Log("Weapon Collection Verified");

            EquipWeapon(viewId);
            Despawn();
            if (PhotonNetwork.isMasterClient)
            {
                StartCoroutine(RespawnTimer());
            }
        }

        private void Despawn()
        {
            mRenderer.enabled = false;
            mCollider.enabled = false;
            mHasBeenCollected = true;
        }

        private void EquipWeapon(int viewId)
        {
            PhotonView.Find(viewId).gameObject.GetComponent<WeaponInventory>().EquipWeapon(mWeaponId);
        }

        private IEnumerator RespawnTimer()
        {
            // If SecondsToRespawn is less than zero that means
            // that we don't want weapons to respawn (i.e. in the 
            // ready menu)
            if (SecondsToRespawn >= 0)
            {
                yield return new WaitForSeconds(SecondsToRespawn);
                mPhotonView.RPC("Respawn", PhotonTargets.All);
            }
        }

        [PunRPC]
        public void Respawn()
        {
            // TODO: get random position for weapon
            //transform.position = Vector3.zero;
            mHasBeenCollected = false;
            mRenderer.enabled = true;
            mCollider.enabled = true;
        }

        public override void OnMasterClientSwitched(PhotonPlayer newPlayer)
        {
            if (PhotonNetwork.isMasterClient && mHasBeenCollected)
            {
                StartCoroutine(RespawnTimer());
            }
        }
    }
}