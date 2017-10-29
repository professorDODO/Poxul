using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {
	[SerializeField] private Transform Enemy;
	private Transform[] EnemyArr;

	void Awake() {
		EnemyArr = new Transform[Global.activeChildCount(Enemy)];
		int j = 0;
		for (int i = 0; i < Enemy.childCount; i++) {
			if (Enemy.GetChild(i).gameObject.activeSelf) {
				EnemyArr[j] = Enemy.GetChild(i);
				j++;
			}
		}
	}

	public void triggerEnemies() {
		for (int i = 0; i < EnemyArr.Length; i++) {
			EnemyArr[i].GetComponent<EnemyBrain>().Senses.GetComponent<EnemyVision>()
				.nonPlayerVisionTrigger(transform.position, 20);
			if (EnemyArr[i].GetComponent<EnemyBrain>().senseState == EnemyBrain.SENSESTATE.SEEING) {
				EnemyArr[i].GetComponent<EnemyBrain>().setMinAlertState(EnemyBrain.ALERTSTATE.ALERTNESS1);
				EnemyArr[i].GetComponent<EnemyBrain>().handleTrigger(transform.position);
				EnemyArr[i].GetComponent<EnemyMessaging>().shout(transform.position);
			}
		}
	}
}
