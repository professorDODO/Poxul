using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandleTrigger : MonoBehaviour {
	[HideInInspector] public Vector3 triggerPos;
	public float searchRadius = 5f;
	private Transform Head;

	void Awake() {
		Head = GetComponent<EnemyBrain>().Head;
	}

	void Update() {
		switch (GetComponent<EnemyBrain>().taskState) {
			case EnemyBrain.TASKSTATE.APROACHTRIGGER:
				if (GetComponent<PathFinding>().navState != PathFinding.NAVSTATE.REACHEDGOAL) {
					GetComponent<PathFinding>().navigateTo(triggerPos);
				} else {
					GetComponent<PathFinding>().navState = PathFinding.NAVSTATE.NONE;
				}
				if (GetComponent<EnemyBrain>().senseState != EnemyBrain.SENSESTATE.SEEING) {
					GetComponent<EnemyBrain>().taskState = EnemyBrain.TASKSTATE.SEARCH;
				}
				break;
			case EnemyBrain.TASKSTATE.SEARCH:
				Head.GetComponent<EnemyLooking>().lookState = EnemyLooking.LOOKSTATE.SEARCH;
				searchTrigger();
				break;
		}
	}

	void searchTrigger() {
		if(GetComponent<PathFinding>().navState != PathFinding.NAVSTATE.NAVIGATE) {
			Vector3 randomDirection = Random.insideUnitSphere * searchRadius + triggerPos;
			GetComponent<PathFinding>().navigateTo(GetComponent<PathFinding>().findNearestNavMeshPos(randomDirection));
		}
	}
}