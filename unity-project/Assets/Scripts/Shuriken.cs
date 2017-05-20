using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	public float speed = 0.1f;
	public int damage = 1;
	public Vector3 normalizedDirection;
	private float timeout = 3;
	private int living = 0;
	public bool isEnemy;

	void Update() {
		//transform.Translate(speed * Time.deltaTime, 0, 0);
		transform.Translate(normalizedDirection * (speed * Time.deltaTime));
		timeout -= Time.deltaTime;
		if (timeout <= 0){
			Destroy(this.gameObject);
			
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log("Triggered");
		if(isEnemy){
		PlayerCharacter player = other.transform.parent.GetComponent<PlayerCharacter>();
		if (player != null) {
			player.Hurt(damage);
			//Debug.Log("PlayerHit");
			Destroy(this.gameObject);
		}
		}
		if(!isEnemy){
		EnemyCharacter enemy = other.transform.parent.GetComponent<EnemyCharacter>();
		if (enemy != null) {
			enemy.Hurt(damage);
			//Debug.Log("PlayerHit");
			Destroy(this.gameObject);
		}
		}
		
	}
	
}
