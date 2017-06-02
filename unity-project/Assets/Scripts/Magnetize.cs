using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnatize : MonoBehaviour {

	private Transform trackedPlayer;
	private Vector3 priviosPos;

	// Use this for initialization
	void Awaken () 
	{
		priviosPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 displacement = transform.position - priviosPos;
		priviosPos = transform.position;
		if(trackedPlayer)
		{
			/*
			Rigidbody2D rigid = trackedPlayer.GetComponent<Rigidbody2D>();
			if(rigid)
			{
				//rigid.velocity = rigid.velocity + ((Vector2) displacement) * (0.25f/Time.fixedDeltaTime);
				rigid.AddForce(displacement * rigid.mass * 500, ForceMode2D.Force);
			}
			else
			{
				trackedPlayer.position += displacement;
			}
			*/
			trackedPlayer.position += displacement;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.transform.root.tag == "Player")
		{
			print("entering");
			trackedPlayer = coll.gameObject.transform.root;
			//trackedPlayer.parent = this.transform;
		}
	}

	void OnCollisionExit2D(Collision2D coll) 
	{
		if (trackedPlayer.GetComponentInChildren<Collider2D>() == coll.collider)
		{
			print("leaving");
			//trackedPlayer.parent = null;
			trackedPlayer = null;
		}
	}

}
