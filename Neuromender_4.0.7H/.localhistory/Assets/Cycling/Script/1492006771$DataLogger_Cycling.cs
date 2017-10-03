using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>
/// Data logger for the 'Cycling' game.
/// </summary>
/// 
public class DataLogger_Cycling : DataLogger_Base
{

    public int DiamondNo = 0;
    public GameObject Cube;
    public GameObject colRPM;
    public float speed;


    // Use this for initialization
    new void Start()
    {

        // Force change the logged game param to Cycling.
        loggedGame = "Cycling";

        base.Start();

        // Specify the username for this datalogging session.
        DATA["User"] = user = login.NAME;
        DATA["GameNum"] = login.config.CyclingGameNum;

    }

    /// <summary>
    /// Implementation of DataLoggerBase's SerialiseData function. Serialise data for a session of the 'Cycling' game locally.
    /// </summary>
    public override void SerialiseData()
    {
        // Increase the number of Cycling games played this session.
        login.config.CyclingGamesPlayedThisSession++;

        base.SerialiseData();

        // Leave immediately if there is no specified user or no specified tracked game.
        if (user != "" && loggedGame != "")
        {
            Debug.Log("Serialising!");

            // Name the new data file after the game, date, and daily repetition number.
            string fileName = loggedGame + "_" + NOW.Day + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(NOW.Month) + NOW.Year
                              + "_Session" + login.config.SessionID + "_Game" + login.config.CyclingGameNum;

            // Create a file in the data folder, in a directory named after the user.
            string dataPath = Application.dataPath + "/Data/" + user + "/" + fileName + ".csv";

            using (STREAM = new StreamWriter(dataPath))
            {
                WriteBasicInfo();

                STREAM.WriteLine("\nDistance:," + 200 + ",Time:," + Cube.GetComponent<Timer>().TimeSec + ",Score:," + Cube.GetComponent<DiamondScore>().CurrentScore);
                /* STREAM.WriteLine("\nAverage Speed:," + Cube.GetComponent<ResultDisplay>().speed + ",RPM:," + colRPM.GetComponent<colliderRPM>().counter);*/

                STREAM.WriteLine("\n\n,,,,,,,,,,,,,,,,,RAW DATA");

                STREAM.Write("\nDiamond Index:,Score:,No of Rotation:,Time Interval:,,,,,,,,,,,,,,,");
                /*STREAM.Write("Elbow Angle,Shoulder->Wrist Distance,Shoulder->Wrist Angle (H),Shoulder->Wrist Angle (V),Wrist Position (L) [X|Y|Z],Elbow Position (L) [X|Y|Z],Shoulder Position (L) [X|Y|Z],");
                STREAM.WriteLine("Chest Position [X|Y|Z],Shoulder Position (R) [X|Y|Z],Elbow Position (R) [X|Y|Z],Wrist Position (R) [X|Y|Z],Time");*/

                STREAM.WriteLine("Time");

                CyclingDataMass dat = null;
                for (int i = 0; i < DiamondNo; i++)
                {
                    dat = (CyclingDataMass)DATA["DIAMONDS_" + i];
                    STREAM.Write(dat.DiamondNum + "," + dat.score + "," + dat.RotationNum + "," + dat.timeInterval + "," + dat.distanceroute + "," + dat.diamondgap + "," + ",,,,,,,,,,,,,,,,");
                    /*STREAM.Write(dat.rawData.ElbowAngle.ToString("0.000") + "," + dat.rawData.ShoulderToWristDistance.ToString("0.000") + "," + dat.rawData.ShoulderToWristAngleHorizontal.ToString("0.000") + "," + dat.rawData.ShoulderToWristAngleVertical.ToString("0.000") + ",");
                     STREAM.Write(dat.rawData.WristLeft.x.ToString("0.000") + " | " + dat.rawData.WristLeft.y.ToString("0.000") + " | " + dat.rawData.WristLeft.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ElbowLeft.x.ToString("0.000") + " | " + dat.rawData.ElbowLeft.y.ToString("0.000") + " | " + dat.rawData.ElbowLeft.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ShoulderLeft.x.ToString("0.000") + " | " + dat.rawData.ShoulderLeft.y.ToString("0.000") + " | " + dat.rawData.ShoulderLeft.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.Chest.x.ToString("0.000") + " | " + dat.rawData.Chest.y.ToString("0.000") + " | " + dat.rawData.Chest.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ShoulderRight.x.ToString("0.000") + " | " + dat.rawData.ShoulderRight.y.ToString("0.000") + " | " + dat.rawData.ShoulderRight.z.ToString("0.000") + ",");
                    STREAM.Write(dat.rawData.ElbowRight.x.ToString("0.000") + " | " + dat.rawData.ElbowRight.y.ToString("0.000") + " | " + dat.rawData.ElbowRight.z.ToString("0.000") + ",");
                    STREAM.WriteLine(dat.rawData.WristRight.x.ToString("0.000") + " | " + dat.rawData.WristRight.y.ToString("0.000") + " | " + dat.rawData.WristRight.z.ToString("0.000") + "," + dat.rawData.time.ToString("yyyy-MM-dd HH:mm:ss"));
                    */
                }

                // Check the database connection.
                DBCons.GetComponent<database>().PingConnection();

                // Submit the data associated with the latest Cycling game to the database IF there is a connection.
                if (DBCons.GetComponent<database>().connected)
                {
                    PushData();
                    Debug.Log("pushin Data");
                }
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
    /// Implementation of DataLoggerBase's PushData function. Push data for a session of the 'Cycling' game to the external database.
    /// </summary>
    public override void PushData()
    {
        CyclingDataMass dat = null;
        string fields = "";
        string values = "";

        DateTime serverTime = DBCons.GetComponent<database>().getDatabaseTime();


        Debug.Log("pushin Data 1");

        for (int i = 0; i < DiamondNo; i++)
        {

            Debug.Log("pushin Data" + i);


            dat = (CyclingDataMass)DATA["DIAMONDS_" + i];
            fields = "";    // Reset for the proceeding loop.
            values = "";

            fields += "CyclingGameID";
            values += 0 + ",";

            fields += ",UserID";
            values += login.config.UserId + ",";

            fields += ",SessionID";
            values += login.config.SessionID + ",";

            fields += ",GameNo";
            values += (login.config.CyclingGameNum) + ",";

            fields += ",DiamondNo";
            values += dat.DiamondNum + ",";

            /* fields += ",Distance";
             values += dat.distance + ",";

             fields += ",TotalTime";
             values += dat.time + ",";

             fields += ",TotalScore";
             values += dat.score + ",";

             fields += ",AverageSpeed";
             values += dat.AvgSpeed + ",";

             fields += ",RPM";
             values += dat.RPM + ",";*/



            fields += ",TimeInterval";
            values += dat.timeInterval + ",";

            fields += ",Score";
            values += dat.score + ",";

            fields += ",Rotation";
            values += dat.RotationNum + ",";

            fields += ",DistanceRoute"; //not fucntioning for now
            values += dat.distanceroute + ",";

            fields += ",DiamondGap"; //not fucntioning for now
            values += dat.diamondgap + ",";



            fields += ",TimeCreated";

            // Sync the current time of the entry (client system clock) with the database's clock to make sure that they are identical.
            DateTime now = DateTime.Now;
            AlertMessenger.syncTimes(now, serverTime);

            values += "'" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            DBCons.GetComponent<database>().SubmitData("CyclingGameData", fields, values);
        }
    }

    public static void PushData(Dictionary<string, object> data)
    {
        CyclingDataMass dat = null;
        string fields = "";
        string values = "";

        GameObject DBCons = GameObject.Find("DatabaseController");

        Debug.Log("pushin Data 2");

        for (int i = 0; i < (int)data["NUM_Diamond"]; i++)
        {
            dat = (CyclingDataMass)data["DIAMONDS_" + i];
            fields = "";    // Reset for the proceeding loop.
            values = "";

            fields += "CyclingGameID";
            values += 0 + ",";

            fields += ",UserID";
            values += data["USER_ID"] + ",";

            fields += ",SessionID";
            values += data["SESSION_ID"] + ",";

            fields += ",GameNo";
            values += data["GAME_NUM"] + ",";

            fields += ",DiamondNo";
            values += dat.DiamondNum + ",";

            /* fields += ",Distance";
             values += data["*****"] + ",";

             fields += ",TotalTime";
             values += data["*****"] + ",";

             fields += ",TotalScore";
             values += dat.score + ",";

             fields += ",AverageSpeed";
             values += dat.AvgSpeed + ",";

             fields += ",RPM";
             values += dat.RPM + ",";*/

            fields += ",TimeInterval";
            values += dat.timeInterval + ",";

            fields += ",Score";
            values += dat.score + ",";

            fields += ",Rotation";
            values += dat.RotationNum + ",";

            fields += ",DistanceRoute"; //not fucntioning for now
            values += dat.distanceroute + ",";

            fields += ",DiamondGap"; //not fucntioning for now
            values += dat.diamondgap + ",";


            fields += ",TimeCreated";

            // Sync the current time of the entry (client system clock) with the database's clock to make sure that they are identical.
            DateTime now = DateTime.Parse((string)data["TIME"]);
            AlertMessenger.syncTimes(now, DBCons.GetComponent<database>().getDatabaseTime());

            values += "'" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            // Submit the latest punch's data to the database.
            DBCons.GetComponent<database>().SubmitData("CyclingGameData", fields, values);
        }
    }

    /// <summary>
    /// Static function for reading data files that are formatted for recording a 'Cycling' game.
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
/// Data mass for the data from a single punch in the 'Cycling' game.
/// 
/// One of these entries is made every time the survivor collide with a diamond
/// </summary>

public class CyclingDataMass : DataMass_Base
{
    public int DiamondNum;
    public int score;
    public int RotationNum;
    public float timeInterval;
    public double distanceroute;
    public double diamondgap;

    public DateTime time;

    public CyclingRawDataMass rawData = null;

    public CyclingDataMass(int DiaNum, int points, int Rotation, float Interval, double route, double gap)
    {
        DiamondNum = DiaNum;
        score = points;
        RotationNum = Rotation;
        timeInterval = Interval;
        distanceroute = route;
        diamondgap = gap;


        time = DateTime.Now;

        rawData = new CyclingRawDataMass();
    }
}

/// <summary>
/// Raw Data entry for positional information. Used during the recording of a 'Cycling' game.
/// </summary>
public class CyclingRawDataMass : DataMass_Base
{
    /*public Vector3 WristLeft;
    public Vector3 ElbowLeft;
    public Vector3 ShoulderLeft;
    public Vector3 Chest;
    public Vector3 ShoulderRight;
    public Vector3 ElbowRight;
    public Vector3 WristRight;

    public float ElbowAngle;
    public float ShoulderToWristDistance;
    public float ShoulderToWristAngleHorizontal;
    public float ShoulderToWristAngleVertical;  */

    public DateTime time;

    public CyclingRawDataMass()
    {
        /*WristLeft = GameObject.Find("WristLeft").transform.position;
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
        }*/
    }
}
