using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour {
	[HideInInspector] public enum NAVSTATE {
		NONE,
		NAVIGATE,
		REACHEDGOAL};
	[HideInInspector] public NAVSTATE navState;
	private MovementToMerge Movement;
	private NavMeshAgent navAgent;
	public float navAccRadius = 0.5f;


	void Awake() {
		Movement = GetComponent<MovementToMerge>();
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.updatePosition = false;
		navAgent.updateRotation = false;
		navState = NAVSTATE.NONE;
	}

	void Update() {
		float xDir = (new Vector3(navAgent.desiredVelocity.x, 0, navAgent.desiredVelocity.z)).normalized.x;
		float yDir = (new Vector3(navAgent.desiredVelocity.x, 0, navAgent.desiredVelocity.z)).normalized.z;
		Movement.move(xDir, yDir, false);
		if ((navAgent.destination - transform.position).magnitude < navAccRadius) {
			navAgent.destination = transform.position;
			Movement.move(-xDir, -yDir, false);
			navState = NAVSTATE.REACHEDGOAL;
		}
		if (navState == NAVSTATE.NAVIGATE) {
			GetComponent<EnemyLooking>().changeDefaultRotation(Quaternion.LookRotation(navAgent.destination
			                                                                           - transform.position),
			                                                   false);
		}

		// TODO: Slowing Down before reaching the Goal

		Global.debugGUI("NAVSTATE", (float)navState);
		navAgent.nextPosition = transform.position;
	}

	public void navigateTo(Vector3 destination) {
		navState = NAVSTATE.NAVIGATE;
		navAgent.destination = destination;
	}
}
