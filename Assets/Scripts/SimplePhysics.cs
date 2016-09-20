using UnityEngine;
using System.Collections;

namespace Filibusters
{
    public class SimplePhysics : MonoBehaviour
    {
        // Audio
        [SerializeField]
        private AudioClip mJumpSound;
        private AudioSource mAudioSource;

        // Physics
        [SerializeField]
        private float mAerialSpeed = 0.05f;
        [SerializeField]
        private float mGravity = -0.5f;
        [SerializeField]
        private float mJumpVel = 12f;
        [SerializeField]
        private float mMaxSpeed = 4f;
        [SerializeField]
        private float mSkin = 0.005f;

        private MotionState mMotionState;
        private bool mGrounded
        {
            get { return mMotionState.mGrounded; }
            set { mMotionState.mGrounded = value; }
        }
        private bool mFacingRight
        {
            get { return mMotionState.mFacingRight; }
            set { mMotionState.mFacingRight = value; }
        }
        private float mVelX
        {
            get { return mMotionState.mVelX; }
            set { mMotionState.mVelX = value; }
        }
        private float mVelY
        {
            get { return mMotionState.mVelY; }
            set { mMotionState.mVelY = value; }
        }
        private float mVelXMult
        {
            get { return mMotionState.mVelXMult; }
            set { mMotionState.mVelXMult = value; }
        }

        private bool mPressedDown = false;
        private float mPrevY = 0f;

        private Vector2 mSize = Vector2.zero;
        private Vector2 mOffset = Vector2.zero;
        public LayerMask mColLayersX;
        public LayerMask mColLayersY;

        void Awake()
        {
            mMotionState = GetComponent<MotionState>();

            if (!(mAudioSource = GetComponent<AudioSource>()))
            {
                Debug.LogWarning("Add an AudioSource component!");
            }

            Vector3 scale = transform.localScale;
            BoxCollider2D bCol;
            if (bCol = GetComponent<BoxCollider2D>())
            {
                mSize = new Vector2(bCol.size.x * scale.x, bCol.size.y * scale.y);
                mOffset = new Vector2(bCol.offset.x * scale.x, bCol.offset.y * scale.y);
            }
            else
            {
                Debug.LogWarning("Add a BoxCollider2D component!");
            }
        }

        void Update()
        {
            // Keep track of the previous position to account for two-way platforms
            mPrevY = transform.position.y + mOffset.y - mSize.y / 2f;

            float xInput = Input.GetAxis("Horizontal");
            // Note: Pressing down results in positive values
            // so I flip the input
            float yInput = Input.GetAxis("Vertical");
            mPressedDown = Mathf.Sign(yInput) == -1f;
            print(mPressedDown);

            if (mGrounded)
            {
                if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Space))
                {
                    mVelY = mJumpVel;
                    mAudioSource.clip = mJumpSound;
                    mAudioSource.Play();
                }
                else
                {
                    mVelX = xInput * mMaxSpeed;
                    // This prevents the y velocity from growing arbitrarily large
                    // while the player is grounded
                    mVelY = mGravity;
                }
            }
            // Allow aerial acceleration
            else
            {
                mVelX = UseAccel(xInput * mAerialSpeed, mVelX, mMaxSpeed);
            }

            Flip(xInput);
            mVelXMult = Mathf.Abs(xInput);

            float deltaX = mVelX * Time.deltaTime;
            float deltaY = mVelY * Time.deltaTime;

            float dirX = Mathf.Sign(deltaX);
            float dirY = Mathf.Sign(deltaY);
            bool facingRight = dirX == 1f;

            deltaX = GetXChange(deltaX, dirX);
            deltaY = GetYChange(deltaY, dirY, facingRight);

            transform.Translate(deltaX, deltaY, 0f);
            mVelY += mGravity;
        }

        // TODO: Bug where character accelerates much more quickly in x dir
        private float UseAccel(float accel, float curSpeed, float maxSpeed)
        {
            float newSpeed = accel + curSpeed;
            return (newSpeed < maxSpeed) ? newSpeed : maxSpeed;
        }

        private void Flip(float dir)
        {
            // Facing left
            if (dir < -Mathf.Epsilon)
            {
                mFacingRight = false;
            }
            // Facing right
            else
            {
                mFacingRight = true;
            }
        }

        private float GetXChange(float delta, float dir)
        {
            for (int height = 0; height < 3; height++)
            {
                if (RaycastX(ref delta, dir, height))
                {
                    break;
                }
            }
            return delta;
        }

        private float GetYChange(float delta, float dir, bool facingRight)
        {
            // Cast rays downward right to left
            if (facingRight)
            {
                for (int width = 2; width > -1; width--)
                {
                    if (RaycastY(ref delta, dir, width))
                    {
                        break;
                    }
                }
            }
            // Cast rays downward left to right
            else
            {
                for (int width = 0; width < 3; width++)
                {
                    if (RaycastY(ref delta, dir, width))
                    {
                        break;
                    }
                }
            }
            return delta;
        }

        private bool RaycastX(ref float delta, float dir, int height)
        {
            float x = transform.position.x + mOffset.x + mSize.x / 2f * dir;
            float y = (transform.position.y + mOffset.y - mSize.y / 2f) + mSize.y / 2f * height;

            Ray2D ray = new Ray2D(new Vector2(x, y), new Vector2(dir, 0));
            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(delta), mColLayersX))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red);

                float distance = Vector2.Distance(ray.origin, hit.point);
                if (distance > mSkin)
                {
                    delta = distance * dir + mSkin;
                }
                else
                {
                    delta = 0;
                }
                return true;
            }
            return false;
        }

        private bool RaycastY(ref float delta, float dir, int width)
        {
            mGrounded = false;

            float x = (transform.position.x + mOffset.x - mSize.x / 2f) + mSize.x / 2f * width;
            float y = transform.position.y + mOffset.y + mSize.y / 2f * dir;

            Ray2D ray = new Ray2D(new Vector2(x, y), new Vector2(0, dir));
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(delta), mColLayersY))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red);

                // Ignore two-way platforms the player was below
                // and fall through these platforms when the player presses down
                // TODO: Make sure the player is above the TOP of the two-way platform's box collider
                GameObject other = hit.transform.gameObject;
                if (other.tag == "TwoWay" && (other.transform.position.y >= mPrevY || mPressedDown))
                {
                    return true;
                }

                float distance = Vector2.Distance(ray.origin, hit.point);
                if (distance > mSkin)
                {
                    delta = distance * dir + mSkin;
                }
                else
                {
                    delta = 0;
                }
                mGrounded = true;
                return true;
            }
            return false;
        }
    }
}
