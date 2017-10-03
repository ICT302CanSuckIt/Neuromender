using UnityEngine;
using System.Collections;

public class GlideLevel : MonoBehaviour {

	Camera mainCamera;
	StrokeRehabControls mainControls;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		mainControls = GameObject.Find ("NeuromendController").GetComponent<StrokeRehabControls>();
	}
	
	// Update is called once per frame
	void Update () {
		float rightAngle = mainControls.getCurrentAngle ("Right");
		float leftAngle = mainControls.getCurrentAngle ("Left");
	}
}
