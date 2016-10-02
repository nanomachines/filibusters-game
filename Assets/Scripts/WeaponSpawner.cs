using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class WeaponSpawner : Photon.PunBehaviour
    {
        private bool mHasBeenCollected;
        private SpriteRenderer mRenderer;
        private BoxCollider2D mCollider;
        private PhotonView mPhotonView;

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
            mCollider = GetComponent<BoxCollider2D>();
            mPhotonView = GetComponent<PhotonView>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Tags.PLAYER &&
                other.gameObject.GetComponent<WeaponInventory>().CanEquip())
            {
                Debug.Log("weapon");
                // this is an id that maps 1-to-1 with the player who collected the weapon
                int viewId = other.gameObject.GetComponent<PhotonView>().viewID;
                mPhotonView.RPC("OnWeaponTriggered", PhotonTargets.MasterClient, viewId);
            }
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