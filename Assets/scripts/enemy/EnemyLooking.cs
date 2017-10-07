using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooking : MonoBehaviour {
	public float lookSpeed = 10f;
	public float lookDirAcc = 1f; // comparison value in degree, when the enemy looks in the direction of last trigger
	public float idleLookAngle = 75f;
	public float idleLookSpeedFac = 5f;
	private Quaternion defaultRotation;
	private Quaternion newDefaultRotation;
	private Quaternion aimedRotation;
	private bool lr;
	private bool defaultRotationChanged = false;

	void Start() {
		defaultRotation = transform.rotation;
		aimedRotation = defaultRotation * Quaternion.Euler(0, idleLookAngle, 0);
		lr = true;
	}

	void Update() {
		handleLooking();
	}

	void handleLooking() {
		if (GetComponent<EnemyBrain>().senseState == EnemyBrain.SENSESTATE.NONE) {
			if (defaultRotationChanged) {
				defaultRotation = newDefaultRotation;
				transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, Time.deltaTime * lookSpeed);
				if (Quaternion.Angle(transform.rotation, newDefaultRotation) <= lookDirAcc) {
					defaultRotationChanged = false;
					aimedRotation = defaultRotation * Quaternion.Euler(0, idleLookAngle, 0);
					lr = true;
				}
			} else {
				lookAround();
			}
		}
	}

	void lookAround() {
		if (Quaternion.Angle(aimedRotation, transform.rotation) < lookDirAcc) {
			if (lr) {
				aimedRotation = defaultRotation * Quaternion.Inverse(Quaternion.Euler(0, idleLookAngle, 0));
			} else {
				aimedRotation = defaultRotation * Quaternion.Euler(0, idleLookAngle, 0);
			}
			lr = !lr;
		} else {
			transform.rotation = Quaternion.Slerp(transform.rotation, aimedRotation,
			                                      lookSpeed * idleLookSpeedFac * Time.deltaTime
			                                      / (Quaternion.Angle(transform.rotation, aimedRotation)));
		}
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
			lookDir = lastLocDir(PlayerArr, noticedPlayer);
		}
		if (senseState == thisSense) {
			transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * lookSpeed);
		}
		if (allFalse && Quaternion.Angle(lookDir, transform.rotation) < lookDirAcc) {
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

	public void changeDefaultRotation(Quaternion newDefaultRot) {
		newDefaultRotation = newDefaultRot;
		defaultRotationChanged = true;
	}
}
