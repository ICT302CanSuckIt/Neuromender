using UnityEngine;
using System.Collections;


public class DistanceController : MonoBehaviour
{

    public ringController[] Rings;
    private int totalRings;
    public int ringCount = 0;
    public ringController ClosestRing;
    public float DistanceToNextRing;
    private HeadsUpDisplay UIController;
    public float XZDist;
    public float DistanceToLastRing;
    private StrokeRehabLevelController levelController;

    // Use this for initialization
    void Start()
    {
        Rings = this.GetComponentsInChildren<ringController>();
        totalRings = Rings.Length;
        Debug.Log("Rings.Length: " + Rings.Length);
        ClosestRing = Rings[ringCount];
        UIController = GameObject.Find("HUD").GetComponent<HeadsUpDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
       // if (totalRings-ringCount > 0)
        //{
            UIController.ringsToGo = totalRings - ringCount;
            UIController.SetDistanceToNext(XZDist);
            UIController.SetDistanceToLast(DistanceToLastRing);
            if (ringCount > 0)
                DistanceToLastRing = Rings[ringCount - 1].xzDist;
            if (ringCount < totalRings)
            {
                XZDist = Rings[ringCount].xzDist;

                DistanceToNextRing = Rings[ringCount].distanceToPlayer;
                //print("Ring (" + ringCount + ") Distance = " + Rings[ringCount].distanceToPlayer);
                if (( int )XZDist < 1)
                {
                    print("Ring (" + ringCount + ") Distance = " + Rings[ringCount].xzDist);
                    ringCount++;

                // Start logging data once a punch has been done.
                Camera.main.GetComponent<Q4Quit>().logData = true;

                // UIController.loaded = false;

            }

            }
       // }
    }
}
