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
		ALERTNESS2}

	;
	// sorted in proirity order
	[HideInInspector] public ALERTSTATE alertState;
	public Transform Player;
	public int enemyIndex;
	public float alertnessStep = 100f;
	private float alertnessMin = 0f;
	private float alertnessMax;
	public float baseAlertnessDecay = 0.1f; // in % of alertnessStep per deltaTime
	private float alertness = 0f;

	void Start() {
		alertnessMax = alertnessStep * (int)ALERTSTATE.ALERTNESS2;
		senseState = SENSESTATE.NONE;
		alertState = ALERTSTATE.NONE;
	}

	void Update() {
		alertnessDecay();
		if (alertState >= ALERTSTATE.ALERTNESS1) {
			handleHighAlertReaction();
		}
		debugGUI("alertness E" + enemyIndex.ToString(), alertness);
		debugGUI("ALERTSTATE E" + enemyIndex.ToString(), (float)alertState);
		debugGUI("SENSESTATE E" + enemyIndex.ToString(), (float)senseState);
	}

	// is called from a Sense script
	public void senseTrigger(float fac) {
		alertness += fac * Time.deltaTime;
		if (alertness >= alertnessStep * (int)ALERTSTATE.ALERTNESS1 && alertState != ALERTSTATE.ALERTNESS2) {
			alertnessMin = alertnessStep * (int)ALERTSTATE.ALERTNESS1;
			alertState = ALERTSTATE.ALERTNESS1;
		}
		if (alertness > alertnessMax) {
			alertState = ALERTSTATE.ALERTNESS2;
			alertness = alertnessMax;
		}
	}

	// decay of the alertness value over time
	void alertnessDecay() {
		if (alertState != ALERTSTATE.ALERTNESS2) {
			alertness -= baseAlertnessDecay * alertnessStep
						 * ((int)ALERTSTATE.ALERTNESS2 / ((int)ALERTSTATE.ALERTNESS2 - (int)alertState)) * Time.deltaTime;
		}
		if (alertness < alertnessMin) {
			alertness = alertnessMin;
		}
	}

	void handleHighAlertReaction() {
		if (alertState == ALERTSTATE.ALERTNESS1) {
			
		} else if (alertState == ALERTSTATE.ALERTNESS2) {
			
		}
	}

	void debugGUI(string element, float value) {
		GameObject.Find("GUI").GetComponent<debugGUI>().debugElement(element, value);
	}
}
