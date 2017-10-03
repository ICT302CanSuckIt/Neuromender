using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GloveRelative : Glove_Base
{
    public enum Handed { HandRight, HandLeft }
    public Handed Hand = Handed.HandRight;

    //public GameObject Joint;
    public GameObject RelativeJoint;
    //public Rigidbody rb;

    public float Latency = 0;
    public float RetractLatency = 0;
    public float Reach;
    public bool Assisted;

    private float rdsTimer = 0.0f;
    private float rdsTime = 3.0f;

    DataLogger_Targets logger = null;           // Data logger that tracks and records the performance of this glove.

    public Camera cam;// = Camera.current;

    //public float RelPosX;
    //public float RelPosY;
    //public float ReachDist;
    public Vector3 HomePosition;
    public Vector3 RelativePosition;

    public float ArmResetDistance;
    public bool Loaded = false;
    public bool startLogging = false;       // Since Reload() is called at initialisation, stop it from logging a useless entry in the data logger with this flag.

    public GameObject DBcons;
    public GameObject Score;
    public GameObject ReloadTarget;

    private bool wasAHit = false;          // True: The latest punch successfully hit the target. FALSE: The latest punch missed.
    private Vector2 hitPos;

    public int CountDownTimer;
    private bool countdownActive;

    void OnLevelWasLoaded()
    {
        if (GameObject.Find("DatabaseController"))
        {
            DBcons = GameObject.Find("DatabaseController");
            ArmResetDistance = (float)DBcons.GetComponent<LoginControl>().config.ArmResetDistance/1000f;
            // Depending on the hand that is affected, change the wrist and shoulder that are being tracked.
            if (DBcons.GetComponent<LoginControl>().config.sideAffected == 1)
            {
                Hand = Handed.HandLeft;
            }
            else if (DBcons.GetComponent<LoginControl>().config.sideAffected == 2)
            {
                Hand = Handed.HandRight;
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        HomePosition = transform.position;
        //Joint = GameObject.Find(Hand.ToString());
        //GetShoulder();

        rb = GetComponent<Rigidbody>();

        if (!Score)
        {
            Score = GameObject.Find("ScoreBoard");
        }

        if(!logger)
        {
            logger = GameObject.Find("DataLogger").GetComponent<DataLogger_Targets>();
        }
        StartCoroutine(CountDownStart());
        Unload();
    }

    // Update is called once per frame
    void Update()
    {          
        if (!Score.GetComponent<ScoreCarnival>().Finished)
        {
            rdsTimer -= Time.deltaTime;
            if (rdsTimer <= 0)
            {
                rdsTimer = rdsTime;
                Score.GetComponent<ScoreCarnival>().SendRawData();
            }
        }

        if (Loaded)
        {
            Latency += Time.deltaTime;
        }
        else
        {
            RetractLatency += Time.deltaTime;   // Assuming that the 'Loaded' flag is only lowered during arm RETRACTION.
        }

        if (!keyboardControlOverride)
        {
            if (!Joint)
            {
                Joint = GameObject.Find(Hand.ToString());
                GetShoulder();
            }
            else
            {
                // calc relative position
                RelativePosition = Joint.transform.position - RelativeJoint.transform.position;
                
                // adjust z value to equal magnitude to solve spherical rotation distance to planar
                Reach = RelativePosition.magnitude;
                RelativePosition.z = Reach;

                Score.GetComponent<ScoreCarnival>().MinMaxReach(Reach);

                rb.MovePosition(HomePosition + RelativePosition);

                // Change the Z depth of the glove to be the distance between the affected wrist and affected shoulder.
                //transform.position = new Vector3(transform.position.x, transform.position.y, Reach);

                if (!Loaded && Joint.transform.position.z < RelativeJoint.transform.position.z + ArmResetDistance && !countdownActive)
                {
                    Reload();
                    startLogging = true;
                }/*
                if (!Loaded && Reach <= ReloadDistance)
                {
                    Reload();
                    startLogging = true;
                }*/
            }
        }
        else
        if (!Loaded && transform.localPosition.z <= ArmResetDistance)
        {
            Reload();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }

    private void GetShoulder()
    {
        float midPoint = cam.WorldToScreenPoint(new Vector3(100, 0f, 1)).y;
        GameObject grid = GameObject.Find("Grid");

        if (Hand == Handed.HandLeft)
        {
            RelativeJoint = GameObject.Find("ShoulderLeft");
            //cam.transform.position = cam.ScreenToWorldPoint(new Vector3(Screen.width - midPoint - 1, midPoint, 0));
            
            cam.transform.position = new Vector3(grid.transform.position.x + 0.2f, grid.transform.position.y, cam.transform.position.z);
            Score.GetComponent<ScoreCarnival>().RightText();
        }
        else
        {
            RelativeJoint = GameObject.Find("ShoulderRight");
            //cam.transform.position = cam.ScreenToWorldPoint(new Vector3(midPoint + 1, midPoint, 0));

            cam.transform.position = new Vector3(grid.transform.position.x - 0.2f, grid.transform.position.y, cam.transform.position.z);
            Score.GetComponent<ScoreCarnival>().LeftText();
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.name == "MissBoard")
        {
            GameObject CurTarget = GameObject.FindGameObjectWithTag("Target");

            float acc = 100 * (1 - Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(CurTarget.transform.position.x, CurTarget.transform.position.y)) / 0.095f);

            Score.GetComponent<ScoreCarnival>().accuracy = acc;
            Score.GetComponent<ScoreCarnival>().hPosX = this.transform.position.x - 100;
            Score.GetComponent<ScoreCarnival>().hPosY = this.transform.position.y;

            Score.GetComponent<ScoreCarnival>().DisplayShot(0, Latency);
            Score.GetComponent<ScoreCarnival>().Miss();

            wasAHit = false;
            hitPos = transform.position - GameObject.Find("Grid").transform.position;

            // Start logging data once an extension has been done.
            Camera.main.GetComponent<Q4Quit>().logData = true;

            Unload();
        }
        else if (other.tag == "Target")
        {
            float acc = 100 * (1 - Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(other.transform.position.x, other.transform.position.y)) / 0.095f);

            Score.GetComponent<ScoreCarnival>().hPosX = this.transform.position.x-100;
            Score.GetComponent<ScoreCarnival>().hPosY = this.transform.position.y;

            int score = (int)(Mathf.Ceil(acc/10));
            if (score < 1) score = 1;

            Vector2 scorePos = cam.WorldToScreenPoint(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z + 0.5f));
            Score.GetComponent<ScoreCarnival>().accuracy = acc;
            Score.GetComponent<ScoreCarnival>().ScorePoints(score, scorePos);

            Score.GetComponent<ScoreCarnival>().DisplayShot(score, Latency);

            other.GetComponent<NodeTarget>().Break();
            Score.GetComponent<ScoreCarnival>().Hit();

            wasAHit = true;
            hitPos = transform.position - GameObject.Find("Grid").transform.position;

            // Start logging data once an extension has been done.
            Camera.main.GetComponent<Q4Quit>().logData = true;

            Unload();
        }
    }

    private void Unload()
    {
        Score.GetComponent<ScoreCarnival>().SetUserPromptText("ARM IN");
        if (!Score.GetComponent<ScoreCarnival>().Finished)
        {
            GameObject activeNode = GameObject.Find("Grid").GetComponent<Grid>().GetActiveNode();

            if(activeNode != null)
            {
                this.GetComponent<Collider>().enabled = false;
                ChangeColor(Color.red);
                Loaded = false;
                activeNode.GetComponent<Node>().Target.SetActive(false);

                // Find the average reach again.
                Grid grid = GameObject.Find("Grid").GetComponent<Grid>();
                grid.StopGridMovement();
                grid.RecalculateAverageReach();

                ReloadTarget.SetActive(true);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Reload()
    {
        Grid grid = GameObject.Find("Grid").GetComponent<Grid>();
        Score.GetComponent<ScoreCarnival>().SetUserPromptText("ARM OUT");
        if (logger != null && startLogging)
        {
            // Make an entry in the data logger for the performance information of the lastest punch / extension.
            int points = (int)(Mathf.Ceil(Score.GetComponent<ScoreCarnival>().accuracy / 10));
            TargetsDataMass dataEntry = new TargetsDataMass(grid.ActiveNode, Latency, RetractLatency, grid.Depth, (int)Score.GetComponent<ScoreCarnival>().accuracy, points, Assisted, wasAHit);
            dataEntry.targetPos = grid.GetActiveNode().transform.localPosition;
            dataEntry.hitPos = hitPos;

            logger.DATA.Add("Punch_" + logger.recordedPunches, dataEntry);
            logger.recordedPunches++;
        }

        grid.AllowNextTargetSpawn();
        grid.LoadNextTargetInSpawnOrder();
        grid.GetActiveNode().GetComponent<Node>().Target.SetActive(true);

        this.GetComponent<Collider>().enabled = true;
        ChangeColor(Color.green);
        Latency = 0;
        RetractLatency = 0;
        Score.GetComponent<ScoreCarnival>().HideShot();
        Score.GetComponent<ScoreCarnival>().SetAssisted(Assisted);
        Score.GetComponent<ScoreCarnival>().ResetMaxReach();
        Score.GetComponent<ScoreCarnival>().SendRawData();
        Loaded = true;


        //GameObject.Find("Grid").GetComponent<Grid>().GetActiveNode().GetComponent<Node>().Target.SetActive(true);

        ReloadTarget.SetActive(false);
    }

    private void ChangeColor(Color color)
    {
        MeshRenderer gameObjectRenderer = this.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        color.a = 0.5f;
        newMaterial.color = color;
        gameObjectRenderer.material = newMaterial;
    }

    public void SetAssisted(bool assisting)
    {
        Assisted = assisting;

        if(Loaded) Score.GetComponent<ScoreCarnival>().SetAssisted(Assisted);
    }

    IEnumerator CountDownStart()
    {
        countdownActive = true;
        for (int i = CountDownTimer; i >= 0; i--)
        {
            if (i > 0) // 3... 2... 1...
            {
                Score.GetComponent<ScoreCarnival>().SetUserPromptText("Starting in: " + i.ToString());
 
                yield return new WaitForSeconds(1.0f);
            }
            else if (i <= 0) // START!
            {
                Score.GetComponent<ScoreCarnival>().SetUserPromptText(""); // Otherwise sometimes it will look like it gets stuck at 1.
                countdownActive = false;
            }
        }
            yield break;
        }
    }