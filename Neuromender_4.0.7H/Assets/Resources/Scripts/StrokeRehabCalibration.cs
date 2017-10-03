using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StrokeRehabCalibration : MonoBehaviour {

    private Text lefthandDepthText;
    private Text righthandDepthText;
    
    private Text lefthandStretchText;
    private Text righthandStretchText;

    private Text lefthandDistanceText;
    private Text righthandDistanceText;

    private Text ArmLengthUnitsZ;
    private Text ArmLengthUnitsX;

    private Text leftAngleMaxText;
    private Text rightAngleMaxText;
    private Text AvgBodyDepthText;

    private Text ArmLengthInput;

    public float lefthandDepth = 0.0f;
    public float righthandDepth = 0.0f;
    public float AvgBodyDepth = 0.0f;

    public float leftHandStretch = 0.0f;
    public float rightHandStretch = 0.0f;

    public float leftHandDistance = 0.0f;
    public float rightHandDistance = 0.0f;

    public float MaxAngleRight = 0.0f;
    public float MaxAngleLeft = 0.0f;
    public float ArmLengthInputFloat;

	// Use this for initialization
	void Start () {
	    DontDestroyOnLoad(this);
		/*
        lefthandDepthText = GameObject.Find("LeftHandDepthText").GetComponent<Text>(); 
        righthandDepthText = GameObject.Find("RightHandDepthText").GetComponent<Text>(); 
        lefthandStretchText = GameObject.Find("LeftHandStretchText").GetComponent<Text>(); 
        righthandStretchText = GameObject.Find("RightHandStretchText").GetComponent<Text>();
        lefthandDistanceText = GameObject.Find("LeftHandDistanceText").GetComponent<Text>();
        righthandDistanceText = GameObject.Find("RightHandDistanceText").GetComponent<Text>();

        leftAngleMaxText = GameObject.Find("LeftMaxText").GetComponent<Text>();
        rightAngleMaxText = GameObject.Find("RightMaxText").GetComponent<Text>();

        AvgBodyDepthText = GameObject.Find("AvgDepthText").GetComponent<Text>();

        ArmLengthInput = GameObject.Find("ArmInputText").GetComponent<Text>();
        ArmLengthUnitsZ = GameObject.Find("ArmLengthUnitsZ").GetComponent<Text>();
        ArmLengthUnitsX = GameObject.Find("ArmLengthUnitsX").GetComponent<Text>();
        */
	}
	
	// Update is called once per frame
	void Update () {
		/*
        string text = ArmLengthInput.GetComponent<Text>().text;
        float.TryParse(text, out ArmLengthInputFloat);
        //hand depths
        lefthandDepthText.text = "" + lefthandDepth;
        righthandDepthText.text = "" + righthandDepth;
        lefthandStretchText.text = "" + leftHandStretch;
        righthandStretchText.text = "" + rightHandStretch;
        lefthandDistanceText.text = "" + leftHandDistance;
        righthandDistanceText.text = "" + rightHandDistance;
        leftAngleMaxText.text = "" + MaxAngleLeft;
        rightAngleMaxText.text = "" + MaxAngleRight;


	    if (ArmLengthInputFloat > 0)
	    {
	        if (lefthandDepth > righthandDepth)
	        {
                ArmLengthUnitsZ.text = "" + (ArmLengthInputFloat / leftHandDistance) + "mm per unit";
	        }
	        else if(righthandDepth > lefthandDepth)
	        {
                ArmLengthUnitsZ.text = "" + (ArmLengthInputFloat / rightHandDistance) + "mm per unit";
	        }

            if (leftHandStretch > rightHandStretch)
            {
                ArmLengthUnitsX.text = "" + (ArmLengthInputFloat / leftHandDistance) + "mm per unit";
            }
            else if (rightHandStretch > leftHandStretch)
            {
                ArmLengthUnitsX.text = "" + (ArmLengthInputFloat / rightHandDistance) + "mm per unit";
            }
	    }
	    AvgBodyDepthText.text = "" + AvgBodyDepth;

*/
        
	}



    private void restrictInputToNumbers()
    {
     //   float temp;
       // string text = armLength.GetComponent<Text>().text;
     //   if (!float.TryParse(text, out temp))
     //   {
      //      armLength.GetComponent<Text>().text = "";
      //  }
    }
}
