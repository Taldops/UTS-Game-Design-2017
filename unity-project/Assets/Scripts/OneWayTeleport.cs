using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayTeleport : MonoBehaviour {

	public GameObject exit;
	public bool cancelMomentum;		//sets velocity to 0 upon entering

	// Use this for initialization
	void Awake () {
		exit.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
		GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		Transform root = other.transform.root;
		root.position = exit.transform.position;
		if(cancelMomentum && root.GetComponent<Rigidbody2D>())
		{
			root.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}

}
