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
			this.gameObject.GetComponent<PlayerControl>().GetHit();
			if(_health <= 0){
				this.gameObject.GetComponent<PlayerControl>().Die();
			}
			timeTillHurt = invincibleTime;
		}
		//Debug.Log("Health: " + _health);
	}
	
	public void getPoints(int value) {
		SoundManager.instance.PlaySingle(SoundManager.instance.pickupCoin);
		_points += value;
		//Debug.Log("Health: " + _health);
	}
	
		public void getHealth(int value) {
		SoundManager.instance.PlaySingle(SoundManager.instance.pickupHealth);
		_health += value;
		//Debug.Log("Health: " + _health);
	}
	
	public bool alive(){
		return (_health > 0);
	}
	void OnGUI(){  
			　　  
		　　
		for (int h = 0; h < _health; h++) {  
			　　
			GUI.DrawTexture (new Rect (screenPositionX + (h * iconSizeX), screenPositionY, iconSizeX, iconSizeY), _healthTexture, ScaleMode.ScaleToFit, true, 0);  
		}  
		int w = Screen.width, j = Screen.height;

	}

}