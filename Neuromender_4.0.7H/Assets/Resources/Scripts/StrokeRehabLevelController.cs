using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum armControlSide
{
    left = 1,
    right = 2,
    bilatteral = 3
}

public class StrokeRehabLevelController : MonoBehaviour {

    public enum Levels
    {
        mainMenu,
        Calibration,
        Glide,
        PunchingNumbers
    }

    private LoginControl userConfig;

    private Levels currLevel;
	private StrokeRehabControls mainControls;

    private HeadsUpDisplay UIController;
	private KinectManager kManager;
   // private StrokeRehabCalibration CalibrationUI;

    private float leftTarget = 70.0f;
    private float rightTarget = 70.0f;
    private bool leftTargetReached = true;
    private bool rightTargetReached = true;
    private float ThresholdIncrease = 0.0f;
	float trackSpeed = 0;

    private float GlideCooldown = 3.0f;

    private bool GlideUILoaded = false;
    private bool CalibrationUILoaded = false;

	private Camera mainCamera;
    private GameObject _cameraPlaceHolderForVr;
	public GameObject pathFollower;


	private GameObject BasicBody;
	private Vector3 jumpHeight;
	private Vector3 returnHeight;
	private Vector3 splineStartHeight;
	private bool LerpToJump = false;
	private bool LerpToPath = false;
	private bool waiting = false;
	private float lerpTime = 0.0f;

	public armControlSide armControl;

    private float leftAngle;
    private float rightAngle;

    List<st_Achievement> achievements;
    List<st_RawTracking> rawTracking;

    DataLogger_Wingman dataLogger = null;

    float rawTrackTime = 0.0f;
    float rawTrackTotal = 0.2f; //5 hertz

	System.IO.StreamWriter file;

	System.DateTime StartOfLevelTime;

    public float startingThreshold = 0.0f;
	public bool loadFailed = false;//for detecting if the player has "loaded" between rings
	public bool PushUp = false; //for animating a miss if the player hasnt "loaded"
	public bool PushDown = false; // for animating a miss if the player hasnt "loiad
	public Vector3 missDiff;//vector used for calculateing trajectory given load failed
	public Vector3 missStart;

    void MyLog(string message)
    {
        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"log.txt", true))
            sw.WriteLine(message);
    }

	// Use this for initialization
	void Start () 
	{
        MyLog("Start");

        StartOfLevelTime = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();
		file = new System.IO.StreamWriter(@"SessionData\LastSession.txt");
        MyLog("file loaded");
		mainCamera = Camera.main;
        currLevel = Levels.Glide;
		mainControls = GameObject.Find("NeuromendController").GetComponent<StrokeRehabControls>();
        MyLog("main controls object found");
		kManager = KinectManager.Instance;
        //pathFollower = GameObject.Find ("PathFollower");
        BasicBody = GameObject.Find ("basic body");
        MyLog("basic body found");
        leftAngle = 0;
        rightAngle = 0;
        _cameraPlaceHolderForVr = GameObject.Find("CameraPlaceholderForVR");

        achievements = new List<st_Achievement>();
        rawTracking = new List<st_RawTracking>();

        dataLogger = GameObject.Find("DataLogger").GetComponent<DataLogger_Wingman>();

        GameObject DBController = GameObject.Find("DatabaseController");

        UIController = GameObject.Find("HUD").GetComponent<HeadsUpDisplay>();
        MyLog("HUD found");

        if (GameObject.Find("DatabaseController"))
        {
            userConfig = DBController.GetComponent<LoginControl>();

            if (DBController.GetComponent<database>().PingConnection())
                userConfig.getUserConfig(userConfig.config.UserId);

            startingThreshold = userConfig.config.angleThreshold;
            armControl = (armControlSide)userConfig.config.sideAffected;

            leftTarget = userConfig.config.angleThreshold;
            rightTarget = userConfig.config.angleThreshold;
            ThresholdIncrease = userConfig.config.angleThresholdIncrease;


            if (userConfig.selectedSpeed == SpeedLevel.slow)
            {
                trackSpeed = userConfig.config.trackSlow;
            }
            else if (userConfig.selectedSpeed == SpeedLevel.medium)
            {
                trackSpeed = userConfig.config.trackMedium;
            }
            else if (userConfig.selectedSpeed == SpeedLevel.fast)
            {
                trackSpeed = userConfig.config.trackFast;
            }

            if (trackSpeed == 0)
            {
                trackSpeed = 300; // 5 mins
            }

            SplineController spline = GameObject.Find("PathFollower").GetComponent<SplineController>();
            spline.Duration = trackSpeed;

            GameObject splineParent = GameObject.Find("splineOne");

            if (userConfig.selectedTrack == TrackName.beach)//beach
            {
                print("Selected Beach");
                splineParent = GameObject.Find("splineTwo");
                GameObject.Find("RingHolder_spTwo").GetComponent<DistanceController>().enabled = true;
                UIController.SetTotalRings(14);

            }
            if (userConfig.selectedTrack == TrackName.forest)//forest
            {
                print("Selected Forest");
                splineParent = GameObject.Find("splineOne");
                GameObject.Find("RingHolder_spOne").GetComponent<DistanceController>().enabled = true;
                UIController.SetTotalRings(13);
                UIController.totalRings = 13;
            }
            if (userConfig.selectedTrack == TrackName.temple)//temple
            {
                print("Selected Temple");
                splineParent = GameObject.Find("splineThree");
                GameObject.Find("RingHolder_spThree").GetComponent<DistanceController>().enabled = true;
                UIController.SetTotalRings(19);
            }

            spline.SplineRoot = splineParent;
        }
        else {
            trackSpeed = 300;
        }
	}

	// Update is called once per frame
	void Update ()
	{
	    float deltaTime = Time.deltaTime;
        switch (currLevel)
	    {
	        case Levels.mainMenu:
	            break;
            case Levels.Glide:
				
				if(kManager.IsUserDetected()){

					GameObject.Find("PathFollower").GetComponent<SplineInterpolator>().enabled=true;
	                if (!GlideUILoaded)
	                {
	                    if (CalibrationUILoaded)
	                    {
	                        CalibrationUILoaded = false;
	                    }
						UIController = GameObject.Find("HUD").GetComponent<HeadsUpDisplay>();
	                    GlideUILoaded = true;
	                }

		            glideControl(deltaTime);
                    doRawTracking();
				}
                if (Input.GetKeyDown(KeyCode.BackQuote))
                {
                    userConfig.config.showDebug = !userConfig.config.showDebug;
                }
	            break;
            case Levels.PunchingNumbers:
	            break;
	    }

	}

    /// <summary>
    /// This function is called every 0.2 seconds, it is used to just put all of the values into a list to be synced out at the end of the level.
    /// </summary>
    private void doRawTracking(int ringNumber = 0, bool forced = false)
    {
        rawTrackTime += Time.deltaTime;
        if (rawTrackTime >= rawTrackTotal || forced)
        {
            //capture raw data
            rawTrackTime = 0.0f;
            st_RawTracking raw;
			raw.SessionID = userConfig.config.SessionID;
			raw.UserID = userConfig.config.UserId;
            raw.leftAngle = leftAngle;
            raw.rightAngle = rightAngle;
            raw.BodyDepth = mainControls.avgBodyDepth;
            raw.leftElbow = mainControls.leftElbow.position;
            raw.leftHand = mainControls.leftHand.position;
            raw.rightElbow = mainControls.rightElbow.position;
            raw.rightHand = mainControls.rightHand.position;
            raw.centerPoint = mainControls.TorsoC.position;
            raw.rawDate = System.DateTime.Now;
            raw.RingNumber = ringNumber;
            raw.AchievementId = UIController.achID; //get achievementID from the UIController.......
            rawTracking.Add(raw);

            // Add the data to the data logger for local and database storage AFTER the game ends (batch sending).
            dataLogger.DATA.Add("RawData_" + dataLogger.currentRawDataIndex, new WingmanRawDataMass(raw, dataLogger.currentRingNumber));
            dataLogger.currentRawDataIndex++;

            string json = JsonConvert.SerializeObject(raw,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            //write to file
            try { file.WriteLine(json); }
            catch { Debug.LogWarning("Attempt top write to a disposed of file"); }
		}
    }

    public void InvokeRawTrackingForAchievementRing(int ringNumber)
    {
        doRawTracking(ringNumber, true);
    }

    private void CalibrationLevel(float dTime)
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currLevel = Levels.Glide;
            SceneManager.LoadScene("KinectPrototypeReporting");
        }

    }

    /// <summary>
    /// This will be the controlling function of the Glide level 
    /// It handles where the character is at any given time.
    /// It will move the characted higher or lower based on the current angle of thier selected side.
    /// </summary>
    /// <param name="dTime"></param>
    private void glideControl(float dTime)
    {
		rightAngle = mainControls.getCurrentAngle ("Right");
		leftAngle = mainControls.getCurrentAngle ("Left");
        GlideCooldown += dTime;
        if (GlideCooldown >= trackSpeed/UIController.totalRings) {
			if (mainControls.MaxAngleLeft != 0) {
				if (leftTargetReached) {
                    //LEFT TARGET ACHIEVED
                    st_Achievement achieved;
                    achieved.task = en_TaskType.ElbowRaise;
                    achieved.Threshold = leftTarget;
                    achieved.TimeAchieved = System.DateTime.Now;
                    achievements.Add(achieved);

                    //leftTarget = leftTarget + ThresholdIncrease;
					UIController.leftAngleTarget = leftTarget;
					leftTargetReached = false;
				} else {
					if (UIController.leftAngle >= leftTarget) {
						leftTargetReached = true;
                        GlideCooldown = 0;

					}
					else
					{
						leftTargetReached = false;
					}
				}
			}

			if (mainControls.MaxAngleRight != 0) {
				if (rightTargetReached) {
                    //RIGHT TARGET ACHIEVED
                    st_Achievement achieved;
                    achieved.task = en_TaskType.ElbowRaise;
                    achieved.Threshold = rightTarget;
                    achieved.TimeAchieved = System.DateTime.Now;
                    achievements.Add(achieved);

                   // rightTarget = rightTarget + ThresholdIncrease;
					UIController.rightAngleTarget = rightTarget;
					rightTargetReached = false;

				} else {
					if (UIController.rightAngle >= rightTarget) {
						rightTargetReached = true;
                        GlideCooldown = 0;
					}
					else{
						rightTargetReached = false;
					}
				}
			}

		} 


		if (UIController.distanceToRing < 15 && !UIController.loaded && !loadFailed && UIController.distanceToRing > 10) {
			loadFailed = true;
			PushUp = true;

			UIController.SetVirtualPhysio("Remember to rest");

		}

		lerpTime += dTime;
		if (armControl == armControlSide.left) {
			if(loadFailed)
			{
				missStart = pathFollower.transform.position + (pathFollower.transform.up.normalized * (leftAngle / 5));
				missDiff = pathFollower.transform.position + (pathFollower.transform.up.normalized * (leftTarget / 5))+ (pathFollower.transform.up.normalized * (15))-missStart;
				if(PushUp)
				{
					
					jumpHeight = missStart+missDiff*(1-UIController.distanceToRing/15.0f);
					if( UIController.distanceToRing>15)
					{
						PushUp = false;
						PushDown = true;
						
					}
				}
				if(PushDown)
				{
					
					jumpHeight = missStart+missDiff*(1-UIController.distanceToLastRing/15.0f);
					if(UIController.distanceToLastRing>=15)
					{
						PushUp = false;
						PushDown = false;
						loadFailed = false;
					}
				}
				
			}
			else

			jumpHeight = pathFollower.transform.position + (pathFollower.transform.up.normalized * (leftAngle/5));
            UIController.SetAffected("left");
		} else if (armControl == armControlSide.right) {
			if(loadFailed)
			{
				missStart = pathFollower.transform.position + (pathFollower.transform.up.normalized * (rightAngle / 5));
				missDiff = pathFollower.transform.position + (pathFollower.transform.up.normalized * (rightTarget / 5))+ (pathFollower.transform.up.normalized * (15))-missStart;
				if(PushUp)
				{
				
				jumpHeight = missStart+missDiff*(1-UIController.distanceToRing/15.0f);
					if( UIController.distanceToRing>15)
					{
						PushUp = false;
						PushDown = true;

					}
				}
				if(PushDown)
				{

					jumpHeight = missStart+missDiff*(1-UIController.distanceToLastRing/15.0f);
					if(UIController.distanceToLastRing>=15)
					{
						PushUp = false;
						PushDown = false;
						loadFailed = false;
					}
				}

			}
			else
            jumpHeight = pathFollower.transform.position + (pathFollower.transform.up.normalized * (rightAngle / 5.0f));
            UIController.SetAffected("right");
		} else if (armControl == armControlSide.bilatteral) {
            jumpHeight = pathFollower.transform.position + (pathFollower.transform.up.normalized * (((leftTarget + rightTarget) / 2) / 5));
            UIController.SetAffected("bilateral");
		}

        _cameraPlaceHolderForVr.transform.position = Vector3.Lerp(mainCamera.transform.position, jumpHeight, lerpTime);
        //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, jumpHeight, lerpTime);
    }



    /// <summary>
    /// Sends data to Either strokeRehabUI OR strokeRehabConfiguration depending on the level
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    public void sendUIData(float value, string name)
    {
        switch (name)
        {
            case "leftHandDepth":
                if (UIController)
                {
                    UIController.lefthandDepth = value;}
                else if (CalibrationUILoaded)
                {
                //    CalibrationUI.lefthandDepth = value;
				}
                break;
            case "rightHandDepth":
                if (UIController)
                {
                    UIController.righthandDepth = value;}
                else if (CalibrationUILoaded)
                {
                 //   CalibrationUI.righthandDepth = value;
				}
                break;
            case "rightHandStretch":
                if (CalibrationUILoaded)
                {
                  //  CalibrationUI.rightHandStretch = value;
                }
                break;
            case "leftHandStretch":
                if (CalibrationUILoaded)
                {
//                    CalibrationUI.leftHandStretch = value;
                }
                break;
            case "rightHandDistance":
                if (CalibrationUILoaded)
                {
                  //  CalibrationUI.rightHandDistance = value;
                }
                break;
            case "leftHandDistance":
                if (CalibrationUILoaded)
                {
                  //  CalibrationUI.leftHandDistance = value;
                }
                break;
            case "rightAngle":
                if (UIController)
                {
                    UIController.rightAngle = value; 
                }
                break;
            case "leftAngle":
                if (UIController)
                {
                    UIController.leftAngle = value;
                }
                break;
            case "maxAngleRight":
                if (UIController)
                {
					//print ("Max RIght = " + value);
                    UIController.maxAngleRight = value;
                }
                else if (CalibrationUILoaded)
                {
                  //  CalibrationUI.MaxAngleRight = value;
                }
                break;
            case "maxAngleLeft":
                if (UIController)
                {
					//print ("Max Left = " + value);
                    UIController.maxAngleLeft = value;
                }
                else if (CalibrationUILoaded)
                {
                  //  CalibrationUI.MaxAngleLeft = value;
                }
                break;
            case "avgBodyDepth":
                if (UIController)
                {
                    UIController.avgBodyDepth = value;
                }
                else if (CalibrationUILoaded)
                {
                   // CalibrationUI.AvgBodyDepth = value;
                }
                break;

        }
    }

    /// <summary>
    /// THis returns the threshold based on what side of the body is selected in the user config.
    /// </summary>
    /// <returns></returns>
	public float getAngleThreshold()
	{
		if (armControl == armControlSide.left) {
			return leftTarget;
		} else if (armControl == armControlSide.right) {
			return rightTarget;
		} else if (armControl == armControlSide.bilatteral) {
			return (leftTarget + rightTarget) / 2;
		}
		return 0;
   }

    /// <summary>
    /// This returns the currentAngle based on what side of the body is selected in the user config.
    /// </summary>
    /// <returns></returns>
    public float getAngle()
    {
        if (armControl == armControlSide.left)
        {
            return leftAngle;
        }
        else if (armControl == armControlSide.right)
        {
            return rightAngle;
        }
        else if (armControl == armControlSide.bilatteral)
        {
            return (leftAngle + rightAngle) / 2;
        }
        return 0;
    }

    public bool syncTracking()
    {
        //close out the file to save it
        Debug.Log("Closing Level Data File.");
		file.Close();

        if (userConfig)
        {
			return userConfig.syncLevelData(achievements, rawTracking, userConfig.config.UserId, userConfig.config.SessionID, StartOfLevelTime);
        }
        else 
            return false;
        //achievements.Clear();
        //rawTracking.Clear();
    }
}


