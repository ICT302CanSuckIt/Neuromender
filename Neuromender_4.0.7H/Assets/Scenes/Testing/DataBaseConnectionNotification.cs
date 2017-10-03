using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class DataBaseConnectionNotification : MonoBehaviour {

    [Tooltip("The object that will be displayed when the database is connected / disconnected.")]
    public GameObject notificationObject = null;

    [Tooltip("TRUE: Show the notification when database IS connected.\nFALSE: Show notification when there is NO connection.")]
    public bool showWhenConnected = false;

    [Tooltip("The database object whos connection status is being queried.")]
    public database DB = null;

	// Use this for initialization
	void Start () {

        // Disable this script if there is no notification attached to it.
        if (notificationObject == null)
            notificationObject = gameObject;

        if (DB == null)
            DB = GameObject.Find("DatabaseController").GetComponent<database>();

        // If we still can't find the database in the scene, disable this script.
        if (DB == null)
            enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (showWhenConnected == DB.PingConnection())
            notificationObject.SetActive(true);
        else
            notificationObject.SetActive(false);

	}
}
