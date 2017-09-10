using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_StatChange : MonoBehaviour {

	public GameObject character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		character.GetComponent<CharStats> ().StatChange (CharStats.STATS.PAIN, 1);
	}
}
