using UnityEngine;
using System.Collections;
using Vuforia;

public class RenderScript : MonoBehaviour {
	
	Texture2D tex;
	Rect rect;
	public bool debugMode = true;

	void Start(){
		tex = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		rect = new Rect (0, 0, Screen.width, Screen.height);
		CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

	void Update(){
		StartCoroutine (CaptureScreen ());
	}

	private IEnumerator CaptureScreen() {
		yield return new WaitForEndOfFrame ();

		tex.ReadPixels (rect, 0, 0, false);
		tex.Apply ();
		//	System.IO.File.WriteAllBytes(Application.dataPath + "/../SavedScreenJPGEncoder2.jpg", bytes);
		
		byte[] bytes = tex.EncodeToJPG (50);
		GetComponent<NetworkView> ().RPC ("SB", RPCMode.Others, bytes); 


	}
	
	//Implementeras på klientsidan.
	[RPC]
	void SB(byte[] array){}
}
