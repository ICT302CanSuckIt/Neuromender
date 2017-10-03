using UnityEngine;
using System.Collections;

public class ColliderLocBasedOnExtension : MonoBehaviour
{

    public GameObject WallCollider;
    public GameObject RightShoulderJoint;

    public Vector3 TempInitialPos;
    public Vector3 ShoulderPos;

    private float MaxExtension;

    //for accessing the arm length from database
    public GameObject DBcons;


    // Use this for initialization
    void Start()
    {
        //TempInitialPos = transform.position;
        // OnLevelWasLoaded();
        DBcons = GameObject.Find("DatabaseController");
        
        //MaxExtension = 0.25f;
        MaxExtension = ((DBcons.GetComponent<LoginControl>().config.ArmMaxExtension / 500) - 0.15f) ;

        Debug.Log("temp pos : " + MaxExtension);


        // ChangeLocExtension();
        // MaxExtension = 0.56f;

        if (RightShoulderJoint) ShoulderPos = RightShoulderJoint.transform.position;


        TempInitialPos.z = ShoulderPos.z + MaxExtension;
        //Debug.Log("temp pos : " + TempInitialPos.z);
        WallCollider.transform.position = new Vector3(transform.position.x, transform.position.y, TempInitialPos.z);
        // Debug.Log(transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(string.Format("MaxExtension = " + MaxExtension));

    }

    //Tp call the arm length to be used for wall collider position
    /* void OnLevelWasLoaded()
     {
         if(GameObject.Find("DatabaseController"))
         {
             DBcons = GameObject.Find("DatabaseController");
             MaxExtension = DBcons.GetComponent<LoginControl>().config.armLength;
         }     
     }
     */


    /* void OnControllerColliderHit (ControllerColliderHit hit)
     {
         hit.gameObject.transform.position = Vector3.zero;
     } 

    void ChangeLocExtension ()
     {
         WallPosition = WallCollider.transform.position;

         Vector3 RelativePosition = new Vector3(3.0f, 0.0f, MaxExtension);

         WallPosition = RelativePosition;


         /*

         //Vector3 relativePos = new Vector3(trans.x, yRelToAvatar, trans.z);
         Vector3 relativePos = new Vector3(trans.x, yRelToAvatar, zRelToAvatar);
         Vector3 newBodyRootPos = WallPos + relativePos;

     } */
}
