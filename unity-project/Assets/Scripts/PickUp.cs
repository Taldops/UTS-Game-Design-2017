using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {
	
	private int value = 100;
    public float secondsToAdd = 5;
	private bool collected = false;

	public AudioClip collectedSound;
	public AudioSource source;

	void Update() {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		//???
//		source.GetComponent<AudioSource> ();
        if (other.transform.parent != null)
        {
            PlayerCharacter player = other.transform.parent.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                if (!collected) {
					
					//Try adding sound
					//source.PlayOneShot(collectedSound);

                    player.getPoints(value);
                    GameObject.Find("Goal Zone").transform.GetComponent<PlaytestScript>().LevelTime += secondsToAdd;
                    collected = true;
                }
                Debug.Log("PlayerPickup");
                Destroy(this.gameObject); 


            }
        }
    }
		
		
	}
	

