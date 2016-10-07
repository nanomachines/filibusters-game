using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class CoinSpawner : Photon.PunBehaviour
    {
        private bool mHasBeenCollected;
        private SpriteRenderer mRenderer;
        private CircleCollider2D mCollider;
        private PhotonView mPhotonView;

        // If SecondsToRespawn is less than zero that means
        // that we don't want coins to respawn (i.e. in the 
        // ready menu)
        [SerializeField]
        private float SecondsToRespawn;

        void Start()
        {
            mHasBeenCollected = false;
            mRenderer = GetComponent<SpriteRenderer>();
            mCollider = GetComponent<CircleCollider2D>();
            mPhotonView = GetComponent<PhotonView>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Tags.PLAYER)
            {
                Debug.Log("coin");
                // this is an id that maps 1-to-1 with the player who collected the coin
                int viewId = other.gameObject.GetComponent<PhotonView>().viewID;
                mPhotonView.RPC("OnCoinTriggered", PhotonTargets.MasterClient, viewId);
            }
        }

        [PunRPC]
        public void OnCoinTriggered(int viewId)
        {
            if (!mHasBeenCollected)
            {
                mHasBeenCollected = true;
                Debug.Log("Master Verified Coin Collected");
                mPhotonView.RPC("OnCoinCollectionVerified", PhotonTargets.AllBuffered, viewId);
            }
        }

        [PunRPC]
        public void OnCoinCollectionVerified(int viewId)
        {
            Debug.Log("Coin Collection Verified");

            AddCoin(viewId);
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

        private void AddCoin(int viewId)
        {
            PhotonView.Find(viewId).gameObject.GetComponent<CoinInventory>().AddCoin();
        }

        private IEnumerator RespawnTimer()
        {
            // If SecondsToRespawn is less than zero that means
            // that we don't want coins to respawn (i.e. in the 
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
            // TODO: get random position for coin
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
