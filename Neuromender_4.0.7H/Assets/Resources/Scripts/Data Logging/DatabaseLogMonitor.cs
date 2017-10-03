using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Monitor script for ensuring that the client application has pushed all of its logged data to the external database.
/// 
/// If there is no connection to the database, then the various data loggers should create the local datafile only.
/// They should then also record the name of the file in the registry file that this script reads from when it checks for pending data to send.
/// If there are any entries in the file, then they will be pushed the next time a connection is found, and subsequently removed from the registry file.
/// 
/// Hopefully this avoids and duplicating of data in the external database.
/// 
/// Format for registry file:
/// 
/// [Username] [GameName] [Datafilepath] (relative to the 'Data' folder)
/// ...
/// 
/// </summary>
public class DatabaseLogMonitor : MonoBehaviour {

    // The text file that this script will use to check for pending datafiles to send.
    private string pendingDataRegistry = "";

    // TRUE: There is data that requires transmitting to the external database.
    private bool pendingData = false;
    public bool PENDING_DATA { get { return pendingData; } }    // Public read-only handle for checking if there is any pending data to send to the external database.

    private GameObject DBCons = null;   // Local handle for the data base controller (mainly used for chacking the connection status).

    private List<string> dat = null;    // Container for all of the pending file entries in the registry (if there are any pending files to send).
    private int datIndex = 0;           // Index for the latest file to send to the external database. should only increment when the previous file is successfully sent.

	// Use this for initialization
	void Start () {

        DBCons = GameObject.Find("DatabaseController");

        // Only check for pending data if there is an established connection to the database.
        if(DBCons.GetComponent<database>().PingConnection())
        {
            dat = new List<string>();

            // Check if there are any files that require pushing to the database.
            CheckPendingData();

            // If there is pending data, push it.
            if (pendingData)
                PrepareAllPendingData();
        }
	}


    // Data is sent during the update loop in case there is an issue with the connection to the database DURING the data transfer.
    void Update()
    {
        if(pendingData)
        {
            string nextEntry = dat[datIndex];
            string[] nextEntrySplit = nextEntry.Split(' ');

            if(DBCons.GetComponent<database>().PingConnection())
            {
                PushDataFile(nextEntrySplit[0], nextEntrySplit[1], nextEntrySplit[2]);
                datIndex++;
            }
        }
    }

    /// <summary>
    /// Check if the registry file has any pending entries, and raise a flag if at least one is found.
    /// </summary>
    void CheckPendingData()
    {
        // Check that the file exists before anything else...
        if (File.Exists(Application.dataPath + "Data/" + pendingDataRegistry))
            // If an entry exists in the file, then its length should not be zero.
            if (new FileInfo(Application.dataPath + "Data/" + pendingDataRegistry).Length != 0)
                pendingData = true;
    }

    /// <summary>
    /// Add a new entry to the registry file that this monitor script uses to check for pending data.
    /// </summary>
    /// <param name="Username">The name of the profile associated with this data file.</param>
    /// <param name="Game">The name of the game that this data file has recorded.</param>
    /// <param name="fileName">The name of the file that stores this data.</param>
    public void AddNewPendingEntry(string Username, string Game, string fileName)
    {
        // Check that the file exists before anything else...
        if (File.Exists(Application.dataPath + "Data/" + pendingDataRegistry))
            using (StreamWriter file = File.AppendText(Application.dataPath + "Data/" + pendingDataRegistry))
            {
                file.WriteLine(Username + " " + Game + " " + fileName);

                file.Close();
            }
    }

    /// <summary>
    /// Push the data from a file into the external database.
    /// </summary>
    /// <param name="Username"></param>
    /// <param name="Game"></param>
    /// <param name="fileName"></param>
    void PushDataFile(string Username, string Game, string fileName)
    {
        // Use the appropriate data logger format to push the data to the external database.
        switch(Game)
        {
            case "Wingman":

                break;

            case "Targets":
                
                DataLogger_Targets.PushData(DataLogger_Targets.ReadDataFrom(Username, fileName));

                break;

            case "Rowing":

                break;

            case "Arm Pedalling":

                break;

            case "Leg Pedalling":

                break;

            default:
                // Do nothing...
                break;
        }
    }

    /// <summary>
    /// Push all of the registered entries in the pending data
    /// </summary>
    void PrepareAllPendingData()
    {
        if(File.Exists(Application.dataPath + "Data/" + pendingDataRegistry))
        {
            using (StreamReader file = new StreamReader(Application.dataPath + "Data/" + pendingDataRegistry))
            {
                // Read all of th eentries that are currently in the file.
                while(!file.EndOfStream)
                    dat.Add(file.ReadLine());

                file.Close();
            }

            // Empty all of the entries fro the registry file once everything has been prepared.
            // File.WriteAllText(Application.dataPath + "Data/" + pendingDataRegistry, "");
        }
            
    }
}
