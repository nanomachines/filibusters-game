using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class CoinInventory : MonoBehaviour
    {
    	[SerializeField]
        private int mCoinCount;
        [SerializeField]
        private int mDepositCount;

        private Text mCoinText;
        private Text mVotesText;

        void Start()
        {
            mCoinCount = 0;
			mDepositCount = 0;

            mCoinText = GameObject.FindWithTag("CoinText").GetComponent<Text>();
            if (!mCoinText)
            {
                Debug.LogError("No text display for coins found! Tag a text object with a CoinText tag.");
            }
            mCoinText.text = "Coin Count: 0";

            mVotesText = GameObject.FindWithTag("VoteText").GetComponent<Text>();
            if (!mVotesText)
            {
                Debug.LogError("No text display for votes found! Tag a text object with a VoteText tag.");
            }
            mVotesText.text = "Votes: 0";
        }

        public void AddCoin()
        {
            ++mCoinCount;
            mCoinText.text = "Coin Count: " + mCoinCount;
        }

        public bool DepositCoin()
        {
        	if (mCoinCount > 0)
        	{
				--mCoinCount;
				++mDepositCount;

                mCoinText.text = "Coin Count: " + mCoinCount;
                mVotesText.text = "Votes: " + mDepositCount;
                return true;
        	}
        	else
        	{
        		return false;
        	}
        }
    }
}
