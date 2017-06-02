using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {

	public float lifespan;

	private float progress;		//goes from 1 to 0
	private float originalAlpha;
	private SpriteRenderer sprite;

	// Use this for initialization
	void Start() 	//Has to be start instead of awake?
	{
		sprite = GetComponent<SpriteRenderer>();
		progress = 1;
		originalAlpha = sprite.color.a;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(progress <= 0)
		{
			GameObject.Destroy(this.gameObject);
		}
		progress = (progress * lifespan - Time.deltaTime)/lifespan;
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, progress * originalAlpha);
	}
}
