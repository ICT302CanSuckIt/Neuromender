using UnityEngine;
using System.Collections;

public class Glove3 : Glove_Base
{
    public enum Fingered { joint_FingersCRT, joint_FingersCLT }
    public Fingered Finger = Fingered.joint_FingersCRT;

    //
    //public Handed assistingHand = Handed.HandLeft;

    //public GameObject Joint;
    //public Rigidbody rb;

    //public bool keyboardControlOverride = false;

    public GloveGood GoodHand;
    public GameObject AssistZone3;

    GameObject DBcons;
    void OnLevelWasLoaded()
    {
        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

            if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1)
            {
                Finger = Fingered.joint_FingersCLT;
                //
                //assistingHand = Handed.HandRight;
            }
            else if (DBcons.GetComponent<LoginControl>().config.sideAffected == 2)
            {
                Finger = Fingered.joint_FingersCRT;
                //
                //assistingHand = Handed.HandLeft;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
    

        Joint = GameObject.Find(Finger.ToString());
       SetGoodHand();
        SetAssist();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyboardControlOverride)
        {
            if (!Joint)
            {
                Joint = GameObject.Find(Finger.ToString());
               SetGoodHand();
                SetAssist();
            }
            else
            {
                rb.MovePosition(Joint.transform.position);
            }
        }
    }

    public void SetGoodHand()
    {
        if (GoodHand)
        {
            Fingered Other = Fingered.joint_FingersCLT;

            if (Finger == Fingered.joint_FingersCLT)
            {
                Other = Fingered.joint_FingersCRT;
            }

            GoodHand.GetComponent<GloveGood>().Joint = GameObject.Find(Other.ToString());
        }
    }

    
    public void SetAssist()
    {
        if (AssistZone3)
        {
            AssistZone3.GetComponent<AssistZone3>().FingersJoint = Joint;

            if (Finger == Fingered.joint_FingersCLT)
            {
                AssistZone3.GetComponent<AssistZone3>().HandJoint = GameObject.Find("joint_HandLT");
               // AssistZone2.GetComponent<AssistZone2>().assistHandJoint = GameObject.Find("joint_FingersCLT");
            }
            else
            {
                AssistZone3.GetComponent<AssistZone3>().HandJoint = GameObject.Find("joint_HandRT");
               // AssistZone2.GetComponent<AssistZone2>().assistHandJoint = GameObject.Find("joint_FingersCRT");
            }
        }
    }
}