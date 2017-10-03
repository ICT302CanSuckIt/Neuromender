using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System;
using System.IO;
using Newtonsoft.Json;


[System.Serializable]
public struct UserConfig
{
    //Affliction
    public int UserId;
    public int sideAffected;
    public bool leftNeglect;
    public float armLength;
    public float sensorDistance;

    //Wingman Restrictions
    public float angleThreshold;
    public float angleThresholdIncrease;
    public int trackSlow;
    public int trackMedium;
    public int trackFast;
    public int WingmanGamesPerDay;
    public int WingmanGamesPerSession;
    public float WingmanIntervalBetweenSession;

    //Cycling Restrictions
    public int DistanceShort;
    public int DistanceMedium;
    public int DistanceLong;
    public int CyclingGamesPerDay;
    public int CyclingGamesPerSession;
    public float CyclingIntervalBetweenSession;
    public float ArmMaxExtension;

    //Target Restrictions
    public int extensionThreshold;
    public int ExtensionThresholdIncrease;
    public float minExtensionThreshold;
    public string GridSize;
    public string GridOrder;
    public int Repititions;
    public int gridMoveCountdownDistance;
    public int gridMoveCountdownTime;
    public int TargetGamesPerDay;
    public int TargetGamesPerSession;
    public float TargetIntervalBetweenSession;
    public int ArmResetDistance;

    public int SessionID;
    public bool showDebug;

    public int TargetGameNum;
    public int TargetGamesPlayedThisSession;
    public int WingmanGameNum;
    public int WingmanGamesPlayedThisSession;
    public int CyclingGameNum;
    public int CyclingGamesPlayedThisSession;
    public int RowingGamenum;
    public int ArmPedalNum;
    public int LegPedalNum;

    // Flags for toggling the availability of each game (i.e enable / disable menu buttons).
    public bool allowWingman;
    public bool allowTargets;
    public bool allowCycling;
    public bool allowRowing;
    public bool allowArmPedalling;
    public bool allowLegPedalling;

    public string LimitWarning;
}

[System.Serializable]
public struct GamesPlayed
{
    public bool WingmanPlayed;
    public bool TargetsPlayed;
    public bool CyclingPlayed;
}

public enum LevelName { }
public enum TrackName
{
    beach = 1,
    forest = 2,
    temple = 3
}
public enum SpeedLevel
{
    slow,
    medium,
    fast
}

public enum DistanceLevel
{
    Short,
    Medium,
    Long
}

public enum GraphicName
{
    Low,
    Medium,
    High
}

public class LoginControl : MonoBehaviour
{

    private int UserID;
    public string UserName;
    public string NAME { get { return UserName; } }
    public UserConfig config;
    public GamesPlayed gamesPlayed;
    public bool LoggedIn = false;
    private bool test = false;
    public LevelName selectedLevel;
    public TrackName selectedTrack;
    public SpeedLevel selectedSpeed;
    public DistanceLevel selectedDistance;
    public GraphicName selectedGraphic;


    private List<float> averageThresholds;

    private string demoUserName = "demonstration";
    private string demoPassWord = "connect";

    // TRUE: THere are more games available, but they cannot be attempted because the last session for EACH AND EVERY game was too recent.
    private bool pendingGames = false;

    Text outputText;
    database Database;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        outputText = GameObject.Find("DatabaseOutputText").GetComponent<Text>();
        Database = GameObject.Find("DatabaseController").GetComponent<database>();
        averageThresholds = new List<float>();
        config.showDebug = false;

        config.TargetGamesPlayedThisSession = 0;
        config.WingmanGamesPlayedThisSession = 0;
        config.CyclingGamesPlayedThisSession = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(config.sideAffected);
    }

    public void forgottenPassword()
    {
        Application.OpenURL("http://vegas.murdoch.edu.au/neuromender_1/Main/ForgottenPassword.php");
    }

    public void helpButton()
    {
        // Not available for Neuromeneder
        Application.OpenURL("http://vegas.murdoch.edu.au/neuromend/Main/FAQ.php");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    // I assume that this updates the database data if the application is closed without logging out properly.
    void OnApplicationQuit()
    {
        logoutSetAverage();
    }

    /// <summary>
    /// Function for dealing with the login to the database.
    /// </summary>
    public void login()
    {
        string Username = GameObject.Find("UserNameInput").GetComponent<InputField>().text;
        string Password = GameObject.Find("PasswordInput").GetComponent<InputField>().text;

        string usernameSQL = "Select * from Users where Username = '" + Username + "'";

        int tempId = 0;
        string dbPass = "";
        LoggedIn = false;
        //Application.LoadLevel("MainMenu");

        if (Database.connected)
        {
            MySqlDataReader User = Database.selectData(usernameSQL);

            if (User != null)
            {
                if (User.HasRows)
                {
                    while (User.Read())
                    {
                        for (int i = 0; i < User.FieldCount; i++)
                        {
                            if (User.GetName(i) == "UserID")
                            {
                                tempId = User.GetInt32(i);
                            }
                            if (User.GetName(i) == "Password")
                            {
                                dbPass = User.GetString(i);
                            }
                        }

                        string sha1Str = HashCode(Password);
                        if (dbPass == sha1Str.ToLower())
                        {
                            outputText.text = "Output: User Logged in!";
                            UserName = Username;
                            LoggedIn = true;
                        }
                        else
                        {
                            outputText.text = "Output: Password Incorrect.";
                        }
                    }
                    User.NextResult();
                }
                else
                {
                    outputText.text = "Output: No User found.";
                }
                //Close the datareader
                User.Close();
            }
            else
            {
                outputText.text = "Output: Error Connecting to Database.";
            }

            if (LoggedIn)
            {
                UserID = tempId;
                if (getUserConfig(UserID))
                {
                    // Before anything else, check which games have been enabled by the clinician for this survivor.
                    GetEnabledGames(UserID);


                    // Checks to see if there are any games enabled for the survivor.
                    if (!config.allowWingman && !config.allowTargets && !config.allowRowing && !config.allowCycling && !config.allowArmPedalling && !config.allowLegPedalling)
                    {
                        config.LimitWarning = LimitAwarenessSettings.NO_GAMES_SET;
                        UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                        return;
                    }

                    // If the 'Wingman' game is enabled, get the relevant data.
                    if (config.allowWingman)
                        getWingmanConfig(UserID);

                    // If the 'Targets' game is enabled, get the relevant data.
                    if (config.allowTargets)
                        getTargetsConfig(UserID);

                    // If the 'Cycling' game is enabled, get the relevant data.
                    if (config.allowCycling)
                        getCyclingConfig(UserID);

                    outputText.text = "Output: User Logged in, user Config Loaded.";

                    ResetPlayed();

                    // Keep the game number counter up to date.
                    //config.WingmanGameNum = GetNumGamesPlayedToday("Wingman");
                    //config.TargetGameNum = GetNumGamesPlayedToday("Targets");

                    config.WingmanGamesPlayedThisSession = 0;
                    config.TargetGamesPlayedThisSession = 0;
                    config.CyclingGamesPlayedThisSession = 0;

                    Debug.Log("Wingman Games Today: " + GetNumGamesPlayedToday("Wingman") + " of " + config.WingmanGamesPerDay);
                    Debug.Log("Targets Games Today: " + GetNumGamesPlayedToday("Targets") + " of " + config.TargetGamesPerDay);
                    Debug.Log("Cycling Games Today: " + GetNumGamesPlayedToday("Cycling") + " of " + config.CyclingGamesPerDay);

                    // Check that all of the games that are marked as available are actually allowed to be played. //mich
                    UpdateGameAvailability();

                    if (!config.allowWingman && !config.allowTargets && !config.allowCycling && !config.allowRowing && !config.allowArmPedalling && !config.allowLegPedalling)
                    {
                        if (GetNumGamesPlayedToday("Targets") >= config.TargetGamesPerDay)// && config.TargetGameNum >= config.TargetGamesPerDay)
                        {
                            config.LimitWarning = LimitAwarenessSettings.ALL_GAMES_PLAYED;
                            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                            return;
                        }
                        else if (GetNumGamesPlayedToday("Wingman") >= config.WingmanGamesPerDay)
                        {
                            config.LimitWarning = LimitAwarenessSettings.ALL_GAMES_PLAYED;
                            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                            return;
                        }
                        else if (GetNumGamesPlayedToday("Cycling") >= config.CyclingGamesPerDay)
                        {
                            config.LimitWarning = LimitAwarenessSettings.ALL_GAMES_PLAYED;
                            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                            return;
                        }

                        DateTime NextTargetsGame = GetNextEarliestSessionTimeFor("Targets");
                        DateTime NextWingmanGame = GetNextEarliestSessionTimeFor("Wingman");
                        DateTime NextCyclingGame = GetNextEarliestSessionTimeFor("Cycling");

                        if ((NextTargetsGame < NextWingmanGame) && (NextTargetsGame < NextCyclingGame))
                        {
                            //TimeSpan diff = LTS.Subtract(DateTime.Now);
                            //string time = diff.Hours + ":" + diff.Minutes + ":" + diff.Seconds;

                            config.LimitWarning = string.Format(LimitAwarenessSettings.COME_BACK_AT, "Targets", NextTargetsGame.ToString("hh:mm tt"));
                            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                            return;
                        }
                        else if ((NextWingmanGame < NextTargetsGame) && (NextWingmanGame < NextCyclingGame))
                        {
                            //TimeSpan diff = LWS.Subtract(DateTime.Now);
                            //string time = diff.Hours + ":" + diff.Minutes + ":" + diff.Seconds;
                            config.LimitWarning = string.Format(LimitAwarenessSettings.COME_BACK_AT, "Wingman", NextWingmanGame.ToString("hh:mm tt"));
                            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                            return;
                        }
                        else if ((NextCyclingGame < NextTargetsGame) && (NextCyclingGame < NextWingmanGame))
                        {
                            //TimeSpan diff = LWS.Subtract(DateTime.Now);
                            //string time = diff.Hours + ":" + diff.Minutes + ":" + diff.Seconds;
                            config.LimitWarning = string.Format(LimitAwarenessSettings.COME_BACK_AT, "Cycling", NextCyclingGame.ToString("hh:mm tt"));
                            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                            return;
                        }

                    }

                    // If none of the games are available to play, usher the survivor to the limit awareness page.
                    if (!config.allowWingman && !config.allowTargets && !config.allowCycling && !config.allowRowing && !config.allowArmPedalling && !config.allowLegPedalling)
                    {
                        config.LimitWarning = LimitAwarenessSettings.ALL_GAMES_PLAYED;
                        UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                        return;
                    }

                    //check game limit
                    /*if (LimitedByPreviousSession() || PlaytimeLimitReached())
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
                        return;
                    }*/

                    string table = "Session";
                    string fields = "UserID, StartTime";
                    System.DateTime now = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();
                    string values = "" + UserID + ",'" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    Database.SubmitData(table, fields, values);

                    string sessStr = "Select * from Session where UserID =" + UserID + " AND StartTime ='" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                    MySqlDataReader Sess = Database.selectData(sessStr);
                    if (Sess != null)
                    {
                        if (Sess.HasRows)
                        {
                            while (Sess.Read())
                            {
                                for (int i = 0; i < Sess.FieldCount; i++)
                                {
                                    if (Sess.GetName(i) == "SessionID")
                                    {
                                        config.SessionID = Sess.GetInt32(i);
                                    }

                                    //Debug.Log(Sess.GetName(i));
                                }
                            }
                        }
                    }

                    //Close the datareader
                    Sess.Close();
                    checkLastSession();
                    //Application.LoadLevel("MainMenu");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Disclaimer");
                }
                else
                {
                    outputText.text = "Output: No User Config Found.";
                }

            }
        }
        else
        {
            if (Username == demoUserName && Password == demoPassWord)
            {
                Debug.Log("Demonstration profile engaged. Loading dummy information!");
                outputText.text = "Demonstration profile engaged. Loading dummy information!";
                GetOfflineData();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Disclaimer");
            }
        }

    }

    /// <summary>
    /// Update the games that are available to the survivor.
    /// </summary>
    public void UpdateGameAvailability()
    {
        // If the number of 'Wingman' games played today is greater than the number allowed OR limited by the last session, disable the option.
        if (GetNumGamesPlayedToday("Wingman") >= config.WingmanGamesPerDay || GetNumGamesInLastIntervalFor("Wingman") + config.WingmanGamesPlayedThisSession >= config.WingmanGamesPerSession)
            config.allowWingman = false;

        // If the number of 'Targets' games played today is greater than the number allowed, disable the option.
        if (GetNumGamesPlayedToday("Targets") >= config.TargetGamesPerDay || GetNumGamesInLastIntervalFor("Targets") + config.TargetGamesPlayedThisSession >= config.TargetGamesPerSession)
            config.allowTargets = false;

        // If the number of 'Cycling' games played today is greater than the number allowed, disable the option.
        if (GetNumGamesPlayedToday("Cycling") >= config.CyclingGamesPerDay || GetNumGamesInLastIntervalFor("Cycling") + config.CyclingGamesPlayedThisSession >= config.CyclingGamesPerSession)
            config.allowCycling = false;
    }

    /// <summary>
    /// In the event the application must be opened, but the database cannot be reached, get dummy data from the config file included ("Demonstration.neuro").
    /// </summary>
    private void GetOfflineData()
    {
        using (StreamReader file = new StreamReader(Application.dataPath + "/Data/Demonstration.neuro"))
        {
            string line = file.ReadLine();  // The lastest line that is being read from the file.
            string[] data;                  // Container for delimited string chunks.
            string[] delim = { ": " };       // Delimiter strings for splitting lines into useful chunks.

            while (!file.EndOfStream)
            {
                switch (line)
                {
                    case "#BASIC":
                        // User ID
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.UserId = UserID = Convert.ToInt32(data[1]); }
                        catch { config.UserId = UserID = 65; }

                        // User Name
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        UserName = data[1];
                        if (UserName == null || UserName == "") UserName = "Demo";

                        break;
                    case "#AFFLICTION":
                        // Side Afflicted
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.sideAffected = Convert.ToInt32(data[1]); }
                        catch { config.sideAffected = 1; }

                        // Left Neglect
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.leftNeglect = Convert.ToBoolean(data[1]); }
                        catch { config.leftNeglect = false; }

                        // Arm Length
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.armLength = Convert.ToSingle(data[1]); }
                        catch { config.armLength = 600.0f; }

                        // SensorDistance
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.sensorDistance = Convert.ToSingle(data[1]); }
                        catch { config.sensorDistance = 1.5f; }

                        break;
                    case "#WINGMAN":
                        // Angle Threshold
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.angleThreshold = Convert.ToSingle(data[1]); }
                        catch { config.angleThreshold = 45; }

                        // Angle Threshold Increase
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.angleThresholdIncrease = Convert.ToSingle(data[1]); }
                        catch { config.angleThresholdIncrease = 2; }

                        // Track (slow)
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.trackSlow = Convert.ToInt32(data[1]); }
                        catch { config.trackSlow = 120; }

                        // Track (medium)
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.trackMedium = Convert.ToInt32(data[1]); }
                        catch { config.trackMedium = 60; }

                        // Track (fast)
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.trackFast = Convert.ToInt32(data[1]); }
                        catch { config.trackFast = 30; }

                        // Games / Day
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.WingmanGamesPerDay = Convert.ToInt32(data[1]); }
                        catch { config.WingmanGamesPerDay = 4; }

                        // Games / Session
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.WingmanGamesPerSession = Convert.ToInt32(data[1]); }
                        catch { config.WingmanGamesPerSession = 2; }

                        // Session Interval Time
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.WingmanIntervalBetweenSession = Convert.ToInt32(data[1]); }
                        catch { config.WingmanIntervalBetweenSession = 3; }

                        break;

                    case "#CYCLING":

                        // Distance(Short)
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.DistanceShort = Convert.ToInt32(data[1]); }
                        catch { config.DistanceShort = 120; }

                        // Distance(medium)
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.DistanceMedium = Convert.ToInt32(data[1]); }
                        catch { config.DistanceMedium = 60; }

                        // Distance(long)
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.DistanceLong = Convert.ToInt32(data[1]); }
                        catch { config.DistanceLong = 30; }

                        // Games / Day
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.CyclingGamesPerDay = Convert.ToInt32(data[1]); }
                        catch { config.CyclingGamesPerDay = 4; }

                        // Games / Session
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.CyclingGamesPerSession = Convert.ToInt32(data[1]); }
                        catch { config.CyclingGamesPerSession = 2; }

                        // Session Interval Time
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.CyclingIntervalBetweenSession = Convert.ToInt32(data[1]); }
                        catch { config.CyclingIntervalBetweenSession = 3; }


                        // 	ArmMaxExtension(Short)
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.ArmMaxExtension = Convert.ToInt32(data[1]); }
                        catch { config.ArmMaxExtension = 120; }

                        break;



                    case "#TARGETS":
                        // Extension Threshold
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.extensionThreshold = Convert.ToInt32(data[1]); }
                        catch { config.extensionThreshold = 300; }

                        // Extension Threshold Increase
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.ExtensionThresholdIncrease = Convert.ToInt32(data[1]); }
                        catch { config.ExtensionThresholdIncrease = 2; }

                        // Min Extension Threshold
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.minExtensionThreshold = Convert.ToSingle(data[1]); }
                        catch { config.minExtensionThreshold = 1.0f; }

                        // Grid Size
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        try { config.GridSize = data[1]; }
                        catch { config.GridSize = "5,3"; }

                        // Grid Order
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            string temp = data[1];
                            for (int i = 2; i < data.Length; i++)
                                temp += "," + data[i];

                            config.GridOrder = temp;
                        }
                        catch { config.GridOrder = "1,2,1,2,10,11,3"; }

                        // Repetitions
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.Repititions = Convert.ToInt32(data[1]); }
                        catch { config.Repititions = 1; }

                        // Grid Move Countdown Distance
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.gridMoveCountdownDistance = Convert.ToInt32(data[1]); }
                        catch { config.gridMoveCountdownDistance = 100; }

                        // Grid Move Countdown Time
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.gridMoveCountdownTime = Convert.ToInt32(data[1]); }
                        catch { config.gridMoveCountdownTime = 5; }

                        // Arm Reset Distance
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.ArmResetDistance = Convert.ToInt32(data[1]); }
                        catch { config.ArmResetDistance = 170; }

                        // Games / Day
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.TargetGamesPerDay = Convert.ToInt32(data[1]); }
                        catch { config.TargetGamesPerDay = 4; }

                        // Games / Session
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.TargetGamesPerSession = Convert.ToInt32(data[1]); }
                        catch { config.TargetGamesPerSession = 3; }

                        // Session Interval Time
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.TargetIntervalBetweenSession = Convert.ToSingle(data[1]); }
                        catch { config.TargetIntervalBetweenSession = 3; }

                        break;
                    case "#OTHER":
                        // Session ID
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.SessionID = Convert.ToInt32(data[1]); }
                        catch { config.SessionID = 1; }

                        // Wingman Game Number
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.WingmanGameNum = Convert.ToInt32(data[1]); }
                        catch { config.WingmanGameNum = 0; }

                        // Target Game Number
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.TargetGameNum = Convert.ToInt32(data[1]); }
                        catch { config.TargetGameNum = 0; }

                        // Cycling Game Number
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.CyclingGameNum = Convert.ToInt32(data[1]); }
                        catch { config.CyclingGameNum = 0; }

                        // Limit Warning
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        config.LimitWarning = data[1];
                        if (config.LimitWarning == null) config.LimitWarning = "";

                        break;
                    case "#ALLOWED GAMES":
                        // Wingman Allowed
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.allowWingman = Convert.ToBoolean(data[1]); }
                        catch { config.allowWingman = false; }

                        // Targets Allowed
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.allowTargets = Convert.ToBoolean(data[1]); }
                        catch { config.allowTargets = false; }

                        // Rowing Allowed
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.allowRowing = Convert.ToBoolean(data[1]); }
                        catch { config.allowRowing = false; }

                        // Cycling Allowed
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.allowCycling = Convert.ToBoolean(data[1]); }
                        catch { config.allowCycling = false; }

                        // Arm Pedalling Allowed
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.allowArmPedalling = Convert.ToBoolean(data[1]); }
                        catch { config.allowArmPedalling = false; }

                        // Leg Pedalling Allowed
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { config.allowLegPedalling = Convert.ToBoolean(data[1]); }
                        catch { config.allowLegPedalling = false; }

                        break;
                    case "#GAMES PLAYED":
                        // Wingman Played
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { gamesPlayed.WingmanPlayed = Convert.ToBoolean(data[1]); }
                        catch { gamesPlayed.WingmanPlayed = false; }

                        // Targets Played
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { gamesPlayed.TargetsPlayed = Convert.ToBoolean(data[1]); }
                        catch { gamesPlayed.TargetsPlayed = false; }

                        // Cycling Played
                        line = file.ReadLine();
                        data = line.Split(delim, StringSplitOptions.None);
                        try { gamesPlayed.CyclingPlayed = Convert.ToBoolean(data[1]); }
                        catch { gamesPlayed.CyclingPlayed = false; }

                        break;
                    default:
                        // Do nothing...
                        break;
                }

                line = file.ReadLine(); // This skips empty lines.
            }
        }
    }

    /// <summary>
    /// This turns the database query to the affliction table into user Config for the game
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool getUserConfig(int id)
    {
        string configSQL = "Select * from Affliction where UserID = " + id;
        MySqlDataReader configReader = Database.selectData(configSQL);

        if (configReader.HasRows)
        {
            while (configReader.Read())
            {
                //Debug.Log("STARTING CONFIG DATA PULL");
                config.UserId = id;
                for (int i = 0; i < configReader.FieldCount; i++)
                {
                    if (configReader.GetName(i) == "SideAffectedID")
                    {
                        config.sideAffected = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "SensorDistance")
                    {
                        config.sensorDistance = configReader.GetFloat(i);
                    }
                    else if (configReader.GetName(i) == "LeftNeglect")
                    {
                        config.leftNeglect = configReader.GetBoolean(i);
                    }
                    else if (configReader.GetName(i) == "TargetGameNum")
                    {
                        config.TargetGameNum = configReader.GetInt32(i);    // <-- Not sure if this is used any more.
                    }
                    //Debug.Log(configReader.GetName(i));
                }
                //Debug.Log("ENDING CONFIG DATA PULL");
            }

            //config.showDebug = false;
            configReader.Close();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Get the data that specifies which game have been enabled by the clinician for this survivor.
    /// </summary>
    /// <param name="id">The unique user ID assigned to the survivor in the database.</param>
    /// <returns>TRUE: The data was successfully acquired.<para>FALSE: There was an issue getting the data.</para></returns>
    bool GetEnabledGames(int id)
    {
        string configSQL = "Select EnabledTargets,EnabledWingman,EnabledCycling from Users where UserID = " + id;
        MySqlDataReader configReader = Database.selectData(configSQL);

        if (configReader.HasRows)
        {
            while (configReader.Read())
            {
                for (int i = 0; i < configReader.FieldCount; ++i)
                {
                    switch (configReader.GetName(i))
                    {
                        case "EnabledTargets":
                            config.allowTargets = configReader.GetBoolean(i);
                            break;
                        case "EnabledWingman":
                            config.allowWingman = configReader.GetBoolean(i);
                            break;
                        case "EnabledCycling":
                            config.allowCycling = configReader.GetBoolean(i);
                            break;
                        default:
                            // Do nothing...
                            break;
                    }
                }
            }
            configReader.Close();
            return true;
        }

        return false;
    }

    /// <summary>
    /// This turns the database query to the WingmanRestrictions table into user Config for the game
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool getWingmanConfig(int id)
    {
        string configSQL = "Select * from WingmanRestrictions where UserID = " + id;
        MySqlDataReader configReader = Database.selectData(configSQL);

        if (configReader.HasRows)
        {
            while (configReader.Read())
            {
                //Debug.Log("STARTING CONFIG DATA PULL");
                config.UserId = id;
                for (int i = 0; i < configReader.FieldCount; i++)
                {
                    if (configReader.GetName(i) == "AngleThreshold")
                    {
                        config.angleThreshold = configReader.GetFloat(i);
                    }
                    else if (configReader.GetName(i) == "ThresholdIncrease")
                    {
                        config.angleThresholdIncrease = configReader.GetFloat(i);
                    }
                    else if (configReader.GetName(i) == "trackSlow")
                    {
                        config.trackSlow = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "trackMedium")
                    {
                        config.trackMedium = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "trackFast")
                    {
                        config.trackFast = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "GamesPerDay")
                    {
                        config.WingmanGamesPerDay = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "GamesPerSession")
                    {
                        config.WingmanGamesPerSession = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "IntervalBetweenSession")
                    {
                        config.WingmanIntervalBetweenSession = configReader.GetFloat(i);
                    }
                }
            }
            configReader.Close();
            return true;
        }

        return false;
    }


    /// <summary>
    /// This turns the database query to the TargetRestrictions table into user Config for the game
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool getTargetsConfig(int id)
    {
        string configSQL = "Select * from TargetRestrictions where UserID = " + id;
        MySqlDataReader configReader = Database.selectData(configSQL);

        if (configReader.HasRows)
        {
            while (configReader.Read())
            {
                //Debug.Log("STARTING CONFIG DATA PULL");
                config.UserId = id;
                for (int i = 0; i < configReader.FieldCount; i++)
                {
                    if (configReader.GetName(i) == "ExtensionThreshold")
                    {
                        config.extensionThreshold = (int)configReader.GetFloat(i);
                    }
                    else if (configReader.GetName(i) == "ExtensionThresholdIncrease")
                    {
                        config.ExtensionThresholdIncrease = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "MinimumExtensionThreshold")
                    {
                        config.minExtensionThreshold = configReader.GetFloat(i);
                    }
                    else if (configReader.GetName(i) == "GridSize")
                    {
                        config.GridSize = configReader.GetString(i);
                    }
                    else if (configReader.GetName(i) == "GridOrder")
                    {
                        config.GridOrder = configReader.GetString(i);
                    }
                    else if (configReader.GetName(i) == "Repetitions")
                    {
                        config.Repititions = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "GamesPerDay")
                    {
                        config.TargetGamesPerDay = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "GamesPerSession")
                    {
                        config.TargetGamesPerSession = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "IntervalBetweenSession")
                    {
                        config.TargetIntervalBetweenSession = configReader.GetFloat(i);
                    }
                    else if (configReader.GetName(i) == "AdjustmentCountdown")
                    {
                        config.gridMoveCountdownTime = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "CountdownDistance")
                    {
                        config.gridMoveCountdownDistance = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "ArmResetDistance")
                    {
                        config.ArmResetDistance = configReader.GetInt32(i);
                    }
                }
            }
            configReader.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// This turns the database query to the WingmanRestrictions table into user Config for the game
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool getCyclingConfig(int id)
    {
        string configSQL = "Select * from CyclingRestrictions where UserID = " + id;
        MySqlDataReader configReader = Database.selectData(configSQL);

        if (configReader.HasRows)
        {
            while (configReader.Read())
            {
                //Debug.Log("STARTING CONFIG DATA PULL");
                config.UserId = id;
                for (int i = 0; i < configReader.FieldCount; i++)
                {
                    if (configReader.GetName(i) == "DistanceShort")
                    {
                        config.DistanceShort = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "DistanceMedium")
                    {
                        config.DistanceMedium = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "DistanceLong")
                    {
                        config.DistanceLong = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "GamesPerDay")
                    {
                        config.CyclingGamesPerDay = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "GamesPerSession")
                    {
                        config.CyclingGamesPerSession = configReader.GetInt32(i);
                    }
                    else if (configReader.GetName(i) == "IntervalBetweenSession")
                    {
                        config.CyclingIntervalBetweenSession = configReader.GetFloat(i);
                    }
                    else if (configReader.GetName(i) == "ArmMaxExtension")
                    {
                        config.ArmMaxExtension = configReader.GetFloat(i);
                    }
                }
            }
            configReader.Close();
            return true;
        }

        return false;
    }



    /// <summary>
    /// This turns the database query to the affliction table into user Config for the game
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool getReachGameData(int id)
    {
        Debug.Log("Conecting to ReachTrackingData");
        string configSQL = "Select * from ReachTrackingData where UserID = " + id + config.SessionID;
        MySqlDataReader configReader = Database.selectData(configSQL);

        Debug.Log("STARTING DATA PULL");

        for (int i = 0; i < configReader.FieldCount; i++)
        {
            Debug.Log(configReader.GetName(i));
        }
        Debug.Log("ENDING DATA PULL");

        config.showDebug = false;
        configReader.Close();
        return true;
    }



    /// <summary>
    /// This function turns a regular string into Sha1 string
    /// Used for comparing to the passwords in the databse
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string HashCode(string str)
    {
        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
        byte[] buffer = encoder.GetBytes(str);
        SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
        string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

        return hash;
    }

    /// <summary>
    /// Breaks down each achievement and rawtracking data, and submits it into the database. NOTE: This should be moved to DataLogger_Wingman.
    /// </summary>
    /// <returns><c>true</c>, if level data was synced, <c>false</c> otherwise.</returns>
    /// <param name="achievements">Achievements.</param>
    /// <param name="rawTracking">Raw tracking.</param>
    /// <param name="UserID">User I.</param>
    /// <param name="SessionId">Session identifier.</param>
    public bool syncLevelData(List<st_Achievement> achievements, List<st_RawTracking> rawTracking, int UserID, int SessionId, System.DateTime startTime)
    {
        Debug.Log("SYNC DATA");
        string table = "";
        string fields = "";

        float SumAverage = 0.0f;
        int averageCount = 0;

        /*
        if (achievements != null)
        {
            table = "Achievement";
            fields = "SessionID, TaskID, ThresholdPassed, TimeAchieved";
            string Inserts = "";
            foreach (st_Achievement ach in achievements)
            {
                SumAverage += ach.Threshold;
                averageCount++;
                if (Inserts != "")
                {
                    Inserts += ",";
                }
                Inserts += " (" + SessionId + "," + ( int )ach.task + "," + ach.Threshold + ",'" + ach.TimeAchieved.ToString("yyyy-MM-dd HH:mm:ss") + "') ";
            }

            float average = SumAverage / averageCount;
            averageThresholds.Add(average);
            //  Database.SubmitDataMiltiLine(table, fields, Inserts); test new achievement output
        }*/

        if (rawTracking != null)
        {
            //going to take a while....
            table = "RawTracking";
            fields = "SessionID, AverageBodyDepth, CentralPontX, CentralPontY, CentralPontZ, RightHandX, RightHandY, RightHandZ, LeftHandX, LeftHandY, LeftHandZ, RightElbowX, RightElbowY, RightElbowZ, LeftElbowX, LeftElbowY, LeftElbowZ, LeftAngle, RightAngle, Time, RingNumber, AchievementID";
            string Inserts = "";
            foreach (st_RawTracking raw in rawTracking)
            {
                if (Inserts != "")
                {
                    Inserts += ",";
                }
                Inserts += "(" + SessionId + "," + raw.BodyDepth;
                Inserts += "," + raw.centerPoint.x + "," + raw.centerPoint.y + "," + raw.centerPoint.z;
                Inserts += "," + raw.rightHand.x + "," + raw.rightHand.y + "," + raw.rightHand.z;
                Inserts += "," + raw.leftHand.x + "," + raw.leftHand.y + "," + raw.leftHand.z;
                Inserts += "," + raw.rightElbow.x + "," + raw.rightElbow.y + "," + raw.rightElbow.z;
                Inserts += "," + raw.leftElbow.x + "," + raw.leftElbow.y + "," + raw.leftElbow.z;
                Inserts += "," + raw.leftAngle + "," + raw.rightAngle + ",'" + raw.rawDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                Inserts += "," + raw.RingNumber + "," + raw.AchievementId + ")";
            }
            Database.SubmitDataMiltiLine(table, fields, Inserts);
        }

        System.DateTime now = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();

        ///Sync out the update to the session
        table = "Session";
        fields = "EndTime ='" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        string where = "SessionID =" + SessionId;
        Database.UpdateData(table, fields, where);

        /// Sync out the level Completed
        table = "LevelCompleted";
        fields = "SessionID, LevelID, SelectedDuration, TimeStarted, TimeFinished";
        string values = "" + SessionId + "," + (int)selectedTrack + ",";

        if (selectedSpeed == SpeedLevel.slow)
        {
            values += config.trackSlow;
        }
        else if (selectedSpeed == SpeedLevel.medium)
        {
            values += config.trackMedium;
        }
        else if (selectedSpeed == SpeedLevel.fast)
        {
            values += config.trackFast;
        }

        values += ",'" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        Database.SubmitData(table, fields, values);

        return true;
    }

    /// <summary>
    /// Looks for the LastSession file
    /// Gets the last values from the file and checks that it was inserted into the database correctly
    /// If ti wasnt, then insert all of the data, if it was then ignore it!
    /// </summary>
    /// <returns><c>true</c>, if last session was checked, <c>false</c> otherwise.</returns>
    public bool checkLastSession()
    {
        List<st_RawTracking> items = new List<st_RawTracking>();
        if (IsFileValid(@"SessionData\LastSession.txt") && !IsFileEmpty(@"SessionData\LastSession.txt"))
        {
            print("valid");
            using (System.IO.StreamReader r = new System.IO.StreamReader(@"SessionData\LastSession.txt"))
            {
                print("reading");
                try
                {
                    while (r.Peek() >= 0)
                    {
                        string json1 = r.ReadLine();
                        st_RawTracking item = JsonConvert.DeserializeObject<st_RawTracking>(json1);
                        items.Add(item);
                    }
                    print("looking at last values");
                    st_RawTracking last = items[items.Count - 1];
                    string SQL = "Select * from RawTracking where SessionID = " + last.SessionID + " AND Time = '" + last.rawDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    MySqlDataReader tracking = Database.selectData(SQL);

                    if (tracking == null || !tracking.HasRows)
                    {
                        tracking.Close();
                        print("syncLasyStuff");
                        //insert the craps
                        syncLevelData(null, items, last.UserID, last.SessionID, GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime());
                        return true;
                    }
                    else
                    {
                        tracking.Close();
                        print("DontSync!");
                    }
                }
                catch (Exception ex)
                {
                    // outputText.text = "Output: Error with local last session data";
                    print("Output: Error with local last session data -  " + ex.ToString());
                }
            }
        }
        else
        {
            print("not valid");
        }
        return false;
    }

    /// <summary>
    /// Checks if the file is an actual valid file,
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private bool IsFileValid(string filePath)
    {
        bool IsValid = true;

        if (!System.IO.File.Exists(filePath))
        {
            IsValid = false;
        }
        else if (System.IO.Path.GetExtension(filePath).ToLower() != ".txt")
        {
            IsValid = false;
        }

        return IsValid;
    }

    /// <summary>
    /// Check if the filepath contains an empty file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private bool IsFileEmpty(string filePath)
    {
        if (new System.IO.FileInfo(filePath).Length == 0)
        {
            // file is empty
            return true;
        }
        else
        {
            // there is something in it
            return false;
        }
    }

    /// <summary>
    /// This function is called when the Quit button is pressed, or the logout button
    /// The main purpose of it is the just work out the average threshold of all the completed levels.
    /// It will then sync out that threshold value to the config of the current User.
    /// This will allow progression throughout multiple sessions, and can be traced by the Physiotharapist.
    /// 
    /// </summary>
    public void logoutSetAverage()
    {
        string table = "";
        string fields = "";
        string where = "";

        System.DateTime now = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();

        ///Sync out the update to the session
        table = "Session";
        fields = "EndTime ='" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        where = "SessionID =" + config.SessionID;
        Database.UpdateData(table, fields, where);
        if (gamesPlayed.WingmanPlayed)
        {
            fields = "WingmanPlayed ='" + config.WingmanGamesPlayedThisSession + "'";
            Database.UpdateData(table, fields, where);
        }
        if (gamesPlayed.TargetsPlayed)
        {
            fields = "TargetsPlayed ='" + config.TargetGamesPlayedThisSession + "'";
            Database.UpdateData(table, fields, where);
        }
        if (gamesPlayed.CyclingPlayed)
        {
            fields = "CyclingPlayed ='" + config.CyclingGamesPlayedThisSession + "'";
            Database.UpdateData(table, fields, where);
        }
    }

    /// <summary>
    /// This function set all the games played values to false on and should only be called on login
    /// </summary>
    public void ResetPlayed()
    {
        gamesPlayed.WingmanPlayed = false;
        gamesPlayed.TargetsPlayed = false;
        gamesPlayed.CyclingPlayed = false;
    }

    /// <summary>
    /// Get the number of games played today by counting the aggregate total of all sessions that were created in the same day.
    /// </summary>
    /// <param name="game"></param>
    /// <returns></returns>
    public int GetNumGamesPlayedToday(string game)
    {
        int result = 0;

        if (Database.connected)
        {
            switch (game)
            {
                // These are the games that are supported in the database. Note that you should be naming your counters similarly 'RowingPlayed', etc.
                case "Targets":
                case "Cycling":
                case "Wingman":

                    DateTime serverTime = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();

                    //check the number of games played today
                    string query = string.Format(@"SELECT * FROM Session WHERE `UserID` = {0} AND `StartTime` >= '{1}' AND `StartTime` <= '{2}' AND `StartTime` < `EndTime`",
                                                    config.UserId, serverTime.ToString("yyyy-MM-dd 00:00:00"), serverTime.ToString("yyyy-MM-dd 23:59:59"));

                    // Send the query to the database and retrieve the table of valid sessions.
                    var sqlResult = Database.selectData(query);

                    // Count all of the numbers of 'Targets' games played in all of the sessions for today.
                    while (sqlResult.Read())
                    {
                        result += sqlResult.GetInt32(game + "Played");
                    }

                    // Close the data reader.
                    sqlResult.Close();

                    break;
                default:
                    // As you add more keys to the database, add the other cases to this switch-case. Until that happens, the number of games wont be returned...
                    break;
            }


        }
        return result;
    }

    /// <summary>
    /// Get the last recorded session for the specified game.
    /// </summary>
    /// <param name="game">The name of the game that is being queried.</param>
    /// <returns>The time stamp for the last session that has a 'game played' recording for the specified game.</returns>
    public DateTime GetLastSessionFor(string game)
    {
        DateTime time = DateTime.Now;

        if (Database.PingConnection())
        {
            switch (game)
            {
                case "Wingman":
                case "Cycling":
                case "Targets":

                    string query = "SELECT `EndTime` FROM `Session` WHERE `UserID` = " + config.UserId + " AND `" + game + "Played` > 0 ORDER BY `EndTime` DESC LIMIT 1";

                    // Send the query to the database and retrieve the table of valid sessions.
                    var sqlResult = Database.selectData(query);

                    // Count all of the numbers of 'Targets' games played in all of the sessions for today.
                    while (sqlResult.Read())
                    {
                        time = sqlResult.GetDateTime("EndTime");
                    }

                    Debug.Log("Last " + game + " game played at: " + time.ToString("yyyy-MM-dd HH:mm:ss"));

                    // Close the data reader.
                    sqlResult.Close();

                    break;
                default:
                    // Do nothing...

                    Debug.Log("Invalid game queried. Current system time returned instead.");

                    break;
            }
        }

        return time;
    }

    /// <summary>
    /// Get the timestamp for the earliest time that the next session for the specified game can be played.
    /// </summary>
    /// <param name="game">The name of the game that is being queried.</param>
    /// <returns></returns>
    public DateTime GetNextEarliestSessionTimeFor(string game)
    {
        DateTime nextTime = DateTime.Now;
        DateTime lastSessionTime = GetLastSessionFor(game);

        int intervalMinutes = 0;

        switch (game)
        {
            case "Wingman":
                intervalMinutes = (int)(config.WingmanIntervalBetweenSession * 60);
                break;
            case "Cycling":
                intervalMinutes = (int)(config.CyclingIntervalBetweenSession * 60);
                break;
            case "Targets":
                intervalMinutes = (int)(config.TargetIntervalBetweenSession * 60);
                break;
            default:
                // Do something horrible :)
                intervalMinutes = 9001;
                break;
        }

        TimeSpan interval = new TimeSpan(0, intervalMinutes, 0);
        nextTime = lastSessionTime.Add(interval);

        return nextTime;
    }

    public int GetNumGamesInLastIntervalFor(string game)
    {
        int num = 0;

        if (Database.PingConnection())
        {
            switch (game)
            {
                case "Wingman":
                case "Cycling":
                case "Targets":

                    DateTime cutOffTime = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();
                    DateTime now = cutOffTime;

                    if (game == "Wingman")
                        cutOffTime = cutOffTime.AddHours(-(config.WingmanIntervalBetweenSession));
                    else
                        if (game == "Targets")
                        cutOffTime = cutOffTime.AddHours(-(config.TargetIntervalBetweenSession));
                    else
                        if (game == "Cycling")
                        cutOffTime = cutOffTime.AddHours(-(config.CyclingIntervalBetweenSession));

                    string query = string.Format("SELECT `" + game + "Played` FROM `Session` WHERE `UserID` = " + config.UserId + " AND `" + game + "Played` > 0 AND `StartTime` >= '{0}' ORDER BY `StartTime` DESC", cutOffTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    // Send the query to the database and retrieve the table of valid sessions.
                    var sqlResult = Database.selectData(query);

                    // Count all of the numbers of 'Targets' games played in all of the sessions for today.
                    while (sqlResult.Read())
                    {
                        num += sqlResult.GetInt32(game + "Played");
                    }

                    // Close the data reader.
                    sqlResult.Close();

                    Debug.Log(num + " " + game + " games have been played since: " + cutOffTime.ToString("HH:mm:ss") + "   (Now: " + now.ToString("HH:mm:ss") + ").");

                    break;
                default:
                    // Do nothing...

                    Debug.Log("Invalid game queried. Current system time returned instead.");

                    break;
            }
        }

        return num;
    }

    /// <summary>
    /// if the user has reached the max play time limit for the session or the day then redirect to the awareness page
    /// </summary>
    public bool PlaytimeLimitReached(string game)
    {
        bool isLimitReached = false;

        switch (game)
        {
            case "Wingman": // Check if the survivor has played the maximum number of 'Wingman' games for this session.

                Debug.Log("Play Limit for " + game + ": " + config.WingmanGamesPlayedThisSession + "out of max " + config.WingmanGamesPerSession + " per session.");
                Debug.Log("Played games for " + game + ": " + GetNumGamesPlayedToday("Wingman") + "out of max " + config.WingmanGamesPerDay + " per day.");

                if (config.WingmanGamesPlayedThisSession >= config.WingmanGamesPerSession)
                {
                    DateTime next = GetNextEarliestSessionTimeFor(game);
                    config.LimitWarning = string.Format(LimitAwarenessSettings.COME_BACK_AT, "Wingman", next.ToString("hh:mm tt"));
                    isLimitReached = true;
                }
                else
                if (GetNumGamesPlayedToday("Wingman") >= config.WingmanGamesPerDay)
                {
                    config.LimitWarning = string.Format(LimitAwarenessSettings.DAILY_LIMIT_REACHED_MESSAGE, game);
                    isLimitReached = true;
                }

                break;

            case "Cycling": // Check if the survivor has played the maximum number of 'Wingman' games for this session.

                Debug.Log("Play Limit for " + game + ": " + config.CyclingGamesPlayedThisSession + "out of max " + config.CyclingGamesPerSession + " per session.");
                Debug.Log("Played games for " + game + ": " + GetNumGamesPlayedToday("Wingman") + "out of max " + config.CyclingGamesPerDay + " per day.");

                if (config.CyclingGamesPlayedThisSession >= config.CyclingGamesPerSession)
                {
                    DateTime next = GetNextEarliestSessionTimeFor(game);
                    config.LimitWarning = string.Format(LimitAwarenessSettings.COME_BACK_AT, "Cycling", next.ToString("hh:mm tt"));
                    isLimitReached = true;
                }
                else
            if (GetNumGamesPlayedToday("Cycling") >= config.CyclingGamesPerDay)
                {
                    config.LimitWarning = string.Format(LimitAwarenessSettings.DAILY_LIMIT_REACHED_MESSAGE, game);
                    isLimitReached = true;
                }

                break;
            case "Targets": // Check if the survivor has played the maximum number of 'Targets' games for this session.

                Debug.Log("Play Limit for " + game + ": " + config.TargetGamesPlayedThisSession + "out of max " + config.TargetGamesPerSession + " per session.");
                Debug.Log("Played games for " + game + ": " + GetNumGamesPlayedToday("Targets") + "out of max " + config.TargetGamesPerDay + " per day.");

                if (config.TargetGamesPlayedThisSession >= config.TargetGamesPerSession)
                {
                    DateTime next = GetNextEarliestSessionTimeFor(game);
                    config.LimitWarning = string.Format(LimitAwarenessSettings.COME_BACK_AT, game, next.ToString("hh:mm tt"));
                    isLimitReached = true;
                }
                else
                if (GetNumGamesPlayedToday("Targets") >= config.TargetGamesPerDay)
                {
                    config.LimitWarning = string.Format(LimitAwarenessSettings.DAILY_LIMIT_REACHED_MESSAGE, game);
                    isLimitReached = true;
                }

                break;
            default:
                break;
        }

        /*if (GetTotalGamesPlayedThisSession() >= config.WingmanGamesPerSession)
        {
            config.LimitWarning = string.Format(LimitAwarenessSettings.SESSION_LIMIT_REACHED_MESSAGE, config.WingmanIntervalBetweenSession);
            isLimitReached = true;
        }

        if (GetTotalGamesPlayedToday() >= config.WingmanGamesPerDay)
        {
            config.LimitWarning = LimitAwarenessSettings.DAILY_LIMIT_REACHED_MESSAGE;
            isLimitReached = true;
        }*/


        return isLimitReached;
    }

    private int GetTotalGamesPlayedToday()
    {
        int result = 0;

        if (Database.PingConnection())
        {
            DateTime now = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();

            //check the number of games played today
            string query = string.Format(@"
            SELECT count(*) AS Result FROM Achievement a
            LEFT JOIN Session s
            ON a.SessionID = s.SessionID
            WHERE s.UserID = {0} AND s.StartTime >= '{1}' AND StartTime <= '{2}'
            ", config.UserId, now.ToString("yyyy-MM-dd 00:00:00"), now.ToString("yyyy-MM-dd 23:59:59"));
            var sqlResult = Database.selectData(query);
            while (sqlResult.Read())
            {
                result = sqlResult.GetInt32("Result");
            }
            sqlResult.Close();
        }
        return result;
    }

    private int GetTotalGamesPlayedThisSession()
    {
        return GetTotalGamesOnSession(config.SessionID);
    }

    private int GetTotalGamesOnSession(int sessionId)
    {
        int result = 0;

        if (Database.PingConnection())
        {
            //check the number of games played today
            string query = string.Format(@"
            SELECT count(*) AS Result FROM Achievement
            WHERE SessionID = {0}
            ", config.SessionID);
            var sqlResult = Database.selectData(query);
            while (sqlResult.Read())
            {
                result = sqlResult.GetInt32("Result");
            }
            sqlResult.Close();
        }
        return result;
    }

    private bool LimitedByPreviousSession(string game)
    {
        //get last session id if any
        int lastSessionId = GetLastSessionId();

        int intervalInMinutes;
        int intervalInHours = 0;
        bool limited = false;

        DateTime now = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();
        DateTime ready = now;
        DateTime lastSessionTime = now;

        switch (game)
        {
            case "Wingman":

                intervalInMinutes = (int)(config.WingmanIntervalBetweenSession * 60);

                break;

            case "Targets":

                intervalInMinutes = (int)(config.TargetIntervalBetweenSession * 60);

                break;


            case "Cycling":

                intervalInMinutes = (int)(config.CyclingIntervalBetweenSession * 60);

                break;


            default:
                // Do nothing...

                intervalInMinutes = 9001;
                intervalInHours = 0;

                break;
        }

        // I'm not sure, but I think that there might need to be another check for when the interval since the last session crosses over to the next day.
        // If its not there, then I didn't get around to doing it, sorry if this causes you any issues... :(
        if ((GetNumGamesInLastIntervalFor(game) + config.WingmanGamesPlayedThisSession > config.WingmanGamesPerSession && game == "Wingman") ||
            (GetNumGamesInLastIntervalFor(game) + config.TargetGamesPlayedThisSession > config.TargetGamesPerSession && game == "Targets") ||
            (GetNumGamesInLastIntervalFor(game) + config.CyclingGamesPlayedThisSession > config.CyclingGamesPerSession && game == "Cycling"))
        {
            //check if last session is outside the interval
            lastSessionTime = GetLastSessionFor(game);

            TimeSpan interval = new TimeSpan(intervalInHours, intervalInMinutes, 0);
            ready = lastSessionTime.Add(interval);

            if (now < ready) //not ready to commence another session
            {
                config.LimitWarning = string.Format(LimitAwarenessSettings.LAST_SESSION_LIMIT_MESSAGE, game, ready.ToString("HH:mm:ss"));
                limited = true;
            }
        }

        Debug.Log("T: " + config.TargetGamesPerSession + "   W: " + config.WingmanGamesPerSession + "   C: " + config.CyclingGamesPerSession);
        Debug.Log(game + " is limited by last session? " + limited + "    Last: " + lastSessionTime.ToString("HH:mm:ss") + "   Ready: " + ready.ToString("HH:mm:ss") + "   Now: " + now.ToString("HH:mm:ss"));

        return limited;
    }

    private int GetLastSessionId()
    {
        int result = -1;
        //check the number of games played today
        string query = string.Format(@"
            SELECT * FROM Session WHERE UserID = {0} ORDER BY SessionId DESC LIMIT 1
            ", config.UserId);
        var sqlResult = Database.selectData(query);
        while (sqlResult.Read())
        {
            result = sqlResult.GetInt32("SessionID");
        }

        sqlResult.Close();

        return result;
    }

    private DateTime GetSessionEndTime(int sessionId)
    {
        DateTime result = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();
        //check the number of games played today
        string query = string.Format(@"
            SELECT * FROM Session WHERE SessionID = {0}
            ", sessionId);
        var sqlResult = Database.selectData(query);
        while (sqlResult.Read())
        {
            var end = sqlResult.GetMySqlDateTime("EndTime"); //there is a possibility of having 00/00/0000 00:00:00 in the entry
            var start = sqlResult.GetMySqlDateTime("StartTime");

            if (end.Year != 0)
                result = end.GetDateTime();
            else
                result = start.GetDateTime();
        }
        sqlResult.Close();

        return result;
    }
}