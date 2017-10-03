using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ScoreCarnival : MonoBehaviour
{
    public int ShotsTotal = 20;
    public int ShotCurrent = 0;
    public int ShotsHit = 0;
    public int ShotsMissed = 0;
    public int Score = 0;
    public int MissCounter = 0;

    public Text ShotTracker;
    public Text Hits;
    public Text Misses;
    public Text Points;
    public Text PointFloating;

    public Text EPHits;
    public Text EPScore;
    public Text EPDistTxt;
    public Text EPDist;

    public Text UserPrompts;

    public float tPosX;
    public float tPosY;
    public float hPosX;
    public float hPosY;
    public float accuracy;
    public float minReach;
    public float maxReach;
    public string shotTime;

    public GameObject ShotScore;
    public Text Point;
    public GameObject ShotLatency;
    public Text Latency;

    private GameObject CurTarget;
    public bool Assisted;

    public GameObject AssistedBox;


    public bool Finished = false;

    public AudioClip missSound;
    public AudioClip hitSound;


    private AudioSource source;
    private float lowPitchRange = .75F;
    private float highPitchRange = 1.5F;
    private float velToVol = .2F;
    private float velocityClipSplit = 10F;

    public List<RectTransform> TextBoxesTop;
    public List<RectTransform> TextBoxesBot;

    public int UID = 0; // user id
    public int TGN = 0; // target game number
    public int Session = 0; // Session ID

    public GameObject DBcons;

    public GameObject EndPanel;

    void OnLevelWasLoaded()
    {

        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

            UID = DBcons.GetComponent<LoginControl>().config.UserId;

            // get and increase game number
            TGN = DBcons.GetComponent<LoginControl>().config.TargetGameNum + 1;
            DBcons.GetComponent<LoginControl>().config.TargetGameNum = TGN;

            Session = DBcons.GetComponent<LoginControl>().config.SessionID;
        }
        else
        {
            // use default values
        }


    }

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (ShotTracker)
        {
            if (Finished)
            {
                ShotTracker.text = "DONE";
            }
            else
            {
                ShotTracker.text = ShotCurrent + " / " + ShotsTotal;
            }
        }
        if (Hits)
        {
            Hits.text = "" + ShotsHit;
        }
        if (Misses)
        {
            Misses.text = "" + ShotsMissed;
        }
        if (Points)
        {
            Points.text = "" + Score;
        }

        PointFloating.color = PointFloating.color - new Color(0, 0, 0, Time.deltaTime * 0.2f);
    }

    public void Hit()
    {
        ShotsHit++;
        source.PlayOneShot(hitSound, 1f);
        CheckFinished();
        MissCounter = 0;
    }
    public void Miss()
    {
        ShotsMissed++;
        source.PlayOneShot(missSound, 1f);
        CheckFinished();

        MissCounter++;
        if (MissCounter == 3)
        {
            MissCounter = 0;
            GameObject.FindGameObjectWithTag("Target").GetComponent<NodeTarget>().TooHard();
        }

    }

    public void ScorePoints(int points, Vector2 floatPos)
    {
        Score += points;

        PointFloating.transform.position = floatPos;
        PointFloating.GetComponent<Text>().text = "" + points;
        PointFloating.GetComponent<Text>().color = Color.black;
    }

    public bool CheckFinished()
    {
        if (ShotCurrent == ShotsTotal)
        {
            Finished = true;

            if (ShotsHit >= ShotsTotal) // perfect game
            {
                EPDistTxt.text = "DISTANCE\nINCREASED\nBY " + DBcons.GetComponent<LoginControl>().config.ExtensionThresholdIncrease + " MM";
                GameObject.Find("Grid").GetComponent<Grid>().IncreaseDepth();
            }

            if (ShotsHit <= 0.5f * ShotsTotal) // shitty game
            {
                EPDistTxt.text = "DISTANCE\nDECREASED\nBY " + DBcons.GetComponent<LoginControl>().config.ExtensionThresholdIncrease + " MM";
                GameObject.Find("Grid").GetComponent<Grid>().DecreaseDepth();
                //Debug.Log(ShotsHit+" "+ShotsTotal);
            }

            LogData();

            DisplayEndPanel();
        }

        return Finished;
    }

    private void LogData()
    {
        // Serialise the data from this session locally.
        if (GameObject.Find("DataLogger"))
        {
            if (GameObject.Find("DataLogger").GetComponent<DataLogger_Targets>())
                GameObject.Find("DataLogger").GetComponent<DataLogger_Targets>().SerialiseData();
            else
                Debug.Log("Cannot find data logger script!");
        }
        else
            Debug.Log("Cannot find data logger object!");
    }

    public void RightText()
    {
        foreach (RectTransform tb in TextBoxesTop)
        {
            tb.anchorMin = new Vector2(1, 1);
            tb.anchorMax = new Vector2(1, 1);
            tb.pivot = new Vector2(1, 1);
            tb.position = new Vector3(Screen.width - 10, tb.position.y, 0);
            //Debug.Log(Screen.width);
        }
        foreach (RectTransform tb in TextBoxesBot)
        {
            tb.anchorMin = new Vector2(1, 0);
            tb.anchorMax = new Vector2(1, 0);
            tb.pivot = new Vector2(1, 0);
            tb.position = new Vector3(Screen.width - 10, tb.position.y, 0);
            //Debug.Log(Screen.width);
        }
    }

    public void LeftText()
    {
        foreach (RectTransform tb in TextBoxesTop)
        {
            tb.anchorMin = new Vector2(0, 1);
            tb.anchorMax = new Vector2(0, 1);
            tb.pivot = new Vector2(0, 1);
            tb.position = new Vector3(10, tb.position.y, 0);
            //Debug.Log(Screen.width);
        }
        foreach (RectTransform tb in TextBoxesBot)
        {
            tb.anchorMin = new Vector2(0, 0);
            tb.anchorMax = new Vector2(0, 0);
            tb.pivot = new Vector2(0, 0);
            tb.position = new Vector3(10, tb.position.y, 0);
            //Debug.Log(Screen.width);
        }
    }

    public void DisplayShot(int points, float latency)
    {
        if (DBcons) DBcons.GetComponent<LoginControl>().gamesPlayed.TargetsPlayed = true;

        ShotScore.SetActive(true);
        ShotLatency.SetActive(true);
        shotTime = "'" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";

        if (points == 0) Point.text = "MISS";
        else Point.text = "" + points + " POINT";
        if (points > 1) Point.text = "" + points + " POINTS";

        Latency.text = latency.ToString("0.00") + " SEC";

        /*
         * SEND SHOT DATA FROM HERE
         * DB NAME      - UNITY NAME    - DESCRIPTION
         * UserID       - UID           - this User
         * GameNoID     - TGN           - ever increasing for current game number
         * RoundID      - ShotCurrent   - shot number just completed
         * SessionID    - Session       - users session number
         * Points       - points        - accuracy on point scale from 0 to 10
         * Accuracy     - accuracy      - accuracy in a percentage format
         * Latency      - latency       - time it took from loading to hit/miss
         * TargetPositionX - tPosX      - target location x-axis
         * TargetPositionX - tPosY      - target location y-axis
         * HitPositionX - hPosX         - hit location x-axis
         * HitPositionX - hPosY         - pos location y-axis
         * Assisted     - Assisted      - boolean for assisting (0 for false, 1 for true)
         * MaximumReach - maxReach      - how far did the user reach for the hit
         * MinimumReach - minReach      - how close to their shoulder did they squeeze
         * TimeCreated  - shotTime      - current real world date/time
         * 
        */
        /*
        string table = "ReachGameData";
        string fields = "";
        string values = "";

        //Debug.Log("****************");

        //Debug.Log("UserID: " + UID);
        fields += "UserID";
        values += UID;

        //Debug.Log("SessionID: " + Session);
        fields += ",SessionID";
        values += "," + Session;

        //Debug.Log("GameNoID: " + TGN);
        fields += ",GameNoID";
        values += "," + TGN;

        //Debug.Log("RoundID: " + ShotCurrent);
        fields += ",RoundID";
        values += "," + ShotCurrent;

        //Debug.Log("Accuracy: " + accuracy);
        fields += ",Accuracy";
        values += "," + accuracy;

        //Debug.Log("Points: " + points);
        fields += ", Points";
        values += "," + points;

        //Debug.Log("tPosX: " + tPosX);
        fields += ", TargetPositionX";
        values += "," + tPosX;

        //Debug.Log("tPosY: " + tPosY);
        fields += ", TargetPositionY";
        values += "," + tPosY;

        //Debug.Log("hPosX: " + hPosX);
        fields += ", HitPositionX";
        values += "," + hPosX;

        //Debug.Log("hPosY: " + hPosY);
        fields += ", HitPositionY";
        values += "," + hPosY;

        //Debug.Log("Assisted: " + Assisted);
        fields += ", Assisted";
        values += "," + Assisted;

        //Debug.Log("MaximumReach: " + maxReach*100);
        fields += ", MaximumReach";
        values += "," + maxReach * 100;

        //Debug.Log("MinimumReach: " + minReach*100);
        fields += ", MinimumReach";
        values += "," + minReach * 100;

        //Debug.Log("Latency: " + latency);
        fields += ", Latency";
        values += "," + latency;

        //Debug.Log("ShotTime: " + shotTime);
        fields += ", TimeCreated";
        values += "," + shotTime;

        //Debug.Log("****************");

        //Debug.Log(fields);
        //Debug.Log(values);


        if (DBcons) DBcons.GetComponent<database>().SubmitData(table, fields, values);

        SendRawData();*/

        // reset minReach
        minReach = maxReach;
    }

    public void SendRawData()
    {
        /*
         * SEND SHOT DATA FROM HERE
         * DB NAME      - UNITY NAME    - DESCRIPTION
         * 
      
	`RawTrackingID` int(11) NOT NULL,
    `UserID` int(11) NOT NULL,
	`SessionID` int(11) NOT NULL,
	`LevelName` varchar(32) NOT NULL,
	`GridSize` int(11) NOT NULL,
	`GameNoID` int(11) NOT NULL,
	`RoundID` int(11) NOT NULL,
	`Time` datetime NOT NULL,
	`ElbowAngle` float NOT NULL,
	`ShoulderToWristDistance` float NOT NULL,
	`ShoulderToWristAngleHorizontal` float NOT NULL,
	`ShoulderToWristAngleVertical` float NOT NULL,
       
    `WristLeftPositionX` float NOT NULL,
    `WristLeftPositionY` float NOT NULL,
    `WristLeftPositionZ` float NOT NULL,
         * repeat pattern for
     ElbowLeft
     ShoulderLeft
     Chest
     ShoulderRight
     ElbowRight
     WristRight
	
         * 
         * 
        */

        string table = "ReachTrackingData";
        string fields = "";
        string values = "";

        //Debug.Log("****************");

        string RTID = "" + UID + Session + TGN + ShotCurrent;
        //Debug.Log("RawTrackingID: " + UID);
        //fields += "RawTrackingID";
        //values += RTID;

        //Debug.Log("UserID: " + UID);
        fields += "UserID";
        values += UID;

        //Debug.Log("SessionID: " + Session);
        fields += ",SessionID";
        values += "," + Session;

        //Debug.Log("LevelName: " + "Targets");
        fields += ",LevelName";
        values += ", 'Targets'";

        //int GridSize = GameObject.Find("Grid").GetComponent<Grid>().Size;
        //Debug.Log("GridSize: " + GridSize);
        fields += ",GridSize";
        values += ",'" + GameObject.Find("Grid").GetComponent<Grid>().rows + "," + GameObject.Find("Grid").GetComponent<Grid>().cols + "'";

        //Debug.Log("GameNoID: " + TGN);
        fields += ",GameNoID";
        values += "," + TGN;

        //Debug.Log("RoundID: " + ShotCurrent);
        fields += ",RoundID";
        values += "," + ShotCurrent;

        string now = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        //Debug.Log("Time: " + now);
        fields += ",Time";
        values += "," + now;

        //joints
        Vector3 WristLeft = GameObject.Find("WristLeft").transform.position;
        Vector3 ElbowLeft = GameObject.Find("ElbowLeft").transform.position;
        Vector3 ShoulderLeft = GameObject.Find("ShoulderLeft").transform.position;
        Vector3 Chest = GameObject.Find("SpineMid").transform.position;
        Vector3 ShoulderRight = GameObject.Find("ShoulderRight").transform.position;
        Vector3 ElbowRight = GameObject.Find("ElbowRight").transform.position;
        Vector3 WristRight = GameObject.Find("WristRight").transform.position;

        float ElbowAngle;
        float ShoulderToWristDistance;
        float ShoulderToWristAngleHorizontal;
        float ShoulderToWristAngleVertical;

        if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1) // left
        {
            ElbowAngle = Vector3.Angle(WristLeft - ElbowLeft, ShoulderLeft - ElbowLeft);
            ShoulderToWristDistance = Vector3.Magnitude(WristLeft - ShoulderLeft);
            ShoulderToWristAngleHorizontal = Mathf.Atan2(WristLeft.x - ShoulderLeft.x, WristLeft.z - ShoulderLeft.z) * 180 / Mathf.PI;
            ShoulderToWristAngleVertical = Mathf.Atan2(WristLeft.x - ShoulderLeft.x, WristLeft.y - ShoulderLeft.y) * -180 / Mathf.PI + 90;
        }
        else // right
        {
            ElbowAngle = Vector3.Angle(WristRight - ElbowRight, ShoulderRight - ElbowRight);
            ShoulderToWristDistance = Vector3.Magnitude(WristRight - ShoulderRight);
            ShoulderToWristAngleHorizontal = Mathf.Atan2(WristRight.x - ShoulderRight.x, WristRight.z - ShoulderRight.z) * 180 / Mathf.PI;
            ShoulderToWristAngleVertical = Mathf.Atan2(WristRight.x - ShoulderRight.x, WristRight.y - ShoulderRight.y) * -180 / Mathf.PI + 90;
        }

        //Debug.Log("ElbowAngle: " + ElbowAngle);
        fields += ",ElbowAngle";
        values += "," + ElbowAngle;

        //Debug.Log("ShoulderToWristDistance: " + ShoulderToWristDistance);
        fields += ",ShoulderToWristDistance";
        values += "," + ShoulderToWristDistance;

        //Debug.Log("ShoulderToWristAngleHorizontal: " + ShoulderToWristAngleHorizontal);
        fields += ",ShoulderToWristAngleHorizontal";
        values += "," + ShoulderToWristAngleHorizontal;

        //Debug.Log("ShoulderToWristAngleVertical: " + ShoulderToWristAngleVertical);
        fields += ",ShoulderToWristAngleVertical";
        values += "," + ShoulderToWristAngleVertical;

        // joints function
        JointsTracking(ref fields, ref values, WristLeft, "WristLeft");
        JointsTracking(ref fields, ref values, ElbowLeft, "ElbowLeft");
        JointsTracking(ref fields, ref values, ShoulderLeft, "ShoulderLeft");
        JointsTracking(ref fields, ref values, Chest, "Chest");
        JointsTracking(ref fields, ref values, ShoulderRight, "ShoulderRight");
        JointsTracking(ref fields, ref values, ElbowRight, "ElbowRight");
        JointsTracking(ref fields, ref values, WristRight, "WristRight");

        //Debug.Log("****************");

        //Debug.Log(fields);
        //Debug.Log(values);

        //if (DBcons) DBcons.GetComponent<database>().SubmitData(table, fields, values);
    }

    private void JointsTracking(ref string fields, ref string values, Vector3 pos, string name)
    {
        fields += "," + name + "PositionX";
        fields += "," + name + "PositionY";
        fields += "," + name + "PositionZ";
        values += "," + pos.x;
        values += "," + pos.y;
        values += "," + pos.z;
    }

    public void HideShot()
    {
        ShotScore.SetActive(false);
        ShotLatency.SetActive(false);
        ShotCurrent++;
    }

    public void SetAssisted(bool assisting)
    {
        Assisted = assisting;

        if (!Finished) AssistedBox.SetActive(assisting);
    }

    public void MinMaxReach(float reach)
    {
        if (reach < minReach) minReach = reach;
        if (reach > maxReach) maxReach = reach;
    }

    public void ResetMaxReach()
    {
        maxReach = 0;
    }

    public void BackToMain()
    {
        //LogData();
        SceneManager.LoadScene("MainMenu");
    }

    public void Replay()
    {
        //LogData();

        LoginControl login = DBcons.GetComponent<LoginControl>();

        // Load the scene if there is another available game possible AND the next game will still be less than (or equal to) the maximum allowed targets games per day AND
        // the cumulative games played recently is less than the max number of 'Targets' games per second.
        if (login.GetNumGamesPlayedToday("Targets") >= login.config.TargetGamesPerDay)
        {
            login.config.LimitWarning = string.Format(LimitAwarenessSettings.DAILY_LIMIT_REACHED_MESSAGE, "Targets");
            SceneManager.LoadScene("LimitAwareness");
        }
        else
        if (login.GetNumGamesInLastIntervalFor("Targets") + login.config.TargetGamesPlayedThisSession >= login.config.TargetGamesPerSession)
        {
            DateTime next = login.GetNextEarliestSessionTimeFor("Targets");
            login.config.LimitWarning = string.Format(LimitAwarenessSettings.COME_BACK_AT, "Targets", next.ToString("hh:mm tt"));
            SceneManager.LoadScene("LimitAwareness");
        }
        else
        {
            SceneManager.LoadScene("Targets");
        }

        /*if (!login.PlaytimeLimitReached("Targets") && login.config.TargetGameNum <= login.config.TargetGamesPerDay && 
            login.GetNumGamesInLastIntervalFor("Targets") + login.config.TargetGamesPlayedThisSession < login.config.TargetGamesPerSession)
            SceneManager.LoadScene("Targets");
        else
        {
            TimeSpan diff = login.GetNextEarliestSessionTimeFor("Targets").Subtract(DateTime.Now);
            string time = diff.Hours + ":" + diff.Minutes + ":" + diff.Seconds;
            login.config.LimitWarning = string.Format(LimitAwarenessSettings.SESSION_LIMIT_REACHED_MESSAGE, "Targets", time);

            SceneManager.LoadScene("LimitAwareness");
        }*/
    }

    public void Quit()
    {
        //LogData();

        if (DBcons)
        {
            DBcons.GetComponent<LoginControl>().logoutSetAverage();
            GameObject.Destroy(DBcons);
        }
        SceneManager.LoadScene("Login");
    }

    public void DisplayEndPanel()
    {
        // turn off scores
        foreach (RectTransform tb in TextBoxesTop)
        {
            tb.gameObject.SetActive(false);
        }
        foreach (RectTransform tb in TextBoxesBot)
        {
            tb.gameObject.SetActive(false);
        }

        // set end panel txts
        EPHits.text = ShotsHit + "/" + ShotsTotal;
        EPScore.text = "" + Score;
        EPDist.text = "" + (GameObject.Find("Grid").GetComponent<Grid>().Depth * 100).ToString("0.0");

        // turn on panel
        UserPrompts.enabled = false;
        EndPanel.SetActive(true);

        Debug.Log("ACTIVE");
    }

    public void SetUserPromptText(string text)
    {
        UserPrompts.text = text;
    }

    public void SetUserPromptVisibility(bool visibility)
    {
        UserPrompts.enabled = visibility;
    }
}
