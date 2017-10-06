using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour {
	//public Transform Player;
	public float soundVolumeRecognition = 0.04f;
	private AudioSource[] audioPlayer;
	public float regard = 50f;
	//factor of alertness-increase when this sense is trigered
	private Transform[] PlayerArr;
	private bool[] noticedPlayer;
	private Quaternion lookDir;

	void Awake() {
		PlayerArr = transform.parent.GetComponent<EnemyBrain>().Player.GetComponent<PlayerLocation>().PlayerArr;
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
				transform.parent.GetComponent<EnemyBrain>().senseTrigger(listeningVolume(PlayerArr[i], audioPlayer[i])
				                                                         / soundVolumeRecognition * regard);
				noticedPlayer[i] = true;
			}
		}
		transform.parent.GetComponent<EnemyLooking>().LookAtSenseTrigger(ref PlayerArr, ref noticedPlayer, ref lookDir,
		                                         						 ref transform.parent.GetComponent<EnemyBrain>().senseState,
		                                         						 EnemyBrain.SENSESTATE.HEARING);
	}

	float listeningVolume(Transform Player, AudioSource audioPlayer) {
		return 1 / (Player.position - transform.position).magnitude * audioPlayer.volume;
	}

	void debugGUI(string element, float value) {
		GameObject.Find("GUI").GetComponent<debugGUI>().debugElement(element, value);
	}
}
