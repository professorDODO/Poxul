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

	void OnTriggerExit(){
		player.GetComponent<Movement> ().isGrounded = false;
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject.layer != LayerMask.NameToLayer("Player")){
			player.GetComponent<Movement> ().isGrounded = true;
		}
	}
}
