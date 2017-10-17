using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementToMerge : MonoBehaviour {

	private Rigidbody rb;
	public float groundDrag = 0.9f;
	public float moveForce = 50;
	public float maxSpeed = 10;
	private float speedCap;
	public float sneakSpeedFac = 0.5f;
	public float rotationSpeed = 10f;

	private Vector3 forwardDir = Vector3.forward;
	private Vector3 rightDir = Vector3.right;
	private bool sneak;
	private float ssFac;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		sneak = false;
		ssFac = 1;
	}

	public void move(Vector2 inputVec, bool rel2Cam) {
		speedCap = maxSpeed * inputVec.magnitude;
		// enables movement relative to the camera angle
		if (rel2Cam) {
			forwardDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
			rightDir = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		}
		// if there is no input, the player "slides" till it stops
		if (inputVec.magnitude == 0f) {
			rb.velocity = new Vector3(rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
		} else {
			// ḿoving in given direction
			rb.AddForce((rightDir * inputVec.x + forwardDir * inputVec.y).normalized * moveForce / groundDrag);
		}
		// reduces the speed to maxSpeed if it goes above
		if (rb.velocity.magnitude > speedCap * ssFac) {
			//future: addForce
			float yVel = rb.velocity.y;
			rb.velocity = rb.velocity.normalized * speedCap * ssFac;
			rb.velocity = new Vector3(rb.velocity.x, yVel, rb.velocity.z);
		}
	}

	public void rotate(Vector2 inputVec, bool rel2Cam) {
		// enables movement relative to the camera angle
		if (rel2Cam) {
			forwardDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
			rightDir = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		}
		Quaternion aimedRotation = new Quaternion();
		if (inputVec.magnitude != 0f) {
			aimedRotation = Quaternion.LookRotation(rightDir * inputVec.x + forwardDir * inputVec.y);
			transform.rotation = Quaternion.Slerp(transform.rotation, aimedRotation,
			                                     rotationSpeed * Time.deltaTime
			                                     / (Quaternion.Angle(transform.rotation, aimedRotation)));
		}
	}

	// reduces the maxspeed to sneakspeed
	public void initiateSneak() {
		sneak = !sneak;
		if (sneak) {
			ssFac = sneakSpeedFac;
		} else {
			ssFac = 1;
		}
	}

	public void movementDebug() {
		Global.debugGUI("sneak P" + GetComponent<CharStats>().playerNumber.ToString(), sneak ? 1 : 0);
	}
}


//!! stickMagnitude should generate a constant velocity
