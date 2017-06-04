using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public UIManager UI;
	private GameObject player;

	public void Start ()
	{
		UI.GetComponentInChildren<Canvas> ().enabled = false;
		Time.timeScale = 1.0f;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void TogglePauseMenu ()
	{
		if (UI.GetComponentInChildren<Canvas> ().enabled) 
		{
			UI.GetComponentInChildren<Canvas> ().enabled = false;
			Time.timeScale = 1.0f;
			player.GetComponent<PlayerControl>().enabled = true;
		} 
		else 
		{
			UI.GetComponentInChildren<Canvas> ().enabled = true;
			Time.timeScale = 0f;
			player.GetComponent<PlayerControl>().enabled = false;
		}
	}
}
