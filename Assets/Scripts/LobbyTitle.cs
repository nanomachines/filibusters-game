using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class LobbyTitle : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Text>().text += PhotonNetwork.room.name;
        }
    }
}
