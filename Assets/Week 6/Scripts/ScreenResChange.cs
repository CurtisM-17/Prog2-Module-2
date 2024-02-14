using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResChange : MonoBehaviour
{
	public void ChangeRes(bool fullRes)
	{
		if (fullRes) Screen.SetResolution(1920, 1080, false);
		else Screen.SetResolution(1280, 720, false);

		Debug.Log(Screen.currentResolution);
	}
}
