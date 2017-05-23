using UnityEngine;
using System.Collections;

public class HealthPickUp : MonoBehaviour {
	
	private int value = 1;
	private bool collected = false;
		
		
	void Update() {
		
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		PlayerCharacter player = other.transform.parent.GetComponent<PlayerCharacter>();
		if (player != null) {
			if(!collected)
			player.getHealth(value);
		    collected = true;
			Debug.Log("PlayerPickup");
			Destroy(this.gameObject);
		}
		}
		
		
	}
	

