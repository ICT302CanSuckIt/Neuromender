using UnityEngine;
using System.Collections;

public class MoveUpDown : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 displace = new Vector3();
	    if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            displace = new Vector3(0, 0.01f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            displace = new Vector3(0, -0.01f);
        }

        this.gameObject.transform.position += displace;
    }
}
