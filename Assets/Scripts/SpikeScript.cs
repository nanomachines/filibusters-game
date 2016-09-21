using UnityEngine;

namespace Filibusters
{
    public class SpikeScript : MonoBehaviour
    {
        SpawnManager Spawner;

        void Start()
        {
            Spawner = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player" && Spawner.IsMyLocalPlayer(col.gameObject))
            {
                Spawner.RespawnLocalPlayer();
            }
        }
    }
}
