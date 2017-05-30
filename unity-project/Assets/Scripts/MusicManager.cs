using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class MusicManager : MonoBehaviour {
    public void Start()
    {
        
    }

    public void SetVolume(float val)
	{
		GetComponent<AudioSource>().volume = val;
	}
}