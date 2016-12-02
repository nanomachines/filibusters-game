using UnityEngine;
using System.Collections;
using WeaponId = Filibusters.GameConstants.WeaponId;

namespace Filibusters
{
    public class BarrelTip : MonoBehaviour
    {
        Transform mBarrelTransform;

        Transform[] mDarkHorseBarrelReferences = new Transform[(int)Aim.NUM_DIRECTIONS];
        Transform[] mVetoBarrelReferences = new Transform[(int)Aim.NUM_DIRECTIONS];
        Transform[] mMagicBulletBarrelReferences = new Transform[(int)Aim.NUM_DIRECTIONS];
        Transform[][] mBarrelReferences;

        PlayerState mPlayerState;

        void Start()
        {
            mPlayerState = GetComponent<PlayerState>();
            mBarrelTransform = transform.Find("BarrelTip");
            LoadBarrelReferences("DarkHorseMappings", mDarkHorseBarrelReferences);
            LoadBarrelReferences("VetoMappings", mVetoBarrelReferences);
            LoadBarrelReferences("MagicBulletMappings", mMagicBulletBarrelReferences);
            mBarrelReferences = new Transform[][] { mDarkHorseBarrelReferences, mVetoBarrelReferences, mMagicBulletBarrelReferences };
        }

        void LoadBarrelReferences(string mappingName, Transform[] barrelReferenceArray)
        {
            var mappingTransform = transform.Find(mappingName);
            barrelReferenceArray[(int)Aim.RIGHT] = mappingTransform.Find("RightBarrel");
            barrelReferenceArray[(int)Aim.RIGHT_UP] = mappingTransform.Find("RightUpBarrel");
            barrelReferenceArray[(int)Aim.RIGHT_DOWN] = mappingTransform.Find("RightDownBarrel");
            barrelReferenceArray[(int)Aim.LEFT] = mappingTransform.Find("LeftBarrel");
            barrelReferenceArray[(int)Aim.LEFT_UP] = mappingTransform.Find("LeftUpBarrel");
            barrelReferenceArray[(int)Aim.LEFT_DOWN] = mappingTransform.Find("LeftDownBarrel");
            barrelReferenceArray[(int)Aim.UP] = mappingTransform.Find("UpBarrel");
            barrelReferenceArray[(int)Aim.DOWN] = mappingTransform.Find("DownBarrel");
        }

        void Update()
        {
            var barrelTransform = GetBarrelTransform();
            mBarrelTransform.position = barrelTransform.position;
            mBarrelTransform.rotation = barrelTransform.rotation;
        }

        public Transform GetBarrelTransform()
        {
            return mBarrelReferences[(int)mPlayerState.mWeaponId][(int)mPlayerState.mAimingDir];
        }
    }
}
