using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsModel {

	LinkedList<Vector3> listOfpoints;
	LinkedList<Vector3> previousPoints;


	public PointsModel() {
		listOfpoints = new LinkedList<Vector3> ();
		previousPoints = new LinkedList<Vector3> ();
		listOfpoints.AddLast (new Vector3(16,1,24));
		listOfpoints.AddLast (new Vector3(55,1,44));
		listOfpoints.AddLast (new Vector3(44,1,44));
		listOfpoints.AddLast (new Vector3(59,1,36));
		listOfpoints.AddLast (new Vector3(6,1,42));
		listOfpoints.AddLast (new Vector3(10,1,9));
		listOfpoints.AddLast (new Vector3(1,1,1));
		listOfpoints.AddLast (new Vector3(36,1,48));
		listOfpoints.AddLast (new Vector3(33,1,37));
		listOfpoints.AddLast (new Vector3(76,1,23));
		
		
		listOfpoints.AddLast (new Vector3(100,1,45));
		listOfpoints.AddLast (new Vector3(31,1,38));
		listOfpoints.AddLast (new Vector3(33,1,12));
		listOfpoints.AddLast (new Vector3(52,1,26));
		listOfpoints.AddLast (new Vector3(83,1,21));
		listOfpoints.AddLast (new Vector3(52,1,46));
		listOfpoints.AddLast (new Vector3(4,1,4));
		listOfpoints.AddLast (new Vector3(19,1,14));
		listOfpoints.AddLast (new Vector3(82,1,43));
		listOfpoints.AddLast (new Vector3(70,1,40));
	}


	public void NextPoint(){
		Debug.Log("NextPoint, dequeing.");
		if (listOfpoints.Count > 1) {
			previousPoints.AddFirst(listOfpoints.First.Value);
			listOfpoints.RemoveFirst ();
		}
	}

	public void PreviousPoint() {
		if (GetPointNumber() > 0) {
			listOfpoints.AddFirst (previousPoints.First.Value);
			previousPoints.RemoveFirst ();
		}
	}

	public Vector3 GetCurrentPoint(){
		return listOfpoints.First.Value;
	}
	
	public int GetPointNumber() {
		return previousPoints.Count+1;
	}



}
