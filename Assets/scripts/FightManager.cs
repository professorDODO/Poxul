using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour {

	// parameters
	[SerializeField] float prepTime = 2;
	[SerializeField] float inputTime = 5;
	[SerializeField] float actionTime = 10;

	public enum FIGHTSTATE{
		NONE,
		FIGHTPREP,
		INPUT,
		ACTION
	};

	public FIGHTSTATE fightState = FIGHTSTATE.NONE;
	[HideInInspector] public FIGHTSTATE lastState;

	[HideInInspector] public  List<Transform> FightParticipants = new List<Transform>();
	public Transform Player;

	public Transform[] PlayerArr;

	public void joinFight(Transform Enemy) {
		if(fightState == FIGHTSTATE.NONE) {
			for (int i = 0; i < PlayerArr.Length; i++) {
				FightParticipants.Add(PlayerArr[i]);
			}
			fightState = FIGHTSTATE.FIGHTPREP;
		}
		if(fightState == FIGHTSTATE.FIGHTPREP) {
			if (!FightParticipants.Contains(Enemy)) {
				FightParticipants.Add(Enemy);
			}
		}
	}

	public List<FreezeInstances> freezeList = new List<FreezeInstances>();

	float timer;

	void Awake(){
		PlayerArr = Player.GetComponent<PlayerLocation>().PlayerArr;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log(fightState);
		Debug.Log(timer);
		
		if(lastState != FIGHTSTATE.FIGHTPREP && fightState == FIGHTSTATE.FIGHTPREP){
			timer = prepTime;
			foreach(FreezePlayer fp in freezeList){
				fp.FreezeMove(fp.rb, fp.lastVelo, fp.lastAngVelo);
				fp.SpecialFreeze();
			}
		}else if(lastState != FIGHTSTATE.INPUT && fightState == FIGHTSTATE.INPUT){
			timer = inputTime;
		}else if(lastState != FIGHTSTATE.ACTION && fightState == FIGHTSTATE.ACTION){
			timer = actionTime;
			foreach(FreezePlayer fp in freezeList){
				fp.UnfreezeMove(fp.rb, fp.lastVelo, fp.lastAngVelo);
				fp.SpecialUnfreeze();
			}
		}

		timer -= Time.deltaTime;
		lastState = fightState;

		/*
		switch(fightState){
			case FIGHTSTATE.ACTION:
				break;
			case FIGHTSTATE.INPUT:
				if(timer <= 0){
					fightState = FIGHTSTATE.ACTION;
				}
				break;
			case FIGHTSTATE.FIGHTPREP:
				if(timer <= 0){
					fightState = FIGHTSTATE.INPUT;
				}
				break;
			case FIGHTSTATE.NONE:
				break;
		}
		*/
		if(fightState == FIGHTSTATE.ACTION){
			
		}else if(fightState == FIGHTSTATE.INPUT){
			if(timer <= 0){
				fightState = FIGHTSTATE.ACTION;
			}
		}else if(fightState == FIGHTSTATE.FIGHTPREP){
			if(timer <= 0){
				fightState = FIGHTSTATE.INPUT;
			}
		}else if(fightState == FIGHTSTATE.NONE){
			
		}

		if (freezeList != null) {
			for (int i = 0; i < freezeList.Count; i++) {
				Global.debugGUI("FightParticipant #" + i.ToString(), freezeList[i]);
			}
		}
	}
}
