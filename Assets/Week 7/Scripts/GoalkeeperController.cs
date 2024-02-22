using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperController : MonoBehaviour
{
	public GameObject keeper;
	public float distanceOffGoalLine = 0.3f;
	Rigidbody2D rb;

	private void Start() {
		rb = keeper.GetComponent<Rigidbody2D>();

		rb.MovePosition(transform.position - new Vector3(0, distanceOffGoalLine, 0));
	}

	private void Update() {
		if (!GettingRelegatedFromLeagueOne.SelectedPlr) return;

		GameObject plr = GettingRelegatedFromLeagueOne.SelectedPlr.gameObject;

		Vector3 difference = (plr.transform.position - transform.position);
		float distance = difference.magnitude;

		//Debug.Log("hi " + Mathf.Max(distanceOffGoalLine, distance / 2));

		Vector3 keeperPos = transform.position + (difference.normalized * Mathf.Min(distanceOffGoalLine, distance / 2));
		
		rb.MovePosition(keeperPos);

	}
}
