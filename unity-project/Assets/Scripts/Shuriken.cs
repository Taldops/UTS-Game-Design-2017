using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	public float speed = 30f;
	public int damage = 1;
	public Vector3 normalizedDirection;
	private float timeout = 2;
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
            if(other.transform.parent != null) { 
		PlayerCharacter player = other.transform.parent.GetComponent<PlayerCharacter>();
        //PlayerCharacter player = other.transform.GetComponent<PlayerCharacter>();
            if (player != null) {
			player.Hurt(damage);
			//Debug.Log("PlayerHit");
			Destroy(this.gameObject);
                }
            }
		}
		if(!isEnemy){
		EnemyCharacter enemy = other.transform.GetComponent<EnemyCharacter>();
		if (enemy != null) {
			enemy.Hurt(damage);
			//Debug.Log("PlayerHit");
			Destroy(this.gameObject);
		}
		}
		
	}
	
}
