using UnityEngine;
using System.Collections;

public class VrInit : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            VrSettings.VrDeviceExists = true;
            UnityEngine.VR.VRSettings.enabled = false;
        }
    }

	// Update is called once per frame
	void Update ()
    {
	
	}
}
