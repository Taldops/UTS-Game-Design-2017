using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public GameManager GM;
	public MusicManager MM;

	private Slider _musicSlider;
	void Start ()
	{
		_musicSlider = GameObject.Find("Music_Slider").GetComponent<Slider>();
	}
		
	void Update ()
	{
		ScanForKeyStroke();
	}

	void ScanForKeyStroke()
	{
		if (Input.GetKeyDown("escape"))     GM.TogglePauseMenu();
	}
		
	public void MusicSliderUpdate(float val)
	{
		MM.SetVolume(val);
	}

	public void MusicToggle(bool val)
	{
		_musicSlider.interactable = val;
		MM.SetVolume(val ? _musicSlider.value : 0f);
	}
}
