/*
 * Mirrors the Sprite horizontally
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour {

	[HideInInspector]
	public float direction;	//1: facing right, -1: facing left

	private SpriteRenderer sprite;

	public void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		//It assumes all sprites face right by default
		direction = (sprite.flipX) ? -1 : 1;
	}

	public void Flip()	//TODO this should probably be its own script
	{
		sprite.flipX = !sprite.flipX;
		direction = (sprite.flipX) ? -1 : 1;
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