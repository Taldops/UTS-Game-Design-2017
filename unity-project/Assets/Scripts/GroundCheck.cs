using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

	[HideInInspector] public bool grounded;

	public Transform topLeft;
	public Transform bottomRight;

	private int mask;

	void Awake()
	{
		mask = LayerMask.GetMask("Ground");
	}

	void FixedUpdate()
	{
		grounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, mask);
	}

}
