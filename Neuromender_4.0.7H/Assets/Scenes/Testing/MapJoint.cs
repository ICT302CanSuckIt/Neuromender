using UnityEngine;
using System.Collections;

public class MapJoint : MonoBehaviour
{
    public KinectInterop.JointType joint = KinectInterop.JointType.HandRight;


    private KinectManager manager = KinectManager.Instance;

    // Use this for initialization
    void Start()
    {
        // create a cylinder
        GameObject RKBone = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // make child of this joint
        RKBone.transform.parent = transform;
        // rename the new sphere as the joint
        RKBone.name = "RKBone";
        // add the script to get the human joint location
        RKBone.AddComponent<RKBone>();
        // set the joint to match its name
            //newJoint.GetComponent<MapJoint>().joint = thisJoint;


        //Debug.Log(this.name + ": " + joint.ToString());
    }

    // Update is called once per frame
    void Update()
    {   
        // get the joint position
        if (manager && manager.IsInitialized())
        {
            if (manager.IsUserDetected())
            {
                long userId = manager.GetPrimaryUserID();

                if (manager.IsJointTracked(userId, ( int )joint))
                {
                    // output the joint position for easy tracking
                    Vector3 jointPos = manager.GetJointPosition(userId, ( int )joint);
                    // moves the object to match the human location
                    transform.localPosition = new Vector3(jointPos.x, jointPos.y, jointPos.z * -1); //use local position to attach it to the parent object
                }
            }
        }
    }
}