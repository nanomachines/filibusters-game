using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Filibusters
{
    public class ErrorToast : MonoBehaviour 
    {
        static readonly float ToastTime = 4f;

        [SerializeField]
        GameObject mError;
        Text mErrorText;
        bool mDisplayError;
        float mCurrentToastTime;

        void Start()
        {
            mErrorText = mError.GetComponent<Text>();
            mCurrentToastTime = 0f;
        }

        public void ToastError(string text)
        {
            mErrorText.text = text;
            mDisplayError = true;
            mCurrentToastTime = 0f;
        }

        public void Update()
        {
            mError.SetActive(mDisplayError);
            if (mCurrentToastTime > ToastTime)
            {
                mDisplayError = false;
            }

            if (mDisplayError)
            {
                mCurrentToastTime += Time.deltaTime;
            }
        }
    }
}


