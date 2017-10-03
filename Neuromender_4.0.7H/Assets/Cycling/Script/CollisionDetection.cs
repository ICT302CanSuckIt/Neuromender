using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {
    public bool crash = false;
    public int numrotation;
    public GameObject colRPM;
    public int timecount;

    // Use this for initialization
    void Start () {
        colRPM = GameObject.Find("RPMCollider");
    }

    // Update is called once per frame
    void Update()
    {
        
          


        }


    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bike")              // If the Diamond collides with the bike
        {
            crash = true;
            numrotation = colRPM.GetComponent<colliderRPM>().counter;
        }
    }
}
