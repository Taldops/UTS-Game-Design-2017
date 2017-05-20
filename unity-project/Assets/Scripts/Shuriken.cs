using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	public float speed = 0.1f;
	public int damage = 1;
	public Vector3 normalizedDirection;
	private float timeout = 6;
	private int living = 0;

	void Update() {
		//transform.Translate(speed * Time.deltaTime, 0, 0);
		transform.Translate(normalizedDirection * (speed * Time.deltaTime));
		timeout -= Time.deltaTime;
		if (timeout <= 0){
			Destroy(this.gameObject);
			
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Triggered");
		PlayerCharacter player = other.GetComponent<PlayerCharacter>();
		if (player != null) {
			player.Hurt(damage);
			Debug.Log("PlayerHit");
			Destroy(this.gameObject);
		}
		
	}
	
}
