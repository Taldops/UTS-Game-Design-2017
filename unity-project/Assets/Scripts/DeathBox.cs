using UnityEngine;
using System.Collections;

public class DeathBox : MonoBehaviour {
	
	public int damage = 100;
	
	void Update() {
	}

	//void OnTriggerEnter2D(Collider2D other) {
	void OnTriggerStay2D(Collider2D other) {
        if(other.transform.parent != null) {
		PlayerCharacter player = other.transform.parent.GetComponent<PlayerCharacter>();
		if (player != null) {
			player.Hurt(damage);
		}
	}
    }
}

