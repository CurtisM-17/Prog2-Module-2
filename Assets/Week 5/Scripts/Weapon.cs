using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	Rigidbody2D rb;
	public Vector2 velocity = Vector2.right;
	public float speed = 6f;
	public float damage = 2f;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, 6);
	}

	private void FixedUpdate() {
		rb.MovePosition(rb.position + velocity * speed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer != 7) return; // Knight only

		collision.gameObject.SendMessageUpwards("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}
}
