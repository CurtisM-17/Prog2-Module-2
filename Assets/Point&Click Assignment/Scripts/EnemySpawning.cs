using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
	public float minSpawnInterval = 5f;
	public float maxSpawnInterval = 10f;
	float timer = 0;
	float lastSpawn = 0;
	float currentInterval = 0;

	public GameObject enemyPrefab;

	private void Update() {
		timer += Time.deltaTime;

		if (timer - lastSpawn >= currentInterval) {
			lastSpawn = timer;
			NewInterval();
			SpawnEnemy();
		}
	}

	public float maxHorDistance = 5f;
	public float maxVerDistance = 2.5f;
	public float minDistFromCenter = 2f;

	void SpawnEnemy() {
		Vector3 pos = new (Random.Range(-maxHorDistance, maxHorDistance), Random.Range(-maxVerDistance, maxVerDistance), 0);

		if (pos.magnitude < minDistFromCenter) SpawnEnemy(); // Regenerate
		else Instantiate(enemyPrefab, pos, enemyPrefab.transform.rotation);
	}

	void NewInterval() {
		currentInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
	}
}
