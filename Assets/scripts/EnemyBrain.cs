using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour {
	[HideInInspector] public enum SENSESTATE {
		NONE,
		HEARING,
		SEEING}

	;
	// sorted in proirity order
	[HideInInspector] public SENSESTATE senseState;
	/*
	 * NONE: the Enemy is not alerted
	 * RUMORS: another Enemy reached ALERTNESS1 and transmitted the info
	 * ALERTNESS1: the Enemy heart or saw something
	 * ALERTNESS2: another Enemy reached ALERTNESS3
	 * ALERTNESS3: the Enemy heart or saw the Player 
	 */
	[HideInInspector] public enum ALERTSTATE {
		NONE,
		RUMORS,
		ALERTNESS1,
		ALERTNESS2,
		ALERTNESS3}

	;
	// sorted in proirity order
	[HideInInspector] public ALERTSTATE alertState;
	public Transform Player;
	public int enemyIndex;
	public float nextAlertState = 100f;
	// the value alertness needs reach to reach the next alertState
	public float alertnessDecay = 5;
	private float alertness = 0f;

	void start() {
		senseState = SENSESTATE.NONE;
		alertState = ALERTSTATE.NONE;
	}

	void Update() {
		decayAlert();
		debugGUI("alertness E" + enemyIndex.ToString(), alertness);
		debugGUI("ALERTSTATE E" + enemyIndex.ToString(), (float)alertState);
	}

	// is called from a Sense script
	public void senseTrigger(float fac) {
		senseDelay(fac);
		//stuff to do when triggered
	}

	// decay of the alertness value over time
	void decayAlert() {
		if (alertness >= 0f && alertState >= ALERTSTATE.NONE) {
			alertness -= alertnessDecay * Time.deltaTime;
		} else if (alertness < 0f && alertState > ALERTSTATE.NONE) {
			alertness = 100;
			alertState--;
		}
		if (alertness < 0f && alertState == ALERTSTATE.NONE) {
			alertness = 0f;
		}
	}

	// handling a delay for enemies to notice what they see or hear
	void senseDelay(float fac) {
		if (alertness < nextAlertState && alertState <= ALERTSTATE.ALERTNESS3) {
			alertness += fac * Time.deltaTime;	
		} else if (alertness >= nextAlertState && alertState < ALERTSTATE.ALERTNESS1) {
			alertness = 0f;
			alertState = ALERTSTATE.ALERTNESS1;
		} else if (alertness >= nextAlertState && alertState >= ALERTSTATE.ALERTNESS1 && alertState < ALERTSTATE.ALERTNESS3) {
			alertness = 0f;
			alertState = ALERTSTATE.ALERTNESS3;
		}
		if (alertness > nextAlertState && alertState == ALERTSTATE.ALERTNESS3) {
			alertness = 100f;
		}
	}

	void debugGUI(string element, float value) {
		GameObject.Find("GUI").GetComponent<debugGUI>().debugElement(element, value);
	}
}
