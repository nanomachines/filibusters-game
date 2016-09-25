using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class CoinInventory : MonoBehaviour
    {
        private int mCoinCount;

        // Use this for initialization
        void Start()
        {
            mCoinCount = 0;
        }

        public void AddCoin()
        {
            ++mCoinCount;
        }
    }
}
