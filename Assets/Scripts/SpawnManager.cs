using UnityEngine;
using UnityEngine.UI;

namespace Filibusters
{
    public class SpawnManager : MonoBehaviour
    {
        private GameObject[] SpawnPoints;
        GameObject LocalPlayer;
        GameObject PlayerUI;

        [HideInInspector]
        public static SpawnManager Instance;

        // Use this for initialization
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

                // Instantiate player UI Canvas and enable it
                PlayerUI = PhotonNetwork.Instantiate("PlayerUI", new Vector3(0, 0, 0), Quaternion.identity, 0);
                Canvas canvas = PlayerUI.GetComponent<Canvas>();
                canvas.enabled = true;
                canvas.worldCamera = mainCamera.GetComponent<Camera>();

                // Set the PlayerUI's parent to the UI GameObject
                GameObject UIParent = GameObject.FindWithTag("UI");
                PlayerUI.transform.SetParent(UIParent.transform);

                // Assign Coin and Votes Text elements to the LocalPlayer's CoinInventory script
                var coinScript = LocalPlayer.GetComponent<CoinInventory>();
                foreach (Transform t in PlayerUI.transform)
                {
                    GameObject child = t.gameObject;

                    if (child.tag == "CoinText")
                    {
                        coinScript.mCoinText = child.GetComponent<Text>();
                    }
                    else if (child.tag == "VoteText")
                    {
                        coinScript.mVotesText = child.GetComponent<Text>();
                    }
                }
        	}
        	else
        	{
        		DestroyObject(this);
        	}
            
        }

        // Update is called once per frame
        void Update()
        {
        }

        public Vector3 GetRandomSpawnPoint()
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }
    }
}
