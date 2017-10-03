using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour {

	public AudioClip missSound;
	public AudioClip hitSound;
	
	
	private AudioSource source;
	private float lowPitchRange = .75F;
	private float highPitchRange = 1.5F;
	private float velToVol = .2F;
	private float velocityClipSplit = 10F;

	// Use this for initialization
	void Start () 
	{

		source = GetComponent<AudioSource>();
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "MissBoard")
		{
			source.PlayOneShot(missSound,1f);
		}
		else
		{
			source.PlayOneShot(hitSound,1f);
		}
	}
}
