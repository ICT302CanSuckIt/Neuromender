using UnityEngine;
using System.Collections;

public class LeftHandZone : MiddlePoint_Base
{

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyboardControlOverride)
            if (Joint)
            {
                rb.MovePosition(Joint.transform.position);
            }
    }


}
