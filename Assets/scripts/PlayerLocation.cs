using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour {

	public Vector3[] pLoc;

	void Start(){
		pLoc = new Vector3[childCount(transform)];
	}

	void Update () {
		pLoc = new Vector3[childCount(transform)];
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).gameObject.activeSelf) {
				pLoc[i] = transform.GetChild(i).position;
			}
		}
	}

	public int childCount(Transform Player) {
		int childCount = 0;
		for (int i = 0; i < Player.childCount; i++) {
			if(Player.GetChild(i).gameObject.activeSelf) {
				childCount++;
			}
		}
		return childCount;
	}
}
