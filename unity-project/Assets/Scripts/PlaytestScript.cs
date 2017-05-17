using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytestScript : MonoBehaviour {

	public float time;		//Playing time displayed in the upper right corner
	public bool completed = false;	//Has the player reached the goal yet?

	private Vector2 startingPos;

	// Use this for initialization
	void Start () {
		startingPos = (Vector2) transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!completed)
		{
			time += Time.deltaTime;
		}

		//Reset Button
		if(Input.GetKeyDown(KeyCode.R))
		{
			transform.position = startingPos;
			time = 0;
			completed = false;
		}
	}

	void OnGUI()
	{
		//int w = Mathf.RoundToInt(Screen.width * size), h = Mathf.RoundToInt(Screen.height * size);
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = Mathf.RoundToInt(2 * h * 0.02f);
		style.normal.textColor = completed ? Color.green : Color.black;
		string text = string.Format("{0:0.0} s", time);
		GUI.Label(rect, text, style);
	}
}
