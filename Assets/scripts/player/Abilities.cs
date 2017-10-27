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
				EnemyArr[i] = Enemy.GetChild(i);
				j++;
			}
		}
	}

	public void triggerEnemies() {
		for (int i = 0; i < EnemyArr.Length; i++) {
			EnemyArr[i].GetComponent<EnemyVision>().nonPlayerVisionTrigger(transform.position, 4);
		}
	}
}
