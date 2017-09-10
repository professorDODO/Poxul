using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class middleLoc : MonoBehaviour {

	public Transform P;
	[HideInInspector]public Vector3 camForward;
	[HideInInspector]public Vector3 camRight;
	
	// Update is called once per frame
	void Update () {
		Vector3[] pLoc = new Vector3[P.childCount];
		for (int i = 0; i < P.childCount; ++i) {
			pLoc[i] = P.GetChild(i).position;
		}
		// midpos of all Players
		Vector3 middle = new Vector3(0,0,0);
		for (int i = 0; i < pLoc.Length; i++) {
			middle += pLoc [i];
		}
		middle = middle / pLoc.Length;
		transform.position = middle;
	}
}
