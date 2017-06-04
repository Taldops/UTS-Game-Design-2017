using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Return : MonoBehaviour {
	
	void Start(){
	}

	public void back(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void back(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

}