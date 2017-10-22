using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLoc : MonoBehaviour {

	public Transform Player;

	void Update() {
		// midpos of all Players
		Transform[] PlayerArr = Player.GetComponent<PlayerLocation>().PlayerArr;
		Vector3 middle = new Vector3(0, 0, 0);
		for (int i = 0; i < PlayerArr.Length; i++) {
			middle += PlayerArr[i].position;
		}
		middle = middle / PlayerArr.Length;
		transform.position = middle;
	}
}
