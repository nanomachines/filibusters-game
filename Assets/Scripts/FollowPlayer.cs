using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    public GameObject mPlayer;

    void Update()
    {
        if (mPlayer)
        {
            Vector3 pos = mPlayer.transform.position;
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }
    }
}
