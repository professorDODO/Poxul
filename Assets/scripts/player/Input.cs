﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Input : MonoBehaviour {

	[HideInInspector] public bool freeze;
	
	private int playerIndex = 1;
	private Movement Movement;
	private Vector2 inputVec;

	void Awake() {
		playerIndex = transform.GetComponent<CharStats>().playerNumber;
		Movement = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
		inputVec = new Vector2 (XCI.GetAxis (XboxAxis.LeftStickX, (XboxController)playerIndex),
			XCI.GetAxis (XboxAxis.LeftStickY, (XboxController)playerIndex));
		
		if (XCI.GetButtonUp (XboxButton.LeftStick, (XboxController)playerIndex)) {
			Movement.initiateSneak ();
		}
		if (XCI.GetButton (XboxButton.A, (XboxController)playerIndex)
		    && Movement.jumpStates != Movement.JUMPSTATES.JUMPING) {
			Movement.jumpStates = Movement.JUMPSTATES.JUMPPREP;
			Movement.jumpPreparation ();
		}
		if (XCI.GetButtonUp (XboxButton.A, (XboxController)playerIndex)
			&& Movement.jumpStates != Movement.JUMPSTATES.JUMPING) {
			Movement.jumpStates = Movement.JUMPSTATES.LAUNCH;
		}
		if (XCI.GetButton(XboxButton.Y, (XboxController)playerIndex)) {
			GetComponent<Abilities>().triggerEnemies();
		}
	}

	void FixedUpdate(){
		if(!freeze){
			if(Movement.jumpStates != Movement.JUMPSTATES.JUMPING){
				Movement.move(inputVec,true);
				Movement.rotate(inputVec,true);
			}
			if(Movement.jumpStates == Movement.JUMPSTATES.LAUNCH){
				Movement.jump();
			}
			Movement.movementDebug();
		}
	}
}
