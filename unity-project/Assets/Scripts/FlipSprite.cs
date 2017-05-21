/*
 * Mirrors the Sprite horizontally
 * Uses transform.scale instead of SpriteRenderer.flipX because of animaion collider adjustments
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour {

	[HideInInspector] public float direction;	//1: facing right, -1: facing left

	public void Awake()
	{
		//It assumes all sprites face right by default
		direction = Mathf.Sign(transform.localScale.x);
	}

	public void Flip()	//TODO this should probably be its own script
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		direction = Mathf.Sign(transform.localScale.x);
	}

	/*
	 * Makes sure the sprite is facing a certain direction.
	 * false: left; true: right
	 * */
	public void FaceDir(bool right)
	{
		if((direction == 1) != right)
		{
			Flip();
		}
	}
}