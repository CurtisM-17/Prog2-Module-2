using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
	Rigidbody2D rb;
	Vector2 destination, aimDir;
	public float maxSpeed = 0.5f;
	public float speed;
	public AnimationCurve faceMouseTransition, acceleration;
	float rotateTimer, speedChangeTimer;
	float rotationAngle;
	public float accelerationSpeed = 1f;
	public float decelerationSpeed = 1f;
	bool decelerating = false;
	float speedChangeDist = 0f;
	bool isBoosting = false;
	float gameTimer = 0;

	int maxHealth = 100;
	public int health;

	public GameObject movingThrusters, boostingThrusters;

	TrailRenderer trail;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		trail = GetComponent<TrailRenderer>();

		destination = transform.position;

		beltSpriteRenderer = rocketBelt.GetComponent<SpriteRenderer>();

		health = maxHealth;

		boost = maxBoost;

		clickIcon.SetActive(true);
		GenerateNewInterval();
	}

	bool lerpingToAimPos = false;
	float startedLerpAtRotation;
	public float rotSpeed = 100f;
	float rotLerpTime;

	public float boostMultiplier = 2f;

	bool showClickIcon = true;
	public GameObject clickIcon;

	private void Update() {
		Movement();

		CheckBoost(); // Boosting

		Shooting(); // Shooting

		HealthPadSpawning();
	}

	///////////// STANDARD MOVEMENT /////////////
	void Movement() {
		destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButton(1)) {
			// Button HOLD

			// Point toward mouse
			aimDir = destination - (Vector2)transform.position;
			rotationAngle = -(Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg);

			movingThrusters.SetActive(true);

			if (Input.GetMouseButtonDown(1)) {
				// Button DOWN
				lerpingToAimPos = true;
				startedLerpAtRotation = rb.rotation;
				speedChangeTimer = 0f;
				speedChangeDist = (maxSpeed - speed);
				decelerating = false;
				showClickIcon = false;
				clickIcon.SetActive(false);

				// Fetch turn time from speed
				float dist = Mathf.Abs(rb.rotation - rotationAngle);
				rotLerpTime = (dist / rotSpeed);
			}

			if (!lerpingToAimPos) rb.rotation = rotationAngle;

			// Accelerate
			if (!isBoosting) {
				if (speed < maxSpeed - 0.01) {
					speedChangeTimer += Time.deltaTime;
					float t = speedChangeDist / accelerationSpeed;

					speed = Mathf.Lerp(0, maxSpeed, acceleration.Evaluate(speedChangeTimer / t));
				} else {
					speed = maxSpeed;
					speedChangeTimer = 0f;
				}
			}

		} else if (Input.GetMouseButtonUp(1)) {
			// Button RELEASED; stop moving
			destination = transform.position;
			speedChangeTimer = 0f;
			decelerating = true;
			speedChangeDist = speed;
			movingThrusters.SetActive(false);

			showClickIcon = true;
			clickIcon.SetActive(true);
		}

		if (decelerating) {
			// Decelerate
			speedChangeTimer += Time.deltaTime;
			float t = speedChangeDist / decelerationSpeed;

			speed = Mathf.Lerp(speedChangeDist, 0, acceleration.Evaluate(speedChangeTimer / t));

			if (speed <= 0.01f) {
				speed = 0f;
				speedChangeTimer = 0;
				decelerating = false;
			}
		}

		// Rotation lerp
		if (lerpingToAimPos) {
			rotateTimer += Time.deltaTime;

			// Fetch distance from rotation point
			float dist = Mathf.Abs(rb.rotation - rotationAngle);

			if (dist >= 0.1f) {
				float t = rotateTimer / (rotLerpTime);

				float curve = faceMouseTransition.Evaluate(t);

				float lerpedValue = Mathf.Lerp(startedLerpAtRotation, rotationAngle, curve);

				rb.rotation = lerpedValue;
			} else {
				// Stop lerping
				rotateTimer = 0;
				lerpingToAimPos = false;
			}
		}

		// Click icon
		if (showClickIcon) {
			clickIcon.transform.position = destination + new Vector2(0.1f, 0.05f);
		}
	}

	///////////// BOOSTING /////////////
	float maxBoost = 100;
	public float boostDeplenishRate = 0.5f;
	public Slider boostMeter;
	float boost;

	void CheckBoost() {
		if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !decelerating && speed > 0 && boost > 0) {
			if (!isBoosting) {
				movingThrusters.SetActive(false);
				boostingThrusters.SetActive(true);

				speed = maxSpeed * boostMultiplier;
				trail.emitting = true;
				isBoosting = true;
			}

			if (boost > boostDeplenishRate)
				boost -= boostDeplenishRate;
			else boost = 0;
		} else {
			if (isBoosting) {
				boostingThrusters.SetActive(false);
				trail.emitting = false;
				isBoosting = false;

				if (speed > maxSpeed) speed = maxSpeed;
			}
		}

		boostMeter.value = boost;
	}

	public bool ReplenishBoost(int amount) {
		if (boost >= maxBoost) return false;

		if (boost + amount > maxBoost) boost = maxBoost;
		else boost += amount;

		return true;
	}

	///////////// SHOOTING /////////////
	float lastShot = 0;
	float lastReload = 0;
	public float fireRate = 0.1f;
	public float reloadRate = 0.5f;
	public GameObject rocket, rocketBelt;
	SpriteRenderer beltSpriteRenderer;
	public Sprite[] rocketBeltStates;
	public GameObject[] rocketSpawnPositions;

	int loadedRockets = 6;

	void Shooting() {
		gameTimer += Time.deltaTime;

		// Respawning
		if (loadedRockets < 6) {
			if (gameTimer - lastShot >= reloadRate*2 && gameTimer - lastReload >= reloadRate) {
				lastReload = gameTimer;
				loadedRockets++;
				UpdateBeltGraphic();
			}
		}

		if (!Input.GetMouseButton(0)) return;
		if (gameTimer - lastShot < fireRate) return;
		if (loadedRockets <= 0) return;

		lastShot = gameTimer;

		GameObject newRocket = Instantiate(rocket);
		GameObject spawnPos = rocketSpawnPositions[loadedRockets - 1];

		newRocket.transform.SetPositionAndRotation(spawnPos.transform.position, spawnPos.transform.rotation);

		loadedRockets--;
		UpdateBeltGraphic();
	}

	public Slider ammoMeter;
	void UpdateBeltGraphic() {
		if (loadedRockets > 0) {
			beltSpriteRenderer.enabled = true;
			beltSpriteRenderer.sprite = rocketBeltStates[loadedRockets - 1];
		} else beltSpriteRenderer.enabled = false;

		ammoMeter.value = loadedRockets;
	}

	///////////// HEALTH /////////////
	public Slider healthBar;
	public void IncrementHealth(int increment) {
		if (health + increment > maxHealth) health = maxHealth;
		else if (health + increment < 0) health = 0;
		else health += increment;

		healthBar.value = health;
	}

	private void FixedUpdate() {
		// Move in face direction
		rb.MovePosition(speed * Time.deltaTime * (Vector2)transform.up + rb.position);
	}

	///////////// POWERUPS /////////////
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.layer != 9) return; // Powerups only
		if (!collision.gameObject.GetComponent<SpriteRenderer>().enabled) return;

		if (collision.gameObject.CompareTag("HealthPickup")) {
			if (health == maxHealth) return;

			IncrementHealth(25);
			Destroy(collision.gameObject);
		}
		else if (collision.gameObject.CompareTag("BoostPickup")) {
			if (boost == 100) return;

			ReplenishBoost(100);
			collision.gameObject.SendMessage("CollectBoostPad", SendMessageOptions.DontRequireReceiver);
		}
	}

	public float minHPInterval = 5f;
	public float maxHPInterval = 15f;
	float lastHPSpawn = 0;
	float currentHPInterval = 0;
	public GameObject healthPadPrefab;

	public float minHPDistance = 1f;
	public float maxHPDistance = 3f;

	public float healthPadLifetime = 90;

	void HealthPadSpawning() { // Every frame
		if (gameTimer - lastHPSpawn >= currentHPInterval) {
			lastHPSpawn = gameTimer;
			GenerateNewInterval();

			SpawnHealthPad();
		}
	}

	void SpawnHealthPad() {
		Vector3 pos = GeneratePadPosition();

		GameObject pad = Instantiate(healthPadPrefab, pos, healthPadPrefab.transform.rotation);

		Destroy(pad, healthPadLifetime);
	}

	Vector3 GeneratePadPosition() {
		Vector3 pos = new Vector3(Random.Range(-maxHPDistance, maxHPDistance), Random.Range(-maxHPDistance, maxHPDistance), 0) + transform.position;

		if ((transform.position - pos).magnitude < minHPDistance) {
			return GeneratePadPosition();
		}

		return pos;
	}

	void GenerateNewInterval() {
		currentHPInterval = Random.Range(minHPInterval, maxHPInterval);
	}
}
