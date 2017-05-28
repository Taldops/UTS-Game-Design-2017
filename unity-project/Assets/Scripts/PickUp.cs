using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
	
	private int value = 100;
	private bool collected = false;
		
		
	void Update() {
		
		
	}

	void OnTriggerEnter2D(Collider2D other) {		
		PlayerCharacter player = other.transform.parent.GetComponent<PlayerCharacter>();
		if (player != null) {
			if(!collected)
			player.getPoints(value);
		    collected = true;
			Debug.Log ("PlayerPickup");
			Destroy(this.gameObject);
			}
		}
		
		
	}
	

