using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
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

    bool facingRight = true;


    Animator animator;

    GameObject me = null;
    GameObject enemy = null;

    // Use this for initialization
    void Start ()
    {
        InputMap = new string[NumInputs];
        for (int i = 0; i < NumInputs; ++i)
        {
            InputMap[i] = "P" + PlayerNum + InputPostFixes[i];
        }

        animator = GetComponent<Animator>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        

        if (players[0].GetComponent<PlayerController>().PlayerNum == PlayerNum)
        {
            me = players[0];
            enemy = players[1];
        }
        else
        {
            me = players[1];
            enemy = players[0];
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
        float left_vertical = Input.GetAxis(InputMap[LeftVerticalId]);
        bool downPressed = left_vertical < -Mathf.Epsilon;
        bool upPressed = left_vertical > Mathf.Epsilon;
		if (IsGrounded && left_vertical > Mathf.Epsilon && jumpTimer > jumpCoolDown)
        {
            SoundManager.instance.Play("jump");
			jumpTimer = 0f;
			GetComponent<Rigidbody2D>().AddForce(JumpForce);
        }

		var rb = GetComponent<Rigidbody2D>();
		animator.SetFloat("vSpeed", rb.velocity.y);
		animator.SetBool("Ground", IsGrounded || rb.velocity.y == 0f);

		animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
		if (rb.velocity.x < -Mathf.Epsilon && facingRight)
        {
			// flip
			GetComponent<SpriteRenderer>().flipX = true;
			facingRight = false;
        }
        if (rb.velocity.x > Mathf.Epsilon && !facingRight)
        {
        	// flip
			GetComponent<SpriteRenderer>().flipX = false;
			facingRight = true;
        }

        bool inRange = Vector2.Distance(me.transform.position, enemy.transform.position) < 5f;
        print(inRange);
        if (((PlayerNum == 1 && Input.GetKeyDown(KeyCode.LeftShift)) || (PlayerNum == 2 && Input.GetKeyDown(KeyCode.RightShift))))
        {
            // ADD HIT SOUND HERE
            animator.Play("PlayerPunch");
            if (inRange)
            {
                enemy.GetComponent<LifeManager>().Die();
            }
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
