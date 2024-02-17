using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

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

	public GameObject movingThrusters, boostingThrusters;

	TrailRenderer trail;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		trail = GetComponent<TrailRenderer>();

		destination = transform.position;
	}

	bool lerpingToAimPos = false;
	float startedLerpAtRotation;
	public float rotSpeed = 100f;
	float rotLerpTime;

	public float boostMultiplier = 2f;

	private void Update() {
		///////////// STANDARD MOVEMENT /////////////
		if (Input.GetMouseButton(1)) {
			// Button HOLD
			destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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

		///////////// BOOSTING /////////////
		if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && !decelerating && speed > 0) {
			movingThrusters.SetActive(false);
			boostingThrusters.SetActive(true);

			speed = maxSpeed * boostMultiplier;
			trail.emitting = true;
		} else {
			boostingThrusters.SetActive(false);
			trail.emitting = false;

			if (speed > maxSpeed) speed = maxSpeed;
		}
	}

	private void FixedUpdate() {
		// Move in face direction
		rb.MovePosition(speed * Time.deltaTime * (Vector2)transform.up + rb.position);
	}
}
