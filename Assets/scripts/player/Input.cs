using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Input : MonoBehaviour {
	private int playerIndex = 1;
	private MovementToMerge Movement;
	private float lsX;
	private float lsY;

	void Awake() {
		playerIndex = transform.GetComponent<CharStats>().playerNumber;
		Movement = GetComponent<MovementToMerge>();
	}
	
	// Update is called once per frame
	void Update () {
		lsX = XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex);
		lsY = XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex);
		if (XCI.GetButtonUp(XboxButton.LeftStick, (XboxController)playerIndex)) {
			Movement.initiateSneak();
		}
		Movement.move(lsX, lsY, true);
		Movement.movementDebug();
	}
}
