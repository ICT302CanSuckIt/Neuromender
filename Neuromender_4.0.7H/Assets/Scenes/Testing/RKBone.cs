using UnityEngine;
using System.Collections;

public class RKBone : MonoBehaviour
{
    public GameObject PJoint;
    public GameObject RKJoint;

    public Vector3 PJPos;
    public Vector3 RKJPos;

    public Vector3 Offset;

    public float width = 0.5f;

    // Use this for initialization
    void Start()
    {
        KinectInterop.JointType joint = GetComponentInParent<MapJoint>().joint;

        PJoint = GameObject.Find(joint.ToString());

        // Set the RKjoint
        switch((int)joint)
        {
            case (int)KinectInterop.JointType.SpineBase:
                Destroy(gameObject);
                break;
            case 2: // Neck
            case 4: // ShoulderLeft
            case 8: // ShoulderRight
                RKJoint = GameObject.Find("SpineShoulder");
                break;
            case 12: // HipLeft
            case 16: // HipRight
                RKJoint = GameObject.Find("SpineBase");
                break;
            case 20: // SpineShoulder
                RKJoint = GameObject.Find("SpineMid");
                break;
            case 21: // HandTipLeft
                RKJoint = GameObject.Find("HandLeft");
                break;
            case 22: // ThumbLeft
                RKJoint = GameObject.Find("WristLeft");
                break;
            case 23: // HandTipRight
                RKJoint = GameObject.Find("HandRight");
                break;
            case 24: // ThumbRight
                RKJoint = GameObject.Find("WristRight");
                break;
            default:
                joint--;
                RKJoint = GameObject.Find(joint.ToString());
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        KinectInterop.JointType joint = GetComponentInParent<MapJoint>().joint;
        
        if (PJoint) PJPos = PJoint.transform.position;
        if (RKJoint) RKJPos = RKJoint.transform.position;

        Offset = RKJPos - PJPos;
        transform.position = PJPos + (Offset * 0.5f);
        transform.localScale = new Vector3(width, Offset.magnitude * 10, width);
        transform.up = Offset;

    }
}
