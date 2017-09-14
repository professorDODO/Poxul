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
	private float FoVZoomIn;
	private float FoVZoomOut;

	void Awake() {
		FoVZoomIn = Camera.main.fieldOfView/Camera.main.aspect - zoomInOffset;
		FoVZoomOut = Camera.main.fieldOfView/Camera.main.aspect - zoomOutOffset;
	}

	void Update() {
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
			if (Vector3.Angle((pLoc[i] - transform.position), (middle - transform.position)) > FoVZoomOut) {
				transform.LookAt (middle);
				transform.position -= transform.forward.normalized * (pLoc [i] - transform.position).magnitude
					* Mathf.Sin (Mathf.PI/180*(Vector3.Angle((pLoc [i] - transform.position), (middle - transform.position)) - FoVZoomOut))
					/ Mathf.Sin (Mathf.PI/180*FoVZoomOut);
			}
		}

		//zoom in
		float maxAngle = 0;
		int playerMaxAngleIndex = -1;
		for (int i = 0; i < pLoc.Length; ++i) {
			if (Vector3.Angle ((pLoc [i] - transform.position), (middle - transform.position)) > maxAngle) {
				maxAngle = Vector3.Angle ((pLoc [i] - transform.position), (middle - transform.position));
				playerMaxAngleIndex = i;
			}
			if (pLoc.Length == 1) playerMaxAngleIndex = 0;
		}
		if (maxAngle < FoVZoomIn) {
			transform.LookAt (middle);
			transform.position -= transform.forward.normalized * (pLoc [playerMaxAngleIndex] - transform.position).magnitude
				* Mathf.Sin (Mathf.PI/180*(Vector3.Angle((pLoc [playerMaxAngleIndex] - transform.position), (middle - transform.position)) - FoVZoomIn))
				/ Mathf.Sin (Mathf.PI/180*FoVZoomIn);
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
}