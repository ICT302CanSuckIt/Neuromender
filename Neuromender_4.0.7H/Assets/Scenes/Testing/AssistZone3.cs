using UnityEngine;
using System.Collections;

public class AssistZone3 : MonoBehaviour
{
    //
    public enum Fingered { joint_FingersCLT, joint_FingersCRT };
    public Fingered Finger = Fingered.joint_FingersCRT;
    //
    //public Handed assistingHand = Handed.HandLeft;
    


    public GameObject FingersJoint;
    public GameObject HandJoint;

    //
    // public GameObject assistFingersJoint;

    public Vector3 FingersPos;
    public Vector3 HandPos;
    //
    // public Vector3 assistHandPos;

    public Vector3 Offset;

    public float width = 2.0f;
    public float lengthFactor = 5.0f;

    //
    public GameObject assistedBox;
    // public GameObject Joint;
    // public Rigidbody rb;

    //sabb for positioning
    public Vector3 HomePosition;



    /*
        void OnLevelWasLoaded()
          {
              if (GameObject.Find("DatabaseController"))
              {
                  DBcons = GameObject.Find("DatabaseController");
                  //
                  //ArmResetDistance = (float)DBcons.GetComponent<LoginControl>().config.ArmResetDistance / 1000f;
                  // Depending on the hand that is affected, change the wrist and shoulder that are being tracked.
                  if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1)
                  {
                      Hand = Handed.HandLeft;
                     // assistingHand = Handed.HandRight;
                  }
                  else if (DBcons.GetComponent<LoginControl>().config.sideAffected == 2)
                  {
                      Hand = Handed.HandRight;
                    //  assistingHand = Handed.HandLeft;
                  }


              }
          }

       */




    // Use this for initialization
    void Start()
    {
        //
        //HomePosition = transform.position;
        //
        //rb = GetComponent<Rigidbody>();
        //
        //getAssistHand();


        /* if (!RGlove)
         {
             RGlove = GameObject.Find("GloveRelative");
         }    
         */
    }

    // Update is called once per frame
    void Update()
    {
        if (FingersJoint) FingersPos = FingersJoint.transform.position;
        //
        Debug.Log("Fingers Position  " + FingersPos.ToString("f3"));

        if (HandJoint) HandPos = HandJoint.transform.position;

        Offset = HandPos - FingersPos;
        transform.position = FingersPos + (Offset * 0.5f);
        transform.localScale = new Vector3(width, Offset.magnitude * lengthFactor, width);
        transform.up = Offset;


       // if (assistHandJoint) assistHandPos = assistHandJoint.transform.position;
        //Debug.Log("Assistance Hand Position  " + assistHandPos.ToString("f3"));
    }



    /*
    private void getAssistHand ()
    {
        if (Hand == Handed.HandLeft)
        {
            assistHandJoint = GameObject.Find("HandRight");

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Debug.Log("Assistance Hand Position  " + transform.position.ToString("f3"));
        }
        else
            if (Hand == Handed.HandRight)
        {
            assistHandJoint = GameObject.Find("HandLeft");

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Debug.Log("Assistance Hand Position  " + transform.position.ToString("f3"));
        }
    }
    */




    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GloveGood")
        {
            // assisting
            assistedBox.SetActive(true);
            // assisting
            /*  if (assistHandPos == ElbowPos)
              {
                  assistedBox.SetActive(true);
              }*/
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "GloveGood")
        {
            // no longer assisting
            assistedBox.SetActive(false);
            // no longer assisting
            /* if (assistHandPos != ElbowPos)
             {
                 assistedBox.SetActive(false);
             }*/
        }
    }
}
