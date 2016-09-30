using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class CoinInventory : MonoBehaviour
    {
        private int mCoinCount;
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
            EventSystem.OnDeathEvent += ResetCoins;
            mCoinCount = 0;
			mDepositCount = 0;
        }

        public void AddCoin()
        {
            ++mCoinCount;
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
            mCoinCount = 0;
        }
    }
}
