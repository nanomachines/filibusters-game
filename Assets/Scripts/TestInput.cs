using UnityEngine;
using System.Collections;

public class TestInput : MonoBehaviour
{
    [SerializeField]
    private float mJumpHeight = 0f;
    private float mJumpTarget = 0f;
    [SerializeField]
    private float mJumpForce = 100f;
    [SerializeField]
    private float mMaxSpeed = 300f;
    [SerializeField]
    private float mVelocityX = 0f;
    [SerializeField]
    private float mVelocityY = 0f;
    [SerializeField]
    private float mAcceleration = 300f;
    [SerializeField]
    private float mGravity = 0f;

    [SerializeField]
    private bool mGrounded = false;
	[SerializeField]
	private bool mFalling = false;
    [SerializeField]
    private bool mJumping = false;
    

    private Vector2 mBoxSize = Vector2.zero;
    private Vector2 mBoxOffset = Vector2.zero;

    private float mDirection = 1f;

    public LayerMask mCollisionLayers;

	void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        mBoxSize = boxCollider.size;
        mBoxOffset = boxCollider.offset;
	}
	
    // TODO: Introduce notion of move amount
	void Update()
    {
        float xTargetVelocity = 0f;
        float xInput = Input.GetAxis("LeftStickXAxis");

        if (mGrounded && Input.GetButtonDown("A"))
        {
            mJumping = true;
            mJumpTarget = transform.position.y + mJumpHeight;
            mAcceleration = 10f;
        }
        else
        {
            mVelocityY = GetDeltaY(mVelocityY * Time.deltaTime + mGravity * Time.deltaTime, mVelocityX);
        }

        if (mJumping && transform.position.y < mJumpTarget)
        {
            mVelocityY = mJumpForce * Time.deltaTime * 0.5f;
        }
        else
        {
            mAcceleration = 300f;
            mJumping = false;
        }

        // If the player changes direction, they should not slide
        // AKA the switch should be immediate
        bool changedDirection = Mathf.Sign(mDirection) != Mathf.Sign(xInput);
        if (mGrounded && changedDirection)
        {
            mVelocityX = 0f;
        }

        if (mGrounded && xInput != 0)
        {
            xTargetVelocity = xInput * mMaxSpeed;
            mVelocityX = IntegrateAccel(xTargetVelocity);
        }
        // Currently the player is immediately stopped on no input
        // TODO: is this necessary? I think it is if accel != maxSpeed
        else if (mGrounded)
        {
            mVelocityX = 0f;
        }

        

        print(mVelocityX);
        mVelocityX = GetDeltaX(mVelocityX * Time.deltaTime);
        transform.Translate(mVelocityX, mVelocityY, 0f);
        mDirection = Mathf.Sign(mVelocityX);
    }

    private float IntegrateAccel(float targetSpeed)
    {
        if (mVelocityX == targetSpeed)
        {
            return mVelocityX;
        }
        float direction = Mathf.Sign(targetSpeed - mVelocityX);
        mVelocityX += mAcceleration * direction * Time.deltaTime;
        // Check and rectify if targetSpeed was overshot
        return direction == Mathf.Sign(targetSpeed - mVelocityX) ? mVelocityX : targetSpeed;
    }

    private float GetDeltaX(float deltaX)
    {
		float direction = Mathf.Sign(deltaX);
        for (int level = 0; level < 3; level++)
        {
            if (XRayHit(direction, ref deltaX, level))
            {
                deltaX = 0f;
                break;
            }
        }
        return deltaX;
    }

    private bool XRayHit(float direction, ref float deltaX, int level)
    {
        float x = transform.position.x + mBoxOffset.x + mBoxSize.x / 2f * direction;
        float y = (transform.position.y + mBoxOffset.y - mBoxSize.y / 2f) + mBoxSize.y / 2f * level;

        RaycastHit2D hit;
        Ray2D ray = new Ray2D(new Vector2(x, y), new Vector2(direction, 0));
        Debug.DrawRay(ray.origin, new Vector2(deltaX, 0), Color.green);
        if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(deltaX), mCollisionLayers))
        {
            Debug.DrawRay(ray.origin, new Vector2(deltaX, 0), Color.red);
            float distance = Vector2.Distance(ray.origin, hit.point);
            if (distance > 0.005f)
            {
                deltaX = distance * direction + 0.005f;
            }
            else
            {
                deltaX = 0;
            }
        }
        return false;
    }

    private float GetDeltaY(float deltaY, float deltaX)
    {
        float directionX = Mathf.Sign(deltaX);
        float direction = Mathf.Sign(deltaY);
        // Facing right; cast rays down right to left
        if (directionX == 1f)
        {
            for (int level = 2; level > -1; level--)
            {
                if (YRayHit(direction, ref deltaY, level))
                {
                    break;
                }
            }
        }
        // Facing left; cast rays down left to right
        else
        {
            for (int level = 0; level < 3; level++)
            {
                if (YRayHit(direction, ref deltaY, level))
                {
                    break;
                }
            }
        }
        
        return deltaY;
    }

    private bool YRayHit(float direction, ref float deltaY, int level)
    {
        mGrounded = false;
        float x = (transform.position.x + mBoxOffset.x - mBoxSize.x / 2f) + mBoxSize.x / 2f * level;
        float y = transform.position.y + mBoxOffset.y + mBoxSize.y / 2f * direction;

        // TODO: Make this a function that x and y can share
        RaycastHit2D hit;
        Ray2D ray = new Ray2D(new Vector2(x, y), new Vector2(0, direction));
        Debug.DrawRay(ray.origin, new Vector2(0f, deltaY), Color.yellow);
        if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(deltaY), mCollisionLayers))
        {
            Debug.DrawRay(ray.origin, new Vector2(0f, deltaY), Color.red);
            float distance = Vector2.Distance(ray.origin, hit.point);
            if (distance > 0.005f)
            {
                deltaY = distance * direction + 0.005f;
            }
            else
            {
                deltaY = 0;
            }
            mGrounded = true;
            return true;
        }
        return false;
    }
}
