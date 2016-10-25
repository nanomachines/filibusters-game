using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Filibusters
{
    public class HowToPlayMenuController : MonoBehaviour
    {
        [SerializeField]
        GameObject[] mTutorialSlides;
        int mCurrentSlide;

        [SerializeField]
        Button mBackButton;

        [SerializeField]
        Button mExitButton;

        [SerializeField]
        Button mNextButton;

        [SerializeField]
        UnityEngine.EventSystems.EventSystem mUiEventSystem;

        void Start()
        {
            mCurrentSlide = 0;
            mTutorialSlides[mCurrentSlide].SetActive(true);
            UpdateButtonInteractivity();

            mBackButton.onClick.AddListener(NavigateBack);
            mNextButton.onClick.AddListener(NavigateNext);
            mExitButton.onClick.AddListener(ExitHowToPlay);
        }
		
        void NavigateBack()
        {
            mTutorialSlides[mCurrentSlide].SetActive(false);
            mTutorialSlides[--mCurrentSlide].SetActive(true);

            UpdateButtonInteractivity();
        }

        void NavigateNext()
        {
            mTutorialSlides[mCurrentSlide].SetActive(false);
            mTutorialSlides[++mCurrentSlide].SetActive(true);
            UpdateButtonInteractivity();
        }

        void ExitHowToPlay()
        {
            SceneManager.LoadScene(Scenes.START_MENU);
        }

        void UpdateButtonInteractivity()
        {
            mBackButton.interactable = mCurrentSlide != 0;
            mNextButton.interactable = mCurrentSlide != (mTutorialSlides.Length - 1);
            if (mCurrentSlide == 0 || mCurrentSlide == mTutorialSlides.Length - 1)
            {
                mUiEventSystem.SetSelectedGameObject(mUiEventSystem.firstSelectedGameObject);
            }
        }
    }
}
