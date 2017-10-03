using UnityEngine;
using System.Collections;

public class VrController : MonoBehaviour
{
    public bool LoadVrOnThisScene = false;

	// Use this for initialization
	void Start ()
    {
	    if(VrSettings.VrDeviceExists)
        {
            UnityEngine.VR.VRSettings.enabled = LoadVrOnThisScene;
        }
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    void OnDestroy()
    {
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            UnityEngine.VR.VRSettings.enabled = false;
        }
    }
}

public static class VrSettings
{
    public static bool VrDeviceExists { get; set; }
}

