using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class SpikeBoundary : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == Tags.PLAYER)
            {
                int playerNum = NetworkManager.GetPlayerNumber(other.gameObject.GetComponent<PhotonView>().owner);
                other.gameObject.GetComponent<LifeManager>().Die();
                EventSystem.OnSuicide(playerNum);
            }
        }
    }
}
