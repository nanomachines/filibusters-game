using UnityEngine;
using System.Collections;

public class SimplePhysics : MonoBehaviour
{

    [SerializeField]
    private float mGravity = -0.5f;
    [SerializeField]
    private float mJumpVel = 12f;
    [SerializeField]
    private float mMaxSpeed = 4f;
    [SerializeField]
    private float mSkin = 0.005f;

    private bool mGrounded = false;

    private float mVelX = 0f;
    private float mVelY = 0f;

    private Vector2 mSize = Vector2.zero;
    private Vector2 mOffset = Vector2.zero;
    public LayerMask mColLayers;

    void Awake()
    {
        BoxCollider2D bCol;
        if (bCol = GetComponent<BoxCollider2D>())
        {
            mSize = bCol.size;
            mOffset = bCol.offset;
        }
        else
        {
            Debug.LogWarning("Add a BoxCollider2D component!");
        }
    }

    void Update()
    {
        if (mGrounded)
        {
            if (Input.GetButtonDown("A"))
            {
                mVelY = mJumpVel;
            }
            else
            {
                float input = Input.GetAxis("LeftStickXAxis");
                mVelX = input * mMaxSpeed;
                // This prevents the y velocity from growing arbitrarily large
                // while the player is grounded
                mVelY = mGravity;
            }
        }

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
        Debug.DrawRay(ray.origin, new Vector2(delta, 0), Color.green);

        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(delta), mColLayers))
        {
            Debug.DrawRay(ray.origin, new Vector2(delta, 0), Color.red);

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
        Debug.DrawRay(ray.origin, new Vector2(0f, delta), Color.yellow);

        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Abs(delta), mColLayers))
        {
            Debug.DrawRay(ray.origin, new Vector2(0f, delta), Color.red);

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
