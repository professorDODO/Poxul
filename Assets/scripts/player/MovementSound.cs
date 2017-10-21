using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSound : MonoBehaviour {

	public int soundPow = 4;
	public float minVelocity = 0.1f;
	private Rigidbody rb;
	private new AudioSource audio;

	void Awake() {
		audio = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		walkingSound();
	}

	void walkingSound() {
		if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= minVelocity) {
			if (!audio.isPlaying) {
				audio.Play();
			}
		} else {
			audio.Stop();
		}
		// percentage of Speed influences the sound volume
		audio.volume = Mathf.Pow(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude
		                         / GetComponent<MovementToMerge>().maxSpeed, soundPow);
	}
}
