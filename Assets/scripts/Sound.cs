using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	public int soundPow = 4;
	private Rigidbody rb;
	private AudioSource audio;

	void Awake()
	{
		audio = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (!audio.isPlaying) {
				audio.Play ();
		}
		// percentage of Speed influences the sound volume
		audio.volume = Mathf.Pow((rb.velocity.x + rb.velocity.z) / GetComponent<Movement>().maxSpeed, soundPow);
	}
}
