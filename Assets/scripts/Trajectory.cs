using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour {

	public GameObject trajPoint;

	// Use this for initialization
	void Start () {
		//test
		//RenderTrajectory(5, 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RenderTrajectory(float hor, float ver){
		for(int iii = 0; iii <= hor; iii++){
			Vector3 position = transform.up * (-(4 * ver / (hor * hor)) * Mathf.Pow ((iii - (hor / 2)), 2) + ver) + transform.forward * iii;
			GameObject traj = Instantiate (trajPoint, this.transform) as GameObject;
			traj.transform.position = transform.position + position;
		}
	}
}
