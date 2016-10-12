using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

namespace Filibusters
{
	public class PlayerUIManager : MonoBehaviour 
	{
		public CoinInventory mInventoryScript;
		private Text mCoinText;
	    private Text mVotesText;

        private Slider mHealthBarControl;
        private Image mHealthBar;
        [SerializeField]
        private Color mHighHealthColor;
        [SerializeField]
        private Color mMedHealthColor;
        [SerializeField]
        private Color mLowHealthColor;

	    void Start()
	    {
	    	// Enable the Canvas and assign the main camera to it
			Camera mainCamera = GameObject.FindGameObjectWithTag(Tags.MAIN_CAMERA).GetComponent<Camera>();
			var canvas = GetComponent<Canvas>();
			canvas.enabled = true;
			canvas.worldCamera = mainCamera;
            canvas.sortingLayerName = Layers.PLAYER;

			// Assign Coin and Votes Text elements
	        foreach (Transform t in transform)
	        {
	            GameObject child = t.gameObject;

	            if (child.tag == Tags.COIN_TEXT)
	            {
	                mCoinText = child.GetComponent<Text>();
	            }
	            else if (child.tag == Tags.VOTE_TEXT)
	            {
	                mVotesText = child.GetComponent<Text>();
	            }
                else if (child.tag == Tags.HEALTH_BAR)
                {
                    mHealthBarControl = child.GetComponent<Slider>();
                }
            }
            mHealthBar = mHealthBarControl.fillRect.gameObject.GetComponentInChildren<Image>();

            // Check for null elements
            Assert.IsNotNull(mCoinText, "No text display for coins found! Tag a text object with a CoinText tag.");
            Assert.IsNotNull(mVotesText, "No text display for votes found! Tag a text object with a VoteText tag.");
            Assert.IsNotNull(mInventoryScript, "No inventory script found! This should have been set in the SpawnManager.");
            Assert.IsNotNull(mHealthBarControl, "No health bar slider attached to the player ui! Tag a slider object with a HealthBar tag.");
            Assert.IsNotNull(mHealthBar, "No health bar image attached to the player ui! Tag a Image object parented under the Slider RectTransform with a HealthBar tag.");

            EventSystem.OnUpdateHealthBarEvent += UpdateHealthBar;
	    }
		
        // TODO: Attach coin updates and deposit updates to event system.
        // Remove player inventory script assignment in SpawnManager
		void Update () 
		{
			mCoinText.text = "Coins: " + mInventoryScript.CoinCount;
			mVotesText.text = "Votes: " + mInventoryScript.DepositCount;
		}

        void UpdateHealthBar(int health)
        {
            mHealthBarControl.value = health;
            if (health > 60)
            {
                mHealthBar.color = mHighHealthColor;
            }
            else if (health <= 20)
            {
                mHealthBar.color = mLowHealthColor;
            }
            else
            {
                mHealthBar.color = mMedHealthColor;
            }
        }
	}

}
