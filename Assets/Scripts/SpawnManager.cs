using UnityEngine;

namespace Filibusters
{
    struct SpawnOrderTuple
    {
        public float distance;
        public int order;
    }

    public class SpawnManager : MonoBehaviour
    {
        private GameObject[] SpawnPoints;
        private SpawnOrderTuple[] mAveragePlayerDistanceToSpawn;
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
                mAveragePlayerDistanceToSpawn = new SpawnOrderTuple[SpawnPoints.Length];
                LocalPlayer = PhotonNetwork.Instantiate(Utility.PlayerNumberToPrefab(NetworkManager.LocalPlayerNumber),
                    SpawnPoints[NetworkManager.LocalPlayerNumber].transform.position, Quaternion.identity, 0);
                LocalPlayer.GetComponent<SimplePhysics>().enabled = true;
                LocalPlayer.GetComponent<AimingController>().enabled = true;
                LocalPlayer.GetComponent<SwapButtonToggle>().enabled = true;

                GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                FollowPlayer followScript = mainCamera.GetComponent<FollowPlayer>();
                followScript.mPlayer = LocalPlayer;

                PlayerUI = Utility.GetChildWithTag(LocalPlayer, Tags.PLAYER_UI);
                PlayerUI.SetActive(true);

                LocalPlayer.GetComponent<PlayerAttack>().enabled = true;
            }
            else
            {
                DestroyObject(this);
            }

            Debug.Log("Player list");
            foreach (var player in PhotonNetwork.playerList)
            {
                Debug.Log(player.ID + " :: " + player.isLocal);
            }
        }

        public Vector3 GetFurthestAverageSpawnPoint(GameObject playerToSpawn)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.PLAYER);
            if (players.Length < 2)
            {
                return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
            }

            for (int i = 0; i < SpawnPoints.Length; ++i)
            {
                float avgDistance = 0;
                Vector3 spawnPos = SpawnPoints[i].transform.position;
                foreach (GameObject player in players)
                {
                    if (player != playerToSpawn)
                    {
                        avgDistance += Vector3.SqrMagnitude(spawnPos - player.transform.position);
                    }
                }
                
                avgDistance /= playerToSpawn == null ? players.Length : (players.Length - 1);
                mAveragePlayerDistanceToSpawn[i].distance = avgDistance;
                mAveragePlayerDistanceToSpawn[i].order = i;
            }

            /*
            int furthestSpawnIndex = 0;
            float furthestSpawnAvgDistance = mAveragePlayerDistanceToSpawn[0];
            for (int i = 1; i < mAveragePlayerDistanceToSpawn.Length; ++i)
            {
                float nextAverageDistance = mAveragePlayerDistanceToSpawn[i];
                if (furthestSpawnAvgDistance < nextAverageDistance)
                {
                    furthestSpawnIndex = i;
                    furthestSpawnAvgDistance = nextAverageDistance;
                }

                else if (Mathf.Abs(furthestSpawnAvgDistance - nextAverageDistance) < Mathf.Epsilon)
                {
                    if (Random.Range(0, 1f) < 0.5f)
                    {
                        furthestSpawnIndex = i;
                        furthestSpawnAvgDistance = nextAverageDistance;
                    }
                }
            }

            return SpawnPoints[furthestSpawnIndex].transform.position;
            */
            // sort in reverse order based on distance
            System.Array.Sort(mAveragePlayerDistanceToSpawn, (x, y) => y.distance.CompareTo(x.distance));

            int roll = Random.Range(0, 10);
            int spawnIndex;
            if (roll < 5)
            {
                spawnIndex = mAveragePlayerDistanceToSpawn[0].order;
            }
            else if (roll < 8)
            {
                spawnIndex = mAveragePlayerDistanceToSpawn[1].order;
            }
            else
            {
                spawnIndex = mAveragePlayerDistanceToSpawn[2].order;
            }
            return SpawnPoints[spawnIndex].transform.position;
        }
    }
}
