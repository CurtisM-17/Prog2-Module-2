using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Knight : MonoBehaviour
{
	Rigidbody2D rb;
	Animator animator;
	Vector2 destination, movement;
	public float speed = 3f;
	bool clickedOnSelf, isDead;
	public float maxHealth = 5f;
	public float health;
	public Slider slider;

	public float clickDamage = 1;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		health = PlayerPrefs.GetFloat("PlayerHealth", maxHealth);

		SendMessage("TakeDamage", 0f, SendMessageOptions.DontRequireReceiver); // Sync animation and health
	}

	private void Update()
	{
		if (isDead) return;

		if (Input.GetMouseButtonDown(0) && !clickedOnSelf && !EventSystem.current.IsPointerOverGameObject())
		{
			destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		if (Input.GetMouseButtonDown(1)) {
			/// Attack
			animator.SetTrigger("Attack");
		}

		animator.SetFloat("Movement", movement.magnitude);
	}

	private void FixedUpdate()
	{
		if (isDead) return;

		movement = destination - (Vector2)transform.position;

		if (movement.magnitude < 0.1)
		{
			movement = Vector2.zero;
		}

		rb.MovePosition(rb.position + movement.normalized * speed * Time.deltaTime);
	}

	private void OnMouseDown()
	{
		if (isDead) return;

		clickedOnSelf = true;
		gameObject.SendMessage("TakeDamage", clickDamage);
	}

	private void OnMouseUp()
	{
		clickedOnSelf = false;
	}

	public void TakeDamage(float dmg)
	{
		health -= dmg;
		health = Mathf.Clamp(health, 0, maxHealth);
		slider.value = health;

		PlayerPrefs.SetFloat("PlayerHealth", health);

		if (health <= 0)
		{
			isDead = true;
			animator.SetTrigger("Die");
		}
		else
		{
			if (dmg > 0 || isDead) animator.SetTrigger("Hurt");
			isDead = false;
		}
	}
}
