using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class DustCloudManager : MonoBehaviour
    {
        public GameObject DustCloudPrefab;
        static readonly int MAX_DUST_CLOUDS = 20;
        static readonly string DUST_TRIGGER = "DustTrigger";

        GameObject[] mDustCloudPool;
        Animator[] mDustCloudAnimators;
        int mCurrentDustCloud;

        PhotonView mPhotonView;

        void Start()
        {
            mDustCloudPool = new GameObject[MAX_DUST_CLOUDS];
            for (int i = 0; i < MAX_DUST_CLOUDS; ++i)
            {
                mDustCloudPool[i] = Instantiate(DustCloudPrefab);
            }
            mCurrentDustCloud = 0;
            mPhotonView = GetComponent<PhotonView>();

            EventSystem.OnJumpEvent += DispatchDustEffect;
        }

        void OnDestroy()
        {
            EventSystem.OnJumpEvent -= DispatchDustEffect;
        }

        void DispatchDustEffect(Vector3 pos)
        {
            mPhotonView.RPC("RunDustEffect", PhotonTargets.All, pos);
        }

        [PunRPC]
        void RunDustEffect(Vector3 pos)
        {
            GameObject dustCloud = mDustCloudPool[mCurrentDustCloud];
            dustCloud.transform.position = pos;
            dustCloud.GetComponent<Animator>().SetTrigger(DUST_TRIGGER);
            mCurrentDustCloud = (mCurrentDustCloud + 1) % MAX_DUST_CLOUDS;
        }
    }
}
