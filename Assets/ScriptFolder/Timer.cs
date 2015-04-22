using UnityEngine;
using System.Collections;

public class Timer{

	float startTime;
	float stopTime;
	public bool isRunning {
		get;
		internal set;
	}

	public void Start(){
		startTime = Time.realtimeSinceStartup;
		isRunning = true;
	}


	public void Stop(){
		stopTime = Time.realtimeSinceStartup;
		isRunning = false;
	}

	public float Elapsed {
		get {
			if (isRunning)
				return Time.realtimeSinceStartup - startTime;
			else
				return stopTime - startTime;
		}
	}
}
