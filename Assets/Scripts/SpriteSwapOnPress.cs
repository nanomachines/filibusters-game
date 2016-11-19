using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Filibusters
{
    public class SpriteSwapOnPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ISubmitHandler
    {
        Image mButtonImage;
        [SerializeField]
        Sprite mPressedSprite;
        Sprite mOriginalSprite;

        void Start()
        {
            mButtonImage = GetComponent<Image>();
            mOriginalSprite = mButtonImage.sprite;
            EventSystem.OnHostOrJoinFailedEvent += OnCancel;
        }

        void OnDestroy()
        {
            EventSystem.OnHostOrJoinFailedEvent -= OnCancel;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            mButtonImage.sprite = mPressedSprite;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            mButtonImage.sprite = mOriginalSprite;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            mButtonImage.sprite = mPressedSprite;
        }

        public void OnCancel()
        {
            mButtonImage.sprite = mOriginalSprite;
        }
    }
}
