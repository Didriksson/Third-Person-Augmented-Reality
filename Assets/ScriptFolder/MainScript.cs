using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

	private int widthCentered = (Screen.width / 2)-75;

	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			if (GUI.Button (new Rect (widthCentered, Screen.height / 2, 150, 75), "Client")) {
				Application.LoadLevel("ClientScene");
			}
			if (GUI.Button (new Rect (widthCentered, (Screen.height / 2) + 90, 150, 75), "Server")) {
				Application.LoadLevel("ServerScene");
			}
		}
	}
}
