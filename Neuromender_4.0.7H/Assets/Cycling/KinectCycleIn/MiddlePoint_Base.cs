using UnityEngine;
using System.Collections;

public class MiddlePoint_Base : MonoBehaviour {

    public GameObject Joint;
    public Rigidbody rb;

    [Tooltip("TRUE: Allow keyboard controls to move this glove")]
    public bool keyboardControlOverride = false;

}
