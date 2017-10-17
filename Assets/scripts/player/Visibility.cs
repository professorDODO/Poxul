using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {
	public Transform LightSource;
	private Transform[] VPnt;
	public float minIntensity = 15f;
	public float coliderRadius = 0.01f;


	void Awake() {
		// keeps track of all active visibilityPoints
		VPnt = new Transform[Global.childCount(transform)];
		int j = 0; // TODO: IMPLEMENT THIS COUNTING METHOD EVERYWHERE ELSE TO IGNORE INACTIVE GO
		for (int i = 0; i < VPnt.Length; i++) {
			if (transform.GetChild(i).gameObject.activeSelf) {
				VPnt[j] = transform.GetChild(i);
				VPnt[j].GetComponent<SphereCollider>().radius = coliderRadius;
				j++;
			}
		}
	}
}
