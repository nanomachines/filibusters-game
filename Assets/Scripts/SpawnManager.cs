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
                LocalPlayer = PhotonNetwork.Instantiate("NetPlayer", GetRandomSpawnPoint(), Quaternion.identity, 0);
                LocalPlayer.GetComponent<SimplePhysics>().enabled = true;
                LocalPlayer.GetComponent<AimingController>().enabled = true;

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

        public Vector3 GetRandomSpawnPoint()
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform.position;
        }
    }
}
