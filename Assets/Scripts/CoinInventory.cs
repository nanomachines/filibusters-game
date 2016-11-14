using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class CoinInventory : MonoBehaviour
    {
        private int mCoinCount;
        private PhotonView mPhotonView;
        public int CoinCount
        {
            get
            {
                return mCoinCount;
            }
        }

        private int mDepositCount;
        public int DepositCount
        {
            get
            {
                return mDepositCount;
            }
        }

        void Start()
        {
            mCoinCount = 0;
            mDepositCount = 0;
            mPhotonView = GetComponent<PhotonView>();
            EventSystem.OnDeathEvent += ResetCoins;
        }

        void OnDestroy()
        {
            EventSystem.OnDeathEvent -= ResetCoins;
        }

        public void AddCoin()
        {
            ++mCoinCount;
            var id = mPhotonView.owner.ID;
            EventSystem.OnCoinCollected(id);
            EventSystem.OnCoinCountUpdated(id, mCoinCount);
        }

        public bool DepositCoin()
        {
            if (mCoinCount > 0)
            {
                --mCoinCount;
                ++mDepositCount;
                EventSystem.OnCoinCountUpdated(mPhotonView.owner.ID, mCoinCount);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetCoins(int playerViewId, Vector3 pos)
        {
            if (playerViewId == mPhotonView.viewID)
            {
                mCoinCount = 0;
                EventSystem.OnCoinCountUpdated(mPhotonView.owner.ID, mCoinCount);
            }
        }
    }
}
