using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultDisplay : MonoBehaviour {

    public Text BikeSpeed;
    public Text BikeRPM;
    public GameObject Cube;
    public GameObject colRPM;

    public float Distance = 200 ;        //Web
    public float Time;
    public float Rotation;
    public float Speed;
    public float RPM;

    private LoginControl userConfig;
    private LoginControl _loginControl;

    private DataLogger_Cycling datalogger = null;

    database Database;



    // Use this for initialization
    void Start()
    {
        //  BikeSpeed = GetComponent<Text>();
        // BikeRPM = GetComponent<Text>();

        //Rotation = colRPM.GetComponent<colliderRPM>().counter;
        // Time = Cube.GetComponent<Timer>().TimeSec;
        //note : Dont reassigned the variable once called. 

        userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        _loginControl = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
       // userConfig.config.CyclingGameNum += 1;

        datalogger = GameObject.Find("DataLogger").GetComponent<DataLogger_Cycling>();

        Database = GameObject.Find("DatabaseController").GetComponent<database>();


    }
    void Update()
    {

       
   
        if ((Input.GetKeyDown("escape")))
        {
            userConfig.gamesPlayed.CyclingPlayed = true;
            DisplayEndPanel();
            LogData();
        }
        if (Cube.GetComponent<CyclistController>().CarIsFinished == true)
        {
            userConfig.gamesPlayed.CyclingPlayed = true;
            DisplayEndPanel();
            LogData();
        }


    }

    public void DisplayEndPanel()
    {
        //Calculation for speed and RPM
        userConfig.gamesPlayed.CyclingPlayed = true;

        Speed = Distance / Cube.GetComponent<Timer>().TimeSec;
        RPM = colRPM.GetComponent<colliderRPM>().counter / Cube.GetComponent<Timer>().TimeSec;

        // set end panel txts
        BikeSpeed.text = Speed.ToString("f2");
        BikeRPM.text = RPM.ToString("f2");

        Debug.Log("Time " + Cube.GetComponent<Timer>().TimeSec);
        Debug.Log("Speed " + Speed);
        Debug.Log("Rotation " + colRPM.GetComponent<colliderRPM>().counter);
        Debug.Log("RPM " + RPM);

       // LogData();

    }

    public void restartLevel()
    {
        if (_loginControl.PlaytimeLimitReached("Cycling"))
            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("Cycling Test3");
    }

    public void returnToLevelSelect()
    {
        if (_loginControl.PlaytimeLimitReached("Cycling"))
            UnityEngine.SceneManagement.SceneManager.LoadScene("LimitAwareness");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        if (GameObject.Find("DatabaseController"))
        {
            GameObject.Find("DatabaseController").GetComponent<LoginControl>().logoutSetAverage();
            GameObject.Destroy(GameObject.Find("DatabaseController"));
        }
        SceneManager.LoadScene("Login");
    }



    


    private void LogData()
    {
        userConfig.gamesPlayed.CyclingPlayed = true;

        // Serialise the data from this session locally.
        if (GameObject.Find("DataLogger"))
        {
            if (datalogger)
            {
                datalogger.SerialiseData();
                userConfig.gamesPlayed.CyclingPlayed = true;
            }
            else
                Debug.Log("Cannot find data logger script!");
        }
        else
            Debug.Log("Cannot find data logger object!");
    }


}


























