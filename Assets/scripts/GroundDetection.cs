using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(){
		player.GetComponent<Movement>().jumpStates = Movement.JUMPSTATES.GROUNDED;
	}

	void OnTriggerStay(Collider other){
		
	}

	void OnTriggerExit(){
		
	}
}
