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

	// Use this for initialization
	void Awake () {
		this.model = new PointsModel ();			

		Screen.orientation = ScreenOrientation.Landscape;
	}

	void Start() {
		AddPointToWall ();
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
		if (activeMarker) {
			model.NextPoint ();
			RemoveChildObjects ();
			activeMarker.EnableRenderer ();
			AddPointToWall ();

			if(model.GetPointNumber() > 10)
				grid = true;
			else
				grid = false;

		} else {
			StartCoroutine(DisplayTextAmountOfSeconds(3));
		}
	}

	public void PreviousPoint() {
		if (activeMarker) {
			model.PreviousPoint ();
			RemoveChildObjects ();
			activeMarker.EnableRenderer ();
			AddPointToWall ();

			if(model.GetPointNumber() > 10)
				grid = true;
			else
				grid = false;

		} else {
			StartCoroutine(DisplayTextAmountOfSeconds(3));
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
			
			if(m != activeMarker)
				instantiatedObj.gameObject.GetComponentInChildren<Renderer>().enabled = false;
			
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
		Debug.Log (startTime);
		
		while (Time.time < (startTime + seconds) && !activeMarker) {
			message = "Cant get next point. No active marker!";
			yield return null;
		}
		message = "";
	}
}