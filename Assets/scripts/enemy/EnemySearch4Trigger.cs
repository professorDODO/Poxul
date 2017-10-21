using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch4Trigger : MonoBehaviour {
	[HideInInspector] public Transform Trigger;

	void Update() {
		switch (GetComponent<EnemyBrain>().taskState) {
			case EnemyBrain.TASKSTATE.APROACH4TRIGGER:
				break;
			case EnemyBrain.TASKSTATE.SEARCH:
				break;
		}
	}
}
