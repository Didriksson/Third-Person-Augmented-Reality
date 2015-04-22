using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
public class MasterMarkerController : MonoBehaviour {

	public List<Marker> markers;
	public Marker activeMarker = null;
	public Object pointobject;
	private float pX, pY, pointNumber;
	private PointsModel model;
	private string message = ""; 
	public bool grid; 
	public int pointsIntervallbeforeToTurnOnGrid = 10;
	public Font myFont;
	private Timer timer;
	private List<MeasurePoint> pointMeassure;
	// Use this for initialization
	void Awake () {
		this.model = new PointsModel ();			
		this.pointMeassure = new List<MeasurePoint> ();
		Screen.orientation = ScreenOrientation.Landscape;
	}

	void Start() {
		AddPointToWall ();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			NextPoint();
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			PreviousPoint();
		}

		if(Input.GetKeyDown(KeyCode.P)){
			foreach(MeasurePoint m in pointMeassure){
				SaveMeasureData();
			}
		}

		if(Input.GetKeyDown(KeyCode.S)){
			MeasurePoint m = GetMeasurePointFromList();
			m.StopMeasure();
		}
	}

	public void ReportDetected(Marker marker){
		Debug.Log ("Report detected!");
	}
	
	public void ReportTracked(Marker marker){		

		MoveWallPlaneToClosestMarker ();

	}

	public void ReportNotTracked(Marker marker){
		marker.DisableRenderer ();
		if (activeMarker == marker) {
			activeMarker = GetClosestMarkerToTarget(model.GetCurrentPoint());
			if(activeMarker){
				activeMarker.EnableRenderer();
				MoveWallPlaneToClosestMarker();
			}
		}
	}
	
	public void RemoveChildObjects() {
		var children = new List<GameObject>();

		foreach (Marker m in markers) {
			foreach (Transform child in m.gameObject.GetComponentsInChildren<Transform>()) {
				if(child.gameObject.tag.Equals("WallPoint"))
					children.Add(child.gameObject);
			}				
		}
		children.ForEach(child => Destroy(child));
	}

	public void NextPoint() {
		if (activeMarker && model.GetPointNumber () < 21) {

			MeasurePoint previous = GetMeasurePointFromList ();
			if (previous != null) {
				Debug.Log ("Next point stop measure");
				previous.StopMeasure ();
			}

			model.NextPoint ();

			MeasurePoint newMeasurePoint = GetMeasurePointFromList ();
			if (newMeasurePoint != null) {
				Debug.Log ("Next point, new measure");
				newMeasurePoint.NewMeasure ();
			} else {
				Debug.Log ("Does not contain");
				MeasurePoint mpoint = new MeasurePoint (model.GetCurrentPoint ());
				mpoint.NewMeasure ();				                                       
				pointMeassure.Add (mpoint);

			}

			RemoveChildObjects ();
			activeMarker.EnableRenderer ();
			AddPointToWall ();

			if (model.GetPointNumber () > 10)
				grid = true;
			else
				grid = false;
			}

		if(model.GetPointNumber() == 21)
			SaveMeasureData();
		


	}

	private MeasurePoint GetMeasurePointFromList(){
		return pointMeassure.Find (point => point.point.Equals (model.GetCurrentPoint ()));
	}
	public void PreviousPoint() {
		if (activeMarker && model.GetPointNumber() > 1) {
			MeasurePoint previous = GetMeasurePointFromList();
			if (previous != null) {
				previous.StopMeasure();
			}
			
			model.PreviousPoint ();

			MeasurePoint newMeasurePoint = GetMeasurePointFromList();
			if (newMeasurePoint != null) {
				Debug.Log("previous point, new measure");
				newMeasurePoint.NewMeasure();
			}


			RemoveChildObjects ();
			activeMarker.EnableRenderer ();
			AddPointToWall ();

			if(model.GetPointNumber() > 10)
				grid = true;
			else
				grid = false;

		}
	}

	private void OnGUI(){
		GUI.skin.label.normal.textColor = Color.red;
		GUI.skin.font = myFont;

		if (!activeMarker)
			GUI.Label (new Rect ((Screen.width / 2) - 50, Screen.height / 2, 100, 25), "Cant Find Marker");
		GUI.Label (new Rect ((Screen.width / 2) - 50, Screen.height / 2, 100, 25), message);

		pointNumber = model.GetPointNumber ();
		pX = model.GetCurrentPoint ().x;
		pY = model.GetCurrentPoint ().z;
		if(pY != -1000 && pY != -2000)
			GUI.Label (new Rect ((Screen.width/100)*5, (Screen.height/100)*5, 100, 50), "Point: " + pointNumber + "\n" + "(" + pX + "," + pY +")");
	}

	private void AddPointToWall() {


		foreach (Marker m in markers) {
			Transform wall = null;
			foreach(Transform t in m.GetComponentsInChildren<Transform>()){
				if(t.tag.Equals("Wall"))
				{	
					wall = t;
					break;
				}
			}

			Vector3 previousScale = wall.gameObject.transform.localScale;
			Quaternion previousRotation = wall.gameObject.transform.rotation;
			Vector3 previousPosition = wall.gameObject.transform.position;

			wall.gameObject.transform.parent = null;
			wall.gameObject.transform.localScale = Vector3.one;
			wall.gameObject.transform.rotation = Quaternion.identity;
			wall.gameObject.transform.position = Vector3.zero;
			
			Vector3 currentPoint = model.GetCurrentPoint();
			GameObject instantiatedObj = (GameObject)Instantiate(pointobject, currentPoint, Quaternion.identity);
			instantiatedObj.gameObject.transform.parent = wall.gameObject.transform;
			
			if(m != activeMarker){
				foreach(Renderer r in instantiatedObj.gameObject.GetComponentsInChildren<Renderer>())
					r.enabled = false;
			}
			wall.gameObject.transform.parent = m.gameObject.transform;
			wall.gameObject.transform.localScale = previousScale;
			wall.gameObject.transform.rotation = previousRotation;
			wall.gameObject.transform.position = previousPosition;
		}
	}

	private void MoveWallPlaneToClosestMarker() {
		if (activeMarker)
			activeMarker.DisableRenderer ();
	
		activeMarker = GetClosestMarkerToTarget(model.GetCurrentPoint());
		activeMarker.EnableRenderer ();

	}

	private Marker GetClosestMarkerToTarget(Vector3 target){
		Marker closestMarker = null;
		float previousDistance = 9999999;
		foreach(Marker m in markers){
			if(m.tracking) {
				float currentDistance = m.GetDistanceToTarget(target);
				if(currentDistance < previousDistance)
				{	closestMarker = m;
					previousDistance = currentDistance;
				}
			}
		}
		return closestMarker;
	}
	
	private IEnumerator DisplayTextAmountOfSeconds(float seconds){
		var startTime = Time.time;
		while (Time.time < (startTime + seconds) && !activeMarker) {
			message = "Cant get next point. No active marker!";
			yield return null;
		}
		message = "";
	}

	private void SaveMeasureData(){
		var date = System.DateTime.Now.Month +"-" + System.DateTime.Now.Day+"-" + System.DateTime.Now.Hour +"_"+ System.DateTime.Now.Minute;
		System.IO.StreamWriter writer = System.IO.File.CreateText (Application.dataPath + "/../Mätningar/Experiment " +  date + ".txt");
		writer.AutoFlush = true;
		int pointnumber = 1;
		foreach(MeasurePoint m in pointMeassure){
			string[] measures = m.GetMeasures();
			writer.WriteLine("Punkt: " + pointnumber++);
			foreach(string s in measures)
				writer.WriteLine(s);
		}
		writer.Close ();
		Debug.Log("Saved data.");


	}

}