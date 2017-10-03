using UnityEngine;
using System.Collections;

public class BoatControl : MonoBehaviour
{
    public enum Handed { HandRight, HandLeft }
    public Handed Hand = Handed.HandRight;

    public GameObject Joint;
    public GameObject Wrist;
    public GameObject Elbow;
    public GameObject Shoulder;

    public float ExtensionAngle;
    public float MaxReach;
    public float CurReach;
    public float MinReach;
    public float ReachPercent;

    public float LastReach;
    public float ThisReach;
    public float DeltaReach;

    public Rigidbody rb;

    public float PowerModifier = 50.0f;

    public bool keyboardControl = true;

    // debug purposes only
    public float Speed;

    public GameObject LeftOar;
    public GameObject RightOar;
    public GameObject LeftCuff;
    public GameObject RightCuff;
    public GameObject LeftPin;
    public GameObject RightPin;
    public bool Pulling = false;


	//what is this for???????????? the game wont work if its not true so why have it
    public bool Racing = true;

    GameObject DBcons;
    void OnLevelWasLoaded()
    {
        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

           // if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1)
            //{
            //    Hand = Handed.HandLeft;
           // }
           // else if (DBcons.GetComponent<LoginControl>().config.sideAffected == 2)
           // {
                Hand = Handed.HandRight;
          //  }

        }
    }


    // Use this for initialization
    void Start()
    {
        Joint = GameObject.Find(Hand.ToString());
        rb = GetComponent<Rigidbody>();

        // Had to add this because the boat was sliding everywhere! It doesn't have to be one.
        if (rb.drag < 1)
            rb.drag = 1;
    }

    // Update is called once per frame
    void Update()
    {
		
        if(keyboardControl)
        {
			
            if (!Joint)
            {
                Joint = GameObject.Find(Hand.ToString());
            }
            else
            {
                if (!Wrist)
                {
                    if (Joint.name == "HandRight")
                    {
                        Wrist = GameObject.Find("WristRight");
                        Elbow = GameObject.Find("ElbowRight");
                        Shoulder = GameObject.Find("ShoulderRight");
						Debug.Log ("Right Hand");
                    }
                    else
                    {
                        Wrist = GameObject.Find("WristLeft");
                        Elbow = GameObject.Find("ElbowLeft");
                        Shoulder = GameObject.Find("ShoulderLeft");
                    }
                }

                // new z pos of hand
                ThisReach = Joint.transform.position.z;
                // calc change
                DeltaReach = ThisReach - LastReach;
                // set last reach
                LastReach = ThisReach;

                // if pulling apply a relative force
                if (DeltaReach < 0)
                {
                    if (!Racing)
                    {
                        rb.AddRelativeForce(Vector3.back * DeltaReach * PowerModifier);
                    }

                    Pulling = true;


                }
                else if (DeltaReach > 0)
                {

					//if (!Racing)
					//{
					//	rb.AddRelativeForce(Vector3.forward * DeltaReach * PowerModifier);
				//	}

                    Pulling = false;


                }

                if (Pulling)
                {	
					
                    // pull oars animation
                   // LeftOar.transform.localEulerAngles = new Vector3(90, 270, 270);

                    LeftOar.transform.localRotation = Quaternion.Lerp(LeftOar.transform.localRotation, Quaternion.Euler(90, 250, 270), Time.deltaTime * 5);
                    //RightOar.transform.localEulerAngles = new Vector3(-90, 270, 90);
                    RightOar.transform.localRotation = Quaternion.Lerp(RightOar.transform.localRotation, Quaternion.Euler(-90, 250, 90), Time.deltaTime * 5);

                    if (Racing)
                    {
                        //LeftCuff.transform.localEulerAngles = new Vector3(283.5f, 0, 90);
                        LeftCuff.transform.localRotation = Quaternion.Lerp(LeftCuff.transform.localRotation, Quaternion.Euler(283.5f, 0, 90), Time.deltaTime * 5);
                        //RightCuff.transform.localEulerAngles = new Vector3(283.5f, 180, 90);
                        RightCuff.transform.localRotation = Quaternion.Lerp(RightCuff.transform.localRotation, Quaternion.Euler(283.5f, 180, 90), Time.deltaTime * 5);
                    }
                }
                else
                {

                    // lift oars animation
                    //LeftOar.transform.localEulerAngles = new Vector3(0, 270, 270);
                    LeftOar.transform.localRotation = Quaternion.Lerp(LeftOar.transform.localRotation, Quaternion.Euler(0, 270, 250), Time.deltaTime * 5);
                   // RightOar.transform.localEulerAngles = new Vector3(0, 270, 90);
                    RightOar.transform.localRotation = Quaternion.Lerp(RightOar.transform.localRotation, Quaternion.Euler(0, 270, 110), Time.deltaTime * 5);

                    if (Racing)
                    {
                        //LeftCuff.transform.localEulerAngles = new Vector3(270, 90, 0);
                        LeftCuff.transform.localRotation = Quaternion.Lerp(LeftCuff.transform.localRotation, Quaternion.Euler(270, 90, 0), Time.deltaTime * 5);
                        //RightCuff.transform.localEulerAngles = new Vector3(270, 270, 0);
                        RightCuff.transform.localRotation = Quaternion.Lerp(RightCuff.transform.localRotation, Quaternion.Euler(270, 270, 0), Time.deltaTime * 5);
                    }
                }

                // elbow angle animation
                /*
                Vector3 W2E = Wrist.transform.position - Elbow.transform.position;
                Vector3 S2E = Shoulder.transform.position - Elbow.transform.position;

                ExtensionAngle = Vector3.Angle(W2E, S2E);

                LeftPin.transform.localEulerAngles = new Vector3(0, 0, 45 - ExtensionAngle * 0.5f);
                RightPin.transform.localEulerAngles = new Vector3(0, 180, 225 - ExtensionAngle * 0.5f);
                */

                // min-max animation
                CurReach = Joint.transform.position.z;

                if (CurReach > MaxReach) MaxReach = CurReach;
                if (CurReach < MinReach) MinReach = CurReach;

                ReachPercent = (CurReach - MinReach) / (MaxReach - MinReach);

               // LeftPin.transform.localEulerAngles = new Vector3(0, 0, 45 - 90 * ReachPercent); // 45 to - 45)   these make it crash
               // RightPin.transform.localEulerAngles = new Vector3(0, 180, 225 - 90 * ReachPercent); // 225 to  135)   crash

                //debug speed check
                //Speed = rb.velocity.magnitude;
            }
        }
        else
        {

            if(Input.GetKeyDown(KeyCode.Space))
                rb.AddRelativeForce(Vector3.forward * 10 * PowerModifier);

       }
        
    }

    public void Calibrate()
    {
        MaxReach = 0;
        MinReach = 0;
    }
}
