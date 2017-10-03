using UnityEngine;
using System.Collections;

public class ReloadTarget : MonoBehaviour
{
    public Vector3 pos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            pos = GameObject.Find("GloveRelative").GetComponent<GloveRelative>().RelativePosition;
            pos.z = GameObject.Find("Grid/MissBoard").transform.position.z;
            transform.position = pos;

     

    }
}
