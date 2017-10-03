using UnityEngine;
using System.Collections;

public class Calibrator : MonoBehaviour
{
    public GameObject SpheroHead;

    public ProximityRing pr;
    public Camera prCam;
    public ScoreRowing sr;

    // Use this for initialization
    void Start()
    {
        SpheroHead = GameObject.Find("Head");
    }

    // Update is called once per frame
    void Update()
    {
        if(!SpheroHead)
        {
            SpheroHead = GameObject.Find("Head");
        }

        this.transform.position = SpheroHead.transform.position + Vector3.up * 0.8f;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "GloveGood")
        {
            Calibrate();
        }
    }

    public void Calibrate()
    {
        if (pr) pr.GetComponent<ProximityRing>().Calibrate();

        if (prCam)
        {
            prCam.transform.position = pr.GetComponent<ProximityRing>().transform.position + Vector3.back*0.5f;
        }

        if(sr)
        {
            sr.StartRace();
        }
    }
}
