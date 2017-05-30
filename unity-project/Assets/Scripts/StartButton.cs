using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnStartGame(string sceneName)
    {
               Application.LoadLevel(sceneName);
             }
     public void OnStartGame(int sceneIndex)
    {
               Application.LoadLevel(sceneIndex);
            }
 }

