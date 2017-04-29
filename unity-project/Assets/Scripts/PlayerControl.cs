/*
 * Heavily modified version of the PlayerControl script from the 2D Platformer sample project
 * */

using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	
	public float moveForceGround = 220f;			// Amount of force added to move the player left and right.
	public float moveForceAir = 90f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 16f;				// The fastest the player can travel in the x axis.
	public float maxFallSpeed = 50;
	public float jumpForce = 1300f;			// Amount of force added when the player jumps.
	public float autoBreak = 8.0f;		// How fast the character stops moving if no direction is pressed
	public float autoBreakAir = 0.96f;
	public float turnFactor = 1.0f;		//Can make the character turn faster or slower. 1.0 is neutral
	public float maxAirAccel = 7f;		//The maximum speed the character can reach by accelerating in the air
	public Vector2 backflipForce = new Vector2(-300, 0);	//Force that gets added to the jump when it is a backflip

	private GroundCheck groundCheck;
	private Animator anim;
	private Rigidbody2D rigid;

	private bool jump = false;				// Condition for whether the player should jump.

	void Awake()
	{
		// Setting up references.
		groundCheck = GetComponentInChildren<GroundCheck>();
		anim = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody2D>();
	}


	void Update()
	{
		//Update Animations:
		anim.SetBool("Grounded", groundCheck.grounded);
		anim.SetFloat("SpeedX", Mathf.Abs(rigid.velocity.x));
		anim.SetFloat("SpeedY", rigid.velocity.y);
		anim.SetInteger("Input", (int) Input.GetAxisRaw("Horizontal"));

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(groundCheck.grounded)
		{
			if(Input.GetButtonDown("Jump"))
			{
				jump = true;
				anim.SetTrigger("Jump");
			}
		}
	}


	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float input = Input.GetAxisRaw("Horizontal");
		float moveForce = input;


		//Moving left and right
		if(groundCheck.grounded)
		{
			moveForce *= moveForceGround;
			if(input * rigid.velocity.x < 0)	//Checks whether the input aligns with the current movement
			{
				moveForce *= turnFactor;
				anim.SetBool("DirAlign", false);
			}
			else
			{
				anim.SetBool("DirAlign", true);
			}
		}
		else 	//If in the air
		{
			if(Mathf.Abs(rigid.velocity.x) < maxAirAccel || input * rigid.velocity.x <= 0)
			{
				moveForce *= moveForceAir;
			}

			if(Input.GetButtonUp("Jump") && rigid.velocity.y > 0)	//stop rising when the jump button is released
			{
				rigid.velocity = new Vector2(rigid.velocity.x, 0);
			}
		}

		//Adjusting movement
		if(Input.GetAxisRaw("Horizontal") == 0)	//Slow down when no direction is pressed
		{
			rigid.AddForce(Vector2.left * rigid.velocity.x * (groundCheck.grounded ? autoBreak : autoBreakAir));
		}
		if(Mathf.Abs(rigid.velocity.x) < 1)		//looks more natural
		{
			rigid.velocity = new Vector2(0, rigid.velocity.y);
		}
		if(Mathf.Abs(rigid.velocity.x) < maxSpeed || input * rigid.velocity.x <= 0)	//move the character
		{
			rigid.AddForce(Vector2.right * moveForce);
		}
		if(rigid.velocity.y < -maxFallSpeed)		//cap fall speed
		{
			rigid.velocity = Vector2.down * maxFallSpeed;
		}


		// If the player should jump...
		if(jump)
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;

			if(anim.GetCurrentAnimatorStateInfo(0).IsName("Skid"))
			{	
				//Backflip!
				//Doing a backflip might do something special
				rigid.AddForce(Vector2.Scale(backflipForce, (Vector2.right * Mathf.Sign(transform.localScale.x))));
				// ^ adding an additional backwards force on a backflip feels good
			}

		}

	}


}
