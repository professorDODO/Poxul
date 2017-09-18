using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Movement : MonoBehaviour {

	private Rigidbody rb;
	public float playerIndex = 1;
	public float groundDrag;
	public float moveForce = 50;
	public float maxSpeed = 10;
	public float sneakSpeedFac = 0.5f;
	[HideInInspector] public bool sneak;
	private float ssFac;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		sneak = false;
		ssFac = 1;
	}
		
	void Update() {
		Vector3 forwardDir = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightDir = new Vector3 (Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		float lsX = XCI.GetAxis (XboxAxis.LeftStickX, (XboxController)playerIndex);
		float lsY = XCI.GetAxis (XboxAxis.LeftStickY, (XboxController)playerIndex);
		if (XCI.GetButtonUp (XboxButton.LeftStick, (XboxController)playerIndex)) {
			sneak = !sneak;
			if (sneak) {
				ssFac = sneakSpeedFac;
			} else {
				ssFac = 1;
			}
			Debug.Log ("Sneak: " + sneak);
		}

		if (XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex) == 0 && XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex) == 0) {
			rb.velocity = new Vector3 (rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
		} else {
			rb.AddForce (rightDir * lsX * moveForce / groundDrag);
			rb.AddForce (forwardDir * lsY * moveForce / groundDrag);
		}
		if (rb.velocity.magnitude > maxSpeed * ssFac) {
			//future: addForce
			float yVel = rb.velocity.y;
			rb.velocity = rb.velocity.normalized * maxSpeed * ssFac;
			rb.velocity = new Vector3 (rb.velocity.x, yVel,rb.velocity.z);
		}
		if (lsX != 0 && lsY != 0) {
			transform.rotation = Quaternion.LookRotation (rightDir * lsX + forwardDir * lsY);
		}
	}
}

//!! stickMagnitude should generate a constant velocity
