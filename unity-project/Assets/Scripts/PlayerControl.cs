using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	// Movement parameters:
	public float moveForceGround = 80f;			// Amount of force added to move the player left and right.
	public float moveForceAir = 50f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 18f;				// The fastest the player can travel in the x axis.
	public float maxFallSpeed = 50;
	public float jumpForce = 1540f;			// Amount of force added when the player jumps.
	public float autoBreak = 8.0f;		// How fast the character stops moving if no direction is pressed
	public float autoBreakAir = 2.0f;
	public float jumpBreak = 25;		//How quickly you stop rising after the jump button is released
	public float maxAirAccel = 14f;		//The maximum speed the character can reach by accelerating in the air
	public Vector2 backflipForce = new Vector2(-300, 0);	//Force that gets added to the jump when it is a backflip

	//reference shorthand
	private OverlapCheck groundCheck;
	private Animator anim;
	private Rigidbody2D rigid;
	private FlipSprite flipper;

	//Animation State Hashes:
	int idleState;
	int runState;
	int skidState;
	int jumpState;
	int fallState;
	int backflipState;

	//internal state
	bool jumping = false;
	float currentInput;		//current horizontal input
	AnimatorStateInfo currentState;

	// Use this for initialization
	void Awake () {
		// Setting up references.
		groundCheck = transform.FindChild("GroundCheck").GetComponent<OverlapCheck>();
		anim = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		flipper = GetComponent<FlipSprite>();

		//Animation state setup
		idleState = Animator.StringToHash("Base Layer.Idle");
		runState = Animator.StringToHash("Base Layer.Run");
		skidState = Animator.StringToHash("Base Layer.Skid");
		jumpState = Animator.StringToHash("Base Layer.Jump");
		fallState = Animator.StringToHash("Base Layer.Fall");
		backflipState = Animator.StringToHash("Base Layer.Backflip");
	}
	
	// Update is called once per frame
	void Update () {
		currentInput = Input.GetAxisRaw("Horizontal");
		currentState = anim.GetCurrentAnimatorStateInfo(0);

		//Parameter update
		anim.SetBool("Grounded", groundCheck.overlaps);
		anim.SetFloat("SpeedX", Mathf.Abs(rigid.velocity.x));
		anim.SetFloat("SpeedY", rigid.velocity.y);
		anim.SetInteger("Input", (int) currentInput);
		anim.SetBool("DirAlign", currentInput * rigid.velocity.x >= 0);

		//Jumping:
		if(currentState.fullPathHash == idleState || currentState.fullPathHash == runState || currentState.fullPathHash == skidState)
		{
			if(Input.GetButtonDown("Jump"))
			{
				jumping = true;
				anim.SetTrigger("Jump");
			}
		}

		//Turnaround
		if(currentState.fullPathHash == runState)
		{
			flipper.FaceDir(rigid.velocity.x > 0);
		}


	}

	void FixedUpdate ()
	{
		//Ground Movement
		if(currentState.fullPathHash == idleState || currentState.fullPathHash == runState || currentState.fullPathHash == skidState)
		{
			rigid.AddForce(Vector2.right * currentInput * moveForceGround);
			if(currentInput == 0)	//Slow down when no direction is pressed
			{
				rigid.AddForce(Vector2.left * rigid.velocity.x * autoBreak);
			}
			if(Mathf.Abs(rigid.velocity.x) < 1)		//looks more natural
			{
				rigid.velocity = new Vector2(0, rigid.velocity.y);
			}
			if(Mathf.Abs(rigid.velocity.x) > maxSpeed)	//cap max speed
			{
				rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * maxSpeed, rigid.velocity.y);
			}

			if(jumping)
			{
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				jumping = false;

				if(currentState.fullPathHash == skidState)
				{	
					//Doing a backflip might do something special
					rigid.AddForce(Vector2.Scale(backflipForce, (Vector2.right * Mathf.Sign(transform.localScale.x))));
					// ^ adding an additional backwards force on a backflip feels good
				}
			}
		}

		//Air Movement
		if(currentState.fullPathHash == jumpState || currentState.fullPathHash == fallState || currentState.fullPathHash == backflipState)
		{
			if(Mathf.Abs(rigid.velocity.x) < maxAirAccel || currentInput * rigid.velocity.x <= 0)
			{
				rigid.AddForce(Vector2.right * currentInput * moveForceAir);
			}
			if(Input.GetButtonUp("Jump") && rigid.velocity.y > 0)	//stop rising when the jump button is released
			{
				rigid.AddForce(Vector2.down * rigid.velocity.y * jumpBreak);
			}
			if(currentInput == 0)	//Slow down when no direction is pressed
			{
				rigid.AddForce(Vector2.left * rigid.velocity.x * autoBreakAir);
			}
			if(rigid.velocity.y < -maxFallSpeed)		//cap fall speed
			{
				rigid.velocity = Vector2.down * maxFallSpeed;
			}

		}
	}

}
