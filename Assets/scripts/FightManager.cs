using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour {

	public bool freeze;
	public bool lastFreeze;

	public List<FreezePlayer> fPlayer = new List<FreezePlayer>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!lastFreeze && freeze){
			foreach(FreezePlayer fp in fPlayer){
				fp.Freeze();
			}
		}else if(lastFreeze && !freeze){
			foreach(FreezePlayer fp in fPlayer){
				fp.Unfreeze();
			}
		}
		lastFreeze = freeze;
	}
}
