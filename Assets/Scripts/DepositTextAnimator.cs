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
        private AnimationCurve mSizeCurve;
        [SerializeField]
        private float mAnimationTime;

        [SerializeField]
        private int mPlayerNum;

        void Start() 
        {
            mDepositText = GetComponent<Text>();
            mOriginalSize = mDepositText.fontSize;
            mSizeDiff = mNewSize - mOriginalSize;
            EventSystem.OnCoinDepositedEvent += StartAnimation;
        }

        void OnDestroy()
        {
            EventSystem.OnCoinDepositedEvent -= StartAnimation;
        }

        void StartAnimation(int ownerId, int newDepositBalance)
        {
            if (NetworkManager.GetPlayerNumber(PhotonPlayer.Find(ownerId)) == mPlayerNum)
            {
                StartCoroutine(AnimateText());
            }
        }
        
        IEnumerator AnimateText()
        {
            float time = 0f;
            while (time < mAnimationTime)
            {
                var sizePercent = mSizeCurve.Evaluate(time);
                mDepositText.fontSize = mOriginalSize + Mathf.FloorToInt(mSizeDiff * sizePercent);
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }
        }
    }
}
