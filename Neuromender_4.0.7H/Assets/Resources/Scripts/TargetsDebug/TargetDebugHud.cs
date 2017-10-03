using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

public class TargetDebugHud : MonoBehaviour {

    public LoginControl userConfig;

    public GameObject kinectPanel;
    public GameObject debugGrid;
    public GameObject debugDisplay;

    // Use this for initialization
    void Start () {

        kinectPanel = GameObject.Find("KinectSkeletonCamera");
        debugGrid = GameObject.Find("DebugGrid");
        debugDisplay = GameObject.Find("DebugDisplay");

        userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();

       // Set the debug display object to hidden by default.
        kinectPanel.SetActive(false);
        debugGrid.SetActive(false);
        debugDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle the debug display with `.
        if (Input.GetKeyDown(KeyCode.BackQuote))
            DebugHUD();
    }

    /// <summary>
    /// Toggle the debug HUD on or off.
    /// </summary>
    public void DebugHUD()
    {
        userConfig.config.showDebug = !userConfig.config.showDebug;
        
        if(userConfig.config.showDebug)
        {
            kinectPanel.SetActive(true);
            debugGrid.SetActive(true);
            debugDisplay.SetActive(true);
        }
        else
        {
            kinectPanel.SetActive(false);
            debugGrid.SetActive(false);
            debugDisplay.SetActive(false);
        }
    }
}
