using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {
	public float fovHor = 70;
	public float fovVer	= 50;
	public float intensityThreshhold = 15f;
	public float regard = 150f; //factor of alertness-increase when this sense is trigered
	private Transform[] PlayerArr;
	private bool[] noticedPlayer;
	private Quaternion lookDir;

	void Start() {
		PlayerArr = transform.parent.GetComponent<EnemyBrain>().Player.GetComponent<PlayerLocation>().PlayerArr;
		lookDir = transform.rotation;
	}

	void Update() {
		noticedPlayer = new bool[PlayerArr.Length];
		for (int i = 0; i < PlayerArr.Length; i++) {
			float sensedIntensity = noticedIntesity(PlayerArr[i]);
			if(sensedIntensity >= intensityThreshhold) {
				noticedPlayer[i] = true;
			}
			debugGUI("sensedIntensity E" + transform.parent.GetComponent<EnemyBrain>().enemyIndex.ToString()
			         + " P" + i.ToString(),
			         sensedIntensity);
		}
		for (int i = 0; i < noticedPlayer.Length; i++) {
			if (noticedPlayer[i]) {
				transform.parent.GetComponent<EnemyBrain>().senseTrigger(regard);
			}
		}
		transform.parent.GetComponent<EnemyLooking>().LookAtSenseTrigger(ref PlayerArr, ref noticedPlayer, ref lookDir,
		             													 ref transform.parent.GetComponent<EnemyBrain>().senseState,
		                                                                 EnemyBrain.SENSESTATE.SEEING);
	}

	float noticedIntesity(Transform Player) {
		float sensedIntensity = 0f;
		Transform[] visiblePoints = new Transform[childCount(Player.Find("visiblePoints"))];
		for (int i = 0; i < visiblePoints.Length; i++) {
			if (Player.Find("visiblePoints").GetChild(i).gameObject.activeSelf) {
				visiblePoints[i] = Player.Find("visiblePoints").GetChild(i);
			}
			float vertical = angleInPlane(transform, visiblePoints[i].position, transform.right);
			float horizontal = angleInPlane(transform, visiblePoints[i].position, transform.up);
			if (vertical <= fovVer / 2 && horizontal <= fovHor / 2) {
				RaycastHit hit;
				if (Physics.Raycast(transform.position, visiblePoints[i].position - transform.position, out hit,
				                    (visiblePoints[i].position - transform.position).magnitude)) {
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
						sensedIntensity += visiblePoints[i].GetComponent<VisibilityPoint>().localIntensity
										   / Mathf.Pow((visiblePoints[i].position - transform.position).magnitude, 2);;
					}
				}
			}
		}
		return sensedIntensity;
	}

	// angles relative to a plane
	public float angleInPlane(Transform from, Vector3 to, Vector3 planeNormal) {
		Vector3 dir = to - from.position;
		Vector3 p1 = project(dir, planeNormal);
		Vector3 p2 = project(from.forward, planeNormal);
		return Vector3.Angle(p1, p2);
	}

	public Vector3 project(Vector3 v, Vector3 onto) {
		return v - (Vector3.Dot(v, onto) / Vector3.Dot(onto, onto)) * onto;
	}

	// enables the ability to see the fov of the gameObject
	void OnDrawGizmosSelected() {
		float viewRange = 10f;
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

	int childCount(Transform GO) {
		int childCount = 0;
		for (int i = 0; i < GO.childCount; i++) {
			if (GO.GetChild(i).gameObject.activeSelf) {
				childCount++;
			}
		}
		return childCount;
	}

	void debugGUI(string element, float value) {
		GameObject.Find("GUI").GetComponent<debugGUI>().debugElement(element, value);
	}
}