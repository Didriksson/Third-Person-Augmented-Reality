using UnityEngine;
using System.Collections;

public class MessageDispatcher : MonoBehaviour {

	public static MessageDispatcher instance;

	private MessageDispatcher(){}

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
	}

	[RPC]
	public void RelayMessage(string msg){


		if (Network.isClient)
			GetComponent<NetworkView> ().RPC ("ReceiveMessage", RPCMode.Server, msg);
		else {
			GetComponent<NetworkView> ().RPC ("ReceiveMessage", RPCMode.Others, msg);
		}
	}

	[RPC]
	public void ReceiveMessage(string msg){
		Debug.Log ("Message received from client: " + msg);
	}
}
