using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSources : MonoBehaviour {
	private Transform[] LghtSrc;
	public float coliderRadius = 0.01f;


	void Awake() {
		// keeps track of all active visibilityPoints
		LghtSrc = new Transform[Global.childCount(transform)];
		int j = 0;
		for (int i = 0; i < LghtSrc.Length; i++) {
			if (transform.GetChild(i).gameObject.activeSelf) {
				LghtSrc[j] = transform.GetChild(i);
				LghtSrc[j].GetComponent<SphereCollider>().radius = coliderRadius;
				j++;
			}
		}
	}
}
