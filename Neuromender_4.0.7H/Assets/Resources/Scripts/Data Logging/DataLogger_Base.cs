using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

/// <summary>
/// 
/// Base class for data logging. All Data logging child classes should need / use the abstract methods in this class.
/// 
/// Note: Example implementation in 'DataLogger_Targets.cs'.
/// 
/// </summary>
public abstract class DataLogger_Base : MonoBehaviour {

    [Tooltip("The game that is being logged. Affects final binary data file name. REQUIRED to be filled for data logging to occur.")]
    public string loggedGame = "";

    [Tooltip("The user profile associated with this performance data. Dictates directory created for all related data. REQUIRED to be filled for data logging to occur.")]
    public string user = "";

    [Tooltip("Counter used to seperate performance logs for multiple plays of the same game in the same day. Affects final binary data file name.")]
    public int dailyRepetitionCounter = 0;

    private StreamWriter stream = null;                                             // FileStream for serialisation / deserialisation of external data files.
    public StreamWriter STREAM { get { return stream; } set { stream = value; } }   // Public read-only handle for the binary formatter.

    private Dictionary<string, object> data = new Dictionary<string, object>(); // Data container for all tracked data. Should support all types of data because of the 'object' value type.
    public Dictionary<string, object> DATA { get { return data; } }             // Public read-only handle for the container of the tracked data.

    public GameObject DBCons = null;
    protected LoginControl login = null;
    protected database DB = null;
    protected DatabaseLogMonitor monitor = null;

    protected DateTime NOW;

    // Use this for initialization
    protected void Start () {

        if (DBCons == null)
            DBCons = GameObject.Find("DatabaseController");

        if (loggedGame == "" || DBCons == null)
        {
            enabled = false;
            Debug.Log("Data logging disabled in this scene because either 'loggedGame' or 'user' param was not set.OR no database was found. Script attached to " + gameObject.name +  " was disabled.");
        }
        else
        {
            login = DBCons.GetComponent<LoginControl>();
            DB = DBCons.GetComponent<database>();
            monitor = DBCons.GetComponent<DatabaseLogMonitor>();

            DATA.Add("Game", loggedGame);

            DATA.Add("User", login.NAME);
        }

	}

    /// <summary>
    /// Compile all of the recorded data into a local binary file.
    /// 
    /// Note: This method is virtual because the way variables are serialised may change depending on the game.
    /// Note: This particular method is not abstract because there is some shared functionality that all child classes should have (see below).
    /// </summary>
    public virtual void SerialiseData()
    {
        // Tell the database that number of games that have been played has increased.
        login.logoutSetAverage();

        NOW = DB.getDatabaseTime();

        // Create the necessary directories if they don't already exist.
        if (!System.IO.Directory.Exists(Application.dataPath + "/Data"))
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Data/");

        if (!System.IO.Directory.Exists(Application.dataPath + "/Data/" + user))
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Data/" + user + "/");
    }

    /// <summary>
    /// Push the recorded data to the external database.
    /// 
    /// Note: This method is abstract because there may be different variables serialised to the database depending on the game.
    /// 
    /// Note: This method should handle (without application-closing error) a situation where the database cannot be reached.
    /// </summary>
    public abstract void PushData();

    /// <summary>
    /// Convenient function for writing the basic information from the datalogger into the open data file.
    /// </summary>
    protected void WriteBasicInfo()
    {
        if(stream != null)
        {
            // Basic Information.
            STREAM.WriteLine("Session: " + login.config.SessionID + ",Game:," + DATA["Game"] + ",User:," + DATA["User"]);
            STREAM.WriteLine("Game No.:," + DATA["GameNum"]);
            STREAM.WriteLine("Day:," + NOW.Day + ",Month:," + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(NOW.Month) + ",Year:," + NOW.Year);
        }
    }
}

/// <summary>
/// An empty class used as the base class for all types of data logger containers. (Example in 'DataLogger_Targets.cs').
/// 
/// The only variables that should be in this class are common settings like any relevant patient data (afflicted side, daily repetition number, etc.)
/// </summary>
[System.Serializable]
public abstract class DataMass_Base
{

}