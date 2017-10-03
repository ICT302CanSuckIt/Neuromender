using UnityEngine;
using System.Collections;

public class ProximityRing : MonoBehaviour
{
    public float ScaleFactor = 0.05f;
    private int NumSpheres = 12;
    public float Proximity = 0.2f;
    public Vector3 Centre;
    public Vector3 Home;

    public KinectInterop.JointType TargetJoint = KinectInterop.JointType.HandRight;
    public GameObject Target;
    
    public float Accuracy;
    public float Reach;

    public float ThisAcc;
    public float ThisReach;

    public float LastAcc;
    public float LastReach;

    public float BestAcc;
    public float BestReach;

    public bool Reaching;
    public bool Calibrated = false;

    public int ShotNum = 1;




    GameObject DBcons;
    void OnLevelWasLoaded()
    {
        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

            if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1)
            {
                TargetJoint = KinectInterop.JointType.HandLeft;
            }
            else if (DBcons.GetComponent<LoginControl>().config.sideAffected == 2)
            {
                TargetJoint = KinectInterop.JointType.HandRight;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < NumSpheres; i++)
        {
            // create a sphere as a proximity ball
            GameObject newProximityBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // scale down the proximity ball
            newProximityBall.transform.localScale = new Vector3(ScaleFactor, ScaleFactor, ScaleFactor);
            // make child of ProximityRing
            newProximityBall.transform.parent = transform;
            // center to parent
            newProximityBall.transform.localPosition = (Vector3.zero);
            // rotate ball equal angle from last
            newProximityBall.transform.Rotate(0, 0, 360.0f / NumSpheres * i);
            newProximityBall.transform.Translate(Vector3.up * Proximity);
            // add the proximity marker script
            newProximityBall.AddComponent<ProximityMarker>();
        }

        if (Target) Calibrate();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) Calibrate();

        if (!Target) // aquire target and calibrate
        {
            Target = GameObject.Find(TargetJoint.ToString());
            //Calibrate();
        }

        if (Calibrated)
        {
            Centre = new Vector3(this.transform.position.x, this.transform.position.y, Target.transform.position.z);
            this.transform.position = Centre;

            Accuracy = (Proximity - Vector3.Distance(Centre, Target.transform.position)) / Proximity;
            if (Accuracy < 0) Accuracy = 0;

            Reach = Vector3.Distance(Home, Target.transform.position);


            if (Reaching)
            {
                if (Reach > ThisReach)
                {
                    ThisReach = Reach;
                    ThisAcc = Accuracy;
                }

                if (Accuracy <= 0 || Reach <= (ThisReach - 0.03f))
                {
                    Reaching = false;
                    LastReach = ThisReach;
                    LastAcc = ThisAcc;
                    ShotNum++;


                    ThisReach = 0;

                    if (LastReach > BestReach)
                    {
                        SetBest();
                    }
                    else if (LastReach == BestReach && LastAcc > BestAcc)
                    {
                        SetBest();
                    }
                }
            }
            else // returning
            {
                if (Reach <= 0.10f)
                {
                    Reaching = true;

                }
            }
        }
    }

    public void Calibrate()
    {
        Debug.Log("Calibrating");
        this.transform.position = Target.transform.position;
        Home = Target.transform.position;
        Reaching = true;
        Calibrated = true;
    }

    public void SetBest()
    {
        BestReach = LastReach;
        BestAcc = LastAcc;
    }
}
