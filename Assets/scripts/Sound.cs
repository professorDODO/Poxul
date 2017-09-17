using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	private Rigidbody rb;
	private AudioSource audio;

	void Start()
	{
		audio = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying) {
				audio.Play ();
		}
		audio.volume = Mathf.Pow(rb.velocity.magnitude / GetComponent<Movement> ().maxSpeed, 4);
	}
}
