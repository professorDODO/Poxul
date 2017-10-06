using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPoint : MonoBehaviour {
	private Transform[] LghtSrc;
	public float localIntensity { get; private set; }

	// Use this for initialization
	void Start () {
		LghtSrc = new Transform[childCount(transform.parent.GetComponent<Visibility>().LightSource)];
		for (int i = 0; i < transform.parent.GetComponent<Visibility>().LightSource.childCount; i++) {
			if (transform.parent.GetComponent<Visibility>().LightSource.GetChild(i).gameObject.activeSelf) {
				LghtSrc[i] = transform.parent.GetComponent<Visibility>().LightSource.GetChild(i);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		localIntensity = calculateLocalIntensity();
	}

	float calculateLocalIntensity() {
		localIntensity = 0f;
		for (int i = 0; i < LghtSrc.Length; i++) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, LghtSrc[i].position - transform.position,
			                    out hit, (LghtSrc[i].position - transform.position).magnitude)) {
				if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Environment")) {
					localIntensity += LghtSrc[i].GetComponent<Light>().intensity
						* Mathf.Pow(LghtSrc[i].GetComponent<Light>().range
						                          / (LghtSrc[i].position - transform.position).magnitude, 2);
				}
			} else {
				localIntensity += LghtSrc[i].GetComponent<Light>().intensity
					* Mathf.Pow(LghtSrc[i].GetComponent<Light>().range
					                          / (LghtSrc[i].position - transform.position).magnitude, 2);
			}
		}
		return localIntensity;
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