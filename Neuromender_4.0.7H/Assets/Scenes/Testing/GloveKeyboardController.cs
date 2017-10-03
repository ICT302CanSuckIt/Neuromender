using UnityEngine;
using System.Collections;

/// <summary>
/// Test controller for use by developers to assist moving a glove around (used in 'Targets' game).
/// 
/// To use : Attach to a glove object. It should automatically assign everything at startup (see Awake() function).
/// 
/// Functionality : It disables the kinect functionality using a boolean variable to allow alternative means of translation. See GloveRelative script for example.
/// 
/// </summary>
public class GloveKeyboardController : MonoBehaviour {

    //------------------------------------------------------------------------------------------
    [Header("Glove")]
    //------------------------------------------------------------------------------------------

    [Tooltip("The glove that is used to hit the targets.")]
    public GameObject gloveObject = null;


    //------------------------------------------------------------------------------------------
    [Header("Movement Keys")]
    //------------------------------------------------------------------------------------------

    [Tooltip("Control key for moving the glove UP.")]
    public KeyCode upKey = KeyCode.W;

    [Tooltip("Control key for moving the glove LEFT.")]
    public KeyCode leftKey = KeyCode.A;

    [Tooltip("Control key for moving the glove DOWN.")]
    public KeyCode downKey = KeyCode.S;

    [Tooltip("Control key for moving the glove RIGHT.")]
    public KeyCode rightKey = KeyCode.D;

    //------------------------------------------------------------------------------------------
    [Header("Movement Modifiers")]
    //------------------------------------------------------------------------------------------

    [Tooltip("Left / Right speed modifier for keyboard movement.")]
    [Range(0.05f, 1.0f)]
    public float horizontalSpeedModifier = 0.75f;

    [Tooltip("Up / Down speed modifier for keyboard movement.")]
    [Range(0.05f, 1.0f)]
    public float verticalSpeedModifier = 0.75f;

    [Tooltip("Forward / Backward speed modifier for keyboard movement.")]
    [Range(0.1f, 1.0f)]
    public float reachSpeedModifier = 0.5f;

    //------------------------------------------------------------------------------------------
    [Header("Other Useful Information")]
    //------------------------------------------------------------------------------------------

    [Tooltip("Position of the nearest point of the glove to the target.")]
    public Vector3 gloveCollisionPoint;

    private SphereCollider coll = null;
    private Glove_Base glove = null;

    // Use this for initialization
    void Start () {

        if (gloveObject == null)
            gloveObject = gameObject;
        
        coll = gloveObject.GetComponent<SphereCollider>();
        glove = gloveObject.GetComponent<Glove_Base>();

        if (coll == null || glove == null)
            enabled = false;
        //else
            //glove.keyboardControlOverride = true;
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKey(upKey))
            gloveObject.transform.position = new Vector3(gloveObject.transform.position.x, gloveObject.transform.position.y + (verticalSpeedModifier * Time.deltaTime), gloveObject.transform.position.z);

        if(Input.GetKey(downKey))
            gloveObject.transform.position = new Vector3(gloveObject.transform.position.x, gloveObject.transform.position.y - (verticalSpeedModifier * Time.deltaTime), gloveObject.transform.position.z);

        if (Input.GetKey(leftKey))
            gloveObject.transform.position = new Vector3(gloveObject.transform.position.x - (horizontalSpeedModifier * Time.deltaTime), gloveObject.transform.position.y, gloveObject.transform.position.z);

        if (Input.GetKey(rightKey))
            gloveObject.transform.position = new Vector3(gloveObject.transform.position.x + (horizontalSpeedModifier * Time.deltaTime), gloveObject.transform.position.y, gloveObject.transform.position.z);

        gloveObject.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * reachSpeedModifier);

        gloveCollisionPoint.Set(gloveObject.transform.position.x, gloveObject.transform.position.y, gloveObject.transform.position.z + coll.radius);
    }
}