using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class CoinInventory : MonoBehaviour
    {
    	[SerializeField]
        private int mCoinCount;
        [SerializeField]
        private int mDepositCount;

        void Start()
        {
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
    }
}
