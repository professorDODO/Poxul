using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLoc : MonoBehaviour {

	public Transform Player;

	void Update () {
		// midpos of all Players
		Vector3[] pLoc = Player.GetComponent<PlayerLocation>().pLoc;
		Vector3 middle = new Vector3(0,0,0);
		for (int i = 0; i < pLoc.Length; i++) {
			middle += pLoc [i];
		}
		middle = middle / pLoc.Length;
		transform.position = middle;
	}
}
