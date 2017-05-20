using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
	public float speed = 10f;
	public Vector3 secondLocation = new Vector3(0,0,0);
	public bool moveUp = true;
	public float moveTime = 3;
	public float movedSoFar;
	
	
	void Start() {
		//_health = 5;
		movedSoFar = moveTime;
	}
	
	
	void Update() {
		//transform.Translate(speed * Time.deltaTime, 0, 0);
		if(moveUp){
		transform.Translate(new Vector3(0,1,0) * (speed * Time.deltaTime));
		}
		else {
		transform.Translate(new Vector3(0,-1,0) * (speed * Time.deltaTime));
		}
		movedSoFar -= Time.deltaTime;
		if(movedSoFar <= 0){
			moveUp = !moveUp;
			movedSoFar = moveTime;
		}
		//this.transform.position - 
		//timeout -= Time.deltaTime;
			
		}
	}
	

