using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	Rigidbody2D rb;
	public GameObject kickoffSpot;
	public GettingRelegatedFromLeagueOne controller;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Goal")) return;

		GettingRelegatedFromLeagueOne.IncrementScore(1);
		controller.UpdateScoreboard();

		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
		gameObject.transform.position = kickoffSpot.transform.position;
	}
}
