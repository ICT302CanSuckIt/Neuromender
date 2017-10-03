using UnityEngine;
using System.Collections;

public class AssistZone : MonoBehaviour
{
    public GameObject HandJoint;
    public GameObject ElbowJoint;

    public Vector3 HandPos;
    public Vector3 ElbowPos;

    public Vector3 Offset;

    public float width = 2.0f;
    public float lengthFactor = 5.0f;


    public GameObject RGlove;

    // Use this for initialization
    void Start()
    {
        if (!RGlove)
        {
            RGlove = GameObject.Find("GloveRelative");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HandJoint) HandPos = HandJoint.transform.position;
        if (ElbowJoint) ElbowPos = ElbowJoint.transform.position;

        Offset = ElbowPos - HandPos;
        transform.position = HandPos + (Offset * 0.5f);
        transform.localScale = new Vector3(width, Offset.magnitude * lengthFactor, width);
        transform.up = Offset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GloveGood")
        {
            // assisting
            if (RGlove)
            {
                RGlove.GetComponent<GloveRelative>().SetAssisted(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "GloveGood")
        {
            // no longer assisting
            if (RGlove)
            {
                RGlove.GetComponent<GloveRelative>().SetAssisted(false);
            }
        }
    }
}
