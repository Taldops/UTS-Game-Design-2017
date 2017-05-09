using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour {

	[HideInInspector]
	public bool facingRight = true;

	public void Awake()
	{
		facingRight = transform.localScale.x > 0;
	}

	public void Flip()	//TODO this should probably be its own script
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		facingRight = !facingRight;
	}

	/*
	 * Makes sure the sprite is facing a certain direction.
	 * false: left; true: right
	 * */
	public void FaceDir(bool right)
	{
		if(facingRight != right)
		{
			Flip();
		}
	}
}
