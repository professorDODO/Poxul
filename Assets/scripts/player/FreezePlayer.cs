using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayer : Freeze, FreezeInstances{

	Input inScr;

	// Use this for initialization
	void Start () {
		inScr = this.GetComponent<Input>();
		InitFreeze(this);
	}

	public override void SpecialFreeze(){
		inScr.freeze = true;
	}

	public override void SpecialUnfreeze(){
		inScr.freeze = false;
	}
}
