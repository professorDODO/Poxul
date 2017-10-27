using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	// MOVE
	// parameters
	public float maxSpeed = 10;
	[SerializeField] float groundDrag = 0.9f;
	[SerializeField] float moveForce = 50;
	[SerializeField] float zeroSpeedAcc = 0.1f;
	[SerializeField] float sneakSpeedFac = 0.5f;
	[SerializeField] float rotationSpeed = 10f;
	// variables
	private Rigidbody rb;
	private Vector3 forwardDir = Vector3.forward;
	private Vector3 rightDir = Vector3.right;
	private bool sneak;
	private float ssFac;

	// JUMP
	// parameters
	[SerializeField] int jumpForce = 250;
	[SerializeField] float jumpCharge = 0.5f;
	[SerializeField] float minJump = 0.5f;
	// variables
	float jumpBuildUp;
	public GameObject trajec;
	public JUMPSTATES jumpStates;

	[HideInInspector] public enum JUMPSTATES{
		GROUNDED,
		JUMPPREP,
		LAUNCH,
		JUMPING
	};


	void Awake() {
		rb = GetComponent<Rigidbody>();
		sneak = false;
		ssFac = 1;
		jumpBuildUp = jumpForce * minJump;
	}

	public void move(Vector2 inputVec, bool rel2Cam) {
		// enables movement relative to the camera angle
		if (rel2Cam) {
			forwardDir = new Vector3 (Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
			rightDir = new Vector3 (Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
		}
		if (inputVec.magnitude <= zeroSpeedAcc) {
			rb.velocity = new Vector3 (rb.velocity.x * groundDrag, rb.velocity.y, rb.velocity.z * groundDrag);
		} else {
			rb.AddForce ((rightDir * inputVec.x + forwardDir * inputVec.y).normalized * moveForce / groundDrag);
			rb.AddForce (-rb.velocity / ssFac / inputVec.magnitude / maxSpeed * 42f);
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

	public void jumpPreparation(){
		if(jumpBuildUp < jumpForce){
			jumpBuildUp += jumpForce * jumpCharge * Time.deltaTime;
		}
		trajec.GetComponent<Trajectory>().RenderTrajectory (new Vector3(rb.velocity.x, jumpBuildUp * Time.fixedDeltaTime / rb.mass, rb.velocity.z));
		Debug.Log (new Vector2 (rb.velocity.x, rb.velocity.z).magnitude);
	}

	public void jump(){
		trajec.GetComponent<Trajectory>().RenderTrajectory (new Vector3(rb.velocity.x, jumpBuildUp * Time.fixedDeltaTime / rb.mass, rb.velocity.z));
		rb.AddForce (new Vector3 (0, jumpBuildUp, 0));
		jumpBuildUp = jumpForce * minJump;
		jumpStates = JUMPSTATES.JUMPING;
	}

	public void movementDebug() {
		Global.debugGUI("sneak P" + GetComponent<CharStats>().playerNumber.ToString(), sneak);
	}
}
