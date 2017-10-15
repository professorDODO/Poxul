using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooking : MonoBehaviour {
	[HideInInspector] public enum LOOKSTATE {
		NONE,
		IDLE,
		RETURN2IDLE,
		TRIGGERED};
	// sorted in proirity order
	[HideInInspector] public LOOKSTATE lookState;
	private Transform Self;
	public float lookSpeed = 10f;
	public float lookDirAcc = 1f; // comparison value in degree, when the enemy looks in the direction of last trigger
	public float idleLookAngle = 75f;
	public float idleLookSpeedFac = 5f;
	private Quaternion aimedRotation;
	private bool lr;

	void Awake() {
		lookState = LOOKSTATE.IDLE;
		Self = transform.parent;
		aimedRotation = Quaternion.Euler(0, idleLookAngle, 0);
		lr = true;
	}

	void Update() {
		handleLooking();
	}

	void handleLooking() {
		if (Self.GetComponent<EnemyBrain>().senseState == EnemyBrain.SENSESTATE.NONE) {
			if (lookState == LOOKSTATE.RETURN2IDLE) {
				return2Idle();
			}
			if (lookState == LOOKSTATE.IDLE) {
				idleLooking();
			}
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
	public void LookAtSenseTrigger(ref Transform[] PlayerArr, ref bool[] noticedPlayer, ref Quaternion lookDir,
	                               ref EnemyBrain.SENSESTATE senseState, EnemyBrain.SENSESTATE thisSense) {
		bool allFalse = true;
		for (int i = 0; i < noticedPlayer.Length; i++) {
			if (noticedPlayer[i]) {
				allFalse = false;
			}
		}
		if (!allFalse && thisSense >= senseState) {
			senseState = thisSense;
			lookState = LOOKSTATE.TRIGGERED;
			lookDir = lastLocDir(PlayerArr, noticedPlayer);
		}
		if (senseState == thisSense) {
			transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * lookSpeed);
		}
		if (allFalse && Quaternion.Angle(lookDir, transform.rotation) < lookDirAcc) {
			lookState = LOOKSTATE.RETURN2IDLE;
			senseState = EnemyBrain.SENSESTATE.NONE;
		}
		for (int i = 0; i < noticedPlayer.Length; i++) {
			noticedPlayer[i] = false;
		}
	}

	// returns the rotation towards the next player who triggered a sense
	public Quaternion lastLocDir(Transform[] PlayerArr, bool[] noticedPlayer) {
		Vector3 minDistLoc = Vector3.zero;
		float minDist = -1;
		for (int i = 0; i < PlayerArr.Length; i++) {
			if (noticedPlayer[i] && minDist == -1) {
				minDistLoc = PlayerArr[i].position;
			}
			if (noticedPlayer[i] && (PlayerArr[i].position - transform.position).magnitude < minDist) {
				minDistLoc = PlayerArr[i].position;
				minDist = (minDistLoc - transform.position).magnitude;
			}
		}
		return Quaternion.LookRotation(minDistLoc - transform.position);
	}
}
