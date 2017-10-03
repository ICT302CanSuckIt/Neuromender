using UnityEngine;
using System.Collections;

public class DestroySystem : MonoBehaviour {

	ParticleSystem ps;

	// Use this for initialization
	void Start () 
	{
		ps = GetComponentInChildren<ParticleSystem> ();
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (ps.IsAlive() == false) 
		{
			Destroy(this.gameObject);
		}
	
	}
}
