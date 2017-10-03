using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;



public class GetData : MonoBehaviour {

	//Leave public
	public float saveTimerReset = 0.25f; 


	//Hide
	public GameObject glove;
	public GameObject elbow;
	public GameObject shoulder;
	public float armExtensionDistance;
	public float armExtensionAngle;
	float saveTimer;
	private LoginControl userConfig;
	public armControlSide armControl;

	// Use this for initialization
	void Start () 
	{
		FindDatabase ();
		glove = GameObject.Find("GloveRelative");
		saveTimer = saveTimerReset;

	}
	
	// Update is called once per frame
	void Update () 
	{
		GetTargets ();
		FindExtension ();

		if (saveTimer > 0) 
		{
			saveTimer -= Time.deltaTime;
		} 
		else 
		{
			SaveToFile();
			saveTimer = saveTimerReset;
		}
	}


	private void GetTargets() //Needs to be called during update as the elbow and shoulder game objects don't exist yet when the method is called in Start(). Could simply set the shoulder and elbow game obejcts in the sphero spawning script though - Michael.
	{
		if (armControl == armControlSide.right) 
		{
			elbow = GameObject.Find ("ElbowRight");
			shoulder = GameObject.Find ("ShoulderRight");
		} 
		else
			if (armControl == armControlSide.left) 
		{
			elbow = GameObject.Find ("ElbowLeft");
			shoulder = GameObject.Find ("ShoulderLeft");
		}
	}

	private void FindExtension()
	{
		armExtensionDistance = Vector3.Distance (glove.transform.position, shoulder.transform.position);
		//armExtensionAngle (No idea how to find this as I sucked at computational maths :( -Michael)
	}


	private void SaveToFile()
	{


		FileStream fileStream = File.Open("SessionData/ArmExtension_SessionData.txt",FileMode.Append, FileAccess.Write);
		StreamWriter fileWriter = new StreamWriter(fileStream);

        string output = "Side Affected:" + armControl + " " + "XPos:" + glove.transform.position.x + " " + "YPos:" + glove.transform.position.y + " " + "ZPos:" + glove.transform.position.z + " " + "Distance:" + armExtensionDistance + " " + "[" + DateTime.Now +"]";

		fileWriter.WriteLine (output);
		fileWriter.Flush ();
		fileWriter.Close();
	}

	void FindDatabase()
	{
		if (GameObject.Find ("DatabaseController")) 
		{
			userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
			armControl = (armControlSide)userConfig.config.sideAffected;
		}
	}
}
