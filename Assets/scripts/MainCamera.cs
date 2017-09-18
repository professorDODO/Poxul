using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class MainCamera : MonoBehaviour {

	public Transform Player;
	public bool invertX;
	public bool invertY;
	public float camZoomAccuracy;
	public float zoomOutOffset;
	public float zoomInOffset;
	public float camSpeed;
	public float minDistance;
	private float fovHorZoomIn;
	private float fovHorZoomOut;
	private float fovVerZoomIn;
	private float fovVerZoomOut;

	void Awake() {
		fovHorZoomIn = Camera.main.fieldOfView * (1 - zoomInOffset);
		fovHorZoomOut = Camera.main.fieldOfView * (1 - zoomOutOffset);
		fovVerZoomIn = Camera.main.fieldOfView/Camera.main.aspect * (1 - zoomInOffset);
		fovVerZoomOut = Camera.main.fieldOfView/Camera.main.aspect * (1 - zoomOutOffset);
	}

	void LateUpdate() {
		//position of players
		Vector3[] pLoc = Player.GetComponent<PlayerLocation>().pLoc;
		// midpos of all Players
		Vector3 middle = transform.parent.position;
		//turning camera
		float rsXInput = 0;
		float rsYInput = 0;
		for(int i = 1; i <= pLoc.Length; i++) {
			rsXInput += (invertX ? -1 : 1) * XCI.GetAxis (XboxAxis.RightStickX, (XboxController)i);
			rsYInput += (invertY ? 1 : -1) * XCI.GetAxis (XboxAxis.RightStickY, (XboxController)i);
		}
		transform.LookAt(middle);
		transform.RotateAround (middle, rsXInput * Vector3.up, Time.deltaTime * camSpeed * Mathf.Abs(rsXInput));
		if (transform.rotation.eulerAngles.x < 88) {
			transform.RotateAround (middle, rsYInput * transform.right, Time.deltaTime * camSpeed * Mathf.Abs(rsYInput));
		} else if (transform.rotation.eulerAngles.x > 88 && transform.rotation.eulerAngles.x < 270 && rsYInput <= 0) {
			transform.RotateAround (middle, rsYInput * transform.right, Time.deltaTime * camSpeed * Mathf.Abs(rsYInput));
		} else if (transform.rotation.eulerAngles.x > 270 && rsYInput >= 0) {
			transform.RotateAround (middle, rsYInput * transform.right, Time.deltaTime * camSpeed * Mathf.Abs(rsYInput));
		}
		//zoom out
		for (int i = 0; i < pLoc.Length; i++) {
			//checking if a player is outside of the view of the camera
			float vertical = AngleInPlane(transform, pLoc[i], transform.right);
			float horizontal = AngleInPlane(transform, pLoc[i], transform.up);
			if (horizontal > fovHorZoomOut) {
				transform.LookAt (middle);
				transform.position -= transform.forward.normalized * (pLoc [i] - transform.position).magnitude
					* Mathf.Sin (Mathf.PI/180*(horizontal - fovHorZoomOut))
					/ Mathf.Sin (Mathf.PI/180*fovHorZoomOut);
			}
			if (vertical > fovVerZoomOut) {
				transform.LookAt (middle);
				transform.position -= transform.forward.normalized * (pLoc [i] - transform.position).magnitude
					* Mathf.Sin (Mathf.PI/180*(vertical - fovVerZoomOut))
					/ Mathf.Sin (Mathf.PI/180*fovVerZoomOut);
			}
		}
		//zoom in
		float maxAngleHor = -1;
		float maxAngleVer = -1;
		int playerMaxAngleHorIndex = -1;
		int playerMaxAngleVerIndex = -1;
		if (pLoc.Length == 1) {
			maxAngleHor = 0;
			maxAngleVer = 0;
			playerMaxAngleHorIndex = 0;
			playerMaxAngleVerIndex = 0;
		} else {
			for (int i = 0; i < pLoc.Length; ++i) {
				float vertical = AngleInPlane (transform, pLoc [i], transform.right);
				float horizontal = AngleInPlane (transform, pLoc [i], transform.up);
				if (vertical > maxAngleVer) {
					maxAngleVer = vertical;
					playerMaxAngleVerIndex = i;
				}
				if (horizontal > maxAngleHor) {
					maxAngleHor = horizontal;
					playerMaxAngleHorIndex = i;
				}
			}
		}
		if (maxAngleVer < fovVerZoomIn && maxAngleHor < fovHorZoomIn) {
			transform.LookAt (middle);
			//check if a Player is nearer to the top or the sides, to determine which Angle should be respected when zooming
			if (maxAngleVer / fovVerZoomIn >= maxAngleHor / fovHorZoomIn) {
				transform.position -= transform.forward.normalized * (pLoc [playerMaxAngleVerIndex] - transform.position).magnitude
				* Mathf.Sin (Mathf.PI / 180 * (AngleInPlane (transform, pLoc [playerMaxAngleVerIndex], transform.right) - fovVerZoomIn))
				/ Mathf.Sin (Mathf.PI / 180 * fovVerZoomIn);
			} else {
				transform.position -= transform.forward.normalized * (pLoc [playerMaxAngleHorIndex] - transform.position).magnitude
				* Mathf.Sin (Mathf.PI / 180 * (AngleInPlane (transform, pLoc [playerMaxAngleHorIndex], transform.up) - fovHorZoomIn))
				/ Mathf.Sin (Mathf.PI / 180 * fovHorZoomIn);
			}
		}
		// set min distance to nearest Player
		for (int i = 0; i < pLoc.Length; i++) {
			if ((pLoc [i] - transform.position).magnitude < minDistance) {
				transform.LookAt(middle);
				transform.position -= transform.forward.normalized * (minDistance * Mathf.Sqrt (1 -
									Mathf.Pow ((pLoc [i] - transform.position).magnitude / minDistance
									* Mathf.Sin (Mathf.PI / 180 * Vector3.Angle ((pLoc [i] - transform.position), (middle - transform.position))), 2))
									- (pLoc [i] - transform.position).magnitude * Mathf.Cos (Mathf.PI / 180 * Vector3.Angle ((pLoc [i] - transform.position), (middle - transform.position))));
			}
		}
		//focus camera at mid point between players
		transform.LookAt(middle);
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
}