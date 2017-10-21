using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPoint : MonoBehaviour {

	private Transform[] LghtSrc;
	public float localIntensity { get; private set; }

	void Start () {
		LghtSrc = new Transform[Global.childCount(transform.parent.GetComponent<Visibility>().LightSource)];
		int j = 0;
		for (int i = 0; i < transform.parent.GetComponent<Visibility>().LightSource.childCount; i++) {
			if (transform.parent.GetComponent<Visibility>().LightSource.GetChild(i).gameObject.activeSelf) {
				LghtSrc[j] = transform.parent.GetComponent<Visibility>().LightSource.GetChild(i);
				j++;
			}
		}
	}

	void Update () {
		localIntensity = calculateLocalIntensity();
	}

	// returns the local intensity depending on lightsource.intensity, lightsource.range and distance 
	float calculateLocalIntensity() {
		localIntensity = 0f;
		for (int i = 0; i < LghtSrc.Length; i++) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, LghtSrc[i].position - transform.position,
			                    out hit, (LghtSrc[i].position - transform.position).magnitude)) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("LightSource")) {
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
}