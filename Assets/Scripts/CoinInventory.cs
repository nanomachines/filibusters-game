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

        public void AddCoin()
        {
            ++mCoinCount;
            EventSystem.OnCoinCollected(GetComponent<PhotonView>().owner.ID);
        }

        public bool DepositCoin()
        {
            if (mCoinCount > 0)
            {
                --mCoinCount;
                ++mDepositCount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetCoins(int playerViewId)
        {
            if (playerViewId == mPhotonView.viewID)
            {
                mCoinCount = 0;
            }
        }
    }
}
