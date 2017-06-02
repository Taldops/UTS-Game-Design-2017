using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO
 * BUGS:
 * 	- rarly, when mashing buttons, wierd stuff still happens
 *  - rarely, wallfreeze will turn the player around
 *  - backflips give inconsistent height for some reason
 * Walljumps:
 * 		 * Better magnetiziation towards walls
		 * Fix VISUAL BUG: Walljumps on walls on the right push the player out while the animation is playing
 * */
public class PlayerControl : MonoBehaviour {

	// Movement parameters:
	public float moveForceGround = 50;			// Amount of force added to move the player left and right.
	public float moveForceAir = 40;			// Amount of force added to move the player left and right.
	public float maxSpeed = 22;				// The fastest the player can travel in the x axis.
	public float maxFallSpeed = 50;
	public float jumpForce = 30;			// Amount of force added when the player jumps.
	public float autoBreak = 8;			// How fast the character stops moving if no direction is pressed
	public float autoBreakAir = 2;		// How much the character deccelerates in the air when no button is pressed
	public float jumpBreak = 25;		//How quickly you stop rising after the jump button is released
	public float maxAirAccel = 14;		//The maximum speed the character can reach by accelerating in the air
	public Vector2 backflipSpeed = new Vector2(-8, 36);	//Force that gets added to the jump when it is a backflip
	public float minJumpHight = 28;		//Maximum speed at which releasing jump stops ascension

	//Walljump parameters
	public float wjAnglePenalty	= 0.25f;	//0: Walljump forcce does not depend on incoming angle, 1: no force when coming in at 90deg
	public float wjForce = 0.8f;			//Overall multiplier on walljump sterength
	public float wjGraceWindow = 0.1f;		//How easy it is to walljump
	public float wjMinUp = 14;			//Minimum upforce a walljump will have
	public float wjMaxX = 1.3f;				//Maximum x velocity after a walljump
	public float wjOutAngleBonus = 0.25f;	//How much of a bonus jumping away from a wall gives you

	//Action parameters:
	public float slideBoost	= 24;		//Speedboost for slide start
	public float slideBreak = 1.6f;		//How fast you lose speed while sliding
	public float slideBreakAirMultiplier = 0.75f;		//How much faster/slower you lose speed after sliding off an edge
	public float maxSlideJumpSpeedFactor = 1.4f; //How much of the normal max speed can be reached with a slideJump
	public float bonkBounceFactor = 1.3f;	//How much bonking bounces you backwards
	public float actionThresh = 16; 	//minimum velocity required for a walljumps and slides

	//Other parameters
	public float checkDepth = 1;		//Depth of wall and ground checks. Higher values mean earlier/more sesitive detection
	public Vector2 hitKnockback = new Vector2(-10, -10);	//Force impact the player recieves upon getting hit
	public Color speedColor;		//The shadow trail will turn this color when the player is going very fast. Alpha is disregarded.

	//reference shorthand
	private Animator anim;
	private Rigidbody2D rigid;
	private FlipSprite flipper;
	private GameObject body;
	private OverlapCheck groundCheck;
	private OverlapCheck wallCheck;

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
	int bonkState;
	int getHitState;

	//internal state
	AnimatorStateInfo currentState;
	float gravMem;						//stores original rigidbody.gravityscale
	float maxSpeedMem;						//stores original maxSpeed
	Color colorMem;
	Vector2 wjVelCache = Vector2.zero;	//stores velocity that will be used for the walljump
	float wjCacheAge;					//Keep track of how old ^ is
	bool busy = false;					//Busy states can't be easily canceled in most cases
	bool actionInProgress = false;		//Makes it so that only one action is applied at a time. Stops wierd interactions
	bool alive = true;

	//Buffers
	bool jumpBuffer = false;	//keep track of jumoing
	bool actionBuffer = false;	//keep track of sliding and diving

	//Force application flags		//TODO Maybe there is a way to generalize these, since usually only one will be active at time?
	bool backflipFlag = false;
	bool slideFlag = false;
	bool diveFlag = false;
	bool jumpFlag = false;
	bool wallstickFlag = false;
	bool wjFlag = false;
	bool rollFlag = false;
	bool bonkFlag = false;
	bool hitFlag = false;

	void Awake () {
		// Setting up references.
		groundCheck = transform.Find("GroundCheck").GetComponent<OverlapCheck>();
		anim = GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		flipper = GetComponentInChildren<FlipSprite>();
		body = transform.Find("Body").gameObject;
		wallCheck = transform.Find("WallCheck").GetComponent<OverlapCheck>();

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
		bonkState = Animator.StringToHash("Base Layer.Bonk");
		getHitState = Animator.StringToHash("Base Layer.GetHit");

		gravMem = rigid.gravityScale;
		maxSpeedMem = maxSpeed;
		colorMem = GetComponent<ShadowTrail>().trailColor;
	}

	// Update is called once per frame
	void Update () {
		updateAnimation();
		updateChecks();

		if(alive)
		{
			bufferActions();
			if(!actionInProgress)
			{
				initiateActions();
				//Busy flag is hadled by each action on their own, because some busy states can be cancelled by specific actions
			}
			//Not sure if the order is important here

			//Indicate going fast enough
			updateTrail();
		}
	}

	void FixedUpdate ()
	{
		if(alive)
		{
			basicMovement();
			stateDependentUpdate();
			if(!wjFlag)
			{
				checkForWalljump();
			}
			applyActionForces();
			//Not sure if the order is important here
		}
		else
		{
			if(rigid.velocity.sqrMagnitude > 1)
			{
				rigid.gameObject.transform.right = -rigid.velocity;
			}
		}
	}

	/*
	* =====================================================================================
	* Public Functions
	* =====================================================================================
	*/

	/*
	* Makes the player get hit
	* Parameter: Multiplier how much the player flies back. Default is 1 for normal knockback
		*/
		public void GetHit(float force = 1)
	{
		rigid.velocity = Vector2.zero;
		anim.SetTrigger("GetHit");
		hitFlag = true;
		actionInProgress = true;
		clearAllFlags();
		clearAllBuffers();
	}

	/*
	* Makes the player move faster or slower. Can be used to implement powerups and such.
	*/
	public void SpeedUp(float factor)
	{
		moveForceGround *= factor;
		moveForceAir *= factor;
		maxSpeed *= factor;
		maxAirAccel *= factor;
		backflipSpeed = new Vector2(backflipSpeed.x * factor, backflipSpeed.y);
		//TODO: also multiply shadow trail fadeTime and frequency
	}

	/*
	 * This kills the player
	 * */
	public void Die()
	{
		alive = false;
	}

	public bool isAlive()
	{
		return alive;
	}

	/*
	* =====================================================================================
	* Update Functions
	* =====================================================================================
	*/

	/*
	* Making the character do an action (e.g. jumping) consits of 3 steps:
		1. If an input is pressed under the right circumstances, buffer the action (method: bufferActions())
		2. When the conditions are met, initiate the action by setting the animatio trigger and physics flags (method: initiateActions())
		3. In the next fixed update, force and speed changes will be applied depending on the set physics flags (method: applyForces())
		When a physics flag is set, actionInProgress also has to be set. When it is resolved, also clear actionInProgress!
		*/

		/*
		* Continual updates during certain animation states
		*/
		private void stateDependentUpdate()
	{
		//Sliding Friction
		if(currentState.fullPathHash == slideState)
		{
			float friction = groundCheck.overlaps ? slideBreak : slideBreak * slideBreakAirMultiplier;
			rigid.AddForce(Vector2.left * rigid.velocity.x * friction, ForceMode2D.Force);
		}

		//Always face forward when running
		if(currentState.fullPathHash == runState && rigid.velocity.x != 0)
		{
			flipper.FaceDir(rigid.velocity.x > 0);
		}

		//Diving
		if(currentState.fullPathHash == diveState)
		{

			Vector2 newRight = (rigid.velocity + flipper.direction * 0.05f * Vector2.right);
			newRight = (newRight.SqrMagnitude() > 1) ? newRight.normalized : newRight;
			body.transform.right = new Vector3(Mathf.Cos(newRight.y), flipper.direction *  newRight.y, 0);
			//Using cos makes for smoother transitions than just transform.right = rigid.velocity
			if(wallCheck.overlaps)	//bonking
			{
				anim.SetTrigger("Bonk");
				bonkFlag = true;
				actionInProgress = true;
			}
			if(groundCheck.overlaps && !wallCheck.overlaps)	//rolling
			{
				anim.SetTrigger("Roll");
				rollFlag = true;
				actionInProgress = true;
			}
		}

		//Setting the running animation speed
		if(currentState.fullPathHash == runState)
		{
			anim.SetFloat("SpeedMod", Mathf.Max(Mathf.Abs(rigid.velocity.x)/maxSpeedMem, 0.3f));
		}
	}

	/*
	 * This is responsible for basic left/right control in the air and on the ground.
	 * */
	private void basicMovement()
	{
		float currentInputMove = Input.GetAxisRaw("Horizontal");
		//Ground Movement
		if(currentlyInState(idleState, runState, skidState))
		{
			rigid.AddForce(Vector2.right * currentInputMove * moveForceGround, ForceMode2D.Force);
			if(currentInputMove == 0)	//Slow down when no direction is pressed
			{
				rigid.AddForce(Vector2.left * rigid.velocity.x * autoBreak, ForceMode2D.Force);
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

		//Air Movement
		if(currentlyInState(jumpState, fallState))		//TODO What about dive and backflip?
		{
			if(Mathf.Abs(rigid.velocity.x) < maxAirAccel || currentInputMove * rigid.velocity.x <= 0)
			{
				rigid.AddForce(Vector2.right * currentInputMove * moveForceAir, ForceMode2D.Force);
			}
			if(Input.GetAxisRaw("Jump") < 1 && currentState.fullPathHash == jumpState && rigid.velocity.y < minJumpHight)	//stop rising when the jump button is released
			{
				rigid.AddForce(Vector2.down * rigid.velocity.y * jumpBreak, ForceMode2D.Force);
			}
			if(currentInputMove == 0)	//Slow down when no direction is pressed
			{
				rigid.AddForce(Vector2.left * rigid.velocity.x * autoBreakAir, ForceMode2D.Force);
			}
			if(rigid.velocity.y < -maxFallSpeed)		//cap fall speed
			{
				rigid.velocity = Vector2.down * maxFallSpeed;
			}
		}
	}

	/*
	 * Buffers actions during certain states, if the corresponding input is pressed.
	 * */
	private void bufferActions()
	{
		//Jumping:
		if(Input.GetButtonDown("Jump") && currentlyInState(idleState, runState, skidState, slideState) && groundCheck.overlaps)
		{
			jumpBuffer = true;
		}
		//Diving and Sliding:
		if(Input.GetButtonDown("Action") && currentlyInState(jumpState, fallState, backflipState, runState, rollState)
			&& (!wallCheck.overlaps || currentState.fullPathHash == walljumpState))
		{
			actionBuffer = true;
		}
	}

	/*
	* If Actions are buffered and their conditions are met, they will be executed here.
	* Execution involves: 1. Setting the animation trigger 2. Setting the force flag
	*/
	private void initiateActions()
	{
		//Sliding
		if(actionBuffer && !busy && groundCheck.overlaps && Mathf.Abs(rigid.velocity.x) > actionThresh && !wallCheck.overlaps
			&& currentlyInState(runState, skidState, idleState)) //Slide
		{
			anim.SetTrigger("Action");
			actionBuffer = false;
			slideFlag = true;
			actionInProgress = true;
			return;
		}

		//Diving
		if(actionBuffer && !busy && !groundCheck.overlaps && rigid.velocity.y < 23 && !wallCheck.overlaps
			&& currentlyInState(jumpState, fallState, backflipState))
		{
			//Commence dive
			rigid.velocity = new Vector2(flipper.direction * Mathf.Max(Mathf.Abs(rigid.velocity.x), maxSpeed) , rigid.velocity.y);
			anim.SetTrigger("Action");
			actionBuffer = false;
			diveFlag = true;
			actionInProgress = true;
			return;
		}

		//Jumping
		if(jumpBuffer && (!busy || (currentlyInState(slideState) && Mathf.Abs(rigid.velocity.x) <= maxSpeed * maxSlideJumpSpeedFactor)))
		{
			if(currentState.fullPathHash == skidState)
			{
				backflipFlag = true;
				anim.SetTrigger("Backflip");
			}
			else
			{
				jumpFlag = true;
				anim.SetTrigger("Jump");
			}

			jumpBuffer = false;
			actionInProgress = true;
			return;
		}
		//TODO Can actionInProgress = true be centralized somehow?
	}

	/*
	 * This contains all physics changes caused by actions and state changes.
	 * The physical effect will be applied if its corresponding flag is set.
	 */
	private void applyActionForces()
	{
		//Jumping
		if(jumpFlag)
		{
			rigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			jumpFlag = false;
			actionInProgress = false;
		}

		//backflip
		if(backflipFlag)
		{	
			//Doing a backflip might do something special
			//rigid.AddForce(alignForward(backflipForce), ForceMode2D.Impulse);
			rigid.velocity = alignForward(backflipSpeed);
			// ^ adding an additional backwards force on a backflip feels good
			backflipFlag = false;
			actionInProgress = false;
		}

		//Walljump freeze
		if(currentState.fullPathHash == walljumpState && wallstickFlag == true)
		{
			rigid.gravityScale = 0;
			rigid.velocity = new Vector2(flipper.direction * 10, rigid.velocity.y * 0.0f);
			flipper.FaceDir(wjVelCache.x > 0);
			clearAllBuffers();
			wallstickFlag = false;
			wjFlag = true;
			//Action still in progress!
		}

		//Walljump proper
		if(currentState.fullPathHash != walljumpState)
		{
			rigid.gravityScale = gravMem;
			if(currentState.fullPathHash == jumpState && wjFlag == true)
			{
				//Calculating and applying outgoing angle and power
				Vector2 incomingVelocity = new Vector2(wjVelCache.x, Mathf.Max(rigid.velocity.y, wjVelCache.y));
				float angleFactor = 0.5f * (Input.GetAxisRaw("Horizontal") * Mathf.Sign(incomingVelocity.x) + 1);	//1 means up, 0 means 45 degrees
				Vector2 jumpvec = (Vector2) Vector3.Slerp(new Vector3(-Mathf.Sign(incomingVelocity.x) * wjMaxX, 1, 0).normalized, Vector3.up, angleFactor);
				Vector2 compareAngle = new Vector2(Mathf.Abs(incomingVelocity.x), Mathf.Abs(incomingVelocity.y)).normalized;
				float inForce = 1 - wjAnglePenalty * Vector2.Angle(Vector2.one.normalized, compareAngle)/45;
				float outForce = 1 + wjOutAngleBonus * Mathf.Abs(jumpvec.x);
				Vector2 newVelocity = inForce * outForce * wjForce * (jumpvec * Mathf.Abs(incomingVelocity.x) + new Vector2(0, incomingVelocity.y));
				if(newVelocity.y < wjMinUp)
				{
					newVelocity = new Vector2(rigid.velocity.x, wjMinUp);
				}
				if(Mathf.Abs(newVelocity.x) < (1-angleFactor) * actionThresh)
				{
					newVelocity = new Vector2(Mathf.Sign(newVelocity.x) * (1-angleFactor) * actionThresh, newVelocity.y);
				}
				if(Mathf.Abs(newVelocity.x) > 1)
				{
					flipper.FaceDir(jumpvec.x > 0);
				}

				rigid.velocity = newVelocity;
				wjVelCache = Vector2.zero;
				wjCacheAge = 10;
			}
			wjFlag = false;
			updateChecks();		//This is here to prevent double walljumps, caused by the check not switching sides fast enough. TODO: Find a better solution
			actionInProgress = false;
		}

		//Entering Bonk
		if(bonkFlag)
		{
			body.transform.up = Vector3.up;
			rigid.velocity = new Vector2(-Mathf.Sign(rigid.velocity.x) * Mathf.Max(Mathf.Abs(rigid.velocity.x) * bonkBounceFactor * 0.3f, 8), rigid.velocity.y);
			bonkFlag = false;
			actionInProgress = false;
		}

		//Entering Slide
		if(slideFlag && groundCheck.overlaps == true) //Slide
		{
			rigid.AddForce(Vector2.right * (flipper.direction * slideBoost), ForceMode2D.Impulse);
			//action = false;
			slideFlag = false;
			actionInProgress = false;
		}

		//Entering Dive
		if(diveFlag && groundCheck.overlaps == false)
		{
			rigid.velocity = new Vector2(flipper.direction * Mathf.Max(Mathf.Abs(rigid.velocity.x), maxSpeed) , rigid.velocity.y);
			diveFlag = false;
			actionInProgress = false;
		}

		//Entering Roll
		if(rollFlag)
		{
			body.transform.up = Vector3.up;
			float newVelX = Mathf.Max(Mathf.Abs(rigid.velocity.x), maxSpeed * 0.75f);
			rigid.velocity = new Vector2(newVelX * flipper.direction, rigid.velocity.y);
			anim.SetFloat("SpeedMod", Mathf.Abs(newVelX)/maxSpeedMem);
			rollFlag = false;
			actionInProgress = false;
		}

		//Getting Hit
		if(hitFlag)
		{
			rigid.AddForce(alignForward(hitKnockback), ForceMode2D.Impulse);
			hitFlag = false;
			actionInProgress = false;
		}
	}

	/*
	 * Checks if the conditions for a walljump are met and initiates one of the input is pressed.
	 * */
	private void checkForWalljump()
	{
		//update Velocity Cache
		if(Mathf.Abs(rigid.velocity.x) > Mathf.Abs(wjVelCache.x) && currentState.fullPathHash != walljumpState)
		{
			wjVelCache = new Vector2(rigid.velocity.x, wjVelCache.y);
		}
		if(rigid.velocity.y > 0 || (Mathf.Abs(rigid.velocity.y) > Mathf.Abs(wjVelCache.y) && currentState.fullPathHash != walljumpState))
		{
			wjVelCache = new Vector2(wjVelCache.x, Mathf.Clamp(Mathf.Max(rigid.velocity.y, wjVelCache.y), -30, 22));
		}
		if(wjCacheAge > wjGraceWindow)
		{
			wjVelCache = rigid.velocity;
			wjCacheAge = 0;
		}
		wjCacheAge += Time.deltaTime;

		//Jump check
		if(currentlyInState(jumpState, fallState, backflipState)
			&& wallCheck.overlaps && Mathf.Abs(wjVelCache.x) >= actionThresh && wjCacheAge < wjGraceWindow
			&& Input.GetAxisRaw("Jump") > 0)
		{
			anim.SetTrigger("Walljump");
			wallstickFlag = true;
			actionInProgress = true;
		}
	}

	/*
	 * Updates the position and size of the overlap checks
	 * */
	private void updateChecks()
	{
		BoxCollider2D bodyColl = body.GetComponent<BoxCollider2D>();
		Vector3 collCenter = body.transform.localPosition + (Vector3) alignForward(bodyColl.offset);
		Vector3 pointToPos;
		Vector3 pointTopRight = collCenter + (Vector3) alignForward(bodyColl.size) * 0.5f;
		pointTopRight = Quaternion.AngleAxis(getRotation(), Vector3.forward) * pointTopRight;
		float sizeX;
		if(currentState.fullPathHash == diveState)
		{
			pointToPos = collCenter + new Vector3(bodyColl.size.x * flipper.direction, -bodyColl.size.y, 0) * 0.5f;
			pointToPos = Quaternion.AngleAxis(getRotation(), Vector3.forward) * pointToPos;
			sizeX = Mathf.Abs(pointTopRight.x - pointToPos.x);
		}
		else
		{
			pointToPos = -0.5f * Vector3.up * bodyColl.size.y;
			sizeX = bodyColl.size.x * 0.7f;
		}
		groundCheck.transform.localPosition = collCenter + pointToPos;
		groundCheck.gameObject.GetComponent<BoxCollider2D>().size = new Vector2 (sizeX, checkDepth);
		//Wallcheck
		float velocitySide = (Mathf.Abs(rigid.velocity.x) > 1) ? Mathf.Sign(rigid.velocity.x) : flipper.direction;
		float naturalOffset = 0.5f * bodyColl.size.x + 0.2f;
		wallCheck.transform.localPosition = collCenter + naturalOffset * velocitySide * Vector3.right + 0.1f * Vector3.up;
		wallCheck.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(checkDepth, checkDepth);
	}

	/*
	 * Updates Animation parameters and triggers the return to the fall and idle states if appropriate 
	 * */
	private void updateAnimation()
	{
		anim.SetFloat("SpeedX", Mathf.Abs(rigid.velocity.x));
		anim.SetInteger("Input", (int) Input.GetAxisRaw("Horizontal"));
		anim.SetBool("DirAlign", Input.GetAxisRaw("Horizontal") * rigid.velocity.x >= 0);
		anim.SetBool("Alive", alive);

		currentState = anim.GetCurrentAnimatorStateInfo(0);
		busy = !currentlyInState(runState, idleState, jumpState, fallState, skidState);		//Official List of non-busy states
		if(Mathf.Abs(rigid.velocity.x) < 1 && Mathf.Abs(rigid.velocity.y) < 1 && !Input.anyKey)
		{
			clearAllBuffers();
		}
		if((!busy || currentlyInState(bonkState)) && groundCheck.overlaps && !currentlyInState(idleState, skidState) && !actionInProgress
			&& (currentState.fullPathHash != runState || Mathf.Abs(rigid.velocity.x) < 6) && !anyFlags() && Mathf.Abs(rigid.velocity.y) < 1)
		{
			anim.SetTrigger("Idle");
		}
		if(currentState.fullPathHash != fallState && !busy && !groundCheck.overlaps && rigid.velocity.y < 6
			&& !anyFlags() && !actionInProgress)
		{
			anim.SetTrigger("Fall");
		}
	}

	/*
	 * Updates the speed dependent shadow trail
	 * */
	private void updateTrail()
	{
		ShadowTrail trail = GetComponent<ShadowTrail>();
		trail.enabled = Mathf.Abs(rigid.velocity.x) > actionThresh;
		float speedFactor = (Mathf.Abs(rigid.velocity.x) - actionThresh) / (2.5f * maxSpeedMem - actionThresh);
		Color trailColor = Color.Lerp(colorMem, speedColor, speedFactor);
		trailColor = new Color(trailColor.r, trailColor.g, trailColor.b, colorMem.a);
		trail.trailColor = trailColor;
	}

	/*
	* =====================================================================================
	* Helper Functions
	* =====================================================================================
	*/

	/*
	* Shorthand for checking if the player is currently in one of a number of states
		*/
		private bool currentlyInState(params int[] stateList)
	{
		for(int i = 0; i < stateList.Length; i++)
		{
			if (currentState.fullPathHash == stateList[i])
			{
				return true;
			}
		}
		return false;
	}

	/*
	 * Shorthand for getting the 2D rotation as a number between -180 and 180.
	 * This makes some calculations easier.
	 * */
	private float getRotation()
	{
		float angle = body.transform.eulerAngles.z;
		return (angle < 180) ? angle : angle - 360;
	}

	/*
	 * Returns whether any actions are buffered or flagged
	 * */
	private bool anyFlags()
	{
		return jumpFlag || wjFlag || wallstickFlag || slideFlag || bonkFlag || diveFlag || rollFlag || jumpBuffer || actionBuffer;
	}

	/*
	 * Sets all flags to false.
	 * EXCEPT hitFlag in actionInProgress.
	 * */
	private void clearAllFlags()
	{
		jumpFlag = false;
		wjFlag = false;
		wallstickFlag = false;
		slideFlag = false;
		bonkFlag = false;
		diveFlag = false;
		rollFlag = false;
	}

	/*
	 * Sets all buffered actions to false.
	 * */
	private void clearAllBuffers()
	{
		jumpBuffer = false;
		actionBuffer = false;
	}

	/*
	 * Makes sure that a vectors x value points in the same direction as the player
	 * */
	private Vector2 alignForward(Vector2 v)
	{
		return new Vector2(v.x * flipper.direction, v.y);
	}

	/*
	 * Gets the name of the state from the hash. Use for debugging. Unity should have this built in.
	 * Can't use switch statement because values are not constant.
	 * */
	private string getStateName(int state)
	{
		if(state == runState) return "Run";
		if(state == idleState) return "Idle";
		if(state == skidState) return "Skid";
		if(state == jumpState) return "Jump";
		if(state == fallState) return "Fall";
		if(state == backflipState) return "Backflip";
		if(state == rollState) return "Roll";
		if(state == walljumpState) return "Walljump";
		if(state == diveState) return "Dive";
		if(state == slideState) return "Slide";
		if(state == bonkState) return "Bonk";
		if(state == getHitState) return "Hitstun";
		return "State not recognized";
	}

	// For Debugging:
	/*
	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
		GUIStyle style = new GUIStyle();
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = Mathf.RoundToInt(2 * h * 0.02f);
		string text = string.Format("{0:0.0} ms)", wjCacheAge);
		string text = (busy) ? "busy" : "free";
		GUI.Label(rect, text, style);
	}
	*/

}