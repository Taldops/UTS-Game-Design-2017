using UnityEngine;
using System.Collections;

public class ShootingPlayer : MonoBehaviour {
	//public float speed = 3.0f;
	//public float obstacleRange = 5.0f;
	
	[SerializeField] private GameObject shurikenPrefab;
	private GameObject _shuriken;
	Vector3 offset = new Vector3(0, 30, 0);
	float fireRate = 0.5f;
	float tillFire = 0;
	
	//private bool _alive;
	
	void Start() {
		//_alive = true;
	}
	
	void Update() {
		tillFire -= Time.deltaTime;
		if (Input.GetAxis("Fire1") > 0){
			if(tillFire <= 0){
				if (_shuriken == null) {
					SoundManager.instance.PlaySingle(SoundManager.instance.playerShoot);
					_shuriken = Instantiate(shurikenPrefab) as GameObject;
		            //GetComponent<Rigidbody>();
		            _shuriken.GetComponent<Shuriken>().veloctiyMod = GetComponentInChildren<Rigidbody2D>().velocity;

		            _shuriken.GetComponent<Shuriken>().normalizedDirection = new Vector3(GetComponentInChildren<FlipSprite>().direction, 0 ,0);
					_shuriken.GetComponent<Shuriken>().isEnemy = false;	
					_shuriken.transform.position = this.transform.position; //+ offset;
					tillFire = fireRate;
				}
			}
		}
	}

/*
	public void SetAlive(bool alive) {
		_alive = alive;
	}
	*/
}
