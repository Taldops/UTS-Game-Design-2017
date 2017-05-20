using UnityEngine;
using System.Collections;

public class EnemyCharacter : MonoBehaviour {
	private int _health;

	void Start() {
		_health = 1;
	}

	public void Hurt(int damage) {
		_health -= damage;
		Debug.Log("Health: " + _health);
		if (_health <= 0){
			Destroy(this.gameObject);
		}
	}
	
}