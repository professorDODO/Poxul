using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooking : MonoBehaviour {
	public float lookSpeed = 5f;
	public float lookAngle = 75f;
	public float lookDirAcc = 1f;
	private Quaternion defaultRotation;
	private Quaternion aimedRotation;
	private bool lr;

	void Start() {
		defaultRotation = transform.rotation;
		aimedRotation = defaultRotation * Quaternion.Euler(0, lookAngle, 0);
		lr = true;
	}

	void Update() {
		if (GetComponent<EnemyBrain>().senseState == EnemyBrain.SENSESTATE.NONE) {
			if (Quaternion.Angle(aimedRotation, transform.rotation) < lookDirAcc) {
				if (lr) {
					aimedRotation = defaultRotation * Quaternion.Inverse(Quaternion.Euler(0, lookAngle, 0));
				} else {
					aimedRotation = defaultRotation * Quaternion.Euler(0, lookAngle, 0);
				}
				lr = !lr;
			} else {
				transform.rotation = Quaternion.Slerp(transform.rotation, aimedRotation, lookSpeed * Time.deltaTime);
			}
		}
	}
}

//Prevent the Enemy from getting stuck at looking at the opposite direction!!
