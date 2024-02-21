using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChrisRichards : MonoBehaviour
{
	Rigidbody2D rb;
	SpriteRenderer sr;
	static Color selectedColor;
	public float speed = 10f;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();

		selectedColor = sr.color;
		Selected(false);
	}

	private void OnMouseDown()
	{
		GettingRelegatedFromLeagueOne.SetSelectedPlayer(this);
	}

	public void Selected(bool isSelected)
	{
		if (isSelected)
		{
			sr.color = Color.green;
		} else
		{
			sr.color = selectedColor;
		}
	}

	public void Move(Vector2 direction)
	{
		rb.AddForce(direction * speed, ForceMode2D.Impulse);
	}
}
