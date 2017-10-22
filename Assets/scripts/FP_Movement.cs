using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class FP_Movement : MonoBehaviour {

	private Rigidbody rb;
	public float groundDrag = 0.5f;
	public float moveForce = 50;
	public float maxSpeed = 10;
	private int playerIndex = 1;
	private float lsX;
	private float lsY;


	void Awake() {
		rb = GetComponent<Rigidbody>();
		playerIndex = transform.GetComponent<CharStats>().playerNumber;
	}

	void Update(){
		lsX = XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex);
		lsY = XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex);

	}

	// FixedUpdate because physics --> avoid frame-precise actions
	void FixedUpdate() {
		// enables movement relative to the camera angle
		Vector3 forwardDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightDir = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

		rb.AddForce(rightDir * lsX * moveForce / groundDrag);
		rb.AddForce(forwardDir * lsY * moveForce / groundDrag);
			
		// reduces the speed to maxSpeed if it goes above
		if (rb.velocity.magnitude > maxSpeed) {
			//future: addForce
			float yVel = rb.velocity.y;
			rb.velocity = rb.velocity.normalized * maxSpeed;
			rb.velocity = new Vector3(rb.velocity.x, yVel, rb.velocity.z);
		}
		// player looks in movement direction
		if (lsX != 0 && lsY != 0) {
			transform.rotation = Quaternion.LookRotation(rightDir * lsX + forwardDir * lsY);
		}
	}
}


//!! stickMagnitude should generate a constant velocity
