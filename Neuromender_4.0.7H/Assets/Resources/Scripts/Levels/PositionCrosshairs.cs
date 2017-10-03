using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PositionCrosshairs : MonoBehaviour {

	public GameObject crosshairs;
	public GameObject targetImage;

	private Image target;

	private Color bullseye = Color.green;
	private Color hitting = Color.blue;
	private Color miss = Color.red;

	// Use this for initialization
	void Start () 
	{

		//targetImage = GetComponentInChildren<Image> ();
		targetImage = GameObject.Find("TargetImage");
		target = targetImage.GetComponent<Image> ();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		PlaceCrosshair ();
	}

	void PlaceCrosshair()
	{
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, transform.forward, out hit)) 
		{
			Debug.DrawRay(this.transform.position,transform.forward,Color.green);
			crosshairs.transform.position = hit.point;

			if(hit.collider.tag == "Target")
			{
				target.color = hitting;
			}
			else
			{
				target.color = miss;
			}
		}
	}
}
