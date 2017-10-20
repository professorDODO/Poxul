using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBrain : MonoBehaviour {
	[HideInInspector] public enum TASKSTATE {
		NONE,
		LOOKING4TRIGGER,
		SEARCH};
	// sorted in proirity order
	[HideInInspector] public TASKSTATE taskState;
	[HideInInspector] public enum SENSESTATE {
		NONE,
		HEARING,
		SEEING};
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
		ALERTNESS2};
	// sorted in proirity order
	[HideInInspector] public ALERTSTATE alertState;
	public Transform Player;
	public Transform Senses;
	public Transform Head;
	public int enemyIndex;
	public float alertnessStep = 100f;
	private float alertnessMin = 0f;
	private float alertnessMax;
	public float baseAlertnessDecay = 0.1f; // in % of alertnessStep per deltaTime
	private float alertness = 0f;
	private int noticedPlayerIndex = -1;

	void Awake() {
		alertnessMax = alertnessStep * (int)ALERTSTATE.ALERTNESS2;
		senseState = SENSESTATE.NONE;
		alertState = ALERTSTATE.NONE;
		taskState = TASKSTATE.NONE;
	}

	void Update() {
		alertnessDecay();
		if (alertState >= ALERTSTATE.ALERTNESS1) {
			handleHighAlertReaction();
		}
		Global.debugGUI("alertness", alertness);
	}

	void LateUpdate() {
		noticedPlayerIndex = -1;
		senseState = SENSESTATE.NONE;
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
		if (alertState >= ALERTSTATE.ALERTNESS1) {
			taskState = TASKSTATE.LOOKING4TRIGGER;
			/*
			THERE SHOULD BE A CLEANER VERSION
			lookatsensetrigger() should return true if the player is visible at the last angle
			false if there is something blocking it, so moving will be initiated
			*/
			if (noticedPlayerIndex != -1) {
				Head.GetComponent<EnemyLooking>().setTriggerRotation(
					Player.GetComponent<PlayerLocation>().PlayerArr[noticedPlayerIndex].position);
			}
		} //else if (alertState == ALERTSTATE.ALERTNESS2) {
			// FIGHT!
			//SceneManager.LoadScene("fightInitiation");
		//}
	}

	public void sensedPlayerIndex(Transform[] PlayerArr, bool[] noticedPlayer) {
		bool allFalse = true;
		for (int i = 0; i < noticedPlayer.Length; i++) {
			if (noticedPlayer[i]) {
				allFalse = false;
			}
		}
		if (!allFalse) {
			noticedPlayerIndex = nearestTrigger(PlayerArr, noticedPlayer);
		} else {
			
		}
	}

	private int nearestTrigger(Transform[] PlayerArr, bool[] noticedPlayer) {
		float minDist = float.PositiveInfinity;
		int temp = -1;
		for (int i = 0; i < PlayerArr.Length; i++) {
			if (noticedPlayer[i] && (PlayerArr[i].position - transform.position).magnitude < minDist) {
				temp = i;
			}
		}
		return temp;
	}
}
