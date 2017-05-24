/*
 * Checks whether the collider overlaps with colliders of gameobjects of the specefied tags.
 * The Collider should be disabled, because THIS SCRIPT WILL DISABLE THE COLLIDER.
 * The Collider itself is needed to make programming and visual reference more intuitive
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCheck : MonoBehaviour {

	[HideInInspector] public bool overlaps;
	public string[] tagList;

	public bool showDetector;

	private Transform topLeft;
	private Transform bottomRight;
	private int mask;
	private BoxCollider2D collider;

	void Awake()
	{
		topLeft = new GameObject().transform;
		bottomRight = new GameObject().transform;
		topLeft.parent = transform;
		bottomRight.parent = transform;
		mask = LayerMask.GetMask(tagList);
		collider = GetComponent<BoxCollider2D>();
		collider.enabled = false;
	}

	void Update()
	{
		//Having it here instead of in fixed update makes it more precise?
		updateArea();
		overlaps = Physics2D.OverlapArea(topLeft.position, bottomRight.position, mask);
		if(showDetector)
		{
			Debug.DrawLine(new Vector3(topLeft.position.x, topLeft.position.y, 0), new Vector3(bottomRight.position.x, topLeft.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(bottomRight.position.x, topLeft.position.y, 0), new Vector3(bottomRight.position.x, bottomRight.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(bottomRight.position.x, bottomRight.position.y, 0), new Vector3(topLeft.position.x, bottomRight.position.y, 0), Color.red, 0, false);
			Debug.DrawLine(new Vector3(topLeft.position.x, bottomRight.position.y, 0), new Vector3(topLeft.position.x, topLeft.position.y, 0), Color.red, 0, false);
		}
	}

	/*
	 * The detection area NEVER CHANGES unless this is called
	*/
	public void updateArea()
	{
		topLeft.position = (Vector2) this.transform.position - (0.5f * Vector2.Scale(collider.size, (Vector2) transform.lossyScale));
		bottomRight.position = (Vector2) this.transform.position + (0.5f * Vector2.Scale(collider.size, (Vector2) transform.lossyScale));
	}


}
