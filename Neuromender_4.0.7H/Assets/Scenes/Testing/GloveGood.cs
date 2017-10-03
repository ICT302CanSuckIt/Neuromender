using UnityEngine;
using System.Collections;

public class GloveGood : Glove_Base
{
    //public GameObject Joint;
    //public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!keyboardControlOverride)
            if (Joint)
            {
                rb.MovePosition(Joint.transform.position);
            }
    }
}
