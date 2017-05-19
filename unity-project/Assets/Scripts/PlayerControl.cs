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

	//Walljump parameters
	public float wjAnglePenalty	= 0.6f;	//0: Walljump forcce does not depend on incoming angle, 1: no force when coming in at 90deg
	public float jumpWindow = 0.3f; //Time the player has for doing a walljump after collision
	public float velocityThresh = 6.0f; //minimum velocity required for a walljump. LESS THAN 1 BREAKS THE WALLJUMP

	//Slide parameters:
	public float slideThresh = 14.0f;	//How fast you need to go to slide
	public float slideBoost	= 800;		//Speedboost for slide start
	public float slideBreak = 2;		//How fast you lose speed while sliding

	//reference shorthand
	private Animator anim;
	private Rigidbody2D rigid;
	private FlipSprite flipper;
	private OverlapCheck groundCheck;
	private OverlapCheck wallCheckFront;
	private OverlapCheck wallCheckBack;

	//Animation State Hashes:
	int idleState;
	int runState;
	int skidState;
	int jumpState;
	int fallState;
	int backflipState;
	int walljumpState;
	int slideState;

	//internal state
	bool jumping = false;
	bool sliding = false;
	float currentInputMove;		//current horizontal input
	float currentInputJump;		//current horizontal input
	AnimatorStateInfo currentState;
	float gravMem;	//stores original rigidbody.gravityscale

	//For Walljumps:
	private Vector2 incomingVelocity = Vector2.zero;
	private float windowTimer = 0;


	void Awake () {
		// Setting up references.
		groundCheck = transform.FindChild("GroundCheck").GetComponent<OverlapCheck>();
		anim = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		flipper = GetComponent<FlipSprite>();
		wallCheckFront = transform.FindChild("WallCheckFront").GetComponent<OverlapCheck>();
		wallCheckBack = transform.FindChild("WallCheckBack").GetComponent<OverlapCheck>();

		//Animation state setup
		idleState = Animator.StringToHash("Base Layer.Idle");
		runState = Animator.StringToHash("Base Layer.Run");
		skidState = Animator.StringToHash("Base Layer.Skid");
		jumpState = Animator.StringToHash("Base Layer.Jump");
		fallState = Animator.StringToHash("Base Layer.Fall");
		backflipState = Animator.StringToHash("Base Layer.Backflip");
		walljumpState = Animator.StringToHash("Base Layer.WallJump");
		slideState = Animator.StringToHash("Base Layer.Slide");

		gravMem = rigid.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
		currentInputMove = Input.GetAxisRaw("Horizontal");
		currentInputJump = Input.GetAxis("Jump");
		currentState = anim.GetCurrentAnimatorStateInfo(0);
		windowTimer -= Time.deltaTime;

		//Parameter update
		anim.SetBool("Grounded", groundCheck.overlaps);
		anim.SetFloat("SpeedX", Mathf.Abs(rigid.velocity.x));
		anim.SetFloat("SpeedY", rigid.velocity.y);
		anim.SetInteger("Input", (int) currentInputMove);
		anim.SetBool("DirAlign", currentInputMove * rigid.velocity.x >= 0);

		//Jumping:
		if(currentState.fullPathHash == idleState || currentState.fullPathHash == runState || currentState.fullPathHash == skidState || currentState.fullPathHash == slideState)
		{
			if(Input.GetButtonDown("Jump"))
			{
				jumping = true;
				anim.SetTrigger("Jump");
			}
		}

		//Turnaround
		if(currentState.fullPathHash == runState && rigid.velocity.x != 0)
		{
			flipper.FaceDir(rigid.velocity.x > 0);
		}

		//Test for Walljump
		bool wallCollision = (wallCheckBack.overlaps && rigid.velocity.x * transform.localScale.x < 0) || (wallCheckFront.overlaps && rigid.velocity.x * transform.localScale.x > 0);
		if((currentState.fullPathHash == jumpState || currentState.fullPathHash == fallState || currentState.fullPathHash == backflipState)
			&& wallCollision && Mathf.Abs(rigid.velocity.x) >= velocityThresh && (windowTimer < 0 || Mathf.Abs(rigid.velocity.x) > Mathf.Abs(incomingVelocity.x)))
		{
			windowTimer = jumpWindow;
			incomingVelocity = rigid.velocity;
			if(windowTimer >= 0 && currentInputJump > 0)
			{
				anim.SetTrigger("Jump");
				windowTimer = -1;
			}
		}

		//Sliding
		if(currentState.fullPathHash == runState && Input.GetButtonDown("Slide") && Mathf.Abs(rigid.velocity.x) > slideThresh)
		{
			sliding = true;
			anim.SetTrigger("Slide");
		}
	}

	void FixedUpdate ()
	{
		//Ground Movement
		if(currentState.fullPathHash == idleState || currentState.fullPathHash == runState || currentState.fullPathHash == skidState || currentState.fullPathHash == slideState)
		{
			if(jumping)
			{
				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				jumping = false;
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
				if(currentState.fullPathHash == skidState)
				{	
					//Doing a backflip might do something special
					rigid.AddForce(Vector2.Scale(backflipForce, (Vector2.right * Mathf.Sign(transform.localScale.x))));
					// ^ adding an additional backwards force on a backflip feels good
				}
			}

			if(currentState.fullPathHash != slideState)
			{
				rigid.AddForce(Vector2.right * currentInputMove * moveForceGround);
				if(currentInputMove == 0)	//Slow down when no direction is pressed
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
			}
		}

		//Air Movement
		if(currentState.fullPathHash == jumpState || currentState.fullPathHash == fallState || currentState.fullPathHash == backflipState)
		{
			if(Mathf.Abs(rigid.velocity.x) < maxAirAccel || currentInputMove * rigid.velocity.x <= 0)
			{
				rigid.AddForce(Vector2.right * currentInputMove * moveForceAir);
			}
			if(currentInputJump < 1 && currentState.fullPathHash == jumpState && rigid.velocity.y > 0)	//stop rising when the jump button is released
			{
				rigid.AddForce(Vector2.down * rigid.velocity.y * jumpBreak);
			}
			if(currentInputMove == 0)	//Slow down when no direction is pressed
			{
				rigid.AddForce(Vector2.left * rigid.velocity.x * autoBreakAir);
			}
			if(rigid.velocity.y < -maxFallSpeed)		//cap fall speed
			{
				rigid.velocity = Vector2.down * maxFallSpeed;
			}
		}

		//Walljump TODO
		/*
		 * Make bad Walljumps better (minimum outgoing x and y speed?)
		 * change angle calculation to allow for more horizontal jumps
		 * make maximum vertical jumps worse?
		 * Give bonus to away jumps?
		 */
		if(currentState.fullPathHash == walljumpState)
		{
			jumping = true;
			rigid.gravityScale = 0;
			rigid.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 0, rigid.velocity.y * 0.5f);
			flipper.FaceDir(incomingVelocity.x > 0);
		}
		else
		{
			rigid.gravityScale = gravMem;
		}
		if((currentState.fullPathHash == jumpState || currentState.fullPathHash == fallState) && jumping)
		{
			float angleFactor = 0.5f * (currentInputMove * Mathf.Sign(incomingVelocity.x) + 1);	//1 means up, 0 means 45 degrees
			Vector2 jumpvec = (Vector2) Vector3.Slerp(new Vector3(-Mathf.Sign(incomingVelocity.x), 1, 0).normalized, Vector3.up, angleFactor);
			Vector2 compareAngle = new Vector2(Mathf.Abs(incomingVelocity.x), Mathf.Abs(incomingVelocity.y)).normalized;
			float force = 1 - wjAnglePenalty * Vector2.Angle(Vector2.one.normalized, compareAngle)/45;
			//float forceOut = 1.1f - angleFactor * 0.2f;
			rigid.velocity = force * (jumpvec * Mathf.Abs(incomingVelocity.x) + new Vector2(0, incomingVelocity.y));
			//rigid.velocity = 35 * jumpvec;		//this alternative also feels ok

			incomingVelocity = Vector2.zero;
			jumping = false;
			if(Mathf.Abs(rigid.velocity.x) > 1)
			{
				flipper.FaceDir(rigid.velocity.x > 0);
			}
		}

		//Slide
		if(sliding)
		{
			sliding = false;
			rigid.AddForce(Vector2.right * (Mathf.Sign(transform.localScale.x) * slideBoost));
		}
		if(currentState.fullPathHash == slideState)
		{
			rigid.AddForce(Vector2.left * rigid.velocity.x * slideBreak);
		}
	}

	/*
	// For Debugging:
	void OnGUI()
	{
		//int w = Mathf.RoundToInt(Screen.width * size), h = Mathf.RoundToInt(Screen.height * size);
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = Mathf.RoundToInt(2 * h * 0.02f);
		style.normal.textColor = (windowTimer > 0) ? new Color (0.2f, 0.9f, 0.2f, 1.0f) : new Color (0.3f, 0.4f, 0.3f, 1.0f);
		string text = string.Format("{0:0.0} ms \n ({1:0.} vel)", windowTimer, rigid.velocity);
		GUI.Label(rect, text, style);
	}
	*/
}
