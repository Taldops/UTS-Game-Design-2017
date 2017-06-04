using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour {
	void Start(){
	}
	public void back(string sceneName)
	{
		Application.LoadLevel(sceneName);
	}
	public void back(int sceneIndex)
	{
		Application.LoadLevel(sceneIndex);
	}
}