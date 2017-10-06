using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSound : MonoBehaviour {

	public int soundPow = 4;
	private Rigidbody rb;
	private AudioSource audio;

	void Awake() {
		audio = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		if (!audio.isPlaying) {
			audio.Play();
		}
		// percentage of Speed influences the sound volume
		audio.volume = Mathf.Pow(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / GetComponent<Movement>().maxSpeed, soundPow);
	}
}
