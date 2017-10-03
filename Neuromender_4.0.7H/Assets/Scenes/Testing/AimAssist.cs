using UnityEngine;
using System.Collections;

public class AimAssist : MonoBehaviour
{
    public GameObject Target;
    public Vector3 AimVector;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AimVector = Target.transform.position;

        Vector3 relativePos = AimVector - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
    }
}
