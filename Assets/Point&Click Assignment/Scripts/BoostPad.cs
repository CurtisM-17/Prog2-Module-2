using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
	float timer = 0;
	public float respawnTime = 5;
	float collectedAt = 0;
	bool collected = false;

	SpriteRenderer sr;

	private void Start() {
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		timer += Time.deltaTime;

		if (collected && timer - collectedAt >= respawnTime) {
			collected = false;
			sr.enabled = true;
		}
	}

	public void CollectBoostPad() {
		collected = true;
		collectedAt = timer;

		//gameObject.SetActive(false);
		sr.enabled = false;
	}
}
