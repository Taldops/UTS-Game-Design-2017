using UnityEngine;
using System.Collections;

public class ShootingEnemy : MonoBehaviour {
	//public float speed = 3.0f;
	//public float obstacleRange = 5.0f;

	public float shootRadiusX = 35;	//How close the player needs to be for the enemy to shoot at them
	public float shootRadiusY = 20;	//How close the player needs to be for the enemy to shoot at them

	[SerializeField] private GameObject shurikenPrefab;
	private GameObject _shuriken;
	Vector3 offset = new Vector3(0, 30, 0);
	float fireRate = 3;
	float tillFire = 0;
	
	//private bool _alive;
	private GameObject player; 
	
	void Start() {
		//_alive = true;
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update() {
		tillFire -= Time.deltaTime;
		if(tillFire <= 0){
			if (_shuriken == null && inRange()) {
				SoundManager.instance.PlaySingle(SoundManager.instance.enemyShoot);
				_shuriken = Instantiate(shurikenPrefab) as GameObject;
				_shuriken.GetComponent<SpriteRenderer>().color = Color.red;
				//GetComponent<Rigidbody>();
				_shuriken.GetComponent<Shuriken>().normalizedDirection = Vector3.Normalize(GameObject.Find("Hero").transform.position - this.transform.position);
				_shuriken.GetComponent<Shuriken>().isEnemy = true;					//Vector3.Normalize(target.Position - character.Position)
				_shuriken.transform.position = this.transform.position; //+ offset;
				//Debug.Log("Fired");
				//_fireball.transform.rotation = transform.rotation;
				tillFire = fireRate;
			}
		}
	}

	private bool inRange()
	{
		return (Mathf.Abs(player.transform.position.x - transform.position.x) < shootRadiusX && Mathf.Abs(player.transform.position.y - transform.position.y) < shootRadiusY);
	}
/*
	public void SetAlive(bool alive) {
		_alive = alive;
	}
	*/
}
