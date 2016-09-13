using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject mPlayer;

    void Update()
    {
        Vector3 pos = mPlayer.transform.position;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
