using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int PlayerNum;

    private float BaseMovement = 20f;
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
        float left_vertical = Input.GetAxis(InputMap[LeftVerticalId]);
        if (IsGrounded && left_vertical > Mathf.Epsilon)
        {
            GetComponent<Rigidbody2D>().AddForce(JumpForce);
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
