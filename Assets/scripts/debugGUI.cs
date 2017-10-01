using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGUI : MonoBehaviour {
	public List<string> debugList = new List<string>();
	public List<string> debugValList = new List<string>();

	public void debugElement(string element, float value) {
		if (!debugList.Contains(element)) {
			debugList.Add(element);
			debugValList.Add(element + ": " + value.ToString());
		} else {
			debugValList[debugList.IndexOf(element)] = element + ": " + value.ToString();
		}
	}

	void OnGUI() {
		GUI.contentColor = Color.green;
		for (int i = 0; i < debugValList.Count; i++) {
			GUI.Label(new Rect(10, i * 12, 1000, 20), debugValList[i]);
		}
	}
}
