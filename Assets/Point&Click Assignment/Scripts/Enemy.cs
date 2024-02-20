using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	Vector3 targetPos;
	public float health = 100f;
	public float speed = 0.5f;
	bool isDead = false;
	float timer;

	Rigidbody2D rb;
	GameObject player;
	SpriteRenderer sr;

	public float attackDistance = 2f;
	public float stoppingDistance = 0.5f;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		sr = GetComponent<SpriteRenderer>();

		targetPos = Vector3.zero; // Planet

		CalculateAngle();
	}

	private void Update() {
		if (isDead) return;

		timer += Time.deltaTime;

		if (targetPos != Vector3.zero) { // Player
			CalculateAngle();
		}

		float distFromTarget = (player.transform.position - transform.position).magnitude;

		// Don't attack the player if far enough from the planet
		if (distFromTarget <= attackDistance && player.transform.position.magnitude < 2) {
			targetPos = player.transform.position;
			CalculateAngle();
		} else if (targetPos != Vector3.zero) {
			targetPos = Vector3.zero;
			CalculateAngle();
		}

		if (targetPos == Vector3.zero) distFromTarget = transform.position.magnitude;

		Shooting(distFromTarget);
	}

	private void FixedUpdate() {
		if (isDead) return;
		if (targetPos == Vector3.zero && (transform.position - targetPos).magnitude < stoppingDistance) return;

		// Move in face direction
		rb.MovePosition(speed * Time.deltaTime * (Vector2)transform.up + rb.position);
	}

	////////// Aim Angle //////////
	void CalculateAngle() {
		Vector3 aimDir = targetPos - transform.position;
		float rotationAngle = -(Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg);

		rb.rotation = rotationAngle;
	}

	////////// Collisions with Player and Rockets //////////
	private void OnCollisionEnter2D(Collision2D collision) {
		if (isDead) return;

		if (collision.gameObject.CompareTag("Player")) {
			//// Player collision; massive damage
			Die();
			collision.gameObject.SendMessage("IncrementHealth", -50);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
	if (!collision.gameObject.CompareTag("PlayerRocket")) return;

		IncrementHealth(-50);
		Destroy(collision.gameObject);
	}

	////////// Health //////////
	public GameObject explosion;
	public GameObject weapons;

	void IncrementHealth(int increment) {
		if (health + increment > 100) health = 100;
		else if (health + increment < 0) health = 0;
		else health += increment;

		if (health == 0) Die();
	}

	void Die() {
		isDead = true;
		sr.enabled = false;
		explosion.SetActive(true);
		weapons.SetActive(false);

		foreach (GameObject bullet in bullets) {
			Destroy(bullet);
		}
		bullets = null;

		Destroy(gameObject, 0.9f);
	}

	////////// Shooting //////////
	public float shootingDistance = 1f;
	public float shotRate = 0.3f;
	float lastShot = 0;
	public GameObject bulletPrefab;

	List<GameObject> bullets = new();

	void Shooting(float distFromTarget) {
		if (distFromTarget > shootingDistance) return;

		if (timer - lastShot >= shotRate) {
			lastShot = timer;

			/// Take shot
			GameObject bullet = Instantiate(bulletPrefab);

			bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
			bullets.Add(bullet);
		}
	}
}
