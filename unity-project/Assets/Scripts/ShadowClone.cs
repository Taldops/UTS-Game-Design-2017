using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowClone : MonoBehaviour {

	public float killDistance = 10;
	private GameObject clone;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(clone && Vector3.Distance(transform.position, clone.transform.position) > killDistance)
		{
			//clone.GetComponent<PlayerControl>().Die();	//TODO
			GameObject.Destroy(clone);
			clone = null;
		}
	}

	public void makeClone(Vector3 offset)	//Can a Vector parameter have a default value?
	{
		clone = GameObject.Instantiate(this.gameObject);
		clone.transform.position = transform.position + offset;
		clone.GetComponent<PlayerCharacter>().enabled = false;
		clone.name = "Shadow Clone";
		clone.transform.Find("Body").GetComponent<SpriteRenderer>().color = new Color(0.6f, 0, 1.0f, 0.75f);
	}

}
