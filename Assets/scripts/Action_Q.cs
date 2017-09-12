using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Q {

	// nur ein billiges tupel
	public class Pair{
		public Action action = null;
		public float time = 0f;
		public Pair (Action action, float time) { this.time = time; this. action = action; }
	}

	// nur ein billiger knoten
	public class Node {
		public Pair value = null;
		public Node next = null;
		public Node parent = null;
		public Node (Pair value) { this.value = value; }
	}

	// nur eine billige liste
	public class Listilist {
		public Node head = null;
		public Node tail = null;

		public bool is_empty () { return head == null; }
		public void addLast (Pair t) { 
			Node n = new Node(t); 
			if (is_empty()) {head = n; tail = n;}
			else { n.parent = tail; tail.next = n; tail = n; }
		}

		public void removeFirst () { 
			if (head == tail) { head = null; tail = null; }
			else { head = head.next; head.parent = null; }}
		
		public void removeLast () { 
			if (head == tail) { head = null; tail = null; }
			else { tail = tail.parent; tail.next = null; }}
	}


	private Listilist queue = new Listilist();

	// eingabe
	public void enqueue (Action action, float time) { queue.addLast(new Pair(action, time)); }
	
	// zur ausfuehrung
	public Action dequeue () {
		// vorsicht NullReferenceException bei leerer queue
		Action re = queue.head.value.action;
		queue.removeFirst();
		return re;
	}

	// zum löschen der letzten eingabe
	public float fall_back () {
		// vorsicht NullReferenceException bei leerer queue
		float re = queue.tail.value.time;
		queue.removeLast();
		return re;
	}

	// fuer spaeter
	public Pair[] to_array () {
		if (queue.is_empty()) {return new Pair[0];}
		
		List<Pair> re = new List<Pair>(); 
		Node n = queue.head;
		re.Add(n.value);
		while (n.next != null){
			n = n.next;
			re.Add(n.value);
		}
		return re.ToArray();
	}

	// zum anschauen fuer jetzt
	public void print_queue () {
		if (queue.is_empty()) { Debug.Log("queue is empty");}
		else { 
			Pair[] a = to_array();
			string s = "i: " + a.Length + ", [ [" + a[0].action.get_name() + ", " + a[0].time + "]";
			for(int i = 1; i < a.Length; i++){ s += ", [" + a[i].action.get_name() + ", " + a[i].time + "]"; }
			s += " ]";
			Debug.Log(s);
		}
	}
}
