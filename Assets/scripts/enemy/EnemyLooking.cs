using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooking : MonoBehaviour {
	[HideInInspector] public enum LOOKSTATE {
		NONE,
		IDLE,
		RETURN2IDLE,
		SEARCH,
		LOOKAT,
		TRIGGERED};
	// sorted in proirity order
	[HideInInspector] public LOOKSTATE lookState;
	private Transform Self;
	public float lookSpeed = 10f;
	public float lookDirAcc = 1f; // comparison value in degree, when the enemy looks in the direction of last trigger
	public float idleLookAngle = 75f;
	public float idleLookSpeedFac = 5f;
	public float searchLookAngle = 75f;
	public float searchLookSpeedFac = 5f;
	private Quaternion aimedRotation;
	private Quaternion currentTriggerRot;
	private bool lr;

	void Awake() {
		lookState = LOOKSTATE.IDLE;
		Self = transform.parent;
		lr = true;
		aimedRotation = Quaternion.Euler(0, idleLookAngle, 0);
		currentTriggerRot = transform.rotation;
	}

	void Update() {
		handleLooking();
	}

	void handleLooking() {
		switch (lookState) {
			case LOOKSTATE.IDLE:
				lrLooking(idleLookAngle, idleLookSpeedFac);
				break;
			case LOOKSTATE.RETURN2IDLE:
				return2Idle();
				break;
			case LOOKSTATE.SEARCH:
				lrLooking(searchLookAngle, searchLookSpeedFac);
				break;
			case LOOKSTATE.LOOKAT:
				executeLookAt();
				break;
			case LOOKSTATE.TRIGGERED:
				executeLookAt();
				break;
		}
	}

	void return2Idle(){
		lr = true;
		aimedRotation = Quaternion.Euler(0, idleLookAngle, 0);
		if (Quaternion.Angle(Self.rotation, transform.rotation) > lookDirAcc) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Self.rotation, lookSpeed * Time.deltaTime);
		} else {
			lookState = LOOKSTATE.IDLE;
		}
	}

	void lrLooking(float angle, float fac) {
		if (Quaternion.Angle(aimedRotation, transform.localRotation) < lookDirAcc) {
			if (lr) {
				aimedRotation = Quaternion.Inverse(Quaternion.Euler(0, angle, 0));
			} else {
				aimedRotation = Quaternion.Euler(0, angle, 0);
			}
			lr = !lr;
		}
		transform.localRotation = Quaternion.Slerp(transform.localRotation, aimedRotation,
		                                      	   lookSpeed * fac * Time.deltaTime
		            							   / (Quaternion.Angle(transform.localRotation, aimedRotation)));
	}

	void executeLookAt() {
		if (Quaternion.Angle(currentTriggerRot, transform.rotation) > lookDirAcc) {
			transform.rotation = Quaternion.Slerp(transform.rotation, currentTriggerRot, Time.deltaTime * lookSpeed);
		} else {
			currentTriggerRot = transform.rotation;
			lookState = LOOKSTATE.NONE;
		}
	}

	// initiate the action to look at a certain position and setting the corresponding lookState
	public void LookAt(Vector3 pos, LOOKSTATE lookStateIn) {
		if (lookStateIn == LOOKSTATE.LOOKAT || lookStateIn == LOOKSTATE.TRIGGERED) {
			lookState = lookStateIn;
			currentTriggerRot = Quaternion.LookRotation(pos - transform.position);
		} else {
			Debug.Log("There was an attempt to set an invalid lookState for E"
			          + Self.GetComponent<EnemyBrain>().enemyIndex.ToString());
		}
	}
}
