using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	void Start () {
		// print(queue.to_array());
		Action_Q queue = new Action_Q();
		queue.print_queue();
		
		Action walk = new Action("walk", 0.2f);
		Action stab = new Action("stab", 3.0f);
		queue.enqueue(walk, 0.5f);
		queue.print_queue();
		queue.enqueue(stab, 2.8f);
		queue.print_queue();

	}


}
