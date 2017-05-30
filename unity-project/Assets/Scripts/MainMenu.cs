﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
		　　  
		　　  public Texture backgroundTexture;  
		　　  public GUIStyle random1;  
	　　      public float guiPlacementX1;  
	　　      public float guiPlacementY1;   
		　　  public bool showGUIOutline = false;
		　　  
		　　  void OnGUI(){  
			　　  
			　　      GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundTexture);  
			　　  
			　　      if (showGUIOutline) {  
				　　          if (GUI.Button (new Rect (Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .4f, Screen.height * .1f), "Start Game")) {  
					　　                  print ("Clicked Play Game"); 
			                         	Application.LoadLevel("ChoiceMap");  
					　　          }  

				　　          
				　　  
				　　          
			　　      } else {  
				　　          if (GUI.Button (new Rect (Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .4f, Screen.height * .1f), "", random1)) {  
					　　                  print ("Clicked Play Game");  
				       
					　　          }  
		
				　　          }  
				　　        
			               
				 
			}  
				　　      }  
			　  
		　　    
		　　

