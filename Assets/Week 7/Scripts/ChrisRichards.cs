using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChrisRichards : MonoBehaviour
{
	//public GameObject ball;
	//public GameObject goal;

	Rigidbody2D rb;
	SpriteRenderer sr;
	static Color selectedColor;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();

		selectedColor = sr.color;
	}

	private void OnMouseDown()
	{
		Selected(true);
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
}
