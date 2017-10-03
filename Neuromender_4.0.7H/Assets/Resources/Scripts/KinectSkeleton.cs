using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KinectSkeleton : MonoBehaviour
{
    public Color SkeletonColor;

    //list the joints that we want
    List<KinectInterop.JointType> kJoints = new List<KinectInterop.JointType>()
    {
        KinectInterop.JointType.SpineBase,
        KinectInterop.JointType.SpineMid,
        KinectInterop.JointType.SpineShoulder,
        KinectInterop.JointType.Neck,
        KinectInterop.JointType.ShoulderLeft,
        KinectInterop.JointType.ShoulderRight,
        KinectInterop.JointType.Head,
        KinectInterop.JointType.HandLeft,
        KinectInterop.JointType.HandRight,
        KinectInterop.JointType.ElbowLeft,
        KinectInterop.JointType.ElbowRight,
        KinectInterop.JointType.WristLeft,
        KinectInterop.JointType.WristRight

    };

    void Start()
    {
        foreach (var joint in kJoints)
        {
            GameObject newJoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newJoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            newJoint.GetComponent<Renderer>().material.color = SkeletonColor;

            newJoint.transform.parent = transform;
            newJoint.transform.localPosition = new Vector3(0, 0, 0);
            newJoint.name = joint.ToString();
            newJoint.AddComponent<MapJoint>();
            newJoint.GetComponent<MapJoint>().joint = joint;
        }
    }
}
