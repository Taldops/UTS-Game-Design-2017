using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCheck : MonoBehaviour {

	[HideInInspector] public bool overlaps;
	public int mask = 0;

	public bool showDetector;

	private Transform topLeft;
	private Transform bottomRight;

	void Awake()
	{
		topLeft = new GameObject().transform;
		bottomRight = new GameObject().transform;
		topLeft.parent = transform;
		bottomRight.parent = transform;
		topLeft.position = (Vector2) this.transform.position - (0.5f * Vector2.Scale(GetComponent<BoxCollider2D>().size, (Vector2) transform.lossyScale));
		bottomRight.position = (Vector2) this.transform.position + (0.5f * Vector2.Scale(GetComponent<BoxCollider2D>().size, (Vector2) transform.lossyScale));
		mask = (mask == 0) ? LayerMask.GetMask("Ground") : mask;
	}

	void Update()
	{
		//Having it here instead of in fixed update makes it more precise?
		overlaps = Physics2D.OverlapArea(topLeft.position, bottomRight.position, mask);
		if(showDetector)
		{
			Debug.DrawLine(new Vector3(topLeft.position.x, topLeft.position.y, 0), new Vector3(bottomRight.position.x, topLeft.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(bottomRight.position.x, topLeft.position.y, 0), new Vector3(bottomRight.position.x, bottomRight.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(bottomRight.position.x, bottomRight.position.y, 0), new Vector3(topLeft.position.x, bottomRight.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(topLeft.position.x, bottomRight.position.y, 0), new Vector3(topLeft.position.x, topLeft.position.y, 0), Color.red, 0, false);
		}
	}

	void FixedUpdate()
	{
		/*
		overlaps = Physics2D.OverlapArea(topLeft.position, bottomRight.position, mask);
		if(showDetector)
		{
			Debug.DrawLine(new Vector3(topLeft.position.x, topLeft.position.y, 0), new Vector3(bottomRight.position.x, topLeft.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(bottomRight.position.x, topLeft.position.y, 0), new Vector3(bottomRight.position.x, bottomRight.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(bottomRight.position.x, bottomRight.position.y, 0), new Vector3(topLeft.position.x, bottomRight.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(topLeft.position.x, bottomRight.position.y, 0), new Vector3(topLeft.position.x, topLeft.position.y, 0), Color.red, 0, false);
		}
		*/
	}

	/*
	 * The detection area NEVER CHANGES unless this is called
	*/
	public void updateArea()
	{
		topLeft.position = (Vector2) this.transform.position - (0.5f * Vector2.Scale(GetComponent<BoxCollider2D>().size, (Vector2) transform.lossyScale));
		bottomRight.position = (Vector2) this.transform.position + (0.5f * Vector2.Scale(GetComponent<BoxCollider2D>().size, (Vector2) transform.lossyScale));
	}


}
