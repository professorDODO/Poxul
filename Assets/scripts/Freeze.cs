using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour {

	public bool freeze = false;

	Rigidbody rb;

	bool lastFreeze;
	Vector3 lastVelo;
	Vector3 lastAngVelo;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddForceAtPosition (Vector3.right * 100, transform.position + new Vector3 (0, 0.3f, 0));
	}
	
	// Update is called once per frame
	void Update () {
		if(!lastFreeze && freeze){
			lastVelo = rb.velocity;
			lastAngVelo = rb.angularVelocity;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.isKinematic = true;
		}else if(lastFreeze && !freeze){
			rb.isKinematic = false;
			rb.velocity = lastVelo;
			rb.angularVelocity = lastAngVelo;
		}
		lastFreeze = freeze;
	}
}
