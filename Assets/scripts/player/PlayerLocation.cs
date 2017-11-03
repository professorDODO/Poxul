using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour {
	public Transform[] PlayerArr { get; private set; }

	void Awake() {
		// keeps track of all active players
		PlayerArr = new Transform[Global.activeChildCount(transform)];
		int j = 0;
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).gameObject.activeSelf) {
				PlayerArr[j] = transform.GetChild(i);
				j++;
			}
		}
	}
}
