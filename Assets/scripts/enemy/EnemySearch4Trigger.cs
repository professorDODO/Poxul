using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch4Trigger : MonoBehaviour {


	void Awake() {
		
	}

	void Update() {
		if (GetComponent<EnemyBrain>().taskState == EnemyBrain.TASKSTATE.LOOKING4TRIGGER) {
			
		}
	}
}
