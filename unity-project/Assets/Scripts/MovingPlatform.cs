using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
	public float speed = 10f;
	public Vector3 secondLocation = new Vector3(0,0,0);
	public bool moveRight = true;
	public float moveTime = 3;
	public float movedSoFar;
	
	
	void Start() {
		//_health = 5;
		movedSoFar = moveTime;
	}
	
	
	void Update() {
		//transform.Translate(speed * Time.deltaTime, 0, 0);
		if(moveRight){
		transform.Translate(new Vector3(1,0,0) * (speed * Time.deltaTime));
		}
		else {
		transform.Translate(new Vector3(-1,0,0) * (speed * Time.deltaTime));
		}
		movedSoFar -= Time.deltaTime;
		if(movedSoFar <= 0){
			moveRight = !moveRight;
			movedSoFar = moveTime;
		}
		//this.transform.position - 
		//timeout -= Time.deltaTime;
			
		}
	}
	

