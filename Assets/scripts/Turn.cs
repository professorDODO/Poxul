using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour {

	public Transform Player;
	public Transform Enemy;
	public Transform Ghosts;

	void Awake () {
		for (int i = 0; i < childCount (Player); i++) {
			GameObject a = new GameObject ("Ghost_" + i);
			a.transform.SetParent(Ghosts);
			a.transform.position = Player.GetChild(i).position;
			print (Player.GetChild (i).GetComponent<CharStats> ());

			//a.AddComponent<CharStats>(Player.GetChild (i).GetComponent<CharStats>());
			CharStats pff = a.AddComponent<CharStats> () as CharStats;
			//pff = Player.GetChild (i).GetComponent<CharStats> ();
			//pff.playerNumber = i;
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



	void Update () {

	}
}
