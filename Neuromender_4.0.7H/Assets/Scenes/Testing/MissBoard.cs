using UnityEngine;
using System.Collections;

public class MissBoard : MonoBehaviour
{
    private float Depth;
    public Camera cam;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Depth = GetComponentInParent<Grid>().Depth;

        Vector3 camPos = cam.transform.position;
        this.transform.position = new Vector3(camPos.x, camPos.y, Depth + 0.1f);
        //this.transform.position = Vector3.forward * (Depth + 0.1f);
    }
}
