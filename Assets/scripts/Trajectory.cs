using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour {

	public GameObject trajPoint;
	[SerializeField] int numPoints = 5;
	[SerializeField] int delay = 15;
	LineRenderer path;

	// Use this for initialization
	void Start () {
		path = gameObject.GetComponent<LineRenderer> ();
		path.SetVertexCount (numPoints);
		path.enabled = false;
	}

	public void RenderTrajectory(Vector3 startVelo){
		path.enabled = true;
		Vector3 position = transform.position;
		Vector3 velo = startVelo;
		for(int iii = 0; iii < numPoints; iii++){
			path.SetPosition (iii, position);
			velo += Physics.gravity * Time.fixedDeltaTime / delay;
			position += velo * Time.fixedDeltaTime / delay;
		}
	}
}
