using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeasurePoint{

	public Vector3 point {
		get;
		internal set;
	}
	private List<Timer> measures;
	private Timer currentMeasure;

	public MeasurePoint(Vector3 point){
		this.point = point;
		measures = new List<Timer> ();
	}

	public void NewMeasure(){
		if (currentMeasure != null)
			throw new UnityException ("Timer already running!");
		else {
			currentMeasure = new Timer ();
			currentMeasure.Start();
		}
	}

	public void StopMeasure(){
		currentMeasure.Stop ();
		measures.Add (currentMeasure);
		currentMeasure = null;
	}

	public string[] GetMeasures(){
		List<string> stringListToAppendTo = new List<string> ();
		foreach (Timer m in measures)
			stringListToAppendTo.Add ("Mätning för punkt: " + point + ": " + m.Elapsed);
	
		return stringListToAppendTo.ToArray();
	}

	public bool Equals(MeasurePoint m)
	{
		return m.point.Equals (this.point);
	}

}
