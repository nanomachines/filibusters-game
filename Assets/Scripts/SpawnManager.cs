using UnityEngine;

namespace Filibusters
{
    public class SpawnManager : MonoBehaviour
    {
        private GameObject[] SpawnPoints;
        GameObject LocalPlayer;

        // Use this for initialization
        void Start()
        {
            SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
            Vector3 spawnPositon = GetRandomSpawnPoint();
            GameObject LocalPlayer = PhotonNetwork.Instantiate("NetPlayer", spawnPositon, Quaternion.identity, 0);
            LocalPlayer.GetComponent<SimplePhysics>().enabled = true;
            FollowPlayer followScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
            followScript.mPlayer = LocalPlayer;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void RespawnLocalPlayer()
        {
            LocalPlayer.transform.position = GetRandomSpawnPoint();
        }

        private Vector3 GetRandomSpawnPoint()
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }
    }
}
