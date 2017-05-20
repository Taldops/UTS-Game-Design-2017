using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	private int _health;

	void Start() {
		_health = 5;
	}

	public void Hurt(int damage) {
		_health -= damage;
		Debug.Log("Health: " + _health);
	}
	
	void OnGUI()
	{
		//int w = Mathf.RoundToInt(Screen.width * size), h = Mathf.RoundToInt(Screen.height * size);
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w/2, h * 5 / 100);
		style.alignment = TextAnchor.LowerRight;
		style.fontSize = Mathf.RoundToInt(2 * h * 0.02f);
		//style.normal.textColor = (windowTimer > 0) ? new Color (0.2f, 0.9f, 0.2f, 1.0f) : new Color (0.3f, 0.4f, 0.3f, 1.0f);
		string text = "Health: " + _health;
		GUI.Label(rect, text, style);
	}
}