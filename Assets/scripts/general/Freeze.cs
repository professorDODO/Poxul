using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FreezeInstances{

}

public class Freeze : MonoBehaviour {

	public GameObject fmObj;
	[HideInInspector] public FightManager fmScr;
	[HideInInspector] public Rigidbody rb;

	[HideInInspector] public Vector3 lastVelo;
	[HideInInspector] public Vector3 lastAngVelo;


	// Use this for initialization
	void Start () {
		
	}

	public void InitFreeze(FreezePlayer thisGO){
		fmScr = fmObj.GetComponent<FightManager>();
		fmScr.freezeList.Add(thisGO);
		rb = GetComponent<Rigidbody> ();
	}

	public void InitFreeze(FreezeEnemy thisGO){
		fmScr = fmObj.GetComponent<FightManager>();
		fmScr.freezeList.Add(thisGO);
		rb = GetComponent<Rigidbody> ();
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

	public virtual void SpecialFreeze(){
		
	}

	public virtual void SpecialUnfreeze(){
		
	}
}
