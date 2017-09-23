using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class MovementVelo : MonoBehaviour {

	// Quaternion turning = Quaternion.Euler(0, cam.transform.EulerAngles.y, 0);
	// Vector3 curVelo = cam.transform.InverseTransformVector(rb.velocity);
	// rb. += turning * transform.forward * acceleration;

	private Rigidbody rb;
	public GameObject cam;
	public float playerIndex;
	public float groundDrag;
	public float acceleration;
	public float maxSpeed;

	float lsX;			// LeftStickX
	float lsY;			// LeftStickY
	//Vector3 dir;
	//Vector3 forwardDir;
	//Vector3 rightDir;

	void SetDirection(float axis, Vector3 camDir){
		float vel = rb.velocity.magnitude * Mathf.Cos (Vector3.Angle (rb.velocity, camDir) * Mathf.PI / 180);
		if(axis < 0 && vel > -maxSpeed || axis > 0 && vel < maxSpeed){
			rb.velocity += camDir * axis * acceleration * Time.deltaTime / groundDrag;
		}
	}

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}
		
	void Update() {
		lsX = XCI.GetAxis (XboxAxis.LeftStickX, (XboxController)playerIndex);
		lsY = XCI.GetAxis (XboxAxis.LeftStickY, (XboxController)playerIndex);
		//forwardDir = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		//rightDir = new Vector3 (Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		if (lsX == 0 && lsY == 0) {
			rb.velocity = new Vector3 (rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
		} else {
			Quaternion turning = Quaternion.Euler (0, cam.transform.eulerAngles.y, 0);
			Vector3 curVelo = cam.transform.InverseTransformVector (rb.velocity);
			if (lsX < 0 && curVelo.x > -maxSpeed || lsX > 0 && curVelo.x < maxSpeed) {
				rb.velocity += turning * transform.right * lsX * acceleration * Time.deltaTime / groundDrag;
			}
			if (lsY < 0 && curVelo.z > -maxSpeed || lsY > 0 && curVelo.z < maxSpeed) {
				rb.velocity += turning * transform.forward * lsY * acceleration * Time.deltaTime / groundDrag;
			}
			//dir = Vector3.zero;
			//SetDirection (lsX, rightDir);
			//SetDirection (lsY, forwardDir);
			//Debug.Log (dir);
			//rb.velocity += dir.normalized * acceleration * Time.deltaTime / groundDrag;
		}
		//Debug.Log (maxSpeed - rb.velocity.magnitude);
	}
}
