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

	private float startTime;
	private float completionTime;
	private int coinTotal;
	private int coinsCollected;

	// Use this for initialization
	void Awake () {
		player = GameObject.Find("Hero");
		//startingPos = player.transform.position;
		startTime = Time.realtimeSinceStartup;
		coinTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        return LevelTime <= 0;
    }

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.transform.root.tag == "Player")
		{
			completed = true;
			completionTime = Time.realtimeSinceStartup - startTime;
			coinsCollected = coinTotal - GameObject.FindGameObjectsWithTag("Coin").Length;
		}
	}

	void OnGUI()
	{
		//int w = Mathf.RoundToInt(Screen.width * size), h = Mathf.RoundToInt(Screen.height * size);
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();
        GUIStyle styletwo = new GUIStyle();
		GUIStyle stylethree = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperRight;
		style.fontSize = Mathf.RoundToInt(2 * h * 0.03f);
		style.normal.textColor = completed ? Color.green : new Color(0.2f, 0, 0, 1);

        Rect recttwo = new Rect(0, 0, w, h);
        styletwo.alignment = TextAnchor.MiddleCenter;
        styletwo.fontSize = Mathf.RoundToInt(h / 5);

		Rect rectthree = new Rect(0, h * 0.45f, w, h * 0.5f);
		stylethree.alignment = TextAnchor.MiddleCenter;
		stylethree.fontSize = Mathf.RoundToInt(2 * h * 0.02f);
		stylethree.normal.textColor = completed ? Color.green : Color.black;
        
		string text = "Countdown: ";
        text += string.Format("{0:0} s", LevelTime);

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
			string textthree = string.Format("Completion Time: {0:0.0} s", completionTime);
			if(coinTotal > 0)
			{
				textthree += string.Format(	"\nCoins Collected: {0:0}/{1:0} ({2:0}%)", coinsCollected, coinTotal, (coinsCollected * 100)/coinTotal);
			}
			GUI.Label(rectthree, textthree, stylethree);
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
