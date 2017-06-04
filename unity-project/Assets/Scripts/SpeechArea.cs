using UnityEngine;
using System.Collections;

public class SpeechArea : MonoBehaviour
{
	
	public string message = "";

	void Update ()
	{
	}

	//void OnTriggerEnter2D(Collider2D other) {
	void OnTriggerStay2D (Collider2D other)
	{
		if (other.transform.parent != null) {
			GameObject player = other.transform.parent.Find ("SpeechBubble").gameObject;
			if (player != null) {
				//player.Hurt(damage);
				player.SetActive (true);
				GameObject Message = player.transform.Find ("Message").gameObject;
				message = message.Replace ("\\n", "\n");
				Message.GetComponent<TextMesh> ().text = message;
			}
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.transform.parent != null) {
			GameObject player = other.transform.parent.Find ("SpeechBubble").gameObject;
			if (player != null) {
				//player.Hurt(damage);
				player.SetActive (false);
				GameObject Message = player.transform.Find ("Message").gameObject;
				Message.GetComponent<TextMesh> ().text = "";
			}
		}
	}

}

