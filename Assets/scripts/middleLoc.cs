using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLoc : MonoBehaviour {

	public Transform P;
	
	// Update is called once per frame
	void Update () {
		Vector3[] pLoc = new Vector3[childCount(P)];
		for (int i = 0; i < P.childCount; i++) {
			if(P.GetChild(i).gameObject.activeSelf) {
				pLoc[i] = P.GetChild(i).position;
			}
		}
		// midpos of all Players
		Vector3 middle = new Vector3(0,0,0);
		for (int i = 0; i < pLoc.Length; i++) {
			middle += pLoc [i];
		}
		middle = middle / pLoc.Length;
		transform.position = middle;
	}

	public int childCount(Transform P) {
		int childCount = 0;
		for (int i = 0; i < P.childCount; i++) {
			if(P.GetChild(i).gameObject.activeSelf) {
				childCount++;
			}
		}
		return childCount;
	}
}
