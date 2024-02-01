using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing : MonoBehaviour
{
	public int playerScore = 0;

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.gameObject.layer != 3) return; // Planes only
		if (!GetComponent<BoxCollider2D>().OverlapPoint(collision.gameObject.transform.position)) return; // Only trigger is inside collider

		Plane planeScript = collision.gameObject.GetComponent<Plane>();

		if (planeScript.isLanding) return;
		planeScript.isLanding = true;

		playerScore++;

		Debug.Log(playerScore);
	}
}
