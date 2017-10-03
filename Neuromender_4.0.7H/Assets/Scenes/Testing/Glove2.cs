using UnityEngine;
using System.Collections;

public class Glove2 : Glove_Base
{
	//what the hell does this script do tho

    public enum Handed { joint_CapitateRT, joint_CapitateLT }
    public Handed Hand = Handed.joint_CapitateRT;
    //
    //public Handed assistingHand = Handed.HandLeft;

    //public GameObject Joint;
    //public Rigidbody rb;

    //public bool keyboardControlOverride = false;

    public GloveGood GoodHand;
    public GameObject AssistZone2;

    GameObject DBcons;
    void OnLevelWasLoaded()
    {
        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

          //  if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1)
           // {
            //    Hand = Handed.joint_CapitateLT;
                //
                //assistingHand = Handed.HandRight;
           // }
           // else if (DBcons.GetComponent<LoginControl>().config.sideAffected == 2)
           //{
                Hand = Handed.joint_CapitateRT;
                //
                //assistingHand = Handed.HandLeft;
            //}
        }
    }
    
    // Use this for initialization
    void Start()
    {

        Joint = GameObject.Find(Hand.ToString());
        SetGoodHand();
        SetAssist();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!keyboardControlOverride)
        {
            if (!Joint)
            {
                Joint = GameObject.Find(Hand.ToString());
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
            Handed Other = Handed.joint_CapitateLT;

            if(Hand == Handed.joint_CapitateLT)
            {
                Other = Handed.joint_CapitateRT;
            }

            GoodHand.GetComponent<GloveGood>().Joint = GameObject.Find(Other.ToString());
        }
    }

    public void SetAssist()   
    {
        if (AssistZone2)
        {            
            AssistZone2.GetComponent<AssistZone2>().HandJoint = Joint;

            if(Hand == Handed.joint_CapitateLT)
            {
                AssistZone2.GetComponent<AssistZone2>().ElbowJoint = GameObject.Find("joint_ElbowLT");
                AssistZone2.GetComponent<AssistZone2>().assistHandJoint = GameObject.Find("joint_HandLT");
            }
            else
            {
                AssistZone2.GetComponent<AssistZone2>().ElbowJoint = GameObject.Find("joint_ElbowRT");
                AssistZone2.GetComponent<AssistZone2>().assistHandJoint = GameObject.Find("joint_HandRT");
            }
        }
    }
}