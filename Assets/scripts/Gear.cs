using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour {

	public enum GEAR{
		HEAD,
		CHEST,
		HAND_LEFT,
		HAND_RIGHT,
		LEGS
	};

	[HideInInspector]
	public GameObject[] gearSlots = new GameObject[5];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		GameObject temp = other.gameObject;
		if(temp.layer == LayerMask.NameToLayer("Gear")){
			GEAR tempType = temp.GetComponent<GearStats> ().type;
			gearSlots [(int)tempType] = temp;
			temp.transform.parent = transform;
			temp.transform.position = transform.position;
			temp.transform.localPosition += new Vector3 (0, 1, 0);
			Debug.Log (gearSlots [(int)tempType].name);
		}
	}
}
