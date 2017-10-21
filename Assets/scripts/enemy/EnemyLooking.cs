using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooking : MonoBehaviour {
	[HideInInspector] public enum LOOKSTATE {
		NONE,
		IDLE,
		SEARCH,
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
			case LOOKSTATE.SEARCH:
				lrLooking(searchLookAngle, searchLookSpeedFac);
				break;
			case LOOKSTATE.TRIGGERED:
				LookAtSenseTrigger();
				break;
		}
	}

	public void return2Idle(){
		if (Quaternion.Angle(Self.rotation, transform.rotation) > lookDirAcc) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Self.rotation, lookSpeed * Time.deltaTime);
		} else {
			lookState = LOOKSTATE.IDLE;
		}
	}

	private void lrLooking(float angle, float fac) {
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

	// look at the postion of the nearist trigger of the the sense with highest priority
	private void LookAtSenseTrigger() {
		if (Quaternion.Angle(currentTriggerRot, transform.rotation) > lookDirAcc) {
			transform.rotation = Quaternion.Slerp(transform.rotation, currentTriggerRot, Time.deltaTime * lookSpeed);
		} else {
			currentTriggerRot = transform.rotation;
		}
	}

	public void setTriggerRotation(Vector3 triggerPos) {
		lookState = LOOKSTATE.TRIGGERED;
		currentTriggerRot = Quaternion.LookRotation(triggerPos - transform.position);
	}
}
