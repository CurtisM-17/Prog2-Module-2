using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	public List<Vector2> points;
	public float pointThreshold = 0.2f;
	Vector2 lastPos;
	LineRenderer lr;

	private void Start()
	{
		lr = GetComponent<LineRenderer>();
		lr.positionCount = 1;
		lr.SetPosition(0, transform.position);
	}

	private void OnMouseDown()
	{
		points = new List<Vector2>();

		lr.positionCount = 1;
		lr.SetPosition(0, transform.position);
	}

	private void OnMouseDrag()
	{
		Vector2 currentPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Vector2.Distance(currentPos, lastPos) > pointThreshold)
		{
			points.Add(currentPos);

			lr.positionCount++;
			lr.SetPosition(lr.positionCount - 1, currentPos);

			lastPos = currentPos;
		}
	}
}
