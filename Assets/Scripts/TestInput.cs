using UnityEngine;
using System.Collections;

public class TestInput : MonoBehaviour
{
    [SerializeField]
    private float mMaxSpeed = 150f;
    [SerializeField]
    private float mCurSpeed = 0f;
    [SerializeField]
    private float mAcceleration = 4f;

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
	
	void Update()
    {
        float xTargetSpeed = 0f;
        float xInput = Input.GetAxis("LeftStickXAxis");

        // If the player changes direction, they should not slide
        // AKA the switch should be immediate
        bool changedDirection = Mathf.Sign(mDirection) != Mathf.Sign(xInput);
        if (changedDirection)
        {
            mCurSpeed = 0f;
        }

        if (xInput != 0)
        {
            xTargetSpeed = xInput * mMaxSpeed;
            mCurSpeed = IntegrateAccel(xTargetSpeed);
        }
        // Currently the player is immediately stopped on no input
        // TODO: is this necessary?
        else
        {
            mCurSpeed = 0f;
        }
        mCurSpeed = xAxisCollisions(mCurSpeed * Time.deltaTime);
        transform.Translate(mCurSpeed, 0f, 0f);
        mDirection = Mathf.Sign(mCurSpeed);

    }

    void LateUpdate()
    {

    }

    private float IntegrateAccel(float targetSpeed)
    {
        if (mCurSpeed == targetSpeed)
        {
            return mCurSpeed;
        }
        float direction = Mathf.Sign(targetSpeed - mCurSpeed);
        mCurSpeed += mAcceleration * direction * Time.deltaTime;
        // Check and rectify if targetSpeed was overshot
        return direction == Mathf.Sign(targetSpeed - mCurSpeed) ? mCurSpeed : targetSpeed;
    }

    private float xAxisCollisions(float deltaX)
    {
		float direction = Mathf.Sign(deltaX);
        for (int level = 0; level < 3; level++)
        {
            float x = transform.position.x + mBoxOffset.x + mBoxSize.x / 2f * direction;
            float y = (transform.position.y + mBoxOffset.y - mBoxSize.y / 2f) + mBoxSize.y / 2f * level;

            RaycastHit2D hit;
            Ray2D ray = new Ray2D(new Vector2(x, y), new Vector2(direction, 0));
            Debug.DrawRay(ray.origin, ray.direction);
            if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(deltaX), mCollisionLayers))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
                deltaX = 0f;
                break;
            }
        }
        return deltaX;
    }

    private float yAxisCollisions(float deltaY)
    {
    	return -1f;
    }
}
