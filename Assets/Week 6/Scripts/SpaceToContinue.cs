using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceToContinue : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

			SceneManager.LoadScene(nextSceneIndex);
		}
	}
}
