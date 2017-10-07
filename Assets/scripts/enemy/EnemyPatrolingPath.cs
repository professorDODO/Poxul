using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolingPath : MonoBehaviour {
	public Transform Path;
	private Transform[] WayPnts; //TODO: CREATE A SCRIPT ON PATH WHICH OFFERS THE NEXT POINT
	public float speed = 10f;
	public float reachPntAcc = 1f;
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
		if ((transform.position - WayPnts[nextWayPntIndex].position).magnitude <= reachPntAcc) {
			nextWayPntIndex++;
			if (nextWayPntIndex > WayPnts.Length - 1) {
				nextWayPntIndex = 0;
			}
			transform.GetComponent<EnemyLooking>().changeDefaultRotation(Quaternion.LookRotation(WayPnts[nextWayPntIndex].position - transform.position));
		}
		moveTo(nextWayPntIndex);	
	}

	void moveTo(int i) {
		transform.position = Vector3.MoveTowards(transform.position, WayPnts[i].position, speed * Time.deltaTime);
	}
}
