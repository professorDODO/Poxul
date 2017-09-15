using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Movement : MonoBehaviour {

	private Rigidbody rb;
	public float playerIndex;
	public float groundDrag;
	public float moveForce;
	public float maxSpeed;

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}
		
	void Update() {
		Vector3 forwardDir = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightDir = new Vector3 (Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		if (XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex) == 0 && XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex) == 0) {
			rb.velocity = new Vector3 (rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
		} else {
			rb.AddForce (rightDir * XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex) * moveForce / groundDrag);
			rb.AddForce (forwardDir * XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex) * moveForce / groundDrag);
		}
		if (rb.velocity.magnitude > maxSpeed) {
			//future: addForce
			float yVel = rb.velocity.y;
			rb.velocity = rb.velocity.normalized * maxSpeed;
			rb.velocity = new Vector3 (rb.velocity.x, yVel,rb.velocity.z);
		}
	}
}
