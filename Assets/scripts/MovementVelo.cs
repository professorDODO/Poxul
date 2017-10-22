using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class MovementVelo : MonoBehaviour {

	// Quaternion turning = Quaternion.Euler(0, cam.transform.EulerAngles.y, 0);
	// Vector3 curVelo = cam.transform.InverseTransformVector(rb.velocity);
	// rb. += turning * transform.forward * acceleration;

	private Rigidbody rb;
	public float playerIndex;
	public float groundDrag;
	public float acceleration;
	public float maxSpeed;

	float lsX;			// LeftStickX
	float lsY;			// LeftStickY
	Vector3 inDir;
	Vector3 rightDir;
	Vector3 forwardDir;
	Vector3 modInDir;

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}
		
	void Update() {
		lsX = XCI.GetAxis (XboxAxis.LeftStickX, (XboxController)playerIndex);
		lsY = XCI.GetAxis (XboxAxis.LeftStickY, (XboxController)playerIndex);
		inDir = new Vector3 (lsX, 0, lsY);
		// enables movement relative to the camera angle
		rightDir = new Vector3 (Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		forwardDir = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		modInDir = Quaternion.Euler (0, Camera.main.transform.eulerAngles.y, 0) * inDir;
		if (lsX == 0 && lsY == 0) {
			rb.velocity = new Vector3 (rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
		}
		if (Mathf.Abs (rb.velocity.magnitude * Mathf.Cos (Vector3.Angle (rb.velocity, modInDir) * Mathf.PI / 180)) >= maxSpeed) {
			inDir = Vector3.zero;
		}
		rb.velocity += modInDir * acceleration / groundDrag * Time.deltaTime;
		Debug.Log (rb.velocity.magnitude * Mathf.Cos (Vector3.Angle (rb.velocity, modInDir) * Mathf.PI / 180));
	}
}
