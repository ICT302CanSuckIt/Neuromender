using UnityEngine;
using System.Collections;

public class colliderRPM : MonoBehaviour {

    public GameObject LeftHandZone;
    private Rigidbody rb;
	public int counter;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "LeftHandSphere")
        {
            counter++;
            Debug.Log("Collision " +counter);
            
        }
       // Debug.Log("RPM = " + counter);
    }
}
