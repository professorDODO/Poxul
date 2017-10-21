using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooking : MonoBehaviour {
	[HideInInspector] public enum LOOKSTATE {
		NONE,
		IDLE,
		RETURN2IDLE,
		SEARCH,
		TRIGGERED};
	// sorted in proirity order
	[HideInInspector] public LOOKSTATE lookState;
	private Transform Self;
	public float lookSpeed = 10f;
	public float lookDirAcc = 1f; // comparison value in degree, when the enemy looks in the direction of last trigger
	public float idleLookAngle = 75f;
	public float idleLookSpeedFac = 5f;
	private Quaternion aimedRotation;
	private Quaternion currentTriggerRot;
	private bool lr;

	void Awake() {
		lookState = LOOKSTATE.IDLE;
		Self = transform.parent;
		lr = false;
		aimedRotation = Quaternion.Euler(0, -idleLookAngle, 0);
		currentTriggerRot = transform.rotation;
	}

	void Update() {
		handleLooking();
		Global.debugGUI("LOOKSTATE E" + Self.GetComponent<EnemyBrain>().enemyIndex.ToString(), lookState);
	}

	void handleLooking() {
		switch (lookState) {
			case LOOKSTATE.IDLE:
				idleLooking();
				break;
			case LOOKSTATE.RETURN2IDLE:
				return2Idle();
				break;
			case LOOKSTATE.TRIGGERED:
				LookAtSenseTrigger();
				break;
		}
	}

	void return2Idle(){
		if (Quaternion.Angle(Self.rotation, transform.rotation) > lookDirAcc) {
			transform.rotation = Quaternion.Slerp(transform.rotation, Self.rotation, lookSpeed * Time.deltaTime);
		} else {
			lookState = LOOKSTATE.IDLE;
		}
	}

	void idleLooking() {
		if (Quaternion.Angle(aimedRotation, transform.localRotation) < lookDirAcc) {
			if (lr) {
				aimedRotation = Quaternion.Inverse(Quaternion.Euler(0, idleLookAngle, 0));
			} else {
				aimedRotation = Quaternion.Euler(0, idleLookAngle, 0);
			}
			lr = !lr;
		}
		transform.localRotation = Quaternion.Slerp(transform.localRotation, aimedRotation,
		                                      	   lookSpeed * idleLookSpeedFac * Time.deltaTime
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
