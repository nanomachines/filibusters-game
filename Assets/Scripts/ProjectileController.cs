using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class ProjectileController : MonoBehaviour 
    {
        private float mVelX;
        public float VelX
        {
            get { return mVelX; }
            set { mVelX = value; }
        }

        private float mVelY;
        public float VelY
        {
            get { return mVelY; }
            set { mVelY = value; }
        }

        void Start()
        {
            
        }
        
        void Update() 
        {
            float deltaX = mVelX * Time.deltaTime;
            float deltaY = mVelY * Time.deltaTime;
            transform.Translate(deltaX, deltaY, 0f);
        }
    }
}
