using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : Freeze {

	Input inScr;

	Vector3 lastVelo;
	Vector3 lastAngVelo;

	// Use this for initialization
	void Start () {
		fmScr = fmObj.GetComponent<FightManager>();
		fmScr.fPlayer.Add(this);
		inScr = this.GetComponent<Input>();
		rb = GetComponent<Rigidbody> ();
	}
	
	public void Freezing(){
		FreezeMove(rb,lastVelo,lastAngVelo);
		inScr.freeze = true;
	}

	public void Unfreezing(){
		UnfreezeMove(rb,lastVelo,lastAngVelo);
		inScr.freeze = false;
	}
}
