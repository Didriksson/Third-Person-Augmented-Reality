using UnityEngine;
using System.Collections;
using Vuforia;

public class Marker : MonoBehaviour, ITrackableEventHandler {
	
	public MasterMarkerController masterController;
	public bool tracking;
	public Vector3 positionOnWall;

	
	#region PRIVATE_MEMBER_VARIABLES
	
	public TrackableBehaviour mTrackableBehaviour;
	
	#endregion // PRIVATE_MEMBER_VARIABLES
	
	
	
	#region UNTIY_MONOBEHAVIOUR_METHODS
	
	void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}

		positionOnWall = gameObject.transform.position;
		DisableRenderer ();
	}
	
	#endregion // UNTIY_MONOBEHAVIOUR_METHODS
	
	
	
	#region PUBLIC_METHODS
	
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		switch (newStatus) {
		case TrackableBehaviour.Status.TRACKED:
			OnTrackingFound();
			break;
		case TrackableBehaviour.Status.DETECTED:
			OnDetected();
			break;
		default:
			OnTrackingLost();
			break;
			
			
			
		}
	}

	
	public float GetDistanceToTarget(Vector3 target){
		return Vector3.Distance (positionOnWall, target);
	}
	
	#endregion // PUBLIC_METHODS
	
	
	
	#region PRIVATE_METHODS
	
	
	private void OnDetected(){
		tracking = true;
		masterController.ReportDetected (this);
	}
	
	private void OnTrackingFound()
	{
		tracking = true;
		masterController.ReportTracked (this);
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
	}


	public void EnableRenderer(){
		Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
		Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
		
		// Enable rendering:
		foreach (Renderer component in rendererComponents)
		{
			component.enabled = true;
		}
		
		// Enable colliders:
		foreach (Collider component in colliderComponents)
		{
			component.enabled = true;
		}
	}

	private void OnTrackingLost()
	{
		tracking = false;
		masterController.ReportNotTracked (this);
	}

	
	public void DisableRenderer()
	{
		Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
		Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
		
		// Disable rendering:
		foreach (Renderer component in rendererComponents)
		{
			component.enabled = false;
		}
		
		// Disable colliders:
		foreach (Collider component in colliderComponents)
		{
			component.enabled = false;
		}
		
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
	}
	
	#endregion // PRIVATE_METHODS
	
	
}