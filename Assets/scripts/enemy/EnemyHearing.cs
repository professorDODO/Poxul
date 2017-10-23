using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {
	//public Transform Player;
	[SerializeField] private float recognizedVolumeThreshhold = 0.04f;
	[SerializeField] private float regard = 50f; //factor of alertness-increase when this sense is trigered
	private Transform Self;
	private Transform[] PlayerArr;

	void Awake() {
		Self = transform.parent.parent;
		PlayerArr = Self.GetComponent<EnemyBrain>().Player.GetComponent<PlayerLocation>().PlayerArr;
	}

	void Update() {
		bool[] noticedPlayer = new bool[PlayerArr.Length];
		for (int i = 0; i < PlayerArr.Length; i++) {
			if (listeningVolume(PlayerArr[i]) >= recognizedVolumeThreshhold) {
				if (EnemyBrain.SENSESTATE.HEARING >= Self.GetComponent<EnemyBrain>().senseState) {
					noticedPlayer[i] = true;
					Self.GetComponent<EnemyBrain>().senseState = EnemyBrain.SENSESTATE.HEARING;
				}
				Self.GetComponent<EnemyBrain>().senseTrigger(listeningVolume(PlayerArr[i])
															 / recognizedVolumeThreshhold * regard);
			}
			if (Self.GetComponent<EnemyBrain>().alertState >= EnemyBrain.ALERTSTATE.ALERTNESS1) {
				Self.GetComponent<EnemyBrain>().sensedPlayerIndex(PlayerArr, noticedPlayer);
			}
		}
	}

	// returns the heard volume depending on the distance
	float listeningVolume(Transform Player) {
		return Player.GetComponent<AudioSource>().volume * 1 / (Player.position - transform.position).magnitude;
	}
}
