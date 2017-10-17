using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {
	public float fovHor = 70;
	public float fovVer	= 50;
	public float intensityThreshhold = 15f;
	public float regard = 150f; //factor of alertness-increase when this sense is trigered
	private Transform Self;
	private Transform[] PlayerArr;
	private bool[] noticedPlayer;
	private Quaternion lookDir;

	void Awake() {
		Self = transform.parent.parent;
		PlayerArr = Self.GetComponent<EnemyBrain>().Player.GetComponent<PlayerLocation>().PlayerArr;
		lookDir = transform.rotation;
	}

	void Update() {
		// checking the noticed intensity for each Player
		noticedPlayer = new bool[PlayerArr.Length];
		for (int i = 0; i < PlayerArr.Length; i++) {
			float sensedIntensity = noticedIntesity(PlayerArr[i]);
			if(sensedIntensity >= intensityThreshhold) {
				noticedPlayer[i] = true;
			}
		}
		for (int i = 0; i < noticedPlayer.Length; i++) {
			if (noticedPlayer[i]) {
				Self.GetComponent<EnemyBrain>().senseTrigger(regard);
			}
		}
		GetComponentInParent<EnemyLooking>().LookAtSenseTrigger(ref PlayerArr, ref noticedPlayer, ref lookDir,
		             										 	ref Self.GetComponent<EnemyBrain>().senseState,
		                                                     	EnemyBrain.SENSESTATE.SEEING);
		Global.debugGUI("noticedIntensity E" + Self.GetComponent<EnemyBrain>().enemyIndex.ToString(), noticedIntesity(PlayerArr[0]));
	}

	// returns the sum of all intensities depending on the distance of the, for the enemy visible, visibilityPoints of a Player
	float noticedIntesity(Transform Player) {
		float sensedIntensity = 0f;
		Transform[] visiblePoints = new Transform[Global.childCount(Player.Find("visiblePoints"))];
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
						// 1/distance^2 as an weight
						sensedIntensity += visiblePoints[i].GetComponent<VisibilityPoint>().localIntensity
										   / Mathf.Pow((visiblePoints[i].position - transform.position).magnitude, 2);;
					}
				}
			}
		}
		return sensedIntensity;
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