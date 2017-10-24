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

    private Vector3 previousPosition;
    public float totalDist;
    
    // Use this for initialization
    void Start()
    {
        Boat = GameObject.Find("Boat");
        previousPosition = Boat.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //total distance the boat has traveled
        totalDist += (Boat.transform.position - previousPosition).magnitude;
        previousPosition = Boat.transform.position;

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
			Distance = totalDist;
			Distance = Mathf.Round (Distance * 10f) / 10f;
			txtDistance.text = "" + (Distance) + " m";
        }

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
