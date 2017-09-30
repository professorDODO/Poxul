using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {
	public Transform LightSource;
	private Transform[] LghtSrc;
	private Transform[] VPnt;
	private bool[] isSeenArr;
	public bool isVisible {get; private set;}
	public float minIntensity = 15f;


	void Start () {
		VPnt = new Transform[childCount(transform)];
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).gameObject.activeSelf) {
				VPnt [i] = transform.GetChild (i);
			}
		}
		LghtSrc = new Transform[childCount(LightSource)];
		for (int i = 0; i < LightSource.childCount; i++) {
			if(LightSource.GetChild(i).gameObject.activeSelf) {
				LghtSrc [i] = LightSource.GetChild (i);
			}
		}
		isSeenArr = new bool[VPnt.Length];
	}

	void Update () {
		for (int i = 0; i < VPnt.Length; i++) {
			for (int j = 0; j < LghtSrc.Length; j++) {
				isSeenArr [i] = visibilityCheck (VPnt [i], LghtSrc [j], minIntensity);
			}
		}
		isVisible = false;
		for (int i = 0; i < isSeenArr.Length; i++) {
			if (isSeenArr [i]) {isVisible = true;}
		}
		debugGUI ("isVisible P" + transform.parent.GetComponent<CharStats>().playerNumber.ToString(), isVisible?1:0);
	}

	//returns false, when the player is in a shadow or too far away from a light source (range and intensity multiplicator included)
	private bool visibilityCheck(Transform VPnt, Transform LghtSrc, float minIntensity){
		if (LghtSrc.GetComponent<Light> ().intensity * Mathf.Pow (LghtSrc.GetComponent<Light> ().range / (LghtSrc.position - VPnt.position).magnitude, 2) > minIntensity) {
			RaycastHit hit;
			if (Physics.Raycast (VPnt.position, LghtSrc.position - VPnt.position, out hit, (LghtSrc.position - VPnt.position).magnitude + 1)) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Enviroment")) {
					return false;
				}
			}
		} else {
			return false;
		}
		return true;
	}

	private int childCount(Transform GO) {
		int childCount = 0;
		for (int i = 0; i < GO.childCount; i++) {
			if (GO.GetChild (i).gameObject.activeSelf) {
				childCount++;
			}
		}
		return childCount;
	}

	void debugGUI(string element, float value){
		GameObject.Find ("GUI").GetComponent<debugGUI> ().debugElement (element, value);
	}
}
