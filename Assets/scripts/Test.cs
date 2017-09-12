using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// hier wird nur getestet

	void Start () {
		
		// print(queue.to_array());
		Action_Q q = new Action_Q();
		q.print_queue();
		
		Action walk = new Action("walk", 0.2f);
		Action stab = new Action("stab", 3.0f);
		q.enqueue(walk, 0.5f);
		q.print_queue();
		q.enqueue(stab, 2.8f);
		q.print_queue();
		
		print (q.fall_back());
		q.print_queue();
		
		q.enqueue(stab, 2.8f);
		q.print_queue();
		
		print (q.dequeue().get_name());
		q.print_queue();
		print (q.dequeue().get_name());
		q.print_queue();

	}
}
