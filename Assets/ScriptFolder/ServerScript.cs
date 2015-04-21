using UnityEngine;
using System.Collections;

public class ServerScript : MonoBehaviour {
	public int targetFrame;
	public int port = 8888;


	void Start() {
		Network.InitializeServer (2, port, false);
	}

	void Update () {
		Application.targetFrameRate = targetFrame;
	}

	void OnPlayerConnected(NetworkPlayer player){
		Debug.Log ("Player connected: " + player);
	}

//	void OnGUI(){
//
//		if (GUI.Button (new Rect (25, 100, 150, 75), "LogOut")) {
//			Network.Disconnect(250);
//			Application.LoadLevel("MainScene");
//		}
//
//		if (GUI.Button (new Rect (25, 190, 150, 75), "Next Point")) {
//			NextPointCall();
//		}
//	}

	[RPC]
	void NextPointCall(){
		MasterMarkerController controller = GameObject.Find("FrameController").GetComponent<MasterMarkerController>();
		controller.NextPoint();
	}
	[RPC]
	void PreviousPointCall(){
		MasterMarkerController controller = GameObject.Find("FrameController").GetComponent<MasterMarkerController>();
		controller.PreviousPoint();
	}

}
