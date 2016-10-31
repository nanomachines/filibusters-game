using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Filibusters
{
    public class DepositTextAnimator : MonoBehaviour 
    {
        private Text mDepositText;
        [SerializeField]
        private int mNewSize;
        private int mOriginalSize;
        private int mSizeDiff;

        [SerializeField]
        private int mPlayerNum;

        void Start() 
        {
            mDepositText = GetComponent<Text>();
            mOriginalSize = mDepositText.fontSize;
            mSizeDiff = mNewSize - mOriginalSize;
            EventSystem.OnCoinDepositedEvent += (int ownerId, int newDepositBalance) => 
            {
                if (NetworkManager.GetPlayerNumber(PhotonPlayer.Find(ownerId)) == mPlayerNum)
                {
                    StartCoroutine(AnimateText());
                }
            };
        }
        
        IEnumerator AnimateText()
        {
            for (int i = 1; i <= 5; i++)
            {
                // I originally meant to divide by 5 here, however, this provides a better effect
                mDepositText.fontSize = mOriginalSize + mSizeDiff * (i / 2);
                yield return new WaitForSeconds(0.05f);
            }
            mDepositText.fontSize = mOriginalSize;
        }
    }
}
