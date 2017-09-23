using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour {
	/*
	 * NONE: the Enemy is not alerted
	 * RUMORS: another Enemy reached ALERTNESS1 and transmitted the info
	 * ALERTNESS1: the Enemy heart or saw something
	 * ALERTNESS2: another Enemy reached ALERTNESS3
	 * ALERTNESS3: the Enemy heart or saw the Player 
	 */
	public enum ALERTSTATE {NONE, RUMORS, ALERTNESS1, ALERTNESS2, ALERTNESS3};
	public ALERTSTATE alertState;
	public float nextAlertState = 100f;
	public float alertnessDecay = 5;
	private float alertness = 0f;

	void start() {
		alertState = ALERTSTATE.NONE;
	}

	void Update() {
		decayAlert();
		debugGUI ("alertness", alertness);
		debugGUI ("ALERTSTATE", (float)alertState);
	}

	public void senseTrigger(float fac) {
		senseDelay (fac);
	}

	void decayAlert() {
		if (alertness >= 0f && alertState >= ALERTSTATE.NONE) {
			alertness -= alertnessDecay * Time.deltaTime;
		} else if(alertness < 0f && alertState > ALERTSTATE.NONE){
			alertness = 100;
			alertState--;
		}
		if (alertness < 0f && alertState == ALERTSTATE.NONE) {
			alertness = 0f;
		}
	}

	void senseDelay(float fac) {
		if (alertness < nextAlertState && alertState <= ALERTSTATE.ALERTNESS3) {
			alertness += fac * Time.deltaTime;	
		} else if(alertness >= nextAlertState && alertState < ALERTSTATE.ALERTNESS3) {
			alertness = 0f;
			alertState++;
		}
		if (alertness > nextAlertState && alertState == ALERTSTATE.ALERTNESS3) {
			alertness = 100f;
		}
	}

	void debugGUI(string element, float value){
	GameObject.Find ("GUI").GetComponent<debugGUI> ().debugElement (element, value);
	}
}
