using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRockets : MonoBehaviour
{
	public float lifetime = 5f;
	public float speed = 8f;
	public float planetDamage = 5f;
	public float playerDamage = 10f;
	Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();

		Destroy(gameObject, lifetime);
	}

	private void FixedUpdate() {
		rb.MovePosition(speed * Time.deltaTime * (Vector2)transform.up + rb.position);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Planet")) {
			collision.gameObject.SendMessage("Damage", planetDamage); // Damage planet
		} else if (collision.gameObject.CompareTag("Player")) {
			collision.gameObject.SendMessage("IncrementHealth", -playerDamage); // Damage player
		} else return;

		Destroy(gameObject);
	}
}
