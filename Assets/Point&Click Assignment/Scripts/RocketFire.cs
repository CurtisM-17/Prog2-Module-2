using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : MonoBehaviour
{
	public float lifetime = 5f;
	public float speed = 10f;
	Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();

		Destroy(gameObject, lifetime);
	}

	private void FixedUpdate() {
		rb.MovePosition(speed * Time.deltaTime * (Vector2)transform.up + rb.position);
	}
}
