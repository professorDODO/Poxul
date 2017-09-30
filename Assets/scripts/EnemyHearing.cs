﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {
	public Transform Player;
	public float soundVolumeRecognition = 0.04f;
	private AudioSource[] audioPlayer;
	public float regard = 50f; //factor of alertness-increase when this sense is trigered
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
		Transform[] PlayerArr = Player.GetComponent<PlayerLocation>().PlayerArr;
		noticedPlayer = new bool[PlayerArr.Length];
		for (int i = 0; i < PlayerArr.Length; i++) {
			if(1/(PlayerArr[i].position - transform.position).magnitude * audioPlayer[i].volume >= soundVolumeRecognition) {
				transform.parent.GetComponent<EnemyBrain>().senseTrigger(regard);
				noticedPlayer [i] = true;
			}
		}
		GetComponent<EnemyVision> ().handleLookAt (ref PlayerArr, ref noticedPlayer, ref lookDir, ref transform.parent.GetComponent<EnemyBrain>().senseState, EnemyBrain.SENSESTATE.HEARING);
	}

	void debugGUI(string element, float value){
		GameObject.Find ("GUI").GetComponent<debugGUI> ().debugElement (element, value);
	}
}
