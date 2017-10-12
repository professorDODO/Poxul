using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour {
	private MovementToMerge Movement;
	private NavMeshAgent navAgent;
	public float navAccRadius = 0.5f;

	public Transform goal; // for testing purpose

	void Awake() {
		Movement = GetComponent<MovementToMerge>();
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.updatePosition = false;
		navAgent.updateRotation = false;
		navigateTo(goal.position);
	}

	void Update() {
		float xDir = (new Vector3(navAgent.desiredVelocity.x, 0, navAgent.desiredVelocity.z)).normalized.x;
		float yDir = (new Vector3(navAgent.desiredVelocity.x, 0, navAgent.desiredVelocity.z)).normalized.z;
		Movement.move(xDir, yDir, false);
		if ((navAgent.destination - transform.position).magnitude < navAccRadius) {
			navAgent.destination = transform.position;
			Movement.move(-xDir, -yDir, false);
		}
		navAgent.nextPosition = transform.position;
	}

	public void navigateTo(Vector3 destination) {
		navAgent.destination = destination;
	}
}
