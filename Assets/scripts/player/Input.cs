using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Input : MonoBehaviour {
	private int playerIndex = 1;
	private MovementToMerge Movement;
	private Vector2 inputVec;

	void Awake() {
		playerIndex = transform.GetComponent<CharStats>().playerNumber;
		Movement = GetComponent<MovementToMerge>();
	}
	
	// Update is called once per frame
	void Update () {
		inputVec = new Vector2(XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerIndex),
		                       XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerIndex));
		if (XCI.GetButtonUp(XboxButton.LeftStick, (XboxController)playerIndex)) {
			Movement.initiateSneak();
		}
		Movement.move(inputVec, true);
		Movement.rotate(inputVec, true);
		Movement.movementDebug();
	}
}
