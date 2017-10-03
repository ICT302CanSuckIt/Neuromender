using UnityEngine;
using System.Collections;

public class EyeTrackingData : MonoBehaviour
{
    private IEyeTrackerData _eyeTrackingData;

	// Use this for initialization
	void Start ()
    {
        _eyeTrackingData = new TobiiEyeX();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(_eyeTrackingData.GetEyeGazePosition());
	}
}
