using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CyclistController : MonoBehaviour
{

    private float speed = 1f;
    public int NewCounter = 0;
    public float DiamondGapTime;
    public float MaxTime;
    public bool CarIsFinished = false;
    public GameObject ResultDisplayBox;

    public Text DistanceText; // -RR
    public double distance; // -RR
    public double Tdistance;// -RR
    public double NTdistance;// -RR
    public double Ndistance;// -RR
    public GameObject Bike;// -RR
    public GameObject colRPM;
    public bool move = false;

    public Polyline path;
    int targetIndex = 1;
    public int numnode; // number of node
    Vector3 velocity;
    Vector3 futurePosition;


    private LoginControl userConfig;
    private LoginControl _loginControl;
    database Database;

    private DataLogger_Cycling datalogger = null;


    // Use this for initialization
    void Start()
    {





        DistanceText = GameObject.FindGameObjectWithTag("DistanceLeft").GetComponent<Text>(); //Distance text -RR
        Bike = GameObject.FindGameObjectWithTag("Bike"); // -RR
        Tdistance = 100; //39362

        //locking the cursor
        Cursor.lockState = CursorLockMode.Locked;

        //Max time to get the diamond
        DiamondGapTime = 6.0f;                 //from the web

        // MaxTime = DiamondGapTime * 11 ;
        transform.position = path.nodes[0];
        futurePosition = path.nodes[1];
        velocity = (path.nodes[targetIndex] - path.nodes[targetIndex - 1]).normalized * speed;




        userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        _loginControl = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        userConfig.config.CyclingGameNum += 1;

        datalogger = GameObject.Find("DataLogger").GetComponent<DataLogger_Cycling>();

        Database = GameObject.Find("DatabaseController").GetComponent<database>();

    }

    // Update is called once per frame
    void Update()
    {

        if (CarIsFinished == false)
        {
            //transform.Translate(speed * Time.deltaTime, 0, 0);

            DiamondGapTime -= Time.deltaTime;

            //   if (colRPM.GetComponent<colliderRPM>().counter > NewCounter)                   //(Input.GetKey(KeyCode.UpArrow))
            //    {
            // speed = 20f;
            // transform.Translate(speed * Input.GetAxis("Vertical")* Time.deltaTime, 0, 0);
            //  Debug.Log("Accelerate pressed ");



            if (colRPM.GetComponent<colliderRPM>().counter > 0) //|| (Input.GetKey(KeyCode.UpArrow))
            {
                Bike.GetComponent<Timer>().enabled = true;



                if ((transform.position - path.nodes[targetIndex]).magnitude < (velocity * Time.deltaTime).magnitude)
                {
                    transform.position = path.nodes[targetIndex];
                    futurePosition = path.nodes[targetIndex + 1];           //position of the view
                    targetIndex++;
                    if (targetIndex == numnode)
                    {
                        CarIsFinished = true;
                        return;
                    }
                    velocity = (path.nodes[targetIndex] - path.nodes[targetIndex - 1]).normalized * speed;
                }
                else
                {
                    transform.position += velocity * Time.deltaTime;
                    //transform.LookAt(futurePosition);               //view of the object
                    //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(futurePosition), Time.deltaTime * 2);

                    Vector3 relativePos = futurePosition - transform.position;

                    Quaternion wantedRotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * 1.5f);
                    Debug.Log("targetIndex :" + targetIndex);


                    // Calcualting distance
                    distance = Vector3.Distance(Bike.transform.position, path.nodes[targetIndex]);

                    Tdistance = distance + Distanceupdate(targetIndex, numnode);

                 

                    DistanceText.text = Tdistance.ToString("f0") + "M";

                    // Debug.Log("distance :" + Tdistance); //caculate the distance -RR
                    Debug.Log(colRPM.GetComponent<colliderRPM>().counter);

                    if (colRPM.GetComponent<colliderRPM>().counter > NewCounter )
                    {
                        velocity = (path.nodes[targetIndex] - path.nodes[targetIndex - 1]).normalized * 2.5f;
                        NewCounter++;
                    }

                    // calcualting distance
                }

                // NewCounter++;
                // }
            }
        }
        else
        {
            ResultDisplayBox.SetActive(true);
            transform.Translate(0, 0, 0);       //stopping the car
        }

        //for cursor to appear
        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
    }


    //Once it hit something it will do the task that have been assigned.
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "FinishLine200")              //Finish Line 200 statement
        {
            Debug.Log("You crossed the finish line");
            userConfig.gamesPlayed.CyclingPlayed = true;
            CarIsFinished = true;
            LogData();
        }
    }

    public double Distanceupdate(int k, int nodes)
    {
        double dist = 0;

        nodes = nodes - 1;

        for (int j = k; j <= nodes; j++)
        {
            Ndistance = Vector3.Distance(path.nodes[j], path.nodes[j + 1]);
            dist = dist + Ndistance;
            // Debug.Log("NTdistance :" + dist); //caculate the distance -RR

        }

        return (dist * 2) - 5;

    }


    private void LogData()
    {
        // Serialise the data from this session locally.
        if (GameObject.Find("DataLogger"))
        {
            if (GameObject.Find("DataLogger").GetComponent<DataLogger_Cycling>())
                GameObject.Find("DataLogger").GetComponent<DataLogger_Cycling>().SerialiseData();
            else
                Debug.Log("Cannot find data logger script!");
        }
        else
            Debug.Log("Cannot find data logger object!");
    }

}
