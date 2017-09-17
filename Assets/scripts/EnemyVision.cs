using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {
	public Transform Player;
	public float fovHor = 70;
	public float fovVer	= 50;
	public float viewRange = 10;

	void Update () {
		Vector3[] pLoc = Player.GetComponent<PlayerLocation>().pLoc;
		for (int i = 0; i < pLoc.Length; i++) {
			float vertical = AngleInPlane(transform, pLoc[i], transform.right);
			float horizontal = AngleInPlane(transform, pLoc[i], transform.up);
			if (vertical <= fovVer/2 && horizontal <= fovHor/2) {
				RaycastHit hit;
				if (Physics.Raycast (transform.position, pLoc [i] - transform.position, out hit, (pLoc [i] - transform.position).magnitude + 1)) {
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player")) {
						Debug.Log (string.Format ("Player{0} is seen", i + 1));
					} else {
						Debug.Log (string.Format ("Player{0} is not seen", i + 1));
					}
				}
			} else {
				Debug.Log (string.Format("Player{0} is not seen", i + 1));
			}
		}
	}
		
	public float AngleInPlane(Transform from, Vector3 to, Vector3 planeNormal) {
		Vector3 dir = to - from.position;
		Vector3 p1 = Project(dir, planeNormal);
		Vector3 p2 = Project(from.forward, planeNormal);
		return Vector3.Angle(p1, p2);
	}

	public Vector3 Project(Vector3 v, Vector3 onto) {
		return v - (Vector3.Dot(v, onto) / Vector3.Dot(onto, onto)) * onto;
	}

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
}