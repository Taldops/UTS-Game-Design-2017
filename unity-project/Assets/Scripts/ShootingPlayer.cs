using UnityEngine;
using System.Collections;

public class ShootingPlayer : MonoBehaviour {
	//public float speed = 3.0f;
	//public float obstacleRange = 5.0f;
	
	[SerializeField] private GameObject shurikenPrefab;
	//Vector3 offset = new Vector3(0, 30, 0);
	public float fireCooldown = 1;
	float tillFire = 0;
	
	//private bool _alive;
	
	void Start() {
		//_alive = true;
	}
	
	void Update ()
	{
		tillFire -= Time.deltaTime;
		if (Input.GetAxis ("Fire1") > 0) {
			if (tillFire <= 0) {
				SoundManager.instance.PlaySingle (SoundManager.instance.playerShoot);
				GameObject shuriken = Instantiate (shurikenPrefab) as GameObject;
				float velModX = 0;
				if(GetComponentInChildren<Rigidbody2D> ().velocity.x * GetComponentInChildren<FlipSprite> ().direction >= 0)
				{
					velModX = GetComponentInChildren<Rigidbody2D> ().velocity.x;
				}

				shuriken.GetComponent<Shuriken> ().veloctiyMod = new Vector2(velModX, GetComponentInChildren<Rigidbody2D> ().velocity.y);

				shuriken.GetComponent<Shuriken> ().normalizedDirection = new Vector3 (GetComponentInChildren<FlipSprite> ().direction, 0, 0);
				shuriken.GetComponent<Shuriken> ().isEnemy = false;	
				shuriken.transform.position = this.transform.position; //+ offset;
				tillFire = fireCooldown;
			}
		}
	}

}
