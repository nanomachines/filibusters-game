using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class WallHitFXManager : MonoBehaviour
    {
        public GameObject WallHitFXPrefab;
        static readonly int MAX_WALL_HIT_CLOUDS = 20;
        static readonly string WALL_HIT_TRIGGER = "WallHitTrigger";

        GameObject[] mFXPool;
        int mCurrentFX;

        PhotonView mPhotonView;

        void Start()
        {
            mFXPool = new GameObject[MAX_WALL_HIT_CLOUDS];
            for (int i = 0; i < MAX_WALL_HIT_CLOUDS; ++i)
            {
                var newFX = Instantiate(WallHitFXPrefab);
                newFX.transform.parent = transform;
                mFXPool[i] = newFX;
            }
            mCurrentFX = 0;
            mPhotonView = GetComponent<PhotonView>();

            EventSystem.OnWallHitEvent += RunWallHitEffect;
        }

        void OnDestroy()
        {
            EventSystem.OnWallHitEvent -= RunWallHitEffect;
        }

        void RunWallHitEffect(Vector3 pos, Vector3 normal)
        {
            GameObject fx = mFXPool[mCurrentFX];
            fx.transform.position = pos;
            fx.GetComponent<Animator>().SetTrigger(WALL_HIT_TRIGGER);
            mCurrentFX = (mCurrentFX + 1) % MAX_WALL_HIT_CLOUDS;
            fx.transform.rotation = Quaternion.FromToRotation(Vector3.left, normal);
        }
    }
}

