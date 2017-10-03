using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public GameObject Glove;
    public Rigidbody rb;
    public float StartingDistance = 0.8f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) Calibrate();

    }


    private void Calibrate()
    {
        if (Glove)
        {
            //rb.Sleep();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            this.transform.eulerAngles = new Vector3(90, 0, 0);
            this.transform.position = Glove.transform.position + Vector3.forward * StartingDistance;
            //rb.WakeUp();
        }
    }
}
