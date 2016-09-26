using UnityEngine;

namespace Filibusters
{
    public class SpawnManager : MonoBehaviour
    {
        private GameObject[] SpawnPoints;
        GameObject LocalPlayer;

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
	            FollowPlayer followScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
	            followScript.mPlayer = LocalPlayer;
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
