using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaytestScript : MonoBehaviour {

	[HideInInspector] public float time;		//Playing time displayed in the upper right corner
	[HideInInspector] public bool completed = false;	//Has the player reached the goal yet?
    [HideInInspector] public bool failed = false;    //Player has Run out of time

    private GameObject player;
	private Vector3 startingPos;
    float timeTillRestart = 2.0f;
    public float LevelTime = 120.0f;

	// Use this for initialization
	void Awake () {
		player = GameObject.Find("Hero");
		//startingPos = player.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if(completed)
        {
            if (timeTillRestart <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            timeTillRestart -= Time.deltaTime;
        }

        failed = loseTime();
		if(!player.transform.GetComponent<PlayerCharacter>().alive() || failed){
            //playerDead();
            if (timeTillRestart <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            timeTillRestart -= Time.deltaTime;
		}

		//Reset Button
		if(Input.GetKeyDown(KeyCode.R))
		{
			
			
			Application.LoadLevel(Application.loadedLevel);
			time = 0;
			completed = false;
		}
	}
    /*
    // playerDead()
    IEnumerator playerDead()
    {

        yield return new WaitForSeconds(3.0f);
        //yield new WaitForSeconds(2.0f);

    }
    */
    bool loseTime()
    {
        LevelTime -= Time.deltaTime;
        Debug.Log("" + LevelTime);
        return LevelTime <= 0;
    }

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.transform.root.tag == "Player")
		{
			completed = true;
		}
	}

	void OnGUI()
	{
		//int w = Mathf.RoundToInt(Screen.width * size), h = Mathf.RoundToInt(Screen.height * size);
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();
        GUIStyle styletwo = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = Mathf.RoundToInt(2 * h * 0.02f);
		style.normal.textColor = completed ? Color.green : Color.black;

        Rect recttwo = new Rect(0, 0, w, h);
        styletwo.alignment = TextAnchor.MiddleCenter;
        styletwo.fontSize = Mathf.RoundToInt(h / 5);
        

        string text = string.Format("{0:0.0} s", LevelTime);

        if (!player.transform.GetComponent<PlayerCharacter>().alive()) {
            styletwo.normal.textColor = Color.red;
            string texttwo = "You Died";
        GUI.Label(recttwo, texttwo, styletwo);
        }

        if (completed)
        {
            styletwo.normal.textColor = Color.green;
            string texttwo = "You Made It";
            GUI.Label(recttwo, texttwo, styletwo);
        }

        if (failed)
        {
            styletwo.normal.textColor = Color.green;
            string texttwo = "You Ran Out of Time";
            GUI.Label(recttwo, texttwo, styletwo);
        }


        GUI.Label(rect, text, style);
        
    }
}
