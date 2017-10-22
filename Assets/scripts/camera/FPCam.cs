using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class FPCam : MonoBehaviour {

	public int playerIndex = 1;
	Vector3 look;
	public float sensivity;

	GameObject character;

	void Start () {
		character = this.transform.gameObject;
	}
	
	void Update () {
		Vector3 r = character.transform.localEulerAngles;
		look = new Vector3 (r.x - sensivity * XCI.GetAxis (XboxAxis.RightStickY, (XboxController)playerIndex), r.y + sensivity * XCI.GetAxis (XboxAxis.RightStickX, (XboxController)playerIndex), 0);

		character.transform.localEulerAngles = new Vector3 (look.x, look.y, 0);
		//character.transform.Rotate (0, XCI.GetAxis (XboxAxis.RightStickX, (XboxController)playerIndex),0);
	}
}
