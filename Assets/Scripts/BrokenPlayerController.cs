using UnityEngine;
using System.Collections;

public class BrokenPlayerController : MonoBehaviour
{
    public int PlayerNum;

    private float BaseMovement = 20f;
    [SerializeField]
    private Vector2 JumpForce = new Vector2(0, 300);

    private const int NumInputs = 5;
    private string[] InputMap;
    private static string[] InputPostFixes = {
        "_Left_Horizontal",
        "_Right_Horizontal",
        "_Left_Vertical",
        "_Right_Vertical",
        "Jump"
    };

	[SerializeField]
    private float fallCoolDown = 0.1f;
    private float fallTimer = 0f;
    [SerializeField]
    private float jumpCoolDown = 0.5f;
    private float jumpTimer = 0f;

    const int LeftHorizontalId = 0;
    const int RightHorizontalId = 1;
    const int LeftVerticalId = 2;
    const int RightVerticalId = 3;
    const int JumpId = 4;

    public Transform GroundCheck;
    public LayerMask WhatIsGround;
    [SerializeField]
    private float GroundCheckRadius = 0.1f;
    bool IsGrounded = false;
    
	// Use this for initialization
	void Start ()
    {
        InputMap = new string[NumInputs];
        for (int i = 0; i < NumInputs; ++i)
        {
            InputMap[i] = "P" + PlayerNum + InputPostFixes[i];
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
        float LeftHorizontal = Input.GetAxis(InputMap[LeftHorizontalId]);
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(BaseMovement * LeftHorizontal, rb.velocity.y);
	}

	void Update ()
    {
		jumpTimer += Time.deltaTime;
		fallTimer += Time.deltaTime;
        float left_vertical = Input.GetAxis(InputMap[LeftVerticalId]);

		bool down = (IsGrounded && left_vertical < -Mathf.Epsilon);
		if (!IsGrounded || down)
    	{
    		// y is not frozen
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    	}
		if (IsGrounded && left_vertical > Mathf.Epsilon && jumpTimer > jumpCoolDown)
        {
			jumpTimer = 0f;
			fallTimer = 0f;
            GetComponent<Rigidbody2D>().AddForce(JumpForce);
            // Y is not frozen
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
		if (IsGrounded && fallTimer > fallCoolDown)
    	{
    		fallTimer = 0f;
    		// re-freeze y
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    	}
	}

    private float AnalogToDigital(float v)
    {
        if (v > Mathf.Epsilon)
        {
            return 1f;
        }
        else if (v < -Mathf.Epsilon)
        {
            return -1f;
        }
        else
        {
            return 0;
        }
    }
}
