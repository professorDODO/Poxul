using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {
	public Transform Player;
	public float soundVolumeRecognition = 0.04f;
	private AudioSource[] audioPlayer;
	public float regard = 50f;
	private bool[] noticedPlayer;
	private Quaternion lookDir;

	void Awake() {
		audioPlayer = new AudioSource[Player.GetComponent<PlayerLocation>().childCount(Player)];
		for (int i = 0; i < Player.GetComponent<PlayerLocation>().childCount(Player); i++) {
			if(Player.GetChild(i).gameObject.activeSelf) {
				audioPlayer[i] = Player.GetChild(i).GetComponent<AudioSource>();
			}
		}
	}

	void Start(){
		lookDir = transform.rotation;
	}

	void Update () {
		Vector3[] pLoc = Player.GetComponent<PlayerLocation>().pLoc;
		noticedPlayer = new bool[pLoc.Length];
		for (int i = 0; i < pLoc.Length; i++) {
			if(1/(pLoc[i] - transform.position).magnitude * audioPlayer[i].volume >= soundVolumeRecognition) {
				transform.parent.GetComponent<EnemyBrain>().senseTrigger(regard);
				noticedPlayer [i] = true;
			}
		}
		bool allFalse = true;
		for (int i = 0; i < noticedPlayer.Length; i++) {
			if (noticedPlayer [i]) {
				allFalse = false;
			}
		}
		if (GetComponent<EnemyVision> ().senseState != EnemyVision.SENSESTATE.SEEING) {
			if (!allFalse) {
				GetComponent<EnemyVision> ().senseState = EnemyVision.SENSESTATE.HEARING;
				lookDir = GetComponent<EnemyVision> ().LastLocDir (pLoc, noticedPlayer);
			}		
			transform.parent.rotation = Quaternion.Slerp (transform.rotation, lookDir, Time.deltaTime * GetComponent<EnemyVision> ().lookAroundSpeed);
		}
		if (allFalse && Quaternion.Angle (lookDir, transform.rotation) < GetComponent<EnemyVision> ().lookDirAcc) {
			GetComponent<EnemyVision> ().senseState = EnemyVision.SENSESTATE.NONE;
		}
		for (int i = 0; i < noticedPlayer.Length; i++) {
			noticedPlayer [i] = false;
		}
	}

	void debugGUI(string element, float value){
		GameObject.Find ("GUI").GetComponent<debugGUI> ().debugElement (element, value);
	}
}
