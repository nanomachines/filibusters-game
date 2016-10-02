using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace Filibusters
{
    public class SimplePhysics : MonoBehaviour
    {
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

        private PlayerState mPlayerState;
        private bool mGrounded
        {
            get { return mPlayerState.mGrounded; }
            set { mPlayerState.mGrounded = value; }
        }
        private bool mFacingRight
        {
            get { return mPlayerState.mFacingRight; }
            set { mPlayerState.mFacingRight = value; }
        }
        private float mVelX
        {
            get { return mPlayerState.mVelX; }
            set { mPlayerState.mVelX = value; }
        }
        private float mVelY
        {
            get { return mPlayerState.mVelY; }
            set { mPlayerState.mVelY = value; }
        }
        // TODO: Remove this
        private float mVelXMult
        {
            get { return mPlayerState.mVelXMult; }
            set { mPlayerState.mVelXMult = value; }
        }

        private bool mPressedDown = false;
        private float mPrevY = 0f;

        private Vector2 mSize = Vector2.zero;
        private Vector2 mOffset = Vector2.zero;
        public LayerMask mColLayersX;
        public LayerMask mColLayersY;
        private int mTwoWay;

        void Awake()
        {
            mTwoWay = LayerMask.NameToLayer("TwoWayPlatform");

            mPlayerState = GetComponent<PlayerState>();

            Vector3 scale = transform.localScale;
            BoxCollider2D bCol;
            bCol = GetComponent<BoxCollider2D>();
            Assert.IsNotNull(bCol, "BoxCollider2D component missing");
            mSize = new Vector2(bCol.size.x * scale.x, bCol.size.y * scale.y);
            mOffset = new Vector2(bCol.offset.x * scale.x, bCol.offset.y * scale.y);
        }

        void Update()
        {
            // Keep track of the previous position to account for two-way platforms
            mPrevY = transform.position.y + mOffset.y - mSize.y / 2f;

            float xInput = 0f;
            float yInput = 0f;
            bool jumpPressed = false;
            HandleInput(ref xInput, ref yInput, ref jumpPressed);
            
            if (mGrounded)
            {
                if (jumpPressed)
                {
                    mVelY = mJumpVel;
                    EventSystem.OnJump();
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

            transform.Translate(deltaX, deltaY, 0f);
            mVelY += mGravity;
        }

        private void HandleInput(ref float xInput, ref float yInput, ref bool jumpPressed)
        {
            // left/right input
            if (Mathf.Abs(xInput = Input.GetAxis("LeftStickXAxis")) <= Mathf.Epsilon)
            {
                xInput = Input.GetAxis("LeftRightKeyboard");
            }

            // down input
            // Note: Pressing down results in positive values so I flip the input
            if (Mathf.Abs(yInput = -Input.GetAxis("LeftStickYAxis")) <= Mathf.Epsilon)
            {
                yInput = Input.GetAxis("DownUpKeyboard");
            }
            mPressedDown = Mathf.Sign(yInput) == -1f;

            // jump input
            jumpPressed = Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Space);
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
                if (RaycastY(ref delta, dir, width))
                {
                    mGrounded = true;
                }
            }
            return delta;
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
            mPlayerState.ResetPosition();
            mGrounded = false;
        	mFacingRight = true;
	        mVelX = 0f;
	        mVelY = 0f;
        	mPressedDown = false;
        	mPrevY = 0f;
        }
    }
}
