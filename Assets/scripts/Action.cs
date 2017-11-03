using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

	// von dieser klasse werden alle aktionen (z.b. laufen, schießen, ausweichen, kitzeln, pupsen) erben

	private string name;					
	private float time;						// execution time

	public Action (string name, float time) {
		this.name = name;
		this.time = time;
	}
	public string get_name () {return name;}
	public float get_time () {return time;}
}
