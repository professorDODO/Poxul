using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : MonoBehaviour {

	public GameObject fmObj;
	FightManager fmScr;

	Rigidbody rb;

	Vector3 lastVelo;
	Vector3 lastAngVelo;

	// Use this for initialization
	void Start () {
		fmScr = fmObj.GetComponent<FightManager>();
		fmScr.fPlayer.Add(this);
		rb = GetComponent<Rigidbody> ();
	}
	
	public void Freeze(){
		lastVelo = rb.velocity;
		lastAngVelo = rb.angularVelocity;
		rb.isKinematic = true;
	}

	public void Unfreeze(){
		rb.isKinematic = false;
		rb.velocity = lastVelo;
		rb.angularVelocity = lastAngVelo;
	}
}
