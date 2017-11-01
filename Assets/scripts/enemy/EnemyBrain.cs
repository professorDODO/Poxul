using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBrain : MonoBehaviour {
	[HideInInspector] public enum TASKSTATE {
		NONE,
		APROACHTRIGGER,
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
	public ALERTSTATE alertState { get; private set; }
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
	[HideInInspector] public bool nonPlayerMode = false;

	void Awake() {
		if (nonPlayerMode) {
			Debug.Log("Enemy E" + enemyIndex.ToString() + " has no Player object; continuing in non-Player-mode");
		}
		alertnessMax = alertnessStep * (int)ALERTSTATE.ALERTNESS2;
		senseState = SENSESTATE.NONE;
		alertState = ALERTSTATE.NONE;
		taskState = TASKSTATE.NONE;
	}

	void Update() {
		Global.debugGUI("alertness E" + enemyIndex.ToString(), alertness);
		alertnessDecay();
		handleHighAlertReaction();
	}

	void LateUpdate() {
		noticedPlayerIndex = -1;
		senseState = SENSESTATE.NONE;
	}

	// is called from a Sense script
	public void senseTrigger(float fac) {
		alertness += fac * Time.deltaTime;
		if (alertness >= alertnessStep * (int)ALERTSTATE.ALERTNESS1 && alertState < ALERTSTATE.ALERTNESS1) {
			alertnessMin = alertnessStep * (int)ALERTSTATE.ALERTNESS1;
			alertState = ALERTSTATE.ALERTNESS1;
		}
		if (alertness > alertnessMax) {
			alertState = ALERTSTATE.ALERTNESS2;
			alertness = alertnessMax;
		}
	}

	public void setMinAlertState(ALERTSTATE alState) {
		if(alState > alertState) {
			alertState = alState;
			alertnessMin = alertnessStep * (int)alState;
			alertness = alertnessMin;
		}
	}

	// decay of the alertness value over time
	void alertnessDecay() {
		if (alertState != ALERTSTATE.ALERTNESS2) {
			alertness -= baseAlertnessDecay * alertnessStep
						 * ((int)ALERTSTATE.ALERTNESS2 - (int)alertState) / (int)ALERTSTATE.ALERTNESS2 * Time.deltaTime;
		}
		if (alertness < alertnessMin) {
			alertness = alertnessMin;
		}
	}

	// handle the Enemies reaction when a high alert state is reached
	void handleHighAlertReaction() {
		if (alertState == ALERTSTATE.ALERTNESS1) {
			if (noticedPlayerIndex != -1) {
				Vector3 triggerPos = Player.GetComponent<PlayerLocation>()
											.PlayerArr[noticedPlayerIndex].position;
				handleTrigger(triggerPos);
				GetComponent<EnemyMessaging>().shout(triggerPos);
			}

		} else if (alertState == ALERTSTATE.ALERTNESS2) {
			Global.joinFight(transform);
			//just Debugging
			if (Global.FightParticipants != null) {
				for (int i = 0; i < Global.FightParticipants.Count; i++) {
					Global.debugGUI("FightParticipant #" + i.ToString(), Global.FightParticipants[i]);
				}
			}
			// FIGHT!
			//SceneManager.LoadScene("fightInitiation");
		}
	}

	public void handleTrigger(Vector3 pos) {
		GetComponent<EnemyBrain>().taskState = TASKSTATE.APROACHTRIGGER;
		GetComponent<EnemyHandleTrigger>().triggerPos = pos;
		Head.GetComponent<EnemyLooking>().LookAt(pos, EnemyLooking.LOOKSTATE.TRIGGERED);
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
		}
	}

	int nearestTrigger(Transform[] PlayerArr, bool[] noticedPlayer) {
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
