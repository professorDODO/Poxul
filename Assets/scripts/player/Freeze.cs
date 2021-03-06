using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour {

	public GameObject fmObj;
	[HideInInspector] public FightManager fmScr;
	[HideInInspector] public Rigidbody rb;

	// Use this for initialization
	void Start () {
		
	}
	
	public void FreezeMove(Rigidbody rb, Vector3 lastVelo, Vector3 lastAngVelo){
		lastVelo = rb.velocity;
		lastAngVelo = rb.angularVelocity;
		rb.isKinematic = true;
	}

	public void UnfreezeMove(Rigidbody rb, Vector3 lastVelo, Vector3 lastAngVelo){
		rb.isKinematic = false;
		rb.velocity = lastVelo;
		rb.angularVelocity = lastAngVelo;
	}
}
