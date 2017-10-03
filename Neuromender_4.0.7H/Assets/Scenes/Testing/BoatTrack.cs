using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controller for the track of the 'Rowing' game.
/// 
/// Ensures that the boat is always pointing in the right direction (or is turning towards it at least).
/// </summary>
public class BoatTrack : MonoBehaviour {

    //*********************---------------------------------------------
    [Header("Player Data")]
    //*********************---------------------------------------------

    [Tooltip("The GameObject that represents the player in the world.")]
    public GameObject boat = null;

    [Tooltip("The turning speed of the boat (degrees per second).")]
    public float boatTurnSpeed = 10.0f;


    //*************************---------------------------------------------
    [Header("Race Track Data")]
    //*************************---------------------------------------------

    [Tooltip("The number of waypoints that the boat will move through during this session.")]
    public int trackLength = 0;

    [Tooltip("The index of the current waypoint that the boat is moving towards.")]
    public int currentWaypoint = 0;

    [Tooltip("TRUE: The session has been completed.")]
    public bool trackFinished = false;

    [Tooltip("The object spawned periodically between waypoints.")]
    public GameObject progressMarker = null;

    [Tooltip("The number of metres between progress markers.")]
    public float stepEveryNMeters = 1;

    [Tooltip("The list of all waypoints in the world.")]
    public List<GameObject> trackWaypoints = null;

    public Vector3 DEST;
    public float boatHeight = -1;

	// Use this for initialization
	void Start () {

        boat = GameObject.Find("Boat");

        boatHeight = boat.transform.position.y;

        if (boat == null)
        {
            enabled = false;
            Debug.Log("Boat not found. Disabling BoatTrack script in " + gameObject.name + ".");
        }

        if(trackWaypoints == null)
            trackWaypoints = new List<GameObject>();

        // Fill the track waypoint list with all of the gameobjects that are its children.
        foreach (Transform child in transform)
            trackWaypoints.Add(child.gameObject);

        if(trackLength == 0)
            trackLength = trackWaypoints.Count;

        // Create the progress markers between the waypoints, if a prefab has been specified.
        if (progressMarker != null)
            for (int i = 0; i < trackWaypoints.Count - 1; ++i)
            {
                float distance = Vector3.Distance(trackWaypoints[i].transform.position, trackWaypoints[i + 1].transform.position);
                Vector3 directon = (trackWaypoints[i + 1].transform.position - trackWaypoints[i].transform.position).normalized;
                int steps = (int)(distance / stepEveryNMeters);
                Debug.Log(steps);

                for (int j = 1; j <= steps; ++j)
                {
                    GameObject obj = Instantiate(progressMarker);

                    // Make sure that the marker is NOT a solid object.
                    if (obj.GetComponent<Collider>())
                        obj.GetComponent<Collider>().isTrigger = true;

                    obj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    obj.transform.localPosition = trackWaypoints[i].transform.position + (directon * j * stepEveryNMeters);
                    obj.transform.SetParent(trackWaypoints[i + 1].transform);
                    obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, -1.0f, obj.transform.localPosition.z);
                }
            }
        else
            Debug.LogWarning("No progress marker was specified for the Boat Track.");

	}
	
	// Update is called once per frame
	void Update () {

        // Unil the track is finished, turn the boat towards the next waypoint on the track.
        if(!trackFinished)
        {
            // Create a direction vector using the horizontal co-ords of the next waypoint, and the initial height of the boat.
            Vector3 dest = new Vector3( trackWaypoints[currentWaypoint].transform.position.x - boat.transform.position.x,
                                        boatHeight,
                                        trackWaypoints[currentWaypoint].transform.position.z - boat.transform.position.z);

            DEST = dest;

            // Rotate towards the next waaypoint.
            boat.transform.rotation = Quaternion.RotateTowards(boat.transform.rotation, Quaternion.LookRotation(dest, Vector3.up), boatTurnSpeed * Time.deltaTime);
        }
	}

    public void NextWaypoint()
    {
        currentWaypoint++;

        if (currentWaypoint == trackLength)
            trackFinished = true;
    }
}
