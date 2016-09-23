using UnityEngine;
using System.Collections;

public class CoinManager : Photon.PunBehaviour
{
	private bool mHasBeenCollected;
	private SpriteRenderer mRenderer;
	private CircleCollider2D mCollider;
	private PhotonView mPhotonView;

	[SerializeField]
	private float SecondsToRespawn;

	void Start()
	{
		mHasBeenCollected = false;
		mRenderer  = GetComponent<SpriteRenderer>();
		mCollider = GetComponent<CircleCollider2D>();
		mPhotonView = GetComponent<PhotonView>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("coin");
		// this is an id that maps 1-to-1 with the player who collected the coin
		int ownerId = other.gameObject.GetComponent<PhotonView>().ownerId;
		mPhotonView.RPC("OnCoinTriggered", PhotonTargets.MasterClient, ownerId);
	}

	[PunRPC]
	public void OnCoinTriggered(int ownerId)
	{
		if (!mHasBeenCollected)
		{
			mHasBeenCollected = true;
			Debug.Log("Master Verified Coin Collected");
			mPhotonView.RPC("OnCoinCollectionVerified", PhotonTargets.All, ownerId);
		}
	}

	[PunRPC]
	public void OnCoinCollectionVerified(int ownerId)
	{
		Debug.Log("Coin Collection Verified");

		Despawn();
		AddCoin(ownerId);
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

	private void AddCoin(int ownerId)
	{

	}

	private IEnumerator RespawnTimer()
	{
		yield return new WaitForSeconds(SecondsToRespawn);
		mPhotonView.RPC("Respawn", PhotonTargets.All);
	}

	[PunRPC]
	public void Respawn()  
	{
		// TODO: get random position for coin
		transform.position = Vector3.zero;
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
