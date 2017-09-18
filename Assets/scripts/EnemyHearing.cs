using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {
	public Transform Player;
	public float soundVolumeRecognition = 0.04f;
	private AudioSource[] audioPlayer;

	void Awake() {
		audioPlayer = new AudioSource[Player.GetComponent<PlayerLocation>().childCount(Player)];
		for (int i = 0; i < Player.GetComponent<PlayerLocation>().childCount(Player); i++) {
			if(Player.GetChild(i).gameObject.activeSelf) {
				audioPlayer[i] = Player.GetChild(i).GetComponent<AudioSource>();
			}
		}
	}

	void Update () {
		Vector3[] pLoc = Player.GetComponent<PlayerLocation>().pLoc;
		for (int i = 0; i < pLoc.Length; i++) {
			if(1/(pLoc[i] - transform.position).magnitude * audioPlayer[i].volume >= soundVolumeRecognition) {
				GetComponent<EnemyVision>().lookAt(pLoc[i]);
			}
		}
	}
}
