using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Planet : MonoBehaviour
{
	public float health = 100f;
	public Slider healthDisplay;

	SpriteRenderer spriteRenderer;

	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		if (!spriteRenderer.isVisible) ScreenIndicator();
		else if (arrow.activeInHierarchy) arrow.SetActive(false);

		healthDisplay.value = health;
	}

	public void DamagePlanet(float dmg) {
		health -= dmg;

		if (health < 0) {
			health = 0;
			Debug.Log("Dead");
		}
	}

	///////////// OFF-SCREEN INDICATOR /////////////
	public GameObject player;
	public GameObject arrow;
	public float arrowDistFromPlr = 1f;

	void ScreenIndicator() {
		arrow.SetActive(true);

		// Point toward planet
		Vector3 aimDir = transform.position - player.transform.position;

		float rotationAngle = -(Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg);

		arrow.transform.eulerAngles = new Vector3(0, 0, rotationAngle);
		arrow.transform.position = player.transform.position + aimDir.normalized * arrowDistFromPlr;
	}
}
