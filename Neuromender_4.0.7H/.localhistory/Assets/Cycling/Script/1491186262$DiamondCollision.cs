﻿using UnityEngine;
using System.Collections;

public class DiamondCollision : MonoBehaviour
{
    public GameObject Bike;
    public int timeWeb;
    public int diamondNo;
    public int DiaScore;
    public GameObject colRPM;
    public GameObject prevDia;
    public GameObject race;
    public int timecount;
    public int prevdiarot;
    public int diarot;
    public float timesecond;
    public int numrotation;
    public double route;
    public double gap;
    public bool hit = false;
    bool diahit = false;

    public float curtime;
    public int currot;
    public int sco;


    public GameObject Cube;
    public GameObject single;
    public GameObject pear;
    public GameObject rose;
    public GameObject round;
    



    DataLogger_Cycling logger = null;           // Data logger that tracks and records the performance of this glove.


    void Start()
    {
        timeWeb = 60;
        if (!logger)
        {
            logger = GameObject.Find("DataLogger").GetComponent<DataLogger_Cycling>();
        }

    }




    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bike" && diahit == false)              // If the Diamond collides with the bike
        {
            hit = true;
            Destroy(single);
            Destroy(pear);
            Destroy(rose);
            Destroy(round);
            Debug.Log("Collision");


            curtime = Cube.GetComponent<Timer>().timer;
            timesecond = curtime - prevDia.GetComponent<DiamondCollision>().curtime;


            if (timesecond < 4)   //lower the time taken,the higher the score.
            {
                Debug.Log("time 10 Dia" + diamondNo);
                Debug.Log("Real :" + Bike.GetComponent<CyclistController>().DiamondGapTime);

                CollideScore(10);
                DiaScore = 10;
            }
            else if (timesecond < 6)
            {
                Debug.Log("time 8 Dia" + diamondNo);
                Debug.Log("Real :" + Bike.GetComponent<CyclistController>().DiamondGapTime);

                CollideScore(8);
                DiaScore = 8;
            }
            else if (timesecond < 8)
            {
                Debug.Log("time 6 Dia" + diamondNo);
                Debug.Log("Real :" + Bike.GetComponent<CyclistController>().DiamondGapTime);

                CollideScore(6);
                DiaScore = 6;
            }
            else if (timesecond < 10)
            {
                Debug.Log("time 4 Dia" + diamondNo);
                Debug.Log("Real :" + Bike.GetComponent<CyclistController>().DiamondGapTime);

                CollideScore(4);
                DiaScore = 4;
            }
            else if (timesecond < 12)
            {
                Debug.Log("time 2 Dia" + diamondNo);
                Debug.Log("Real :" + Bike.GetComponent<CyclistController>().DiamondGapTime);

                CollideScore(3);
                DiaScore = 3;
            }
            else
            {
                CollideScore(2);
                DiaScore = 2;
            }

            /*
            sco = 20 - (int)timesecond;
            CollideScore(sco);
                DiaScore = sco;
                 */
            diahit = true;

            ResetDiamondTime();

            Debug.Log(" Dia " + diamondNo);



            currot = colRPM.GetComponent<colliderRPM>().counter;

            numrotation = currot - prevDia.GetComponent<DiamondCollision>().numrotation;
           
            gap = Cube.GetComponent<CyclistController>().Ndistance;

            Debug.Log("Curent timer " + Cube.GetComponent<Timer>().timer + "minuse before timer " + prevDia.GetComponent<DiamondCollision>().curtime);


            Debug.Log("timesecond  " + timesecond);


            Debug.Log("Curent RPM " + colRPM.GetComponent<colliderRPM>().counter + "minuse before RPM " + prevDia.GetComponent<DiamondCollision>().currot);


            Debug.Log("numrotation  " + numrotation);


            logging();

        }
    }

    public void CollideScore(int score)
    {
        Bike.GetComponent<DiamondScore>().UpdateScore(score);               //Update the score 
    }

    public void ResetDiamondTime()
    {
        Bike.GetComponent<CyclistController>().DiamondGapTime = 6.0f;       //reset to the time that have been set bt the clinician
                                                                            // Bike.GetComponent<CyclistController>().speed = 0f;                  // reset the speed before the acceleration
    }

    private void logging()
    {



        if (logger != null)
        {
            // Make an entry in the data logger for the performance information of the lastest  / extension.

            CyclingDataMass dataEntry = new CyclingDataMass(diamondNo, DiaScore, numrotation, timesecond, route, gap);
            logger.DATA.Add("DIAMONDS_" + logger.DiamondNo, dataEntry);
            logger.DiamondNo++;

        }




    }



    void Update()
    {




    }


}