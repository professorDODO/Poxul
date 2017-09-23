using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {
	public Transform Player;
	public float fovHor = 70;
	public float fovVer	= 50;
	public float viewRange = 10;
	public float lookAroundSpeed = 10;
	public float regard = 150f; //factor of alertness-increase when this sense is trigered
	public float lookDirAcc = 1f; // comparison value in degree, when the enemy looks in the direction of last trigger
	private bool[] noticedPlayer;
	private Quaternion lookDir;

	void Start(){
		lookDir = transform.rotation;
	}

	void Update () {
		Vector3[] pLoc = Player.GetComponent<PlayerLocation>().pLoc;
		noticedPlayer = new bool[pLoc.Length];
		for (int i = 0; i < pLoc.Length; i++) {
			float vertical = angleInPlane(transform, pLoc[i], transform.right);
			float horizontal = angleInPlane(transform, pLoc[i], transform.up);
			if (vertical <= fovVer/2 && horizontal <= fovHor/2) {
				RaycastHit hit;
				// if there is any colider in the way to the player, the gameObject looks at the player
				if (Physics.Raycast (transform.position, pLoc [i] - transform.position, out hit, (pLoc [i] - transform.position).magnitude + 1)) {
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
						if (hit.transform.Find ("visiblePoints").GetComponent<Visibility> ().isVisible) {
							transform.parent.GetComponent<EnemyBrain> ().senseTrigger (regard);
							noticedPlayer [i] = true;
						}
					}
				}
			}
		}
		handleLookAt (ref pLoc, ref noticedPlayer, ref lookDir, ref transform.parent.GetComponent<EnemyBrain>().senseState, EnemyBrain.SENSESTATE.SEEING);
		debugGUI ("SESNSESTATE", (float)transform.parent.GetComponent<EnemyBrain>().senseState);
	}

	// look at the postion of the nearist trigger of the the sense with highest priority
	public void handleLookAt(ref Vector3[] pLoc, ref bool[] noticedPlayer, ref Quaternion lookDir, ref EnemyBrain.SENSESTATE senseState, EnemyBrain.SENSESTATE thisSense){
		bool allFalse = true;
		for (int i = 0; i < noticedPlayer.Length; i++) {
			if (noticedPlayer [i]) {
				allFalse = false;
			}
		}
		if (!allFalse && thisSense >= senseState) {
			senseState = thisSense;
			lookDir = lastLocDir (pLoc, noticedPlayer);
		}
		if (senseState == thisSense) {
			transform.parent.rotation = Quaternion.Slerp (transform.rotation, lookDir, Time.deltaTime * lookAroundSpeed);
		}
		if (allFalse && Quaternion.Angle (lookDir, transform.rotation) < lookDirAcc) {
			senseState = EnemyBrain.SENSESTATE.NONE;
		}
		for (int i = 0; i < noticedPlayer.Length; i++) {
			noticedPlayer [i] = false;
		}
	}

	// returns the rotation towards the next player who triggered a sense
	public Quaternion lastLocDir(Vector3[] pLoc, bool[] noticedPlayer){
		Vector3 minDistLoc = Vector3.zero;
		float minDist = -1;
		for (int i = 0; i < pLoc.Length; i++) {
			if (noticedPlayer[i] && minDist == -1) {
				minDistLoc = pLoc [i];
			}
			if (noticedPlayer[i] && (pLoc[i] - transform.position).magnitude < minDist) {
				minDistLoc = pLoc [i];
				minDist = (minDistLoc - transform.position).magnitude;
			}
		}
		return Quaternion.LookRotation (minDistLoc - transform.position);
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
		Quaternion leftRayRotation = Quaternion.AngleAxis(-fovHor/2, transform.up);
		Quaternion rightRayRotation = Quaternion.AngleAxis(fovHor/2, transform.up);
		Quaternion topRayRotation = Quaternion.AngleAxis(-fovVer/2, transform.right);
		Quaternion downRayRotation = Quaternion.AngleAxis(fovVer/2, transform.right);
		Vector3 leftRayDirection = leftRayRotation * transform.forward;
		Vector3 rightRayDirection = rightRayRotation * transform.forward;
		Vector3 topRayDirection = topRayRotation * transform.forward;
		Vector3 downRayDirection = downRayRotation * transform.forward;
		Gizmos.DrawRay(transform.position, leftRayDirection * viewRange);
		Gizmos.DrawRay(transform.position, rightRayDirection * viewRange);
		Gizmos.DrawRay(transform.position, topRayDirection * viewRange);
		Gizmos.DrawRay(transform.position, downRayDirection * viewRange);
	}

	void debugGUI(string element, float value){
		GameObject.Find ("GUI").GetComponent<debugGUI> ().debugElement (element, value);
	}
}