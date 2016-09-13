using UnityEngine;
using System.Collections;

public class TestInput : MonoBehaviour
{
    [SerializeField]
    private float mMaxSpeed = 300f;
    [SerializeField]
    private float mCurXSpeed = 0f;
    [SerializeField]
    private float mCurYSpeed = 0f;
    [SerializeField]
    private float mAcceleration = 300f;
    [SerializeField]
    private float mGravity = 0f;

    [SerializeField]
    private bool mGrounded = false;
	[SerializeField]
	private bool mFalling = false;
    

    private Vector2 mBoxSize = Vector2.zero;
    private Vector2 mBoxOffset = Vector2.zero;

    private float mDirection = 1f;

    public LayerMask mCollisionLayers;

	void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        mBoxSize = boxCollider.size;
        mBoxOffset = boxCollider.offset;
        print(mBoxOffset);
	}
	
    // TODO: Introduce notion of move amount
	void Update()
    {
        float xTargetSpeed = 0f;
        float xInput = Input.GetAxis("LeftStickXAxis");

        // If the player changes direction, they should not slide
        // AKA the switch should be immediate
        bool changedDirection = Mathf.Sign(mDirection) != Mathf.Sign(xInput);
        if (changedDirection)
        {
            mCurXSpeed = 0f;
        }

        if (xInput != 0)
        {
            xTargetSpeed = xInput * mMaxSpeed;
            mCurXSpeed = IntegrateAccel(xTargetSpeed);
        }
        // Currently the player is immediately stopped on no input
        // TODO: is this necessary? I think it is if accel != maxSpeed
        else
        {
            mCurXSpeed = 0f;
        }

        mCurXSpeed = GetDeltaX(mCurXSpeed * Time.deltaTime);
        mCurYSpeed = GetDeltaY(mCurYSpeed * Time.deltaTime - mGravity * Time.deltaTime, mCurXSpeed);
        transform.Translate(mCurXSpeed, mCurYSpeed, 0f);
        mDirection = Mathf.Sign(mCurXSpeed);
    }

    void LateUpdate()
    {

    }

    private float IntegrateAccel(float targetSpeed)
    {
        if (mCurXSpeed == targetSpeed)
        {
            return mCurXSpeed;
        }
        float direction = Mathf.Sign(targetSpeed - mCurXSpeed);
        mCurXSpeed += mAcceleration * direction * Time.deltaTime;
        // Check and rectify if targetSpeed was overshot
        return direction == Mathf.Sign(targetSpeed - mCurXSpeed) ? mCurXSpeed : targetSpeed;
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
            return true;
        }
        return false;
    }
}
