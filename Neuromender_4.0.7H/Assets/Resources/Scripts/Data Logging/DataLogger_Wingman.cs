using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

/// <summary>
/// Data logger for the 'Wingman' game.
/// </summary>
public class DataLogger_Wingman : DataLogger_Base {

    [Tooltip("The current ring that the data logger is tracking performance to.")]
    public int currentRingNumber = 0;

    [Tooltip("Index for the latest entry in the raw data tracking.\n(Raw data is taken much more frequently than regular game data).")]
    public int currentRawDataIndex = 0;

    // Use this for initialization
    new void Start()
    {
        // Force change the logged game param to Wingman.
        loggedGame = "Wingman";

        base.Start();

        // Specify the username for this datalogging session.
        DATA["User"] = user = login.NAME;
        DATA["GameNum"] = login.config.WingmanGameNum;
    }

    /// <summary>
    /// Implementation of DataLoggerBase's SerialiseData function. Serialise data for a session of the 'Wingman' game locally.
    /// </summary>
    public override void SerialiseData()
    {
        // Increase the number of targets games played this session.
        login.config.WingmanGamesPlayedThisSession++;

        base.SerialiseData();

        // Leave immediately if there is no specified user or no specified tracked game.
        if (user != "" && loggedGame != "")
        {
            Debug.Log("Serialising!");

            // Name the new data file after the game, date, and daily repetition number.
            string fileName = loggedGame + "_" + NOW.Day + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(NOW.Month)
                              + NOW.Year
                              + "_Session" + login.config.SessionID + "_Game" + login.config.WingmanGameNum;

            // Create a file in the data folder, in a directory named after the user.
            string dataPath = Application.dataPath + "/Data/" + user + "/" + fileName + ".csv";

            using (STREAM = new StreamWriter(dataPath))
            {
                WriteBasicInfo();

                string trackStr = "Track:,";
                if (login.selectedTrack == TrackName.beach) trackStr += "Beach,";
                else if (login.selectedTrack == TrackName.forest) trackStr += "Forest,";
                else if (login.selectedTrack == TrackName.temple) trackStr += "Temple,";
                else trackStr += "Unknown,";

                string spdStr = "Speed:," + login.selectedSpeed.ToString() + ",";

                string timeStr = "Time:,";
                if (login.selectedSpeed == SpeedLevel.fast) timeStr += login.config.trackFast + " (sec),";
                else if (login.selectedSpeed == SpeedLevel.medium) timeStr += login.config.trackMedium + " (sec),";
                else if (login.selectedSpeed == SpeedLevel.slow) timeStr += login.config.trackSlow + " (sec),";
                else timeStr += "Unknown,";

                STREAM.WriteLine(trackStr + spdStr + timeStr);
                STREAM.WriteLine("Target Angle:," + login.config.angleThreshold + ",Angle Incr:," + login.config.angleThresholdIncrease);

                STREAM.WriteLine("\nBASIC DATA");
                STREAM.WriteLine("Ring Num,Score,Arm Angle,Assisted");

                WingmanDataMass dat = null;

                //foreach (string str in DATA.Keys)
                    //Debug.Log(str);

                for(int i = 0; i < currentRingNumber; i++)
                {
                    dat = (WingmanDataMass)DATA["Ring_" + i];
                    STREAM.WriteLine(dat.ringNum + "," + dat.score + "," + dat.armAngle.ToString("0.000") + "," + dat.assisted);
                }
                dat = null;

                STREAM.WriteLine("\nRAW DATA");
                STREAM.WriteLine("BodyDepth, CentralPont (X | Y | Z),RightHand (X | Y | Z),LeftHand (X | Y | Z),RightElbow (X | Y | Z),LeftElbow (X | Y | Z),LeftAngle,RightAngle,Time,RingNumber,AchievementID");

                WingmanRawDataMass rawDat = null;
                for(int i = 0; i < currentRawDataIndex; ++i)
                {
                    rawDat = (WingmanRawDataMass)DATA["RawData_" + i];
                    STREAM.WriteLine(   rawDat.BodyDepth.ToString("0.000") + "," +
                                        rawDat.centerPoint.x.ToString("0.000") + " | " + rawDat.centerPoint.y.ToString("0.000") + " | " + rawDat.centerPoint.z.ToString("0.000") + "," +
                                        rawDat.rightHand.x.ToString("0.000") + " | " + rawDat.rightHand.y.ToString("0.000") + " | " + rawDat.rightHand.z.ToString("0.000") + "," +
                                        rawDat.leftHand.x.ToString("0.000") + " | " + rawDat.leftHand.y.ToString("0.000") + " | " + rawDat.leftHand.z.ToString("0.000") + "," +
                                        rawDat.rightElbow.x.ToString("0.000") + " | " + rawDat.rightElbow.y.ToString("0.000") + " | " + rawDat.rightElbow.z.ToString("0.000") + "," +
                                        rawDat.leftElbow.x.ToString("0.000") + " | " + rawDat.leftElbow.y.ToString("0.000") + " | " + rawDat.leftElbow.z.ToString("0.000") + "," +
                                        rawDat.leftAngle.ToString("0.000") + "," + rawDat.rightAngle.ToString("0.000") + "," + rawDat.time.ToString("yyyy-MM-dd HH:mm:ss") + "," + rawDat.RingNumber + "," + rawDat.AchieveID );
                }

                STREAM.Close();

                Debug.Log("To Zip: " + fileName);
                FileZipping.ZipData(fileName + ".csv", fileName, user);
            }
        }
    }

    /// <summary>
    /// Implementation of DataLoggerBase's PushData function. Push data for a session of the 'Wingman' game to the external database.
    /// </summary>
    public override void PushData()
    {
        // Didn't get enough time to sort through the Wingman-related code to pull out the data logging functionality.
    }

    public static void PushData(Dictionary<string, object> data)
    {

    }
}

/// <summary>
/// Data mass for the data from a single punch in the 'Wingman' game.
/// 
/// One of these entries is made every time the survivor goes past / through a ring.
/// </summary>
public class WingmanDataMass : DataMass_Base
{
    public int ringNum;
    public int score;
    public float armAngle;
    public bool assisted;
    public DateTime time;

    public WingmanDataMass(int ring, int sc, float armAng, bool assist)
    {
        ringNum = ring;
        score = sc;
        armAngle = armAng;
        assisted = assist;

        time = DateTime.Now;
    }
}

/// <summary>
/// Raw Data entry for positional information. Used during the recording of a 'Wingman' game.
/// </summary>
public class WingmanRawDataMass : DataMass_Base
{
    public float BodyDepth;
    public Vector3 rightHand;
    public Vector3 rightElbow;
    public Vector3 leftHand;
    public Vector3 leftElbow;
    public Vector3 centerPoint;
    public float rightAngle;
    public float leftAngle;
    public DateTime time;
    public int RingNumber;
    public int AchieveID;

    public WingmanRawDataMass(st_RawTracking raw, int currentRing)
    {
        leftAngle = raw.leftAngle;
        rightAngle = raw.rightAngle;
        BodyDepth = raw.BodyDepth;
        leftElbow = raw.leftElbow;
        leftHand = raw.leftHand;
        rightElbow = raw.rightElbow;
        rightHand = raw.rightHand;
        centerPoint = raw.centerPoint;
        time = DateTime.Now;
        RingNumber = currentRing;
        AchieveID = raw.AchievementId;
    }
}