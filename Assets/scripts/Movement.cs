using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Movement : MonoBehaviour {

	private Rigidbody rb;
	public float groundDrag = 0.5f;
	public float moveForce = 50;
	public float maxSpeed = 10;
	public float sneakSpeedFac = 0.5f;
	private int playerIndex = 1;
	private bool sneak;
	private float ssFac;

	public GameObject trajec;

	public int jumpForce = 250;
	public float jumpCharge = 0.5f;
	float lsX;
	float lsY;
	bool jumpPrep;
	bool lastJumpPrep;
	[HideInInspector]
	public bool isGrounded;
	[HideInInspector]
	public float jumpBuildUp;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		playerIndex = transform.GetComponent<CharStats>().playerNumber;
		sneak = false;
		ssFac = 1;
	}

	void Update(){
		lsX = XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex);
		lsY = XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex);
		if (XCI.GetButtonUp (XboxButton.LeftStick, (XboxController)playerIndex)) {
			sneak = !sneak;
		}
		jumpPrep = XCI.GetButton (XboxButton.A, (XboxController)playerIndex);
		if(jumpPrep && jumpBuildUp < jumpForce){
			jumpBuildUp += jumpForce * jumpCharge * Time.deltaTime;
			//trajec.GetComponent<Trajectory>().RenderTrajectory ();
		}
	}

	// FixedUpdate because physics --> avoid frame-precise actions
	void FixedUpdate() {
		// enables movement relative to the camera angle
		Vector3 forwardDir = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightDir = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		// reduces the maxspeed to sneakspeed
		if (sneak) {
			ssFac = sneakSpeedFac;
		} else {
			ssFac = 1;
		}
			debugGUI("sneak P" + playerIndex.ToString(), sneak ? 1 : 0);
		// jump
		if (lastJumpPrep && !jumpPrep && isGrounded) {
			rb.AddForce (new Vector3 (0, jumpBuildUp, 0));
			jumpBuildUp = 0;
		} else if(isGrounded){
			// if there is no input, the player "slides" till it stops
				if (lsX == 0 && lsY == 0) {
				rb.velocity = new Vector3(rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
			} else {
				// ḿoving in given direction
				rb.AddForce(rightDir * lsX * moveForce / groundDrag);
				rb.AddForce(forwardDir * lsY * moveForce / groundDrag);
			}
			// reduces the speed to maxSpeed if it goes above
			if (rb.velocity.magnitude > maxSpeed * ssFac) {
				//future: addForce
				float yVel = rb.velocity.y;
				rb.velocity = rb.velocity.normalized * maxSpeed * ssFac;
				rb.velocity = new Vector3(rb.velocity.x, yVel, rb.velocity.z);
			}
			// player looks in movement direction
			if (lsX != 0 && lsY != 0) {
				transform.rotation = Quaternion.LookRotation(rightDir * lsX + forwardDir * lsY);
			}
		}
		lastJumpPrep = jumpPrep;
		Debug.Log (isGrounded);
	}

	void debugGUI(string element, float value) {
		GameObject.Find("GUI").GetComponent<debugGUI>().debugElement(element, value);
	}

}


//!! stickMagnitude should generate a constant velocity
