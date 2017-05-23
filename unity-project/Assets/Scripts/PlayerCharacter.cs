using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	private int _health;
	private int _points;
	private float invincibleTime = 1;
	private float timeTillHurt = 0;
	public Texture _healthTexture;
	public float screenPositionX;  
	public float screenPositionY;   
	public int iconSizeX=25;  
	public int iconSizeY=25;  

	void Start() {
		_health = 5;
		_points = 0;
	}
	
	void Update(){
		timeTillHurt -= Time.deltaTime;
	}

	public void Hurt(int damage) {
		if(timeTillHurt <= 0){
		_health -= damage;
		 timeTillHurt = invincibleTime;
		}
		//Debug.Log("Health: " + _health);
	}
	
	public void getPoints(int value) {
		_points += value;
		//Debug.Log("Health: " + _health);
	}
	
		public void getHealth(int value) {
		_health += value;
		//Debug.Log("Health: " + _health);
	}
	
	public bool alive(){
		return (_health > 0);
	}
	void OnGUI(){  
			　　  
		　　      for (int h =0; h < _health; h++) {  
			　　          GUI.DrawTexture(new Rect(screenPositionX + (h*iconSizeX),screenPositionY,iconSizeX,iconSizeY),_healthTexture,ScaleMode.ScaleToFit,true,0);  
				　　      }  
			　　  }  
	}
	/*
	void OnGUI()
	{
		//int w = Mathf.RoundToInt(Screen.width * size), h = Mathf.RoundToInt(Screen.height * size);
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w/2, h * 5 / 100);
		style.alignment = TextAnchor.LowerRight;
		style.fontSize = Mathf.RoundToInt(2 * h * 0.02f);
		//style.normal.textColor = (windowTimer > 0) ? new Color (0.2f, 0.9f, 0.2f, 1.0f) : new Color (0.3f, 0.4f, 0.3f, 1.0f);
		string text = "Health: " + _health + " Score: " + _points;
		GUI.Label(rect, text, style);
	}
	*/
