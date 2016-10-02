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
        // TODO: Remove this
        private float mVelXMult
        {
            get { return mMotionState.mVelXMult; }
            set { mMotionState.mVelXMult = value; }
        }

        private bool mPressedDown = false;
        private float mPrevY = 0f;

        // Prevent a player from holding jump and
        // bouncing around out of control
        private readonly float mHoldJumpCooldown = 0.15f;
        private bool mPrevWasGrounded = true;
        private bool mJumpable = true;
        private bool mJumpButtonHeld = false;

        private static readonly float FullXInputThreshold = Mathf.Sqrt(2) / 2f;

        private Vector2 mSize = Vector2.zero;
        private Vector2 mOffset = Vector2.zero;
        public LayerMask mColLayersX;
        public LayerMask mColLayersY;
        private int mTwoWay;

        void Awake()
        {
            mTwoWay = LayerMask.NameToLayer("TwoWayPlatform");

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
            mPrevWasGrounded = mGrounded;

            float xInput = 0f;
            float yInput = 0f;
            bool jumpPressed = false;
            HandleInput(ref xInput, ref yInput, ref jumpPressed);
            
            if (mGrounded)
            {
                if (jumpPressed)
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
                    mVelY = Mathf.Max(mGravity, mVelY);
                }
            }
            // Allow aerial acceleration
            else
            {
                mVelX = UseAccel(xInput * mAerialSpeed, mVelX, xInput * mMaxSpeed);
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

            // if we have stopped moving in the Y dir,
            // update our y speed to zero
            if (deltaY == 0f)
            {
                mVelY = 0;
            }

            transform.Translate(deltaX, deltaY, 0f);
            if (mPressedDown)
            {
                mVelY += mGravity * .5f;
            }
            mVelY += mGravity;
        }

        private void HandleInput(ref float xInput, ref float yInput, ref bool jumpPressed)
        {
            // left/right input
            if (Mathf.Abs(xInput = Input.GetAxis("LeftStickXAxis")) <= Mathf.Epsilon)
            {
                xInput = Input.GetAxis("LeftRightKeyboard");
            }
            if (xInput > FullXInputThreshold)
            {
                xInput = 1f;
            }
            else if (xInput < -FullXInputThreshold)
            {
                xInput = -1f;
            }

            // down input
            // Note: Pressing down results in positive values so I flip the input
            if (Mathf.Abs(yInput = -Input.GetAxis("LeftStickYAxis")) <= Mathf.Epsilon)
            {
                yInput = Input.GetAxis("DownUpKeyboard");
            }
            mPressedDown = Mathf.Sign(yInput) == -1f;

            // jump if the Y axis is pushed up and our jump is enabled
            mJumpButtonHeld = ((Mathf.Sign(yInput) == 1f && yInput > Mathf.Epsilon) ||
                Input.GetButton("A-Jump") || Input.GetAxis("LT-Jump") > Mathf.Epsilon);
            jumpPressed = mJumpButtonHeld && mJumpable;
            // disables jump after pressing it
            mJumpable = (jumpPressed ^ mJumpable) && mJumpable;
        }

        private float UseAccel(float accel, float curSpeed, float maxSpeed)
        {
            float newSpeed = accel + curSpeed;
            return (Mathf.Abs(newSpeed) < Mathf.Abs(maxSpeed)) ? newSpeed : maxSpeed;
        }

        private void Flip(float dir)
        {
            // Facing left
            if (dir < 0)
            {
                mFacingRight = false;
            }
            // Facing right
            else if (dir > 0)
            {
                mFacingRight = true;
            }
        }

        private float GetXChange(float delta, float dir)
        {
            for (int i = 0; i < 5; i++)
            {
                float height = 0.2f + i * 0.4f;
                if (RaycastX(ref delta, dir, height))
                {
                    break;
                }
            }
            return delta;
        }

        private float GetYChange(float delta, float dir, bool facingRight)
        {
            mGrounded = false;
            // TODO: Set a begin end and increment to avoid having identical for loops
            for (int i = 0; i < 5; i++)
            {
                float width = 0.2f + i * 0.4f;
                // raycast in our moving direction to update our y delta
                RaycastY(ref delta, dir, width);
                // raycast to the floor always to check if we are grounded
                float tmpDel = 0.01f;
                mGrounded = RaycastY(ref tmpDel, -1, width) || mGrounded;
            }

            // if we have just become grounded then start our jump
            // cooldown and allow us to jump when its over
            if (mGrounded && !mPrevWasGrounded)
            {
                if (mJumpButtonHeld)
                {
                    StartCoroutine(JumpCooldown());
                }
                else
                {
                    mJumpable = true;
                }
            }
            return delta;
        }

        // reenable jump after cooldown time elapses
        private IEnumerator JumpCooldown()
        {
            yield return new WaitForSeconds(mHoldJumpCooldown);
            mJumpable = true;
        }

        private bool RaycastX(ref float delta, float dir, float height)
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

        private bool RaycastY(ref float delta, float dir, float width)
        {
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
                if (ShouldPassThroughPlatform(hit.transform.gameObject))
                {
                    return false;
                }

                // OTHERWISE Ground the player
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

        private bool ShouldPassThroughPlatform(GameObject other)
        {
            if (other.layer == mTwoWay)
            {
                BoxCollider2D bCol = other.GetComponent<BoxCollider2D>();
                float size = bCol.size.y / 2f;
                float offset = bCol.offset.y;

                // Get the top position of the box collider
                Vector3 localPos = other.transform.localPosition;
                float topOfsInWorldCoords = other.transform.TransformVector(new Vector3(localPos.x, size + offset)).y;
                float colTop = other.transform.position.y + topOfsInWorldCoords;
                // If the collider is above the player or the down key was pressed
                // pass through the platform
                return colTop >= mPrevY || mPressedDown;
            }
            return false;
        }

        public void ResetPhysicsState(Vector3 pos)
        {
            transform.position = pos;
            mMotionState.ResetPosition();
            mGrounded = false;
        	mFacingRight = true;
	        mVelX = 0f;
	        mVelY = 0f;
        	mPressedDown = false;
        	mPrevY = 0f;
        }
    }
}
