using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlTwo : MonoBehaviour {

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

	//Action parameters:
	public float slideThresh = 14.0f;	//How fast you need to go to slide
	public float slideBoost	= 800;		//Speedboost for slide start
	public float slideBreak = 2;		//How fast you lose speed while sliding
	public float maxSlideJumpSpeedFactor = 1.3f; //How much of the normal max speed can be reached with a slideJump
	public Vector2 vaultImpulse = new Vector2(200, 600);	//Fore that gets applied when a vault is performed

	//reference shorthand
	private Animator anim;
	private Rigidbody2D rigid;
	private FlipSprite flipper;
	private GameObject body;
	private OverlapCheck groundCheck;
	private OverlapCheck[] wallChecks;
	private OverlapCheck vaultCheck;

	//Animation State Hashes:
	int idleState;
	int runState;
	int skidState;
	int jumpState;
	int fallState;
	int backflipState;
	int walljumpState;
	int slideState;
	int diveState;
	int rollState;
	int vaultState;

	//internal state
	bool jumping = false;	//keep track of jumoing
	bool action = false;	//keep track of sliding and diving
	bool vault = false;		//has the vault impulse been applied yet?
	float currentInputMove;		//current horizontal input
	float currentInputJump;		//current horizontal input
	AnimatorStateInfo currentState;
	float gravMem;	//stores original rigidbody.gravityscale
	float vCheckX;	//Stores original x cooridinate of vaultCheck

	//For Walljumps:
	private Vector2 incomingVelocity = Vector2.zero;
	private float windowTimer = 0;


	void Awake () {
		// Setting up references.
		groundCheck = transform.Find("GroundCheck").GetComponent<OverlapCheck>();
		anim = GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		flipper = GetComponentInChildren<FlipSprite>();
		body = transform.Find("Body").gameObject;
		wallChecks = new OverlapCheck[2];
		wallChecks[0]= transform.Find("WallCheck1").GetComponent<OverlapCheck>();
		wallChecks[1] = transform.Find("WallCheck2").GetComponent<OverlapCheck>();
		vaultCheck = transform.Find("VaultCheck").GetComponent<OverlapCheck>();

		//Animation state setup
		idleState = Animator.StringToHash("Base Layer.Idle");
		runState = Animator.StringToHash("Base Layer.Run");
		skidState = Animator.StringToHash("Base Layer.Skid");
		jumpState = Animator.StringToHash("Base Layer.Jump");
		fallState = Animator.StringToHash("Base Layer.Fall");
		backflipState = Animator.StringToHash("Base Layer.Backflip");
		walljumpState = Animator.StringToHash("Base Layer.WallJump");
		slideState = Animator.StringToHash("Base Layer.Slide");
		diveState = Animator.StringToHash("Base Layer.Dive");
		rollState = Animator.StringToHash("Base Layer.Roll");
		vaultState = Animator.StringToHash("Base Layer.Vault");

		gravMem = rigid.gravityScale;
		vCheckX = vaultCheck.transform.localPosition.x;
 
						  
																 
											 
							   
				
  
			   
  

						 
																 
											 
								  

	}
 
	public float inputMove(){
		Vector2 playerpos = GameObject.Find("Hero").transform.position;
		Vector2 enemypos = this.transform.position;
		if (playerpos.x < enemypos.x)
			return -1.0f;
		
			return 1.0f;
	}

	public bool inputJump(){
		Vector2 playerpos = GameObject.Find("Hero").transform.position;
		Vector2 enemypos = this.transform.position;
		return playerpos.y > enemypos.y;

	}
	
	public float inputJumpAxis(){
		Vector2 playerpos = GameObject.Find("Hero").transform.position;
		Vector2 enemypos = this.transform.position;
		if( playerpos.y > enemypos.y)
			return 1.0f;
		else
			return -1.0f;

	}						  
																 
											 
							   
			   
	  
				

  
 
	// Update is called once per frame
	void Update () {
	
		currentInputMove = inputMove();
		currentInputJump = inputJumpAxis();

  
								 
		currentState = anim.GetCurrentAnimatorStateInfo(0);
		windowTimer -= Time.deltaTime;
		vaultCheck.transform.localPosition = new Vector3 (flipper.direction * vCheckX, vaultCheck.transform.localPosition.y, vaultCheck.transform.localPosition.z);

		//Parameter update
		anim.SetBool("Grounded", groundCheck.overlaps);
		anim.SetFloat("SpeedX", Mathf.Abs(rigid.velocity.x));
		anim.SetFloat("SpeedY", rigid.velocity.y);
		anim.SetInteger("Input", (int) currentInputMove);
		anim.SetBool("DirAlign", currentInputMove * rigid.velocity.x >= 0);
		anim.SetBool("CouldVault", !vaultCheck.overlaps);

		//Jumping:
		if(currentState.fullPathHash == idleState || currentState.fullPathHash == runState || currentState.fullPathHash == skidState || currentState.fullPathHash == slideState)
		{
			if(inputJump())
			{
				jumping = true;
							
			}
		}


		//Turnaround
		if(currentState.fullPathHash == runState && rigid.velocity.x != 0)
		{
			flipper.FaceDir(rigid.velocity.x > 0);
		}

		//Rotating towards neutral
		if(currentState.fullPathHash == vaultState || currentState.fullPathHash == jumpState || currentState.fullPathHash == fallState)
		{
			Vector3 from = flipper.direction * body.transform.right;
			Vector3 to = Vector3.right * flipper.direction;
			body.transform.right = flipper.direction * Vector3.Slerp(from, to, 4 * currentState.speed * Time.deltaTime);
		}

		//Test for Walljump
		int frontCheck = (flipper.direction * (wallChecks[0].gameObject.transform.position.x - transform.position.x) > 0) ? 0 : 1;
		bool wallCollision = (wallChecks[frontCheck].overlaps && rigid.velocity.x * flipper.direction > 0) || (wallChecks[1 - frontCheck].overlaps && rigid.velocity.x * flipper.direction < 0);
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

		//Action States
		//Sliding
		if(((currentState.fullPathHash == runState && Mathf.Abs(rigid.velocity.x) > slideThresh)		//Sliding conditions
			|| (currentState.fullPathHash == jumpState || currentState.fullPathHash == fallState))			//Diving conditions
			&& Input.GetButtonDown("Action"))															//Input for either
		{
			action = true;
		}
		//Dive orientation
		if(currentState.fullPathHash == diveState)
		{
			Vector2 v = (rigid.velocity + flipper.direction * 0.05f * Vector2.right);
			v = (v.SqrMagnitude() > 1) ? v.normalized : v;
			body.transform.right = new Vector3(Mathf.Cos(v.y), flipper.direction *  v.y, 0);
		}
		//Rolling after diving resets orientation and vault status
		if(currentState.fullPathHash == rollState)
		{
			body.transform.up = Vector3.up;
			vault = false;
		}
	}

	void FixedUpdate ()
	{
		//Ground Movement
		if(currentState.fullPathHash == idleState || currentState.fullPathHash == runState || currentState.fullPathHash == skidState || currentState.fullPathHash == slideState)
		{
			body.transform.up = Vector3.up;
			if(jumping
				&& (currentState.fullPathHash != slideState || Mathf.Abs(rigid.velocity.x) <= maxSpeed * maxSlideJumpSpeedFactor)) //Prevent immediate jumpout
																
	
															
	
													  
	
															 
	
																							
	

			  
			{
				/* BUG:
				 * The above condition doesn't work as intended, because somhow the current state is still running EVEN THOUGH YOU CAN SEE THE SPRITE SLIDING. 
				 * Even the extended condition in the next line doesn't work. Thus you can jump out of slide immediately by pressing Action and Jump at the same time.
				 * TODO Fix if possible*/
				//print("WAT: " + (currentState.fullPathHash != slideState && !anim.GetBool("Action") && !anim.IsInTransition(0)));
				jumping = false;
				anim.SetTrigger("Jump");
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
				if(currentState.fullPathHash == skidState)
				{	
					//Doing a backflip might do something special
					rigid.AddForce(Vector2.Scale(backflipForce, (Vector2.right * flipper.direction)));
					// ^ adding an additional backwards force on a backflip feels good
				}
			}

			if(action) //Slide
			{
				action = false;
				rigid.AddForce(Vector2.right * (flipper.direction * slideBoost));
				anim.SetTrigger("Action");
			}

			//Friction
			if(currentState.fullPathHash == slideState)
			{
				rigid.AddForce(Vector2.left * rigid.velocity.x * slideBreak);
			}
			else
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
		if(currentState.fullPathHash == jumpState || currentState.fullPathHash == fallState || currentState.fullPathHash == backflipState || currentState.fullPathHash == vaultState)
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
			//Diving
			if(action)
			{
				//Commence dive
				rigid.velocity = new Vector2(flipper.direction * Mathf.Max(Mathf.Abs(rigid.velocity.x), maxSpeed) , rigid.velocity.y);
				anim.SetTrigger("Action");
				action = false;
				vault = true;
			}
			if(currentState.fullPathHash == vaultState && vault)
			{
				vault = false;
				rigid.AddForce(vaultImpulse);
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
			rigid.velocity = new Vector2(flipper.direction * 0, rigid.velocity.y * 0.5f);
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
