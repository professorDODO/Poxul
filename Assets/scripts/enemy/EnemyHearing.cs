using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {
	//public Transform Player;
	public float soundVolumeRecognition = 0.04f;
	public float regard = 50f; //factor of alertness-increase when this sense is trigered
	private Transform Self;
	private Transform[] PlayerArr;
	private AudioSource[] audioPlayer;
	private bool[] noticedPlayer;
	private Quaternion lookDir;

	void Awake() {
		Self = transform.parent.parent;
		PlayerArr = Self.GetComponent<EnemyBrain>().Player.GetComponent<PlayerLocation>().PlayerArr;
		audioPlayer = new AudioSource[PlayerArr.Length];
		for (int i = 0; i < audioPlayer.Length; i++) {
			audioPlayer[i] = PlayerArr[i].GetComponent<AudioSource>(); 
		}
	}

	void Start() {
		lookDir = transform.rotation;
	}

	void Update() {
		noticedPlayer = new bool[PlayerArr.Length];
		for (int i = 0; i < PlayerArr.Length; i++) {
			if (listeningVolume(PlayerArr[i], audioPlayer[i]) >= soundVolumeRecognition) {
				Self.GetComponent<EnemyBrain>().senseTrigger(listeningVolume(PlayerArr[i], audioPlayer[i])
				                                                         / soundVolumeRecognition * regard);
				noticedPlayer[i] = true;
			}
		}
		GetComponentInParent<EnemyLooking>().LookAtSenseTrigger(ref PlayerArr, ref noticedPlayer, ref lookDir,
		                                         				ref Self.GetComponent<EnemyBrain>().senseState,
		                                         				EnemyBrain.SENSESTATE.HEARING);
	}

	// returns the heard volume depending on the distance
	float listeningVolume(Transform Player, AudioSource audioPlayer) {
		return 1 / (Player.position - transform.position).magnitude * audioPlayer.volume;
	}
}
