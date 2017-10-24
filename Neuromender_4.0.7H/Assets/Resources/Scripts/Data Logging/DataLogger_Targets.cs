using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>
/// Data logger for the 'Targets' game.
/// </summary>
public class DataLogger_Targets : DataLogger_Base {

    public Grid grid = null;            // Local handle for the grid that is being tracked.
    public int recordedPunches = 0;     // The number of punches that were recorded.

    private ScoreCarnival scoreBoard = null;

	// Use this for initialization
	new void Start () {

        // Force change the logged game param to Targets.
        loggedGame = "Targets";

        base.Start();

        // Specify the username for this datalogging session.
        DATA["User"] = user = login.NAME;
        DATA["GameNum"] = login.config.TargetGameNum;
        scoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreCarnival>();

        grid = GameObject.Find("Grid").GetComponent<Grid>();

        // Do not do basic functionality if the grid is not found.
        if (grid == null)
        {
            enabled = false;
            Debug.LogWarning("Grid not found! Script attached to " + gameObject.name + " was disabled.");
        }
    }

    /// <summary>
    /// Implementation of DataLoggerBase's SerialiseData function. Serialise data for a session of the 'Targets' game locally.
    /// </summary>
    public override void SerialiseData()
    {
        // Increase the number of targets games played this session.
        login.config.TargetGamesPlayedThisSession++;

        base.SerialiseData();

        // Leave immediately if there is no specified user or no specified tracked game.
        if (user != "" && loggedGame != "")
        {
            Debug.Log("Serialising!");

            // Name the new data file after the game, date, and daily repetition number.
            string fileName = loggedGame + "_" + NOW.Day + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(NOW.Month) + NOW.Year
                              + "_Session" + login.config.SessionID + "_Game" + login.config.TargetGameNum;

            // Create a file in the data folder, in a directory named after the user.
            string dataPath = Application.dataPath + "/Data/" + user + "/" + fileName + ".csv";

            using (STREAM = new StreamWriter(dataPath))
            {
                WriteBasicInfo();

                STREAM.WriteLine("\nCols:," + grid.cols + ",Rows:," + grid.rows + ",Target Scale:," + grid.targetScaleMod + ",Score:," + scoreBoard.Score);
                STREAM.WriteLine("Max Reach Distance:," + login.config.extensionThreshold + ",Min Reach Distance:," + login.config.minExtensionThreshold);

                STREAM.WriteLine("\n\n,,,,,,,,,,,,,,,,,RAW DATA");

                STREAM.Write("\nTarget Index:,Target Distance:,Extend Time:,Retract Time:,Assisted?,Hit?,Accuracy:,Points:,Target Position [X|Y], Hit Position [X|Y],,,");
                STREAM.Write("Elbow Angle,Shoulder->Wrist Distance,Shoulder->Wrist Angle (H),Shoulder->Wrist Angle (V),Wrist Position (L) [X|Y|Z],Elbow Position (L) [X|Y|Z],Shoulder Position (L) [X|Y|Z],");
                STREAM.WriteLine("Chest Position [X|Y|Z],Shoulder Position (R) [X|Y|Z],Elbow Position (R) [X|Y|Z],Wrist Position (R) [X|Y|Z],Time");

                TargetsDataMass dat = null;
                for (int i = 0; i < recordedPunches; i++)
                {
                    dat = (TargetsDataMass)DATA["Punch_" + i];
                    STREAM.Write(dat.targetNum + "," + dat.reachDistance + "," + dat.reachTime.ToString("0.000") + "," + dat.retractTime.ToString("0.000") + "," + 
                                 dat.assisted + "," + dat.wasAHit + "," + dat.accuracy + "," + dat.score + "," +
                                 dat.targetPos.x.ToString("0.000") + " | " + dat.targetPos.y.ToString("0.000") + "," + 
                                 dat.hitPos.x.ToString("0.000") + " | " + dat.hitPos.y.ToString("0.000") + ",,,");

                    STREAM.Write(dat.rawData.ElbowAngle.ToString("0.000") + "," + dat.rawData.ShoulderToWristDistance.ToString("0.000") + "," + dat.rawData.ShoulderToWristAngleHorizontal.ToString("0.000") + "," + dat.rawData.ShoulderToWristAngleVertical.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.WristLeft.x.ToString("0.000") + " | " + dat.rawData.WristLeft.y.ToString("0.000") + " | " + dat.rawData.WristLeft.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ElbowLeft.x.ToString("0.000") + " | " + dat.rawData.ElbowLeft.y.ToString("0.000") + " | " + dat.rawData.ElbowLeft.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ShoulderLeft.x.ToString("0.000") + " | " + dat.rawData.ShoulderLeft.y.ToString("0.000") + " | " + dat.rawData.ShoulderLeft.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.Chest.x.ToString("0.000") + " | " + dat.rawData.Chest.y.ToString("0.000") + " | " + dat.rawData.Chest.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ShoulderRight.x.ToString("0.000") + " | " + dat.rawData.ShoulderRight.y.ToString("0.000") + " | " + dat.rawData.ShoulderRight.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ElbowRight.x.ToString("0.000") + " | " + dat.rawData.ElbowRight.y.ToString("0.000") + " | " + dat.rawData.ElbowRight.z.ToString("0.000") + ",");
                    STREAM.WriteLine(dat.rawData.WristRight.x.ToString("0.000") + " | " + dat.rawData.WristRight.y.ToString("0.000") + " | " + dat.rawData.WristRight.z.ToString("0.000") + "," + dat.rawData.time.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                // Check the database connection.
                DBCons.GetComponent<database>().PingConnection();

                // Submit the data associated with the latest targets game to the database IF there is a connection.
                if (DBCons.GetComponent<database>().connected)
                    PushData();
                else
                {
                    // Write an entry in the data file registry to remind the program to push the data the next time a connection is received.
                    monitor.AddNewPendingEntry(user, loggedGame, fileName);
                }

                STREAM.Close();
            }

            Debug.Log("To Zip: " + fileName);
            FileZipping.ZipData(fileName + ".csv", fileName, user, "test");
        }
        else
        {
            Debug.Log("Error: No specified 'user' or 'logged game' param. This script is attached to the " + gameObject.name + " game object.");
        }
    }

    /// <summary>
    /// Implementation of DataLoggerBase's PushData function. Push data for a session of the 'Targets' game to the external database.
    /// </summary>
    public override void PushData()
    {
        TargetsDataMass dat = null;
        string fields = "";
        string values = "";

        DateTime serverTime = DBCons.GetComponent<database>().getDatabaseTime();

        for (int i = 0; i < recordedPunches; i++)
        {
            dat = (TargetsDataMass)DATA["Punch_" + i];
            fields = "";    // Reset for the proceeding loop.
            values = "";

            fields += "ReachGRDataID";
            values += 0 + ",";

            fields += ",UserID";
            values += login.config.UserId + ",";

            fields += ",SessionID";
            values += login.config.SessionID + ",";

            fields += ",GameNoId";
            values += (login.config.TargetGameNum) + ",";

            fields += ",RoundID";
            values += i + ",";

            fields += ",Accuracy";
            values += dat.accuracy + ",";

            fields += ",Points";
            values += dat.score + ",";

            fields += ",TargetPositionX";
            values += dat.targetPos.x.ToString("0.000") + ",";

            fields += ",TargetPositionY";
            values += dat.targetPos.y.ToString("0.000") + ",";

            fields += ",HitPositionX";
            values += dat.hitPos.x.ToString("0.000") + ",";

            fields += ",HitPositionY";
            values += dat.hitPos.y.ToString("0.000") + ",";

            fields += ",Assisted";
            values += dat.assisted + ",";

            fields += ",MaximumReach";
            values += login.config.extensionThreshold + ",";

            fields += ",MinimumReach";
            values += login.config.minExtensionThreshold + ",";

            fields += ",Latency";
            values += dat.reachTime + ",";

            fields += ",TimeCreated";

            // Sync the current time of the entry (client system clock) with the database's clock to make sure that they are identical.
            DateTime now = DateTime.Now;
            AlertMessenger.syncTimes(now, serverTime);

            values += "'" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            // Submit the latest punch's data to the database.
            DBCons.GetComponent<database>().SubmitData("ReachGameData", fields, values);
        }
    }

    public static void PushData(Dictionary<string, object> data)
    {
        TargetsDataMass dat = null;
        string fields = "";
        string values = "";

        GameObject DBCons = GameObject.Find("DatabaseController");

        for (int i = 0; i < (int)data["NUM_PUNCHES"]; i++)
        {
            dat = (TargetsDataMass)data["Punch_" + i];
            fields = "";    // Reset for the proceeding loop.
            values = "";

            fields += "ReachGRDataID";
            values += 0 + ",";

            fields += ",UserID";
            values += data["USER_ID"] + ",";

            fields += ",SessionID";
            values += data["SESSION_ID"] + ",";

            fields += ",GameNoId";
            values += data["GAME_NUM"] + ",";

            fields += ",RoundID";
            values += data["ROUND_ID"] + ",";

            fields += ",Accuracy";
            values += dat.accuracy + ",";

            fields += ",Points";
            values += dat.score + ",";

            fields += ",TargetPositionX";
            values += dat.targetPos.x.ToString("0.000") + ",";

            fields += ",TargetPositionY";
            values += dat.targetPos.y.ToString("0.000") + ",";

            fields += ",HitPositionX";
            values += dat.hitPos.x.ToString("0.000") + ",";

            fields += ",HitPositionY";
            values += dat.hitPos.y.ToString("0.000") + ",";

            fields += ",Assisted";
            values += dat.assisted + ",";

            fields += ",MaximumReach";
            values += data["MAX_EXTENSION"] + ",";

            fields += ",MinimumReach";
            values += data["MIN_EXTENSION"] + ",";

            fields += ",Latency";
            values += dat.reachTime + ",";

            fields += ",TimeCreated";

            // Sync the current time of the entry (client system clock) with the database's clock to make sure that they are identical.
            DateTime now = DateTime.Parse((string)data["TIME"]);
            AlertMessenger.syncTimes(now, DBCons.GetComponent<database>().getDatabaseTime());

            values += "'" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            // Submit the latest punch's data to the database.
            DBCons.GetComponent<database>().SubmitData("ReachGameData", fields, values);
        }
    }

    /// <summary>
    /// Static function for reading data files that are formatted for recording a 'Targets' game.
    /// </summary>
    /// <param name="Username">The name of the profile that this data file belongs to.</param>
    /// <param name="fileName">The name of the data file that is being read from.</param>
    /// <returns></returns>
    public static Dictionary<string, object> ReadDataFrom(string Username, string fileName)
    {
        if (File.Exists(Application.dataPath + "Data/" + Username + fileName))
            using (StreamReader file = new StreamReader(Application.dataPath + "Data/" + Username + "/" + fileName))
            {
                // TODO: Read an existing data file back into the program to send to the external database.

                return null;
            }
        else
            return null;
    }
}

/// <summary>
/// Data mass for the data from a single punch in the 'Targets' game.
/// 
/// One of these entries is made every time the survivor completes a punch (arm is extended AND retracted).
/// </summary>
public class TargetsDataMass : DataMass_Base
{
    public int targetNum;           // The target index in the grid.
    public float reachTime;         // The time taken to extend the arm to the contact point.
    public float retractTime;       // The time taken to return the arm to the body.
    public float reachDistance;     // The distance that the arm was extended.
    public int accuracy;            // The proximity the punch was to the centre of the target.
    public int score;               // The points awarded to reflect the punch's performance.
    public bool assisted;           // Was the action assisted by another arm? TRUE: Yes.
    public bool wasAHit;            // Was the action a successful hit? TRUE: Yes.
    public Vector2 targetPos;       // The position of the target (world co-ordinates).
    public Vector2 hitPos;          // The point of collision of the hit.

    public DateTime time;

    public TargetsRawDataMass rawData = null;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="targNum">The target index in the grid.</param>
    /// <param name="reach">The time taken to extend the arm to the contact point.</param>
    /// <param name="retract">The time taken to return the arm to the body.</param>
    /// <param name=
    /// 
    /// >The distance that the arm was extended.</param>
    /// <param name="acc">The proximity the punch was to the centre of the target.</param>
    /// <param name="points">The points awarded to reflect the punch's performance.</param>
    /// <param name="assist">Was the action assisted by another arm? TRUE: Yes.</param>
    /// <param name="hit">Was the action a successful hit? TRUE: Yes.</param>
    public TargetsDataMass(int targNum, float reach, float retract, float distance, int acc, int points, bool assist, bool hit)
    {
        targetNum = targNum;
        reachTime = reach;
        retractTime = retract;
        reachDistance = distance;
        accuracy = acc;
        score = points;
        assisted = assist;
        wasAHit = hit;

        time = DateTime.Now;

        rawData = new TargetsRawDataMass();     // Calculate the raw data for the punch.
    }
}

/// <summary>
/// Raw Data entry for positional information. Used during the recording of a 'Targets' game.
/// </summary>
public class TargetsRawDataMass : DataMass_Base
{
    public Vector3 WristLeft;
    public Vector3 ElbowLeft;
    public Vector3 ShoulderLeft;
    public Vector3 Chest;
    public Vector3 ShoulderRight;
    public Vector3 ElbowRight;
    public Vector3 WristRight;

    public float ElbowAngle;
    public float ShoulderToWristDistance;
    public float ShoulderToWristAngleHorizontal;
    public float ShoulderToWristAngleVertical;

    public DateTime time;

    public TargetsRawDataMass()
    {
        WristLeft = GameObject.Find("WristLeft").transform.position;
        ElbowLeft = GameObject.Find("ElbowLeft").transform.position;
        ShoulderLeft = GameObject.Find("ShoulderLeft").transform.position;
        Chest = GameObject.Find("SpineMid").transform.position;
        ShoulderRight = GameObject.Find("ShoulderRight").transform.position;
        ElbowRight = GameObject.Find("ElbowRight").transform.position;
        WristRight = GameObject.Find("WristRight").transform.position;

        time = DateTime.Now;

        if (GameObject.Find("DatabaseController").GetComponent<LoginControl>().config.sideAffected == 1) // left
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
    }
}