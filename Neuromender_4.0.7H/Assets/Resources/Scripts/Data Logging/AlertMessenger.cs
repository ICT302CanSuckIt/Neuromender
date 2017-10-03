using UnityEngine;
using System;
using System.IO;

/// <summary>
/// Messenger for sending alerts to the database for various situations.
/// </summary>
public static class AlertMessenger{

    /// <summary>
    /// Enumeration for specifying the type of alert that is being sent.
    /// 
    /// Current alerts:
    /// ----------------
    /// 
    /// (0) NULL - Alert has not been specified. It will not be sent.
    /// 
    /// (1) GameNotDone - The game has not been completed by the survivor within the given time limit.
    /// 
    /// (2) 
    /// 
    /// </summary>
    public enum ALERT { NULL = 0, GameNotDone = 1 }

    // Local handle for the database controller.
    private static database db = null;

    // Public read-only handle for the database controller. Finds the database if the current value of this param is null.
    private static database DB { get { if (db == null) db = GameObject.Find("DatabaseController").GetComponent<database>(); return db; } }

    // The file that contains any pending alerts that were recorded during a disconnected session (no database connection).
    private static string alertRegistryFilePath = Application.dataPath + "/Data/AlertRegistry.txt";

    // The format for the fields that are in a NeuroAlert entry in the database.
    private static string fields = "AlertID,ParentID,SubjectID,Date,Seen,Description";

    /// <summary>
    /// Check the registry file for any pending alerts that were recorded. If pending alerts exist, send them to the external database.
    /// </summary>
    public static void SendPendingAlerts()
    {
        // Check that the file exists before continuing.
        if(File.Exists(alertRegistryFilePath))
        {
            using (StreamReader file = new StreamReader(alertRegistryFilePath))
            {
                NeuroAlert newAlert = null;

                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();

                    // Each line in the registry file should be the ToString output of a NeuroAlert. Thus it should be able to fit in the second constructor overload.
                    newAlert = new NeuroAlert(line);

                    // TODO: Send the data to the database. Also check for duplicate entries before pushing new data.
                    GameObject.Find("DatabaseController").GetComponent<database>().SubmitData("", fields, newAlert.ToString());
                }
            }

            // Since all alerts have been sent to the database, clear the registry file.
            File.WriteAllText(alertRegistryFilePath, string.Empty);
        }
    }

    /// <summary>
    /// Send a new alert to the database.
    /// </summary>
    /// <param name="type">The type of alert that is being sent.</param>
    /// <param name="description">A brief description of what the alert is about / for.</param>
	public static void SendAlert(ALERT type, string description = "")
    {
        if(type != ALERT.NULL)
        {
            // Get the current time stamp from the database.
            DateTime dbTimeStamp = DB.getDatabaseTime();

            // Create a new alert to send to the database.
            NeuroAlert newAlert = new NeuroAlert(0, 0, (int)type, dbTimeStamp, description);

            // TODO: Send the data to the database.
            GameObject.Find("DatabaseController").GetComponent<database>().SubmitData("", fields, newAlert.ToString());
        }
    }

    /// <summary>
    /// Sync the values of a DateTime entry with a certain clock.
    /// </summary>
    /// <param name="affected">The DateTime object that is being affected by this function.</param>
    /// <param name="syncClock">The DateTime that is being compared to the client clock before the sync occurs.</param>
    public static void syncTimes(DateTime affected, DateTime syncClock)
    {
        // Calculate the differences between the sync clock and the client's clock for each of the important (recorded) date time fields.
        DateTime serverTime = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();

        int diffYears = syncClock.Year - serverTime.Year;
        int diffMonths = syncClock.Month - serverTime.Month;
        int diffDays = syncClock.Day - serverTime.Day;
        int diffHours = syncClock.Hour - serverTime.Hour;
        int diffMinutes = syncClock.Minute - serverTime.Minute;
        int diffSeconds = syncClock.Second - serverTime.Second;

        // Add the calculated differences to each of the important fields.
        affected.AddYears(diffYears);
        affected.AddMonths(diffMonths);
        affected.AddDays(diffDays);
        affected.AddHours(diffHours);
        affected.AddMinutes(diffMinutes);
        affected.AddSeconds(diffSeconds);
    }
}

/// <summary>
/// Datamass for a single alert to be sent to the external database to notify a clinician / etc.
/// </summary>
[Serializable]
public class NeuroAlert: DataMass_Base
{
    public int alertID;
    public int parentID;
    public int subjectID;
    public DateTime timeStamp;      // Time stamp;
    public bool seen;               // Has this alert been seen? This is not used in the application, but is passed to the database for the field that is on that side.
    public string description = ""; // Description of the alert.

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="a_ID">The ID for the alert.</param>
    /// <param name="p_ID">The ID for the parent (? unsure...)</param>
    /// <param name="s_ID">The ID for the subject of the alert.</param>
    /// <param name="sync">DateTime of the database. In case the client machine has the wrong time or date.</param>
    /// <param name="ExtraInfo">The description of the alert.</param>
    public NeuroAlert(int a_ID, int p_ID, int s_ID, DateTime sync, string ExtraInfo = "")
    {
        alertID = a_ID;
        parentID = p_ID;
        subjectID = s_ID;

        timeStamp = GameObject.Find("DatabaseController").GetComponent<database>().getDatabaseTime();
        AlertMessenger.syncTimes(timeStamp, sync);

        seen = false;

        description = ExtraInfo;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="formattedString">A string formatted to match the output of a NeuroAlert ToString() method call.</param>
    public NeuroAlert(string formattedString)
    {
        string[] data = formattedString.Split(',');

        alertID = Convert.ToInt32(data[0]);
        parentID = Convert.ToInt32(data[1]);
        subjectID = Convert.ToInt32(data[2]);

        timeStamp = DateTime.Parse(data[3]);

        seen = Convert.ToBoolean(data[4]);

        description = data[5];
    }

    /// <summary>
    /// Produce a string format for sending to the external database.
    /// </summary>
    /// <returns>A string formatted to be sent to the external database directly with a MySQL statement.</returns>
    public override string ToString()
    {
        string output = alertID + "," + parentID + "," + subjectID + "," + timeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "," + seen + "," + description;

        return output;
    }
}