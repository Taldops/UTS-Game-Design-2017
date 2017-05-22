using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceMap : MonoBehaviour {
	　　  
	　　  public Texture backgroundTexture;  
	　　  public GUIStyle random1;  
	　　  
	　　  public float guiPlacementX1;  
	　　  public float guiPlacementX2;  
	　　  public float guiPlacementX3;  
	　　  public float guiPlacementX4; 
	      public float guiPlacementX5;  
	　　  public float guiPlacementY1;
	      public float guiPlacementY2;  
	　　  public float guiPlacementY3;
	      public float guiPlacementY4;  
	　　  public float guiPlacementY5;
	　　  public string Level1Name;
	      public string Level2Name;
	      public string Level3Name;
	      public string Level4Name;
	      public string Level5Name;
	　　  public bool showGUIOutline = true;
	  
	　　  
	　　  void OnGUI(){  
		　　  
		　　      GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);  
		　　  
		　　      if (showGUIOutline) {  
			　　          if (GUI.Button (new Rect (Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "LEVEL 1")) {  
				　　                  print ("Clicked LEVEL 1"); 
				Application.LoadLevel(Level1Name);  
				　　          }  
			      if (GUI.Button (new Rect (Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "LEVEL 2")) {  
				　　                  print ("Clicked LEVEL 2");  
				Application.LoadLevel(Level2Name);
				　　          }  
			      if (GUI.Button (new Rect (Screen.width * guiPlacementX3, Screen.height * guiPlacementY3, Screen.width * .5f, Screen.height * .1f), "LEVEL 3")) {  
				　　                  print ("Clicked LEVEL 3");  
				Application.LoadLevel(Level3Name);
				　　          } 
			　　  if (GUI.Button (new Rect (Screen.width * guiPlacementX4, Screen.height * guiPlacementY4, Screen.width * .5f, Screen.height * .1f), "LEVEL 4")) {  
				　　                  print ("Clicked LEVEL 4");  
				Application.LoadLevel(Level4Name);
				　　          } 
			　　  if (GUI.Button (new Rect (Screen.width * guiPlacementX5, Screen.height * guiPlacementY5, Screen.width * .5f, Screen.height * .1f), "LEVEL 5")) {  
				　　                  print ("Clicked LEVEL 5");  
				Application.LoadLevel(Level5Name);
			 	　　          } 
		   
		　　      } else {  
			　　          if (GUI.Button (new Rect (Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "", random1)) {  
				　　                  print ("Clicked LEVEL 1");  
				          if (GUI.Button (new Rect (Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "", random1)) {  
					　　                  print ("Clicked LEVEL 2"); 
					　　          }  
				if (GUI.Button (new Rect (Screen.width * guiPlacementX3, Screen.height * guiPlacementY3, Screen.width * .5f, Screen.height * .1f), "", random1)) {  
					　　                  print ("Clicked LEVEL 3"); 
					　　          }  
				if (GUI.Button (new Rect (Screen.width * guiPlacementX4, Screen.height * guiPlacementY4, Screen.width * .5f, Screen.height * .1f), "", random1)) {  
					　　                  print ("Clicked LEVEL 4"); 
					　　          }  
				if (GUI.Button (new Rect (Screen.width * guiPlacementX5, Screen.height * guiPlacementY5, Screen.width * .5f, Screen.height * .1f), "", random1)) {  
					　　                  print ("Clicked LEVEL 5"); 
					　　          }  
				　　          }  
			　　        


		}  
		　　      }  
	　　  }  
　　    
　　