using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDrills : MonoBehaviour
{
    public int ShotsTotal = 20;
    public int ShotCurrent = 1;
    public Text ShotTracker;

    private string State;
    public Text txtState;

    private float Accuracy;
    public Text txtAccuracy;
    private float Reach;
    public Text txtReach;

    private float LastAcc;
    public Text txtLastAcc;
    private float LastReach;
    public Text txtLastReach;

    private float BestAcc;
    public Text txtBestAcc;
    private float BestReach;
    public Text txtBestReach;

    public ProximityRing pr;

    public GameObject DBcons;

    void OnLevelWasLoaded()
    {
        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");

            int reps = DBcons.GetComponent<LoginControl>().config.Repititions;
            if (reps <= 0) reps = 20;
            ShotsTotal = reps;
        }
        else
        {
            // use default values
        }
    }




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShotCurrent = pr.ShotNum;

        if (txtState)
        {
            if (pr.Reaching)
            {
                txtState.text = "REACH";
            }
            else
            {
                txtState.text = "RETURN";
            }
        }

        if (ShotTracker)
        {
            ShotTracker.text = ShotCurrent + " / " + ShotsTotal;
        }

        if (txtAccuracy)
        {
            txtAccuracy.text = "" + ( int )(pr.Accuracy * 100);
        }

        if (txtReach)
        {
            txtReach.text = "" + ( int )(pr.Reach * 100);
        }

        if (txtLastAcc)
        {
            txtLastAcc.text = "" + ( int )(pr.LastAcc * 100);
        }

        if (txtLastReach)
        {
            txtLastReach.text = "" + ( int )(pr.LastReach * 100);
        }

        if (txtBestAcc)
        {
            txtBestAcc.text = "" + ( int )(pr.BestAcc * 100);
        }

        if (txtBestReach)
        {
            txtBestReach.text = "" + ( int )(pr.BestReach * 100);
        }
    }
}
