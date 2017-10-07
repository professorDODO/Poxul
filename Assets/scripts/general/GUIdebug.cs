using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIdebug : MonoBehaviour {

	private void OnGUI() {
		GUI.contentColor = Color.green;
		for (int i = 0; i < Global.debugValList.Count; i++) {
			GUI.Label(new Rect(10, i * 12, 1000, 20), Global.debugValList[i]);
		}
	}
}
