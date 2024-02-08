using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
	public GameObject weaponPrefab;

	public void SpawnWeapon() {
		Instantiate(weaponPrefab);
	}
}
