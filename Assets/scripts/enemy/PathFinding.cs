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
	public float breakRadius = 2f;
	public float navAccRadius = 0.5f;


	void Awake() {
		Movement = GetComponent<MovementToMerge>();
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.updatePosition = false;
		navAgent.updateRotation = false;
		navState = NAVSTATE.NONE;
	}

	void Update() {
		if (navState == NAVSTATE.NAVIGATE) {
			executeNavigation();
		}
	}

	private void executeNavigation(){
		Vector2 inputVec = new Vector2((new Vector3(navAgent.desiredVelocity.x, 0,
		                                            navAgent.desiredVelocity.z)).normalized.x,
		                               (new Vector3(navAgent.desiredVelocity.x, 0,
		                                            navAgent.desiredVelocity.z)).normalized.z);
		float speedCap = 1f;
		if ((navAgent.destination - transform.position).magnitude < breakRadius + navAccRadius) {
			speedCap = ((navAgent.destination - transform.position).magnitude + navAccRadius)
					   / (breakRadius + navAccRadius);
		}
		Movement.move(speedCap * inputVec, false);
		if ((navAgent.destination - transform.position).magnitude < navAccRadius) {
			navAgent.destination = transform.position;
			navState = NAVSTATE.REACHEDGOAL;
		}
		Movement.rotate(inputVec, false);
		navAgent.nextPosition = transform.position;
	}

	public void navigateTo(Vector3 destination) {
		navState = NAVSTATE.NAVIGATE;
		navAgent.destination = destination;
	}
}

// TODO: Slowing Down before reaching the Goal