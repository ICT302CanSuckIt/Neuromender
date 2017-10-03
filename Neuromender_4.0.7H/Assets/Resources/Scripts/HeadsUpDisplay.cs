using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
//sing System.Runtime.Extensions;

/// <summary>
/// This class Handles rendering the UI
/// That is All it does.
/// </summary>

public class HeadsUpDisplay : MonoBehaviour
{

    public float leftAngle = 0.0f;
    public float rightAngle = 0.0f;
    public float leftAngleTarget = 0.0f;
    public float rightAngleTarget = 0.0f;
    public float maxAngleLeft = 0.0f;
    public float maxAngleRight = 0.0f;
    public float lefthandDepth = 0.0f;
    public float righthandDepth = 0.0f;
    public float avgBodyDepth = 0.0f;


    public int score = 0;
    public int rings = 0;

    public int ringValue = 10;
    public float threshold = 0.0f;
    public int ringsToGo = 0;
    public int totalRings = 0;
    public float distanceToRing = 0f;
    public float distanceToLastRing = 0.0f;
    public string endComment = " ";
    public float endThresh = 0.0f;
    public string levelState = "playing";
    public bool showDebug = false;
    public string sideAffected = "";

    public string virtualPhysio = " ";
    private string[] physioPositive = new string[4] { "Good Job!", "Doing well!", "Excellent!", "Well done!" };
    public float messageCooldown = 0f;
    public float messageCoolMax = 3.0f;
    public string directionAid = " ";
    public float helperCooldown = 0f;
    public float helperCoolMax = 3.0f;
    public int pointsGained = 0;
    public float pointsCooldown = 0f;
    public float pointsCoolMax = 3.0f;

    //// old debug
    //private Text leftAngleText;
    //private Text rightAngleText;
    private Text leftAngleTargetText;
    //private Text rightAngleTargetText;
    //private Text leftAngleMaxText;
    //private Text rightAngleMaxText;
    //private Text lefthandDepthText;
    //private Text righthandDepthText;
    //private Text avgBodyDepthText;

    // new debug
    //private Text targetAngleText;
    //private Text currentAnglesText;
    private Text leftAngleText;
    private Text rightAngleText;
    //private Text maxAnglesText;
    private Text leftAngleMaxText;
    private Text rightAngleMaxText;

    private Text scoreText;
    private Text ringsText;
    private Text ringsToGoText;
    private Text distanceToRingText;
    private Text pointsGainedText;
    private Text directionAidText;
    private Text virtualPhysioText;

    private Text endCommentText;
    private Text endThreshText;
    private Text endScoreText;
    private Text endRingsText;

    public Image imgRightTarget;
    public Image imgRightCurrent;
    public Image imgLeftTarget;
    public Image imgLeftCurrent;
    public Texture2D endTexture;

    private StrokeRehabControls SRC;
    private AssistZone2 AZ2;

    //private ScoreData scoreData;
    private GameObject sideCamera;
    private GameObject scorePanel;
    private GameObject debugPanel;
    private GameObject kinectSkeletonPanel;
    private GameObject virtualPhysioPanel;
    private GameObject popUpPanel;
    private GameObject pizzaAngles;
    private GameObject pizzaAnglesLeft;
    private GameObject pizzaAnglesRight;
    private GameObject endDisplayPanel;
    private GameObject endOptionsPanel;
    private GameObject miscPanel;
    public GameObject splash;
    public GameObject camObj;

    database Database;
    private LoginControl userConfig;
    private LoginControl _loginControl;
    public List<float> anglesReached;
    List<float> ringAngles;
    List<int> ringScore;
    List<int> loadedList;
    List<int> patAssist;
    private StrokeRehabLevelController levelController;
    public float averageAngle;
    bool updateDB;//switch for updating db, so it only happens once at the end of game
    public bool loaded = false;
    public bool gotRingBool = false;
    public float reloadThreshold = 20.0f;
    public int ringsRemain = 0;
    public int reloadedRings = 0;
    public MySqlDataReader achievementID;
    public int achID;

    private DataLogger_Wingman datalogger = null;

    private int tempDistanceToRing;

    //these really should be placed in a proper game logic class. A massive refactor is needed, take out the game logic out of the HUD - Alex.A
    private const float THRESHOLD_INCREASE_PERCENTAGE = 0.7f; // 70%
    private const float THRESHOLD_DECREASE_PERCENTAGE = 0.5f; // 50%

    // Use this for initialization
    void Start()
    {
        //Debug.LogError("test");
        InitUI();
        Database = GameObject.Find("DatabaseController").GetComponent<database>();
        userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        _loginControl = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        levelController = GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>();
        averageAngle = 0.0f;
        updateDB = true;
        anglesReached = new List<float>();
        ringAngles = new List<float>();
        ringScore = new List<int>();
        patAssist = new List<int>();
        loadedList = new List<int>();
        reloadedRings = 0;
        userConfig.config.WingmanGameNum += 1;

        datalogger = GameObject.Find("DataLogger").GetComponent<DataLogger_Wingman>();

        if (sideCamera)
            sideCamera.SetActive(false);

        //totalRings = 0;
        //		if(SplineController.SplineRoot.CompareTag("Path1"))
        //			totalRings = 15;
        //		else if(SplineController.SplineRoot.CompareTag("Path2"))
        //			totalRings = 16;
        //		else if(SplineController.SplineRoot.CompareTag("Path3"))
        //			totalRings = 17;
        //		else
        //			totalRings = 10;

        showDebug = GameObject.Find("DatabaseController").GetComponent<LoginControl>().config.showDebug;
        //messageCooldown = 0.0f;// messageCoolMax;
        //pointsCooldown = 0.0f;// pointsCoolMax;
        score = 0;
        rings = 0;

        ringsToGo = totalRings;
        ringsRemain = totalRings;
        distanceToRing = 20;
        pointsGained = 0;
        //SetDirectionAid("");
        directionAid = "";
        SetVirtualPhysio("Go!");
        CheckBodyStraight();

        //ratio = Screen.height / Screen.width;
        Time.timeScale = 1.0f;
        splash.GetComponent<GUITexture>().enabled = false;
        //levelOver = false;
        //paused = false;
        camObj.GetComponent<Camera>().enabled = true;

        levelState = "playing";
        scorePanel.SetActive(true);
        virtualPhysioPanel.SetActive(true);
        popUpPanel.SetActive(true);
        pizzaAngles.SetActive(true);
        debugPanel.SetActive(showDebug);
        miscPanel.SetActive(true);
        endDisplayPanel.SetActive(false);
        endOptionsPanel.SetActive(false);
        PizzaHUD();

        //set the Achievement table entry to be used for logging data - Alex.A
        SetAchievementEntryDB();
    }

    //might want to move this to a separate class.
    private void InitUI()
    {
        //// OLD DEBUG TEXT
        //leftAngleText = GameObject.Find("LeftAngleText").GetComponent<Text>();
        //rightAngleText = GameObject.Find("RightAngleText").GetComponent<Text>();
        //leftAngleTargetText = GameObject.Find("LeftTargetText").GetComponent<Text>();
        //rightAngleTargetText = GameObject.Find("RightTargetText").GetComponent<Text>();
        //leftAngleMaxText = GameObject.Find("LeftMaxText").GetComponent<Text>();
        //rightAngleMaxText = GameObject.Find("RightMaxText").GetComponent<Text>();
        //lefthandDepthText = GameObject.Find("LeftHandDepthText").GetComponent<Text>(); 
        //righthandDepthText = GameObject.Find("RightHandDepthText").GetComponent<Text>(); 
        //avgBodyDepthText = GameObject.Find("AvgDepthText").GetComponent<Text>();

        // NEW DEBUG TEXT                
        leftAngleTargetText = GameObject.Find("LeftTargetText").GetComponent<Text>();
        //currentAnglesText = GameObject.Find("CurrentAnglesText").GetComponent<Text>();
        leftAngleText = GameObject.Find("LeftAngleText").GetComponent<Text>();
        rightAngleText = GameObject.Find("RightAngleText").GetComponent<Text>();
        //maxAnglesText = GameObject.Find("MaxAnglesText").GetComponent<Text>();
        leftAngleMaxText = GameObject.Find("LeftMaxText").GetComponent<Text>();
        rightAngleMaxText = GameObject.Find("RightMaxText").GetComponent<Text>();

        // SCORE TEXT
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        ringsText = GameObject.Find("RingsText").GetComponent<Text>();
        ringsToGoText = GameObject.Find("RingsToGoText").GetComponent<Text>();
        distanceToRingText = GameObject.Find("DistanceToRingText").GetComponent<Text>();
        pointsGainedText = GameObject.Find("PointsGainedText").GetComponent<Text>();
        directionAidText = GameObject.Find("DirectionAidText").GetComponent<Text>();
        virtualPhysioText = GameObject.Find("VirtualPhysioText").GetComponent<Text>();

        // END TEXT
        endCommentText = GameObject.Find("EndCommentText").GetComponent<Text>();
        endThreshText = GameObject.Find("EndThreshText").GetComponent<Text>();
        endScoreText = GameObject.Find("EndScoreText").GetComponent<Text>();
        endRingsText = GameObject.Find("EndRingsText").GetComponent<Text>();

        // GAME OBJECTS
        SRC = GameObject.Find("NeuromendController").GetComponent<StrokeRehabControls>();
        AZ2 = GameObject.Find("AssistZone2").GetComponent<AssistZone2>();


        //scoreData = GameObject.Find("ScoreData").GetComponent<ScoreData>();
        sideCamera = GameObject.Find("SideCamera");
        scorePanel = GameObject.Find("Score Panel");
        debugPanel = GameObject.Find("DeBug Panel");
        kinectSkeletonPanel = GameObject.Find("KinectSkeleton");
        virtualPhysioPanel = GameObject.Find("VirtualPhysio Panel");
        popUpPanel = GameObject.Find("PopUp Panel");
        pizzaAngles = GameObject.Find("PizzaAngles");
        pizzaAnglesLeft = GameObject.Find("PizzaLeftHud");
        pizzaAnglesRight = GameObject.Find("PizzaRightHud");
        if (GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>().armControl == armControlSide.right)
        {
            pizzaAnglesLeft.SetActive(false);
            pizzaAnglesRight.SetActive(true);
            imgRightTarget.transform.eulerAngles = new Vector3(0, 0, rightAngleTarget);
            imgRightCurrent.transform.eulerAngles = new Vector3(0, 0, rightAngle);
        }
        else if (GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>().armControl == armControlSide.left)
        {
            pizzaAnglesLeft.SetActive(true);
            pizzaAnglesRight.SetActive(false);
            imgLeftTarget.transform.eulerAngles = new Vector3(0, 0, -leftAngleTarget);
            imgLeftCurrent.transform.eulerAngles = new Vector3(0, 0, -leftAngle);
        }
        miscPanel = GameObject.Find("Misc Panel");
        endDisplayPanel = GameObject.Find("End Display Panel");
        endOptionsPanel = GameObject.Find("End Options Panel");

        //pointsGainedText.guiText.enabled = true;
    }

    public void SetTotalRings(int setTotalRings)
    {
        totalRings = setTotalRings;
    }

    public void SetAffected(string side)
    {
        sideAffected = side;
    }

    public void SetDistanceToNext(float setDistToNext)
    {
        distanceToRing = setDistToNext;
    }

    public void SetDistanceToLast(float setDistToLast)
    {
        distanceToLastRing = setDistToLast;
    }

    public void SetDirectionAid(string aid)
    {
        directionAid = aid;
        helperCooldown = 0f;
    }

    public void SetVirtualPhysio(string newMessage)
    {
        //Random r = new Random();
        //int i = r.Range(0, physioPositive.Length);
        if (newMessage == "")
        {
            int i = Random.Range(0, physioPositive.Length);
            virtualPhysio = physioPositive[i];
        }
        else
            virtualPhysio = newMessage;

        messageCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        updateValues();

        if (levelState == "playing")
        {
            // normal HUD
            playHUD();
        }
        else if (levelState == "paused")
        {
            // pause menu
            pauseHUD();
        }
        else if (levelState == "finished")
        {
            // end splash
            endHUD();
        }
        else
        {
            // error
            playHUD();
        }

        PizzaHUD();
        if (sideCamera)
            depthHandCamera();
    }

    private void updateValues()
    {
        //score = scoreData.GetScore();
        //rings = scoreData.rings;
        //ringsToGo = totalRings - ringsPassed;
        //distanceToRing = 0;
        //pointsGained = 0;
        //directionAid = " ";
        //virtualPhysio = " ";
        messageCooldown += Time.deltaTime;
        pointsCooldown += Time.deltaTime;
    }

    private void playHUD()
    {
        if (!scorePanel.activeInHierarchy)
        {
            scorePanel.SetActive(true);
            virtualPhysioPanel.SetActive(true);
            popUpPanel.SetActive(true);
            pizzaAngles.SetActive(true);
            debugPanel.SetActive(showDebug);
            miscPanel.SetActive(true);
            endDisplayPanel.SetActive(false);
            endOptionsPanel.SetActive(false);
        }

        showDebug = GameObject.Find("DatabaseController").GetComponent<LoginControl>().config.showDebug;
        //	showDebug = true;
        if (showDebug)
        {
            debugPanel.SetActive(true);
            kinectSkeletonPanel.SetActive(true);

            //// Old debug
            //leftAngleText.text = " " + leftAngle.ToString("n2");
            //rightAngleText.text = " " + rightAngle.ToString("n2");

            leftAngleTargetText.text = " " + leftAngleTarget.ToString("n2");
            //rightAngleTargetText.text = " " + rightAngleTarget.ToString("n2");

            ////max Angle Update
            //rightAngleMaxText.text = " " + maxAngleRight.ToString("n2");
            //leftAngleMaxText.text = " " + maxAngleLeft.ToString("n2");

            //////hand depths
            //lefthandDepthText.text = " " + lefthandDepth.ToString("n2");
            //righthandDepthText.text = " " + righthandDepth.ToString("n2");
            //avgBodyDepthText.text = " " + avgBodyDepth.ToString("n2");

            //// new debug
            //targetAngleText.text = " " + leftAngleTarget.ToString("n2");
            //currentAnglesText.text = "Current  Raise Angles";
            leftAngleText.text = " " + leftAngle.ToString("n2");
            rightAngleText.text = " " + rightAngle.ToString("n2");
            //maxAnglesText.text = "Maximum Raise Angles";
            leftAngleMaxText.text = " " + maxAngleLeft.ToString("n2");
            rightAngleMaxText.text = " " + maxAngleRight.ToString("n2");
        }
        else
        {
            debugPanel.SetActive(false);
            kinectSkeletonPanel.SetActive(false);
        }

        scoreText.text = "Score: " + score;
        ringsText.text = "Rings: " + rings;
        ringsToGoText.text = " " + ringsToGo + " Rings Left";

        if (messageCooldown >= messageCoolMax)
        {
            virtualPhysioText.text = "";
            //distanceToRingText.text = "Next ring: " + distanceToRing + "m";
            CheckBodyStraight();
            CheckCloseToRing();
        }
        else /*if ( positiveFeedback)*/
        {
            //RandomisePhysio();
            virtualPhysioText.text = "" + virtualPhysio;
            //distanceToRingText.text = "";
        }

        if (loaded || rings == 0)
            SetDirectionAid("Elbow Up");
        else
            SetDirectionAid("Elbow Down");

        if (sideAffected == "right")
        {

            if (rightAngle <= reloadThreshold)
                loaded = true;
            if (loaded && rightAngle > rightAngleTarget - 5)//check near, if near set aid to " "
                SetDirectionAid(" ");
        }
        else if (sideAffected == "left")
        {

            if (leftAngle <= reloadThreshold)
                loaded = true;
            if (loaded && leftAngle > leftAngleTarget - 5)
                SetDirectionAid(" ");
        }
        if (ringsRemain >= 0 && distanceToRing < 1)
        {



            /*if(sideAffected == "right")
			anglesReached.Add(rightAngle);
			else if (sideAffected == "left")
				anglesReached.Add (leftAngle);*/


            if (sideAffected == "right")
            {
                if (rightAngle != 0)
                {
                    ringAngles.Add(rightAngle);
                    Debug.Log("Angle : " + rightAngle + " loaded = " + loaded);
                    if (loaded)
                    {
                        averageAngle += rightAngle;
                        //  Debug.Log("**ADDING = " + rightAngle + " " + ringsRemain);
                        reloadedRings++;
                    }
                    if (loaded)
                    {
                        loadedList.Add(1);
                    }
                    else
                    {
                        loadedList.Add(0);
                    }
                }

            }
            else if (sideAffected == "left")
            {
                if (leftAngle != 0)
                {
                    ringAngles.Add(leftAngle);
                    if (loaded)
                    {
                        averageAngle += leftAngle;
                        Debug.Log("**ADDING 12332= " + leftAngle);
                        reloadedRings++;
                    }
                    if (loaded)
                    {
                        loadedList.Add(1);
                    }
                    else
                    {
                        loadedList.Add(0);
                    }
                }
            }

            //RING ANGLE DATA IS RECORDED HERE - NEED TO LOG TO RAW TRACKING DATA TOO - NEED A BETTER PLACE TO DO THIS - ALEX.A
            var mainControl = GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>();
            mainControl.InvokeRawTrackingForAchievementRing(ringAngles.Count);

            //Add score here, can't rely on the collider. Formula taken from RingCollider.cs
            float angleDiff = System.Math.Abs(mainControl.getAngle() - mainControl.getAngleThreshold());
            int theScore = 10 - ((int)angleDiff);
            theScore = (theScore < 0) ? 0 : theScore; //prevent score from going less than 0
            ringScore.Add(theScore);

            //trying to add assisted here
            //(SRC.Assisted).Add(patAssist);

            if (AZ2.Assistedsign == true)
            {
                patAssist.Add(1);
            }
            else
            {
                patAssist.Add(0);
            }

            // Add a new entry for the basic game data.
            Debug.Log("PING");
            datalogger.DATA.Add("Ring_" + datalogger.currentRingNumber, new WingmanDataMass(datalogger.currentRingNumber, theScore, mainControl.getAngle(), AZ2.Assistedsign));
            datalogger.currentRingNumber++;


            Debug.Log("**NEW TOTAL AVG = " + averageAngle);
            ringsRemain--;
            loaded = false;
        }

        //ringsToGo = totalRings

        //distanceToRingText.text = "Next ring: " + distanceToRing.ToString("n2") + "m";
        tempDistanceToRing = (int)distanceToRing;
        distanceToRingText.text = "Next ring: " + tempDistanceToRing.ToString("n2") + "m";

        if (pointsCooldown >= pointsCoolMax || pointsGained == 0)
            pointsGainedText.text = "";
        else
            pointsGainedText.text = "+" + pointsGained;

        if (helperCooldown >= helperCoolMax)
            directionAidText.text = "";
        else
            directionAidText.text = "" + directionAid;
        //Straighten Up!
        //Ready... Rest!
        //Perfect!
    }

    private void pauseHUD()
    {

    }

    private void endHUD()
    {
        scorePanel.SetActive(false);
        debugPanel.SetActive(false);
        virtualPhysioPanel.SetActive(false);
        popUpPanel.SetActive(false);
        pizzaAngles.SetActive(false);
        miscPanel.SetActive(false);
        endDisplayPanel.SetActive(true);
        endOptionsPanel.SetActive(true);
        /*	foreach (float angle in anglesReached)
            {
                averageAngle += angle;
            }
            averageAngle = averageAngle / anglesReached.Count;*/
        //averageAngle = averageAngle / totalRings;
        averageAngle = averageAngle / reloadedRings;

        //endThresh = GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>().getAngleThreshold();
        float startingThresh = GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>().startingThreshold;
        endThresh = startingThresh;
        if (rings / (float)totalRings >= THRESHOLD_INCREASE_PERCENTAGE)
            endThresh = userConfig.config.angleThreshold + userConfig.config.angleThresholdIncrease;
        else if (rings / (float)totalRings < THRESHOLD_DECREASE_PERCENTAGE)
            endThresh = userConfig.config.angleThreshold - userConfig.config.angleThresholdIncrease;

        endCommentText.text = endComment;
        if ((rings / (float)totalRings >= THRESHOLD_INCREASE_PERCENTAGE) || (rings / (float)totalRings < THRESHOLD_DECREASE_PERCENTAGE))
            endThreshText.text = "Your threshold changed from " + startingThresh + " to " + endThresh;
        else
            endThreshText.text = "Current threshold is " + startingThresh;
        endScoreText.text = "Score: " + score;
        endRingsText.text = "You got " + rings + " out of " + totalRings + " rings";

        string table = "";
        string fields = "";
        string where = "";
        if (updateDB)
        {

            if (datalogger)
                datalogger.SerialiseData();

            updateDB = false; //flag to turn off database update

            Debug.Log("Sending to DB avg angle = " + averageAngle);
            for (int i = 0; i < totalRings; i++)
            {
                table = "AchievementRings";
                //fields = "AcheivementID, RingNumber, Angle, Reloaded";
                fields = "AcheivementID, GameNo, RingNumber, Angle, Reloaded, Score, Assisted";
                //values =  achID + "," + (i+1) + ","+ ringAngles[i] + "," + loadedList[i] ;
                string values = achID + "," + userConfig.config.WingmanGameNum + "," + (i + 1) + "," + ringAngles[i] + "," + loadedList[i] + "," + ringScore[i] + "," + patAssist[i + 1];
                Database.SubmitData(table, fields, values);
                Debug.Log("Game (" + userConfig.config.WingmanGameNum + ") ring (" + (i + 1) + ") angle : " + ringAngles[i] + " Loaded: " + loadedList[i]);
            }


            if (rings / (float)totalRings >= THRESHOLD_INCREASE_PERCENTAGE)
            {

                ///Sync out the update to the session
                table = "WingmanRestrictions";
                fields = "AngleThreshold =" + (userConfig.config.angleThreshold + userConfig.config.angleThresholdIncrease) + "";
                where = "UserID =" + userConfig.config.UserId;
                Database.UpdateData(table, fields, where);
            }
            else if (rings / (float)totalRings < THRESHOLD_DECREASE_PERCENTAGE)
            {
                ///Sync out the update to the session
                table = "WingmanRestrictions";
                fields = "AngleThreshold =" + (userConfig.config.angleThreshold - userConfig.config.angleThresholdIncrease) + "";
                where = "UserID =" + userConfig.config.UserId;
                Database.UpdateData(table, fields, where);
            }

            float avgScore = ringScore.Sum() / (float)totalRings;

            //update achievement entry
            table = "Achievement";
            fields = "Completed=1, ThresholdPassed=" + averageAngle + ", TimeAchieved='" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', Score='" + avgScore + "'";
            where = "AcheivementID =" + achID;
            Database.UpdateData(table, fields, where);
        }

    }

    private void GlideHUD()
    {
        if (SRC.isLeftHandInGlide())
        {

        }
        if (SRC.isRightHandInGlide())
        {

        }
    }

    private void depthHandCamera()
    {
        if (sideCamera.activeSelf == false)
        {
            if (lefthandDepth > 6 || righthandDepth > 6)
            {
                sideCamera.SetActive(true);
            }
        }
        else
        {
            if (lefthandDepth < 6 && righthandDepth < 6)
            {
                sideCamera.SetActive(false);
            }
        }
    }

    private void PizzaHUD()
    {

        if (sideAffected == "right")
        {
            if (loaded && rightAngleTarget != 0)
                imgRightTarget.transform.eulerAngles = new Vector3(0, 0, rightAngleTarget);
            else
            {
                imgRightTarget.transform.eulerAngles = new Vector3(0, 0, reloadThreshold);

            }

            imgRightCurrent.transform.eulerAngles = new Vector3(0, 0, rightAngle);
        }
        else if (sideAffected == "left")
        {
            if (loaded && leftAngleTarget != 0)
                imgLeftTarget.transform.eulerAngles = new Vector3(0, 0, -leftAngleTarget);
            else
                imgLeftTarget.transform.eulerAngles = new Vector3(0, 0, -reloadThreshold);

            imgLeftCurrent.transform.eulerAngles = new Vector3(0, 0, -leftAngle);
        }
    }

    private void CheckBodyStraight()
    {
        if (!SRC.isBodyStraight)
        {
            virtualPhysio = "Straighten your body!";
            messageCooldown = messageCoolMax / 2;
        }
    }

    private void CheckCloseToRing()
    {


        //if (distanceToRing < 2)
        //loaded = false;


    }

    public void PauseToggle()
    {

    }

    public void GotRing(int points, float ang)
    {
        rings++;
        score += points;
        pointsGained = points;
        pointsCooldown = 0.0f;
        SetVirtualPhysio("");
        //anglesReached.Add(ang); 
        //loaded = false;
        gotRingBool = true;
    }

    public void EndLevel()
    {
        levelState = "finished";
        FinalScore();
        splash.GetComponent<GUITexture>().enabled = true;
        splash.GetComponent<GUITexture>().texture = endTexture;

        //levelOver = true;
        camObj.GetComponent<Camera>().enabled = false;
        Time.timeScale = 0.0f;
    }

    public void FinalScore()
    {
        GameState.Instance.lastRings = rings;
        GameState.Instance.lastThresh = threshold;

        GameState.Instance.lastScore = score;

        GameState.Instance.lastLevel = true;
    }

    public void restartLevel()
    {
        if (_loginControl.PlaytimeLimitReached("Wingman"))
            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("TestNeuromend3_CMaster");
    }

    public void returnToLevelSelect()
    {
        if (_loginControl.PlaytimeLimitReached("Wingman"))
            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        if (GameObject.Find("DatabaseController"))
        {
            GameObject.Find("DatabaseController").GetComponent<LoginControl>().logoutSetAverage();
            GameObject.Destroy(GameObject.Find("DatabaseController"));
        }
        SceneManager.LoadScene("Login");
    }

    /// <summary>
    /// Need to move all of these Database functionality somewhere else. Make a dedicated class to handle Database operations
    /// Alex.A
    /// </summary>
    private void SetAchievementEntryDB()
    {
        string table = "Achievement";
        string fields = "SessionID, TaskID, ThresholdPassed, TimeAchieved";
        string values = "";
        values += userConfig.config.SessionID + "," + 1 + "," + 0 + ",'" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        Database.SubmitData(table, fields, values);
        userConfig.gamesPlayed.WingmanPlayed = true;

        //get achievement id
        achievementID = Database.selectData("Select AcheivementID From Achievement WHERE Completed=0 ORDER BY AcheivementID DESC LIMIT 1");
        if (achievementID != null)
        {
            if (achievementID.HasRows)
            {
                achievementID.Read();
                achID = achievementID.GetInt32(0);
            }
            achievementID.Close();
        }

    }
}
