/*
 * Use this script for debugging
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScript : MonoBehaviour {

	public KeyCode slower;		//Halves timescale
	public KeyCode faster;		//Doubles timescale
	public KeyCode normal;		//Sets timescale to 1
	public KeyCode pause;		//Sets timescale to 0 and back to the last value

	private float lastScale = 1;

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(slower))
		{
			Time.timeScale *= 0.5f;
			if (Time.timeScale == 0)
			{
				lastScale *= 0.5f;
			}
		}
		if(Input.GetKeyDown(faster))
		{
			Time.timeScale *= 2.0f;
			if (Time.timeScale == 0)
			{
				lastScale *= 2f;
			}
		}
		if(Input.GetKeyDown(normal))
		{
			Time.timeScale = 1;
		}	
		if(Input.GetKeyDown(pause))
		{
			//UnityEditor.EditorApplication.isPaused = true;
			if(Time.timeScale > 0)
			{
				lastScale = Time.timeScale;
				Time.timeScale = 0;
			}
			else
			{
				Time.timeScale = lastScale;
			}
		}	

	}

}
