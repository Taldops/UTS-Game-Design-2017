using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO
 * - Interpolate when frequency > FPS
 * */
public class ShadowTrail : MonoBehaviour {

	public Color trailColor;
	public float fadeTime;
	public float frequency;

	private float countdown = 0;
	private SpriteRenderer sprite;

	// Use this for initialization
	void Awake () 
	{
		sprite = transform.Find("Body").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		countdown -= Time.deltaTime;
		if(countdown <= 0)
		{
			spawnImage();
			countdown = 1/frequency;
		}
	}

	private void spawnImage()
	{
		GameObject image = new GameObject();
		image.transform.position = sprite.gameObject.transform.position;
		image.transform.rotation = sprite.gameObject.transform.rotation;
		image.transform.localScale = sprite.gameObject.transform.lossyScale;
		image.AddComponent<SpriteRenderer>();
		image.GetComponent<SpriteRenderer>().sprite = sprite.sprite;
		image.GetComponent<SpriteRenderer>().color = trailColor;
		image.GetComponent<SpriteRenderer>().sortingOrder = -1;
		image.AddComponent<Fade>();
		image.GetComponent<Fade>().lifespan = fadeTime;
		image.name = "Shadow Trail";
	}
}
