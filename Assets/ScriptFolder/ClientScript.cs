using UnityEngine;
using System.Collections;

public class ClientScript : MonoBehaviour {
	
	private Texture2D receivedTexture;
	public string IP = "127.0.0.1";
	public int PORT = 8888;
	public GameObject target;
	public int targetFrameRate;
	public ScaleMode scaleMode;
	private int ping;
	public Font myFont;

	
	void Start() {
		Network.Connect (IP, PORT);
		receivedTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		Application.targetFrameRate = targetFrameRate;
		QualitySettings.vSyncCount = 0;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}


	void FixedUpdate(){
		if(Network.isClient)
			ping = Network.GetLastPing(Network.connections[0]);
	}
	
	void Update() {
		Application.targetFrameRate = targetFrameRate;
	}
	
	void OnGUI() {
		GUI.skin.font = myFont;
		GUI.skin.label.normal.textColor = Color.white;

		if (!Network.isClient) {
			if (GUI.Button (new Rect (Screen.width/2-75, Screen.height/3, 200, 75), "Connect")) {
				Network.Connect (IP, PORT);
			}

			GUI.Label(new Rect(Screen.width/2-200,Screen.height/3*1.5f,150,40), "Address:" );
			IP = GUI.TextField(new Rect(Screen.width/2-50,Screen.height/3*1.5f,250,40), IP);

			GUI.Label(new Rect(Screen.width/2-200,Screen.height/3*2,150,40), "Port:" );
			PORT = int.Parse(GUI.TextField(new Rect(Screen.width/2-50,Screen.height/3*2,100,40), PORT.ToString()));


		} else {
			
			if (receivedTexture != null) {
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), receivedTexture, scaleMode,true, 0);
			}
			
			if (GUI.Button (new Rect (Screen.width - 225, Screen.height-100, 200, 75), "Next point")) {
				GetComponent<NetworkView> ().RPC ("NextPointCall", RPCMode.Server); 
			}

			if (GUI.Button (new Rect (25, Screen.height-100, 225, 75), "Previous point")) {
				GetComponent<NetworkView> ().RPC ("PreviousPointCall", RPCMode.Server); 
			}

			
			if (GUI.Button (new Rect (Screen.width - 55, 20, 35, 35), "X")) {
				Network.Disconnect(250);
				Application.LoadLevel("MainScene");
			}
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect((Screen.width - 100),20,80,40), ping.ToString());

		}
	}
	
	[RPC]
	void SB(byte[] receivedByte){

		receivedTexture.LoadImage(receivedByte);
//		System.IO.File.WriteAllBytes (Application.dataPath + "/../SavedScreenJPGEncoder2.jpg", receivedByte);
	}
	
	[RPC]
	void NextPointCall(){} 

	[RPC]
	void PreviousPointCall(){} 

	
}