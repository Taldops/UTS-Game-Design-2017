/*
 * Modified version of the PlayerControl script from the 2D Platformer sample project
 * */

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public GameObject subject;		//Which gameobject to follow

	//parameters
	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How fast the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How fast the camera catches up with it's target movement in the y axis.
	public Vector2 maxXAndY = new Vector2(float.MaxValue, float.MaxValue);		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY = Vector2.one * float.MinValue;		// The minimum x and y coordinates the camera can have.


	void Awake()
	{
		if (maxXAndY == Vector2.zero && minXAndY == Vector2.zero)
		{
			maxXAndY = new Vector2(float.MaxValue, float.MaxValue);		// The maximum x and y coordinates the camera can have.
			minXAndY = Vector2.one * float.MinValue;		// The minimum x and y coordinates the camera can have.
		}
	}

	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - subject.transform.position.x) > xMargin;
	}


	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - subject.transform.position.y) > yMargin;
	}

	void Update ()
	{
		TrackPlayer();
	}

	void FixedUpdate ()
	{
		//TrackPlayer();	//Why would this be here instead of in Update()?
	}
	
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		float distanceTurbo = 0.5f * Vector2.Distance(transform.position, subject.transform.position) * Vector2.Distance(transform.position, subject.transform.position);

		// If the player has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, subject.transform.position.x, distanceTurbo * xSmooth * Time.deltaTime);

		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, subject.transform.position.y, distanceTurbo * ySmooth * Time.deltaTime);

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
