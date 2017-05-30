using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {
	
	public int damage = 1;
	
	void Update() {
	}

	//void OnTriggerEnter2D(Collider2D other) {
	void OnTriggerStay2D(Collider2D other) {
		PlayerCharacter player = other.transform.parent.GetComponent<PlayerCharacter>();
		if (player != null) {
			player.Hurt(damage);
		}
		EnemyCharacter enemy = other.transform.parent.GetComponent<EnemyCharacter>();
		if (enemy != null) {
			enemy.Hurt(damage);			
		}
		
	}
	
}

