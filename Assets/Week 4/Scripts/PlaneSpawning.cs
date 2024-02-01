using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawning : MonoBehaviour
{
	float timer = 0;
	float lastSpawnTime = 0;
	public float minInterval, maxInterval;
	float activeInterval = 0;

	public GameObject blankPlane;
	public List<Sprite> planePrefabs;
	public GameObject planeContainer;

	private void Start()
	{
		NewInterval();
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (timer - lastSpawnTime >= activeInterval)
		{
			SpawnPlane();
		}
	}

	private void SpawnPlane()
	{
		lastSpawnTime = timer;

		NewInterval();

		// Position
		Vector3 pos = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
		float rot = Random.Range(0, 360);
		float speed = Random.Range(1, 3);

		GameObject plane = Instantiate(blankPlane, pos, Quaternion.Euler(0, 0, rot));

		plane.GetComponent<Plane>().speed = speed;
		plane.GetComponent<SpriteRenderer>().sprite = planePrefabs[Random.Range(0, planePrefabs.Count)];
		plane.transform.localScale = new Vector3(5, 5, 5);
	}

	private void NewInterval()
	{
		activeInterval = Random.Range(minInterval, maxInterval);
	}
}
