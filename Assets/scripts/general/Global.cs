﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	//this part will be at the fightManager
	[HideInInspector] public enum FIGHTSTATE {
		NONE,
		FIGHTPREP,
		FIGHT};
	// sorted in proirity order
	[HideInInspector] public static FIGHTSTATE fightState;
	[HideInInspector] public static List<Transform> FightParticipants = new List<Transform>();
	public static Transform Player;

	public static void joinFight(Transform Enemy) {
		Transform[] PlayerArr = Player.GetComponent<PlayerLocation>().PlayerArr;
		if(fightState == FIGHTSTATE.NONE) {
			for (int i = 0; i < PlayerArr.Length; i++) {
				FightParticipants.Add(PlayerArr[i]);
			}
			fightState = FIGHTSTATE.FIGHTPREP;
		}
		if(fightState == FIGHTSTATE.FIGHTPREP) {
			if (!FightParticipants.Contains(Enemy)) {
				FightParticipants.Add(Enemy);
			}
		}
	}


	// counts active childs
	public static int activeChildCount(Transform GO) {
		int childCount = 0;
		for (int i = 0; i < GO.childCount; i++) {
			if (GO.GetChild(i).gameObject.activeSelf) {
				childCount++;
			}
		}
		return childCount;
	}


	// Angles relative in a plane
	public static float angleInPlane(Transform from, Vector3 to, Vector3 planeNormal) {
		Vector3 dir = to - from.position;
		Vector3 p1 = project(dir, planeNormal);
		Vector3 p2 = project(from.forward, planeNormal);
		return Vector3.Angle(p1, p2);
	}

	private static Vector3 project(Vector3 v, Vector3 onto) {
		return v - (Vector3.Dot(v, onto) / Vector3.Dot(onto, onto)) * onto;
	}


	// handles promts which will be displayed on screen
	public static List<string> debugList = new List<string>();
	public static List<string> debugValList = new List<string>();

	public static void debugGUI(string element, object value) {
		if (!debugList.Contains(element)) {
			debugList.Add(element);
			debugValList.Add(element + ": " + value.ToString());
		} else {
			debugValList[debugList.IndexOf(element)] = element + ": " + value.ToString();
		}
	}
}
