using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour {
	[HideInInspector] public Vector3[] pLoc;

	void Awake(){
		pLoc = new Vector3[childCount(transform)];
	}

	void Update () {
		// storing the player locations
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).gameObject.activeSelf) {
				pLoc[i] = transform.GetChild(i).position;
			}
		}
	}

	// counts only active childs
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
