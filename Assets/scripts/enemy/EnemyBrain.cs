using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	 * ALERTNESS1: the Enemy got triggered
	 * ALERTNESS2: an trigger gained full attention
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
		Global.debugGUI("ALERTSTATE E" + enemyIndex.ToString(), (float)alertState);
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

	// handle the Enemies reaction when a high alert state is reached
	void handleHighAlertReaction() {
		if (alertState == ALERTSTATE.ALERTNESS1) {
			
		} else if (alertState == ALERTSTATE.ALERTNESS2) {
			SceneManager.LoadScene("fightInitiation");
		}
	}
}
