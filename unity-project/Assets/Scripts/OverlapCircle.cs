using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircle : MonoBehaviour {

	[HideInInspector] public bool overlaps;
	public string[] tagList;

	private int mask;
	private float radius;

	void Awake()
	{
		mask = LayerMask.GetMask(tagList);
		radius = GetComponent<CircleCollider2D>().radius;
	}

	void Update()
	{
		//Having it here instead of in fixed update makes it more precise?
		updateArea();
		overlaps = Physics2D.OverlapCircle(transform.position, radius, mask);
	}

	/*
	 * The detection area NEVER CHANGES unless this is called
	*/
	public void updateArea()
	{
		radius = GetComponent<CircleCollider2D>().radius;
	}
}
