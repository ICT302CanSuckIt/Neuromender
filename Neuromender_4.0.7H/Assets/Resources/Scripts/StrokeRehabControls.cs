using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public struct st_RawTracking
{
    //these 2 are here for if the game crashes only.
    public int SessionID;
    public int UserID;

    public float BodyDepth;
    public Vector3 rightHand;
    public Vector3 rightElbow;
    public Vector3 leftHand;
    public Vector3 leftElbow;
    public Vector3 centerPoint;
    public float rightAngle;
    public float leftAngle;
    public DateTime rawDate;

    //new field to keep track ring and achievement (game) id
    public int RingNumber;
    public int AchievementId;
}

public enum en_TaskType //has to match up with database
{
    ElbowRaise = 1,
    FistExtend = 2
}

public struct st_Achievement
{
    public en_TaskType task;
    public float Threshold;
    public DateTime TimeAchieved;
}

/// <summary>
/// Class that handles All of the Stroke Rehab controlls.
/// Possibly might split into 2 sub classes 
///     Glide Control
///     Punching Control
/// </summary>
/// 
public class StrokeRehabControls : MonoBehaviour
{

    public Transform leftHand;
    public Transform rightHand;

    public GameObject assistedBox;

    public Transform leftElbow;
    public Transform rightElbow;

    //Glide Bounding Box
    public Transform head;
    public Transform Neck;
    public Transform TorsoC;
    public Transform centerWaist;
    public Transform rightShoulder;
    public Transform leftShoulder;

    private StrokeRehabLevelController lvlController;
    private LoginControl login;

    private float maxLeft = 0.0f;

    public float MaxAngleLeft
    {
        get { return maxLeft; }
    }
    private float maxRight = 0.0f;
    public float MaxAngleRight
    {
        get { return maxRight; }
    }
    private float leftHandDepth = 0.0f;
    private float rightHandDepth = 0.0f;
    public float avgBodyDepth = 0.0f;

    private float rightAngle = 0.0f;
    private float leftAngle = 0.0f;

    /// <summary>
    /// This variable is used for the "Keep Body Straight"
    /// Should be converted into a config option in the database (we didnt think of that until too late)
    /// </summary>
    private int maxBodyBendThreshold = 20;

    public float RightAngle
    {
        get { return rightAngle; }
    }

    public float LeftAngle
    {
        get { return leftAngle; }
    }

    private AvatarController kinectController;

    public bool isBodyStraight;

    public bool Assisted = false;

    private float userDetectedCountdown = 2.0f;
    // Use this for initialization
    void Start()
    {
        // DontDestroyOnLoad(this);
        lvlController = GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>();

        login = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //update all variables

        // If the arm is being assisted, then raise the appropriate flag.
        if ((Vector3.Distance(leftElbow.position, rightHand.position) < 0.5f && login.config.sideAffected == 1) ||
            (Vector3.Distance(rightElbow.position, leftHand.position) < 0.5f && login.config.sideAffected == 2))
            Assisted = true;
        else
            Assisted = false;

       
        //check rhand
        GlideControl(rightElbow.position, rightHand.position, "right");
        GlideControl(leftElbow.position, leftHand.position, "left");




        UpdateVariables();
    }


    private void UpdateVariables()
    {
        avgBodyDepth = (Neck.position.z + centerWaist.position.z + leftShoulder.position.z + rightShoulder.position.z) / 4;
        // lvlController.sendUIData(avgBodyDepth, "avgBodyDepth");

        Vector3 RightHandDistanceVector = rightHand.position - rightShoulder.position;
        float rightHandDistance = RightHandDistanceVector.magnitude;

        Vector3 LeftHandDistanceVector = leftHand.position - leftShoulder.position;
        float leftHandDistance = LeftHandDistanceVector.magnitude;

        lvlController.sendUIData(leftHandDistance, "leftHandDistance");
        lvlController.sendUIData(rightHandDistance, "rightHandDistance");


        lvlController.sendUIData(leftHand.position.z, "leftHandDepth");
        lvlController.sendUIData(rightHand.position.z, "rightHandDepth");


        lvlController.sendUIData(leftHand.position.x, "leftHandStretch");
        lvlController.sendUIData(rightHand.position.x, "rightHandStretch");
    }

    public float getCurrentAngle(string side)
    {
        if (side == "Right")
        {
            return rightAngle;
        }
        else if (side == "Left")
        {
            return leftAngle;
        }
        else
        {
            return 0.0f;
        }
    }

    private Vector3 CenterNeckTorso()
    {
        var difference = Neck.position - TorsoC.position;
        var midPoint = TorsoC.position + difference * 0.5f;
        return midPoint;
    }

    /// <summary>
    /// Controlling Function for the Glide, 
    /// For the prototype is just spitting out values to the UI class
    /// should probably return something for the actuall gameplay.
    /// </summary>
    /// <param name="elbow"></param>
    /// <param name="hand"></param>
    /// <param name="label"></param>
    private void GlideControl(Vector3 elbow, Vector3 hand, string label)
    {
        //Angle between Center Shoulder, Middle of the Chest, and THe Elbow
        float centerOfBodyX = (centerWaist.position.x + CenterNeckTorso().x) / 2;
        float centerOfBodyY = (centerWaist.position.y + CenterNeckTorso().y) / 2;
        float centerOfBodyZ = (centerWaist.position.z + CenterNeckTorso().z) / 2;
        Vector3 centerOfBody = new Vector3(centerOfBodyX, centerOfBodyY, centerOfBodyZ);

        //actual vector from the center of shoulder to the center of body  
        Vector3 centreToShoulder = CenterNeckTorso() - centerOfBody;

        elbow.z = centreToShoulder.z;
        //actual vector from center of shoulder to the right hand
        Vector3 shoulderToElbow = CenterNeckTorso() - elbow;
        float ShoulderAngle;

        // float ShoulderAngle = Vector3.Angle(centreToShoulder, shoulderToElbow);
        Vector2 leftElbowV2 = new Vector2(leftElbow.position.x, leftElbow.position.y);
        Vector2 rightElbowV2 = new Vector2(rightElbow.position.x, rightElbow.position.y);
        Vector2 leftShoulderV2 = new Vector2(leftShoulder.position.x, leftShoulder.position.y);
        Vector2 rightShoulderV2 = new Vector2(rightShoulder.position.x, rightShoulder.position.y);
        Vector2 leftShoulderI2 = new Vector2(-leftShoulderV2.y, leftShoulderV2.x);
        Vector2 rightShoulderI2 = new Vector2(-rightShoulderV2.y, rightShoulderV2.x);

        /*
		if (label == "right") {
			ShoulderAngle = Vector3.Angle (rightShoulder.position - rightElbow.position, Neck.position - TorsoC.position);
		} else {
			ShoulderAngle = Vector3.Angle (leftShoulder.position - leftElbow.position, Neck.position - TorsoC.position);
		}*/

        if (label == "right")
        {

            ShoulderAngle = Vector2.Angle(rightShoulderV2 - rightElbowV2, rightShoulderV2 - leftShoulderV2);
            if (rightElbowV2.y < rightShoulderV2.y)
                ShoulderAngle -= 90.0f;
            else
                ShoulderAngle = 270 - ShoulderAngle;


        }
        else
        {
            ShoulderAngle = Vector2.Angle(leftShoulderV2 - leftElbowV2, leftShoulderV2 - rightShoulderV2);
            if (leftElbowV2.y < leftShoulderV2.y)
                ShoulderAngle -= 90.0f;
            else
                ShoulderAngle = 270 - ShoulderAngle;
        }




        //Check if IsBodyStraight

        Vector3 CenterStraight = Neck.position - TorsoC.position;
        Vector3 VecUp = Vector3.up;
        float bodyBend = Vector3.Angle(CenterStraight, VecUp);
        if (bodyBend > maxBodyBendThreshold) // THIS COULD BE SET UP AS A USER CONFIG VALUE
        {
            isBodyStraight = false;
        }
        else
        {
            isBodyStraight = true;
        }



        if (KinectManager.Instance.IsUserDetected())
        {
            userDetectedCountdown -= Time.deltaTime;
        }
        if (userDetectedCountdown <= 0)
        {
            if (label == "right")
            {
                rightAngle = ShoulderAngle;
                lvlController.sendUIData(ShoulderAngle, "rightAngle");
                if (ShoulderAngle > maxRight)
                {
                    maxRight = ShoulderAngle;
                    lvlController.sendUIData(maxRight, "maxAngleRight");
                }

            }
            else
            {

                leftAngle = ShoulderAngle;
                lvlController.sendUIData(ShoulderAngle, "leftAngle");
                if (ShoulderAngle > maxLeft)
                {
                    maxLeft = ShoulderAngle;
                    lvlController.sendUIData(maxLeft, "maxAngleLeft");
                }
            }
        }

    }

    private bool IsHandCenter(Vector3 hand)
    {

        if (hand.y > centerWaist.position.y && hand.y < head.position.y) //head can interchange with centershoulder if need be. (head is used for greater fault tolerance of the kinect)
        {
            if (hand.x > leftShoulder.position.x && hand.x < rightShoulder.position.x)
            {
                if (hand.z <= avgBodyDepth + 4)
                {

                    return true;
                }
            }
        }
        return false;
    }

    public bool isLeftHandInGlide()
    {
        return IsHandCenter(leftHand.position);
    }

    public bool isRightHandInGlide()
    {
        return IsHandCenter(rightHand.position);
    }
}
