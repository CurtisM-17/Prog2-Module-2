using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	public List<Vector2> points;
	public float pointThreshold = 0.1f;
	Vector2 lastPos;
	LineRenderer lr;
	Vector2 currentPos;
	Rigidbody2D rb;
	public float speed = 1f;
	public AnimationCurve landing;
	float landingTimer = 0f;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
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

	private void FixedUpdate()
	{
		currentPos = new Vector2(transform.position.x, transform.position.y);

		if (points.Count > 0)
		{
			Vector2 direction = points[0] - currentPos;
			float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

			rb.rotation = -angle;
		}

		rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.deltaTime);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			landingTimer += 0.1f * Time.deltaTime;
			float lerp = landing.Evaluate(landingTimer);
			if (transform.localScale.z < 0.1f)
			{
				Destroy(gameObject);
			}
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, lerp);
		}

		if (points.Count > 0 && Vector2.Distance(currentPos, points[0]) < pointThreshold)
		{
			points.RemoveAt(0);

			for (int i = 0; i < lr.positionCount - 2; i++)
			{
				lr.SetPosition(i, lr.GetPosition(i+1));
			}
			lr.positionCount--;
		}
	}
}
