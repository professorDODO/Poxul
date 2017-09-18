using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;


public class FP_Movement : MonoBehaviour {

	private Rigidbody rb;
	public float playerIndex = 1;
	public float groundDrag;
	public float moveForce = 50;
	public float maxSpeed = 10;
	
	void Awake() { rb = GetComponent<Rigidbody>(); }


	void Update() {
		Vector3 forwardDir = new Vector3 (0,0,1);
		Vector3 rightDir = new Vector3 (1,0,0);
		float lsX = XCI.GetAxis (XboxAxis.RightStickX, (XboxController)playerIndex);
		float lsY = XCI.GetAxis (XboxAxis.RightStickY, (XboxController)playerIndex);

		if (XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex) == 0 && XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex) == 0) {
			rb.velocity = new Vector3 (rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
		} else {
			rb.AddForce (rightDir * lsX * moveForce / groundDrag);
			rb.AddForce (forwardDir * lsY * moveForce / groundDrag);
		}
		if (rb.velocity.magnitude > maxSpeed) {
			//future: addForce
			float yVel = rb.velocity.y;
			rb.velocity = rb.velocity.normalized * maxSpeed;
			rb.velocity = new Vector3 (rb.velocity.x, yVel,rb.velocity.z);
		}
		if (lsX != 0 && lsY != 0) {
			transform.rotation = Quaternion.LookRotation (rightDir * lsX + forwardDir * lsY);
		}
	}
}
