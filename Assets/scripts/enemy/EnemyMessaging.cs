using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMessaging : MonoBehaviour {
	[SerializeField] private float shoutRadius = 100;

	public void shout(Vector3 triggerPos) {
		if (!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
		Collider[] nearbyEnemyCol = Physics.OverlapSphere(transform.position, shoutRadius,
		                                                  1 << LayerMask.NameToLayer("Enemy"));
		for (int i = 0; i < nearbyEnemyCol.Length; i++) {
			if (nearbyEnemyCol[i].gameObject.GetComponent<EnemyMessaging>() && nearbyEnemyCol[i].gameObject != gameObject) {
				nearbyEnemyCol[i].gameObject.GetComponent<EnemyMessaging>().receive(triggerPos);
			}
		}
	}

	public void receive(Vector3 triggerPos) {
		GetComponent<EnemyBrain>().setMinAlertState(EnemyBrain.ALERTSTATE.ALERTNESS1);
		GetComponent<EnemyBrain>().handleTrigger(triggerPos);
	}
}