using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {

	public float fovHor = 70;
	public float fovVer	= 50;
	public float viewRange = 10;
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmosSelected()
	{
		Quaternion topLeftRayRotation = Quaternion.AngleAxis(-fovHor/2, Vector3.up);
		topLeftRayRotation = Quaternion.AngleAxis(-fovVer/2, Vector3.right);
		Quaternion topRightRayRotation = Quaternion.AngleAxis(fovHor/2, Vector3.up);
		topRightRayRotation = Quaternion.AngleAxis(-fovVer/2, Vector3.right);
		Vector3 topLeftRayDirection = topLeftRayRotation * transform.up;
		Vector3 rightRayDirection = topRightRayRotation * transform.up;
		Gizmos.DrawRay(transform.position, topLeftRayDirection * viewRange);
		Gizmos.DrawRay(transform.position, rightRayDirection * viewRange);
	}
}
