using UnityEngine;

namespace Filibusters
{
    public class SpawnManager : MonoBehaviour
    {
        private GameObject[] SpawnPoints;

        // Use this for initialization
        void Start()
        {
            SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
            Vector3 spawnPositon = GetRandomSpawnPoint();
            GameObject localPlayer = PhotonNetwork.Instantiate("NetPlayer", spawnPositon, Quaternion.identity, 0);
            localPlayer.GetComponent<SimplePhysics>().enabled = true;
            FollowPlayer followScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
            followScript.mPlayer = localPlayer;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private Vector3 GetRandomSpawnPoint()
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }
    }
}
