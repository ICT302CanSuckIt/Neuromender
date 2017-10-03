using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

/// <summary>
/// Data logger for the 'Arm Pedalling' game.
/// </summary>
public class DataLogger_ArmPedalling : DataLogger_Base {

	// Use this for initialization
	new void Start () {

        base.Start();

	}

    /// <summary>
    /// Implementation of DataLoggerBase's SerialiseData function. Serialise data for a session of the 'Wingman' game locally.
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
    /// Implementation of DataLoggerBase's PushData function. Push data for a session of the 'Wingman' game to the external database.
    /// </summary>
    public override void PushData()
    {

    }

    public static void PushData(Dictionary<string, object> data)
    {

    }
}

/// <summary>
/// Data mass for the data from a single punch in the 'Arm Pedalling' game.
/// 
/// Not sure when one of these will be made (that is up to you :D ).
/// </summary>
public class ArmPedalDataMass : DataMass_Base
{
    public DateTime time;

    public ArmPedalDataMass()
    {
        time = DateTime.Now;
    }
}

/// <summary>
/// Raw Data entry for positional information. Used during the recording of a 'Arm Pedalling' game.
/// </summary>
public class ArmPedalRawDataMass : DataMass_Base
{
    public DateTime time;

    public ArmPedalRawDataMass()
    {
        time = DateTime.Now;
    }
}