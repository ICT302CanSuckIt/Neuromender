using UnityEngine;
using System.Collections;

public class Basic_mapping : MonoBehaviour {

	public GameObject mappedObject;
	public bool reverse;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(reverse)
		{
			mappedObject.transform.localRotation = Inverse(this.transform.localRotation);
				//Quaternion.Inverse(this.transform.localRotation);
				//
		}
		else
		{
			mappedObject.transform.localRotation = this.transform.localRotation;
		}
	}

	/// <summary>
	/// Inverse the specified inQ.
	/// Inversing the rotations because for some reason that we couldnt find the rotation on the left arm it was broken.
	/// Also we have set the y to not rotate because it was causing issues.
	/// </summary>
	/// <param name="inQ">In q.</param>
	Quaternion Inverse(Quaternion inQ)
	{
		float x, y, z, w;
		x = inQ.x;
		y = inQ.y;
		z = inQ.z;
		w = inQ.w;
		return new Quaternion(-x, y, -z, w);

	}
}
