using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour {

	public int soundPow = 4;
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
		audio.volume = Mathf.Pow((rb.velocity.x + rb.velocity.z) / GetComponent<Movement>().maxSpeed, soundPow);
	}
}
