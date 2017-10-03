using UnityEngine;
using System.Collections;

public class Glove : Glove_Base
{
    public enum Handed { HandRight, HandLeft }
    public Handed Hand = Handed.HandRight;

    //public GameObject Joint;
    //public Rigidbody rb;

    //public bool keyboardControlOverride = false;

    public GloveGood GoodHand;
    public GameObject AssistZone;

    GameObject DBcons;
    void OnLevelWasLoaded()
    {
        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

            if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1)
            {
                Hand = Handed.HandLeft;
            }
            else if (DBcons.GetComponent<LoginControl>().config.sideAffected == 2)
            {
                Hand = Handed.HandRight;
            }
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
        if (!keyboardControlOverride)
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
            Handed Other = Handed.HandLeft;

            if (Hand == Handed.HandLeft)
            {
                Other = Handed.HandRight;
            }

            GoodHand.GetComponent<GloveGood>().Joint = GameObject.Find(Other.ToString());
        }
    }

    public void SetAssist()
    {
        if (AssistZone)
        {
            AssistZone.GetComponent<AssistZone>().HandJoint = Joint;

            if (Hand == Handed.HandLeft)
            {
                AssistZone.GetComponent<AssistZone>().ElbowJoint = GameObject.Find("ElbowLeft");
            }
            else
            {
                AssistZone.GetComponent<AssistZone>().ElbowJoint = GameObject.Find("ElbowRight");
            }
        }
    }
}