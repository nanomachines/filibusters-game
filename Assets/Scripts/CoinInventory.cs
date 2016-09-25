using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class CoinInventory : MonoBehaviour
    {
        private int mCoinCount;
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

        public void DepositCoin()
        {
        	--mCoinCount;
			++mDepositCount;
        }
    }
}
