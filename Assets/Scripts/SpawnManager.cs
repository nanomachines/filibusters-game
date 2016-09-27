using UnityEngine;
using UnityEngine.UI;

namespace Filibusters
{
    public class SpawnManager : MonoBehaviour
    {
        private GameObject[] SpawnPoints;
        GameObject LocalPlayer;
        private GameObject PlayerUI;

        [HideInInspector]
        public static SpawnManager Instance;

        void Start()
        {
        	if (Instance == null)
        	{
        		Instance = this;
				SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
	            Vector3 spawnPositon = GetRandomSpawnPoint();
	            LocalPlayer = PhotonNetwork.Instantiate("NetPlayer", spawnPositon, Quaternion.identity, 0);
	            LocalPlayer.GetComponent<SimplePhysics>().enabled = true;

                GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                FollowPlayer followScript = mainCamera.GetComponent<FollowPlayer>();
	            followScript.mPlayer = LocalPlayer;

	            // Create the player's UI and associate the player's inventory script with its ui script
				PlayerUI = PhotonNetwork.Instantiate("PlayerUI", new Vector3(0, 0, 0), Quaternion.identity, 0);
				var uiScript = PlayerUI.GetComponent<PlayerUIManager>();
				uiScript.enabled = true;
				uiScript.mInventoryScript = LocalPlayer.GetComponent<CoinInventory>();
        	}
        	else
        	{
        		DestroyObject(this);
        	}
            
        }

        public Vector3 GetRandomSpawnPoint()
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }
    }
}
