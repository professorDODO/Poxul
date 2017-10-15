using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolingPath : MonoBehaviour {
	public Transform Path;
	private Transform[] WayPnts; //TODO: CREATE A SCRIPT ON PATH WHICH OFFERS THE NEXT POINT
	private int nextWayPntIndex = 0;

	void Start() {
		WayPnts = new Transform[Path.childCount];
		int j = 0;
		for (int i = 0; i < WayPnts.Length; i++) {
			if (Path.GetChild(i).gameObject.activeSelf) {
				WayPnts[j] = Path.GetChild(i);
				j++;
			}
		}
	}

	void Update () {
		if (GetComponent<PathFinding>().navState == PathFinding.NAVSTATE.REACHEDGOAL) {
			nextWayPntIndex++;
			if (nextWayPntIndex > WayPnts.Length - 1) {
				nextWayPntIndex = 0;
			}
			GetComponent<PathFinding>().navState = PathFinding.NAVSTATE.NONE;
		}
		if (GetComponent<PathFinding>().navState == PathFinding.NAVSTATE.NONE) {
			GetComponent<PathFinding>().navigateTo(WayPnts[nextWayPntIndex].position);
			GetComponent<EnemyLooking>().changeDefaultRotation(Quaternion.LookRotation(WayPnts[nextWayPntIndex].position
			                                                                           - transform.position),
			                                                   true);
		}
	}
}
