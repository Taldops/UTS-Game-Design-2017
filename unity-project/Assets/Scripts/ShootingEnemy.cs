using UnityEngine;
using System.Collections;

public class ShootingEnemy : MonoBehaviour {
	//public float speed = 3.0f;
	//public float obstacleRange = 5.0f;
	
	[SerializeField] private GameObject shurikenPrefab;
	private GameObject _shuriken;
	Vector3 offset = new Vector3(0, 30, 0);
	float fireRate = 3;
	float tillFire = 0;
	
	//private bool _alive;
	
	void Start() {
		//_alive = true;
	}
	
	void Update() {
		//if (_alive) {
		//	transform.Translate(0, 0, speed * Time.deltaTime);
					tillFire -= Time.deltaTime;
					if(tillFire <= 0){
					if (_shuriken == null) {
						_shuriken = Instantiate(shurikenPrefab) as GameObject;
						//GetComponent<Rigidbody>();
						_shuriken.GetComponent<Shuriken>().normalizedDirection = Vector3.Normalize(GameObject.Find("Hero").transform.position - this.transform.position); //Vector3.Normalize(target.Position - character.Position)
						_shuriken.transform.position = this.transform.position; //+ offset;
						//Debug.Log("Fired");
						//_fireball.transform.rotation = transform.rotation;
						tillFire = fireRate;
					}
					}
			
		//}
	}
/*
	public void SetAlive(bool alive) {
		_alive = alive;
	}
	*/
}
