using UnityEngine;

namespace Filibusters
{
    public class SpawnManager : MonoBehaviour
    {
        private Vector3[] SpawnLocations = {
        new Vector3(0.09f, -0.317f, 0f),
        new Vector3(-14.45f, -0.31f, 0f)
    };


        // Use this for initialization
        void Start()
        {
            Vector3 spawnPositon = SpawnLocations[Random.Range(0, SpawnLocations.Length)];
            GameObject localPlayer = PhotonNetwork.Instantiate("NetPlayer", spawnPositon, Quaternion.identity, 0);
            localPlayer.GetComponent<SimplePhysics>().enabled = true;
            FollowPlayer followScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
            followScript.mPlayer = localPlayer;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
