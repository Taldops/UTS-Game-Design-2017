using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnStartGame (string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}

	public void OnStartGame (int sceneIndex)
	{
		SceneManager.LoadScene (sceneIndex);
	}

}

