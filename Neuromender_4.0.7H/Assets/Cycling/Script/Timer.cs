using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{

    public Text timerText;
    public Text FinishTimeText;
    private float startTime;
    public GameObject Vespa;

    bool keepTiming;
    public float timer;
    public float TimeSec;

    public void Start()
    {
        
            StartTimer();
        
    }

    void Update()
    {

       
            
            if (Vespa.GetComponent<CyclistController>().CarIsFinished == true)
            {
                Debug.Log("Timer stopped at " + TimeToString(StopTimer()));
                TimeSec = StopTimer();
                Debug.Log("Timer " + TimeSec);
            }

            if (keepTiming)
            {
                UpdateTime();
            }
        
    }

    void UpdateTime()               //increase time
    {
        timer = Time.time - startTime;
        timerText.text = TimeToString(timer);
        FinishTimeText.text = TimeToString(timer);
    }

   /* float GetTime(float Tmr)               //increase time
    {
         return minutes;
    }*/

    float StopTimer()               //stop the time
    {
        keepTiming = false;
        return timer;
    }

    void ResumeTimer()              //resume the time
    {
        keepTiming = true;
        startTime = Time.time - timer;
    }

    public void StartTimer()               //starting the time
    {
        keepTiming = true;
        startTime = Time.time;
    }

    string TimeToString(float t)                //calculaion for time
    {
        string minutes = ((int)t / 60).ToString();          //minutes
        string seconds = (t % 60).ToString("f2");           //seconds
        return minutes + ":" + seconds;                     //return the time for it to be display
    }

}
