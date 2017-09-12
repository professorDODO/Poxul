using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Q {

	public class Tupel{
		public Action action = null;
		public float time = 0f;
		public Tupel (Action action, float time) { this.time = time; this. action = action; }
	}

	public class Listilist {
		public class Node {
			public Tupel value = null;
			public Node next = null;
			public Node parent = null;
			public Node (Tupel value) { this.value = value; }
		}

		public Node head = null;
		public Node tail = null;

		public bool is_empty () { return head == null; }
		public void addLast (Tupel t) { Node n = new Node(t); n.parent = tail; tail.next = n; tail = n; }
		
		public void removeFirst () { 
			if (head == tail) { head = null; tail = null; }
			else { head = head.next; head.parent = null; }}
		
		public void removeLast () { 
			if (head == tail) { head = null; tail = null; }
			else { tail = tail.parent; tail.next = null; }}
	}

	private Listilist queue = new Listilist();

	public void enqueue (Action action, float time) { queue.addLast(new Tupel(action, time)); }
	
	public Action dequeue () {
		Action re = queue.head.value.action;
		queue.removeFirst();
		return re;
	}

	public float fall_back () {
		float re = queue.tail.value.time;
		queue.removeLast();
		return re;
	}
}
