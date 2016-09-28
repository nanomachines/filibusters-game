using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
	public class PlayerUIManager : MonoBehaviour 
	{
		public CoinInventory mInventoryScript;
		private Text mCoinText;
	    private Text mVotesText;

	    void Start()
	    {
	    	// Enable the Canvas and assign the main camera to it
			Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			var canvas = GetComponent<Canvas>();
			canvas.enabled = true;
			canvas.worldCamera = mainCamera;
            canvas.sortingLayerName = "UI";

			// Assign Coin and Votes Text elements
	        foreach (Transform t in transform)
	        {
	            GameObject child = t.gameObject;

	            if (child.tag == "CoinText")
	            {
	                mCoinText = child.GetComponent<Text>();
	            }
	            else if (child.tag == "VoteText")
	            {
	                mVotesText = child.GetComponent<Text>();
	            }
	        }

	        // Check for null elements
			if (!mCoinText)
            {
                Debug.LogError("No text display for coins found! Tag a text object with a CoinText tag.");
            }

			if (!mVotesText)
            {
                Debug.LogError("No text display for votes found! Tag a text object with a VoteText tag.");
            }

			if (!mInventoryScript)
			{
                Debug.LogError("No inventory script found! This should have been set in the SpawnManager.");
            }
	    }
		
		void Update () 
		{
			mCoinText.text = "Coins: " + mInventoryScript.CoinCount;
			mVotesText.text = "Votes: " + mInventoryScript.DepositCount;
		}
	}

}
