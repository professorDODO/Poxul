using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : Freeze {

	Input inScr;

	[HideInInspector] public Vector3 lastVelo;
	[HideInInspector] public Vector3 lastAngVelo;

	// Use this for initialization
	void Start () {
		fmScr = fmObj.GetComponent<FightManager>();
		fmScr.fPlayer.Add(this);
		inScr = this.GetComponent<Input>();
		rb = GetComponent<Rigidbody> ();
	}

	public override void SpecialFreeze(){
		inScr.freeze = true;
	}

	public override void SpecialUnfreeze(){
		inScr.freeze = false;
	}
}
