using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//what does this script do??
//THERE WAS A TINY MAN

/*
 * the tiny man is under the map is controling the cyclist movement. It uses Kinect to track the user and the tiny man mimicks the users movments.
 * There is a sphere on the mans hand, when that sphere collides with the white screen the bike is pushed forward a little bit. The Man is placed at a distnce
 * from the white screen equal to the users maximum arm extension, which is information reteived from the database.
*/

public class CyclistController : MonoBehaviour
{

    private float speed = 5f;
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
    int targetIndex = 1; //the node you're moving towards
    public int numnode; // number of nodes
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

        //locking the cursor
        Cursor.lockState = CursorLockMode.Locked;

        //Max time to get the diamond
        DiamondGapTime = 6.0f;                 //from the web

        transform.position = path.nodes[0];
        futurePosition = path.nodes[1];

        userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        _loginControl = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        userConfig.config.CyclingGameNum += 1;

        datalogger = GameObject.Find("DataLogger").GetComponent<DataLogger_Cycling>();

        Database = GameObject.Find("DatabaseController").GetComponent<database>();

    }

    // Update is called once per frame
    void Update()
    {
		Debug.Log ("velocity = " + velocity);
        if (CarIsFinished == false)
        {
			
            DiamondGapTime -= Time.deltaTime;

            if (colRPM.GetComponent<colliderRPM>().counter > 0)// || (Input.GetKey(KeyCode.UpArrow)))
            {
                Bike.GetComponent<Timer>().enabled = true;


				if ((transform.position - path.nodes[targetIndex]).magnitude < (velocity * speed * Time.deltaTime).magnitude) //keeps you oo track  this oneeeeeee
                {
					//if your next movment will put you past the target position, this sets your position to the target position and chnaged the target position to the next node
                    transform.position = path.nodes[targetIndex];
                    futurePosition = path.nodes[targetIndex + 1];           //position of the view
                    targetIndex++;
                    if (targetIndex == numnode)
                    {
                        CarIsFinished = true;
                        return;
                    }
					velocity = (path.nodes [targetIndex] - path.nodes [targetIndex - 1]).normalized;
              	}else {
					transform.position += velocity * speed * Time.deltaTime;
                    

                    Vector3 relativePos = futurePosition - transform.position;

                    Quaternion wantedRotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * 1.1f); //make sure bike is always facing the next node
					//Debug.Log ("Moving at 5.5");
                 //   Debug.Log("targetIndex :" + targetIndex);

                    // Calcualting distance
                    distance = Vector3.Distance(Bike.transform.position, path.nodes[targetIndex]);

                    Tdistance = distance + Distanceupdate(targetIndex, numnode);

                  //  Debug.Log("distance is " + distance + "Plus " + Distanceupdate(targetIndex, numnode) + " equal " + Tdistance + "nodes is" + numnode);

                    DistanceText.text = Tdistance.ToString("f0") + "M";

                    // Debug.Log("distance :" + Tdistance); //caculate the distance -RR
                //    Debug.Log(colRPM.GetComponent<colliderRPM>().counter);

					if (colRPM.GetComponent<colliderRPM> ().counter > NewCounter) {
						velocity = (path.nodes [targetIndex] - path.nodes [targetIndex - 1]).normalized;// * 10.5f;
						speed = 7.5f;
						Debug.Log ("Moving at 7.5");
						NewCounter++;
					} else {
						if (speed > 1.0f)
							speed = speed - 0.2f;
					}

				}  // calcualting distance
                

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

        return (dist * 2);

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
