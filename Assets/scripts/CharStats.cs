using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour {

	// ENUMS

	public enum TYPE{
		PLAYER,
		ENEMY,
		OBJECT
	};

	public enum STATS{
		HEALTH,
		PAIN,
		STRENGTH,
		SPEED,
	};

	// STATS

	public TYPE type;
	public int playerNumber;


	// default stat variables
	int[] defStats = new int[4];	// adjust array length if needed

	// dynamic stat variables
	[HideInInspector]
	public float[] dynStats = new float[4];	// adjust array length if needed

	// METHODS
	public void StatChange(STATS stat, float change){
		dynStats [(int)stat] += change;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//StatChange (STATS.HEALTH, 2);
		Debug.Log (dynStats [(int)STATS.PAIN]);
	}
}
