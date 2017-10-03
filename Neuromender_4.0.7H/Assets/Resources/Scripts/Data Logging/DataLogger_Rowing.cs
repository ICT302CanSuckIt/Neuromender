using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

/// <summary>
/// Data logger for the 'Rowing' game.
/// </summary>
public class DataLogger_Rowing : DataLogger_Base {

	// Use this for initialization
	new void Start () {

        base.Start();

	}

    /// <summary>
    /// Implementation of DataLoggerBase's SerialiseData function. Serialise data for a session of the 'Rowing' game locally.
    /// </summary>
    public override void SerialiseData()
    {
        base.SerialiseData();

        // Leave immediately if there is no specified user or no specified tracked game.
        if (user != "" && loggedGame != "")
        {
            Debug.Log("Serialising!");


        }
    }

    /// <summary>
    /// Implementation of DataLoggerBase's PushData function. Push data for a session of the 'Rowing' game to the external database.
    /// </summary>
    public override void PushData()
    {

    }

    public static void PushData(Dictionary<string, object> data)
    {

    }
}

/// <summary>
/// Data mass for the data from a single punch in the 'Rowing' game.
/// 
/// Not sure when one of these will be made (that is up to you :D ).
/// </summary>
public class RowingDataMass : DataMass_Base
{
    public DateTime time;

    public RowingDataMass()
    {
        time = DateTime.Now;
    }
}

/// <summary>
/// Raw Data entry for positional information. Used during the recording of a 'Rowing' game.
/// </summary>
public class RowingRawDataMass : DataMass_Base
{
    public DateTime time;

    public RowingRawDataMass()
    {
        time = DateTime.Now;
    }
}