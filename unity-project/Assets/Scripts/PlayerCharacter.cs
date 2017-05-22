using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	public Texture _healthTexture;
	private int _points;
	private float invincibleTime = 1;
	private float timeTillHurt = 0;
	public float screenPositionX;  
	public float screenPositionY;   
	public int iconSizeX=25;  
	public int iconSizeY=25;  
	public int _health=5;


	void Start() {
		_points = 0;
	}
	
	void Update(){
		timeTillHurt -= Time.deltaTime;
	}

	public void Hurt(int damage) {
		if (timeTillHurt <= 0) {
			_health -= damage;
			timeTillHurt = invincibleTime;
		}
	}


	
	public void getPoints(int value) {
		_points += value;
		//Debug.Log("Health: " + _health);
	}
	

		void OnGUI(){  
			　　  
		　　      for (int h =0; h < _health; h++) {  
			　　          GUI.DrawTexture(new Rect(screenPositionX + (h*iconSizeX),screenPositionY,iconSizeX,iconSizeY),_healthTexture,ScaleMode.ScaleToFit,true,0);  
				　　      }  
			　　  }  
	}
    
