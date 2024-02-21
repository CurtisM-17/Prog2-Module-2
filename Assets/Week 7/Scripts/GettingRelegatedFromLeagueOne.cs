using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GettingRelegatedFromLeagueOne : MonoBehaviour
{
	public Slider chargeSlider;
	float charge;
	public float maxCharge = 1f;
	Vector2 direction;
	public float chargeSpeed = 1f;
	public TextMeshProUGUI scoreDisplay;

	public static ChrisRichards SelectedPlr { get; private set; }
	public static int Score { get; private set; }

	public static void SetSelectedPlayer(ChrisRichards plr)
	{
		if (SelectedPlr)
		{
			SelectedPlr.Selected(false);
		}

		plr.Selected(true);
		SelectedPlr = plr;
	}

	public static void IncrementScore(int inc)
	{
		Score += inc;
	}

	public void UpdateScoreboard()
	{
		scoreDisplay.text = Score.ToString() + " : 0";
	}

	private void FixedUpdate()
	{
		if (direction != Vector2.zero)
		{
			SelectedPlr.Move(direction);
			direction = Vector2.zero;
			charge = 0;
			chargeSlider.value = charge;
		}
	}

	private void Update()
	{
		if (!SelectedPlr) return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			charge = 0;
			direction = Vector2.zero;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			charge += chargeSpeed * Time.deltaTime;
			chargeSlider.value = charge;
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			charge = Mathf.Clamp(charge, 0, maxCharge);

			direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - SelectedPlr.transform.position).normalized * charge;
		}
	}
}
