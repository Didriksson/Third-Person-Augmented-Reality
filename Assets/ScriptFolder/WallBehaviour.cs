using UnityEngine;
using System.Collections;

public class WallBehaviour : MonoBehaviour {

	private float privAlphaValue;
	MasterMarkerController controller;
	void Start() {
	
		Renderer render = GetComponentInChildren<Renderer> ();
		privAlphaValue = render.material.color.a;
		controller = GameObject.Find("FrameController").GetComponent<MasterMarkerController>();

	}

	void Update() {
		RenderGrid (controller.grid);
	}

	// Use this for initialization
	public void RenderGrid (bool grid) {
		if (!grid) {
			Renderer[] r = GetComponentsInChildren<Renderer>();
			foreach(Renderer rend in r){
				Color c = rend.material.color;
				c.a = 0f;
				rend.material.color = c;
			}

		} else {
			Renderer[] r = GetComponentsInChildren<Renderer>();
			foreach(Renderer rend in r){
				Color c = rend.material.color;
				c.a = privAlphaValue;
				rend.material.color = c;
			}
			
		}
	}
	

}
