using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {
	//public Transform Player;
	public float soundVolumeRecognition = 0.04f;
	public float regard = 50f; //factor of alertness-increase when this sense is trigered
	public bool weightRegardByPlayerNumber = true;
	private Transform Self;
	private Transform[] PlayerArr;
	private AudioSource[] audioPlayer;
	public bool[] noticedPlayer {get; private set;}

	void Awake() {
		Self = transform.parent.parent;
		PlayerArr = Self.GetComponent<EnemyBrain>().Player.GetComponent<PlayerLocation>().PlayerArr;
		if (weightRegardByPlayerNumber) {
			regard = regard / PlayerArr.Length;
		}
		audioPlayer = new AudioSource[PlayerArr.Length];
		for (int i = 0; i < audioPlayer.Length; i++) {
			audioPlayer[i] = PlayerArr[i].GetComponent<AudioSource>(); 
		}
		noticedPlayer = new bool[PlayerArr.Length];
	}

	void Update() {
		for (int i = 0; i < PlayerArr.Length; i++) {
			if (listeningVolume(PlayerArr[i], audioPlayer[i]) >= soundVolumeRecognition) {
				if (EnemyBrain.SENSESTATE.HEARING >= Self.GetComponent<EnemyBrain>().senseState) {
					noticedPlayer[i] = true;
					Self.GetComponent<EnemyBrain>().senseState = EnemyBrain.SENSESTATE.HEARING;
				}
				Self.GetComponent<EnemyBrain>().senseTrigger(listeningVolume(PlayerArr[i], audioPlayer[i])
				                                                         / soundVolumeRecognition * regard);
			}
		}
		if (Self.GetComponent<EnemyBrain>().alertState >= EnemyBrain.ALERTSTATE.ALERTNESS1) {
			Self.GetComponent<EnemyBrain>().sensedPlayerIndex(PlayerArr, noticedPlayer);
		}
	}
		
	void LateUpdate() {
		for (int i = 0; i < noticedPlayer.Length; i++) {
			noticedPlayer[i] = false;
		}
	}

	// returns the heard volume depending on the distance
	float listeningVolume(Transform Player, AudioSource audioPlayer) {
		return 1 / (Player.position - transform.position).magnitude * audioPlayer.volume;
	}
}
