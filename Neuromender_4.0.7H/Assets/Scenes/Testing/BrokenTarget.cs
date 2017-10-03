using UnityEngine;
using System.Collections;

public class BrokenTarget : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        transform.Rotate(Vector3.forward * Random.Range(0.0f,360.0f));

        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<TargetPiece>();
            //Debug.Log(child.name);
        }

        Destroy(gameObject, 5);
    }
}
