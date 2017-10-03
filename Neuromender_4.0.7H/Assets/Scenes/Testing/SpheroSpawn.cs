using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpheroSpawn : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        // Spawn sphere for each Kinect point
        for(int JT = 0; JT < 25; JT++)
        {
            // get the joint name for this index
            KinectInterop.JointType thisJoint = ( KinectInterop.JointType )JT;
            // create a sphere
            GameObject newJoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //GameObject newJoint = new GameObject();

            // scale down the joint sphere
            newJoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            
            // make child of Sphero
            newJoint.transform.parent = transform;
            // rename the new sphere as the joint
            newJoint.name = thisJoint.ToString();
            // add the script to get the human joint location
            newJoint.AddComponent<MapJoint>();
            // set the joint to match its name
            
            /*
            if(Mirror == false)
            {
                thisJoint = KinectInterop.GetMirrorJoint(thisJoint);
            }
            */
            newJoint.GetComponent<MapJoint>().joint = thisJoint;
            
            //Debug.Log(thisJoint.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}