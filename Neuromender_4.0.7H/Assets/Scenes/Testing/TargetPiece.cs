using UnityEngine;
using System.Collections;

public class TargetPiece : MonoBehaviour
{
    public MeshCollider mc;
    public Rigidbody rb;


    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<MeshCollider>();
        mc = GetComponent<MeshCollider>();
        mc.convex = true;



        gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;

        rb.AddExplosionForce(5, transform.position-transform.localPosition , 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
