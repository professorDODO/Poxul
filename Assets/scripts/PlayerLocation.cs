using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour {
	public Transform[] PlayerArr { get; private set; }

	void Awake() {
		PlayerArr = new Transform[childCount(transform)];
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).gameObject.activeSelf) {
				PlayerArr[i] = transform.GetChild(i);
			}
		}
	}

	// counts only active childs
	public int childCount(Transform Player) {
		int childCount = 0;
		for (int i = 0; i < Player.childCount; i++) {
			if (Player.GetChild(i).gameObject.activeSelf) {
				childCount++;
			}
		}
		return childCount;
	}
}
