﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {
	[SerializeField] private float fovHor = 70;
	[SerializeField] private float fovVer	= 50;
	[SerializeField] private float distanceAcc = 0.2f;
	[SerializeField] private float intensityThreshhold = 1f;
	[SerializeField] private float nearDistanceRecognition = 4f;
	[SerializeField] private float regard = 300f; //factor of alertness-increase when this sense is trigered
	private Transform Self;
	private Transform[] PlayerArr;

	void Awake() {
		Self = transform.parent.parent;
		if (Self.GetComponent<EnemyBrain>().Player == null || Self.GetComponent<EnemyBrain>().Player.Equals(null)) {
			Self.GetComponent<EnemyBrain>().nonPlayerMode = true;

		} else {
			PlayerArr = Self.GetComponent<EnemyBrain>().Player.GetComponent<PlayerLocation>().PlayerArr;
		}
	}

	void Update() {
		if (!Self.GetComponent<EnemyBrain>().nonPlayerMode) {
			playerVisionTrigger();
		}
	}

	// checking the noticed intensity for each Player and creating a sense reaction
	// in case of noticedIntensity >= threshhold
	void playerVisionTrigger() {
		float[] sensedIntensity = new float[PlayerArr.Length];
		bool[]noticedPlayer = new bool[PlayerArr.Length];
		for (int i = 0; i < PlayerArr.Length; i++) {
			sensedIntensity[i] = noticedIntesity(PlayerArr[i]);
			if(sensedIntensity[i] >= intensityThreshhold) {
				if (EnemyBrain.SENSESTATE.SEEING >= Self.GetComponent<EnemyBrain>().senseState) {
					noticedPlayer[i] = true;
					Self.GetComponent<EnemyBrain>().senseState = EnemyBrain.SENSESTATE.SEEING;
				}
				Self.GetComponent<EnemyBrain>().senseTrigger(sensedIntensity[i] / intensityThreshhold * regard);
			} else if(sensedIntensity[i] < intensityThreshhold
					  && (PlayerArr[i].position - transform.position).magnitude <= nearDistanceRecognition) {
				if (EnemyBrain.SENSESTATE.SEEING >= Self.GetComponent<EnemyBrain>().senseState) {
					noticedPlayer[i] = true;
					Self.GetComponent<EnemyBrain>().senseState = EnemyBrain.SENSESTATE.SEEING;
				}
				// magic number to overcome virtualAlertnessDecayBug
				Self.GetComponent<EnemyBrain>().senseTrigger(7000);
			}
		}
		if (Self.GetComponent<EnemyBrain>().alertState >= EnemyBrain.ALERTSTATE.ALERTNESS1) {
			Self.GetComponent<EnemyBrain>().sensedPlayerIndex(PlayerArr, noticedPlayer);
		}
	}

	// returns the sum of all intensities depending on the distance of the,
	// for the enemy visible, visibilityPoints of a Player
	float noticedIntesity(Transform Player) {
		float sensedIntensity = 0f;
		Transform[] visiblePoints = new Transform[Global.activeChildCount(Player.Find("visiblePoints"))];
		for (int i = 0; i < visiblePoints.Length; i++) {
			if (Player.Find("visiblePoints").GetChild(i).gameObject.activeSelf) {
				visiblePoints[i] = Player.Find("visiblePoints").GetChild(i);
			}
			float vertical = Global.angleInPlane(transform, visiblePoints[i].position, transform.right);
			float horizontal = Global.angleInPlane(transform, visiblePoints[i].position, transform.up);
			if (vertical <= fovVer / 2 && horizontal <= fovHor / 2) {
				RaycastHit hit;
				if (Physics.Raycast(transform.position, visiblePoints[i].position - transform.position, out hit)) {
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
						if((visiblePoints[i].position - transform.position).magnitude >= distanceAcc) {
							// 1/distance^2 as an weight
							sensedIntensity += visiblePoints[i].GetComponent<VisibilityPoint>().localIntensity
										   	   / Mathf.Pow((visiblePoints[i].position - transform.position).magnitude, 2);
						}
					}
				}
			}
		}
		return sensedIntensity;
	}

	public void nonPlayerVisionTrigger(Vector3 pos, float intensity) {
		float sensedIntensity = 0f;
		float vertical = Global.angleInPlane(transform, pos, transform.right);
		float horizontal = Global.angleInPlane(transform, pos, transform.up);
		if (vertical <= fovVer / 2 && horizontal <= fovHor / 2) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, pos - transform.position, out hit)) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
					if((pos - transform.position).magnitude >= distanceAcc) {
						if (EnemyBrain.SENSESTATE.SEEING >= Self.GetComponent<EnemyBrain>().senseState) {
							Self.GetComponent<EnemyBrain>().senseState = EnemyBrain.SENSESTATE.SEEING;
						}
						// 1/distance^2 as an weight
						sensedIntensity = intensity / Mathf.Pow((pos - transform.position).magnitude, 2);
					}
				}
			}
		}
		if (sensedIntensity >= intensityThreshhold) {
			Self.GetComponent<EnemyBrain>().senseTrigger(sensedIntensity / intensityThreshhold * regard);
		}
	}

	// enables the ability to see the fov of the gameObject
	void OnDrawGizmosSelected() {
		float viewRange = 15f;
		Quaternion leftRayRotation = Quaternion.AngleAxis(-fovHor / 2, transform.up);
		Quaternion rightRayRotation = Quaternion.AngleAxis(fovHor / 2, transform.up);
		Quaternion topRayRotation = Quaternion.AngleAxis(-fovVer / 2, transform.right);
		Quaternion downRayRotation = Quaternion.AngleAxis(fovVer / 2, transform.right);
		Vector3 leftRayDirection = leftRayRotation * transform.forward;
		Vector3 rightRayDirection = rightRayRotation * transform.forward;
		Vector3 topRayDirection = topRayRotation * transform.forward;
		Vector3 downRayDirection = downRayRotation * transform.forward;
		Gizmos.DrawRay(transform.position, leftRayDirection * viewRange);
		Gizmos.DrawRay(transform.position, rightRayDirection * viewRange);
		Gizmos.DrawRay(transform.position, topRayDirection * viewRange);
		Gizmos.DrawRay(transform.position, downRayDirection * viewRange);
	}
}