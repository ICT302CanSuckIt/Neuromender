using UnityEngine;
using System.Collections;

public class PlayVideo : MonoBehaviour {

    private MovieTexture mtex;
    public bool videoPlaying = false;
	public bool stopped;
    public GameObject GUIManager;

    // Use this for initialization
    void Start()
    {
        mtex = this.GetComponent<Renderer>().material.mainTexture as MovieTexture;
    }

    // Update is called once per frame
    void Update()
    {
        if (mtex.isPlaying)
        {
            videoPlaying = true;
        }
        else
        {
            StopHowToVideo();
        }

    }

    public void PlayHowToVideo()
    {
        if(mtex == null)
            mtex = this.GetComponent<Renderer>().material.mainTexture as MovieTexture;

		this.GetComponent<MeshRenderer>().enabled = true;
        mtex.Play();
    }

    public void StopHowToVideo()
    {
        if (mtex == null)
            mtex = this.GetComponent<Renderer>().material.mainTexture as MovieTexture;

        if(GUIManager.GetComponent<TaskSelectBtnManager>().menu != "TaskSelect")
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            videoPlaying = false;
            stopped = true;
            mtex.Stop();
        }
		
    }
}
