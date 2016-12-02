﻿using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class AimingController : MonoBehaviour
    {
        PlayerState mPlayerState;
        float mAngle;

        private bool mFacingRight
        {
            get { return mPlayerState.mFacingRight; }
            set { mPlayerState.mFacingRight = value; }
        }

        private Aim mAimingDir
        {
            get { return mPlayerState.mAimingDir; }
            set { mPlayerState.mAimingDir = value; }
        }

        private WeaponId mWeaponId
        {
            get { return mPlayerState.mWeaponId;  }
        }

        // Use this for initialization
        void Start()
        {
            mPlayerState = GetComponent<PlayerState>();
        }
        
        // Update is called once per frame
        public void Update()
        {
            UpdateAimFromInput();
        }

        void UpdateAimFromInput()
        {
            var x = InputWrapper.Instance.RightXInput;
            var y = InputWrapper.Instance.RightYInput;
            if (x == 0 && y == 0)
            {
                return;
            }

            float angle = Mathf.Acos(x) * 180 / Mathf.PI;

            if (y < -Mathf.Epsilon)
            {
                angle = -angle;
            }

            mAngle = angle;

            if (angle > -22.5 && angle <= 22.5)
            {
                // 0
                mAimingDir = Aim.RIGHT;
                mFacingRight = true;
            }

            else if (angle > 22.5 && angle <= 67.5)
            {
                // 45
                mAimingDir = Aim.RIGHT_UP; 
                mFacingRight = true;
            }
            else if (angle > 67.5 && angle <= 90)
            {
                // 90
                mAimingDir = Aim.UP; 
                mFacingRight = true;
            }
            else if (angle > 90 && angle <= 112.5)
            {
                // 90
                mAimingDir = Aim.UP; 
                mFacingRight = false;
            }
            else if (angle > 112.5 && angle <= 157.5)
            {
                // 135
                mAimingDir = Aim.LEFT_UP; 
                mFacingRight = false;
            }
            else if ((angle > 157.5 && angle <= 180.0) ||
                (angle > -180 && angle <= -157.5))
            {
                // 180
                mAimingDir = Aim.LEFT;
                mFacingRight = false;
            }
            else if (angle > -67.5 && angle <= -22.5)
            {
                // -45
                mAimingDir = Aim.RIGHT_DOWN;
                mFacingRight = true;
            }
            else if (angle > -112.5 && angle <= -90)
            {
                // -90
                mAimingDir = Aim.DOWN; 
                mFacingRight = false;
            }
            else if (angle > -90 && angle <= -67.5)
            {
                // -90
                mAimingDir = Aim.DOWN; 
                mFacingRight = true;
            }
            else if (angle > -157.5 && angle <= -112.5)
            {
                // -135
                mAimingDir = Aim.LEFT_DOWN;
                mFacingRight = false;
            }
        }
    }
}
