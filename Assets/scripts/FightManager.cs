using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour {

	// parameters
	[SerializeField] float prepTime = 2;
	[SerializeField] float inputTime = 5;
	[SerializeField] float actionTime = 10;

	public enum FIGHTSTATES{
		NONE,
		FIGHTPREP,
		INPUT,
		ACTION
	};

	public FIGHTSTATES state = FIGHTSTATES.NONE;
	[HideInInspector] public FIGHTSTATES lastState;

	public List<FreezePlayer> fPlayer = new List<FreezePlayer>();

	float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log(state);
		Debug.Log(timer);
		
		if(lastState != FIGHTSTATES.FIGHTPREP && state == FIGHTSTATES.FIGHTPREP){
			timer = prepTime;
			foreach(FreezePlayer fp in fPlayer){
				fp.Freezing();
			}
		}else if(lastState != FIGHTSTATES.INPUT && state == FIGHTSTATES.INPUT){
			timer = inputTime;
		}else if(lastState != FIGHTSTATES.ACTION && state == FIGHTSTATES.ACTION){
			timer = actionTime;
			foreach(FreezePlayer fp in fPlayer){
				fp.Unfreezing();
			}
		}

		timer -= Time.deltaTime;
		lastState = state;

		/*
		switch(state){
			case FIGHTSTATES.ACTION:
				break;
			case FIGHTSTATES.INPUT:
				if(timer <= 0){
					state = FIGHTSTATES.ACTION;
				}
				break;
			case FIGHTSTATES.FIGHTPREP:
				if(timer <= 0){
					state = FIGHTSTATES.INPUT;
				}
				break;
			case FIGHTSTATES.NONE:
				break;
		}
		*/
		if(state == FIGHTSTATES.ACTION){
			
		}else if(state == FIGHTSTATES.INPUT){
			if(timer <= 0){
				state = FIGHTSTATES.ACTION;
			}
		}else if(state == FIGHTSTATES.FIGHTPREP){
			if(timer <= 0){
				state = FIGHTSTATES.INPUT;
			}
		}else if(state == FIGHTSTATES.NONE){
			
		}
	}
}
