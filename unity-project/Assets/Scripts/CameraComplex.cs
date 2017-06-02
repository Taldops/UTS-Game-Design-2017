/*
 * WORK IN PROGRESS! DO NOT USE RIGHT NOW!
 * */

using UnityEngine;
using System.Collections;

public class CameraComplex : MonoBehaviour 
{
	/*
	 * Commenting everything to get rid of warnings. Uncomment if you want to work on this.
	 * */
	/*
	public GameObject subject;		//Which gameobject to follow

	private Camera cam;
	private float standardZoom;

	//parameters
	public float xSmooth = 0.8f;		// How fast the camera catches up with it's target movement in the x axis.
	public float ySmooth = 0.8f;		// How fast the camera catches up with it's target movement in the y axis.
	public float speedZoomFactor = 0.2f;
	public float speeZoomThreshY = 40.0f;
	public float speeZoomThreshX = 10.0f;
	public float zoomSmooth = 2.0f;
	public float zoomReturnFactor = 3.0f;

	void Awake()
	{
		cam = GetComponent<Camera>();
		standardZoom = cam.orthographicSize;
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
		Vector2 subjVel = subject.GetComponent<Rigidbody2D>().velocity;
		float direction = (Mathf.Abs(subjVel.x) > Mathf.Abs(subjVel.y)) ? subjVel.x : subjVel.y;
		float distanceTurbo = 0.5f * Vector2.Distance(transform.position, subject.transform.position) * Vector2.Distance(transform.position, subject.transform.position);
		float zoomTarget = cam.orthographicSize;
		if(Mathf.Abs(subjVel.x) < speeZoomThreshX && Mathf.Abs(subjVel.x) < speeZoomThreshY && Mathf.Abs(subjVel.y) < speeZoomThreshX && Mathf.Abs(subjVel.y) < speeZoomThreshY)
		{
			zoomTarget = standardZoom;
		}

		if(Mathf.Abs(subjVel.x) > Mathf.Abs(subjVel.y) && Mathf.Abs(subjVel.x) > speeZoomThreshX)
		{
			zoomTarget = standardZoom + speedZoomFactor * (Mathf.Abs(subjVel.x) - speeZoomThreshX);
		}
		if(Mathf.Abs(subjVel.y) > Mathf.Abs(subjVel.x) && Mathf.Abs(subjVel.y) > speeZoomThreshY)
		{
			zoomTarget = standardZoom + speedZoomFactor * (Mathf.Abs(subjVel.y) - speeZoomThreshY);
		}

		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomSmooth * Time.deltaTime * ((cam.orthographicSize > zoomTarget) ? zoomReturnFactor : 1));

		float targetX = transform.position.x;
		float targetY = transform.position.y;

		float newX = Mathf.Lerp(transform.position.x, subject.transform.position.x, distanceTurbo * xSmooth * Time.deltaTime);
		float newY = Mathf.Lerp(transform.position.y, subject.transform.position.y, distanceTurbo * ySmooth * Time.deltaTime);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(newX, newY, transform.position.z);
	}
	*/
}
