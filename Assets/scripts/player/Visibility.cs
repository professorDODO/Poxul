using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {
	public Transform LightSource;
	private Transform[] VPnt;
	public float minIntensity = 15f;


	void Start() {
		VPnt = new Transform[childCount(transform)];
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).gameObject.activeSelf) {
				VPnt[i] = transform.GetChild(i);
			}
		}
	}

	void Update() {
		float localIntensity = 0;
		for (int i = 0; i < VPnt.Length; i++) {
			localIntensity += VPnt[i].GetComponent<VisibilityPoint>().localIntensity;
		}
	}

	int childCount(Transform GO) {
		int childCount = 0;
		for (int i = 0; i < GO.childCount; i++) {
			if (GO.GetChild(i).gameObject.activeSelf) {
				childCount++;
			}
		}
		return childCount;
	}

	void debugGUI(string element, float value) {
		GameObject.Find("GUI").GetComponent<debugGUI>().debugElement(element, value);
	}
}
