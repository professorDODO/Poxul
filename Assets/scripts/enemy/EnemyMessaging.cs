using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMessaging : MonoBehaviour {
	[SerializeField] private float shoutRadius = 100;

	public void shout(Vector3 triggerPos) {
		Collider[] nearbyEnemyCol = Physics.OverlapSphere(transform.position, shoutRadius,
		                                                  1 << LayerMask.NameToLayer("Enemy"));
		for (int i = 0; i < nearbyEnemyCol.Length; i++) {
			EnemyMessaging enMsg = nearbyEnemyCol[i].gameObject.GetComponent<EnemyMessaging>();
			try {
				if(enMsg) {
					enMsg.receive(triggerPos);
				}
			} catch {
				throw new ArgumentNullException();
			}
		}
	}

	public void receive(Vector3 triggerPos) {
		GetComponent<EnemyBrain>().setMinAlertState(EnemyBrain.ALERTSTATE.ALERTNESS1);
		GetComponent<EnemyBrain>().handleTrigger(triggerPos);
	}
}
