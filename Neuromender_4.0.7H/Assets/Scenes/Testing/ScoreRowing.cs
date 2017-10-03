using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ScoreRowing : MonoBehaviour
{
    public bool Racing = false;

    public float RaceTime = 0;
    public Text txtRaceTime;

    public float Distance = 0;
	public  Text txtDistance;
    public GameObject Boat;


    // Use this for initialization
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Racing)
        {
            RaceTime += Time.deltaTime;


            //Debug.Log(RaceTime.ToString("mm:ss"));

        }


        if (txtRaceTime)
        {
            txtRaceTime.text = String.Format("{0:0}:{1:00}", Mathf.Floor(RaceTime / 60), RaceTime % 60);
        }

        if (txtDistance)
        {
			Distance = Boat.transform.position.z;
			Distance = Mathf.Round (Distance * 10f) / 10f;
			txtDistance.text = "" + (Distance) + " m";
        }
        //Debug.Log(RaceTime);
    }

    public void StartRace()
    {
        if (!Racing)
        {
            Racing = true;
            Boat.GetComponent<BoatControl>().Racing = true;
        }

        Boat.GetComponent<BoatControl>().Calibrate();
    }
}
