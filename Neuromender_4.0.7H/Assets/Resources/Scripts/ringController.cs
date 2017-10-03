using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ringController : MonoBehaviour {

	public GameObject splineNode;
	private StrokeRehabLevelController lvlControl;

	public Material[] ringMats;

    public float distanceToPlayer;
	public float xzDist;
    Vector3 playerPos;

	public MeshCollider myCollider;
	// Use this for initialization
	void Start () {
        this.transform.position = splineNode.transform.position + splineNode.transform.up.normalized * 7.5f;
		lvlControl = GameObject.Find ("NeuromendController").GetComponent<StrokeRehabLevelController> ();

		int material = UnityEngine.Random.Range (0, ringMats.Length);
		this.GetComponent<Renderer>().material = ringMats [material];
        playerPos = GameObject.Find("basic body").transform.position;
        distanceToPlayer = (this.transform.position - playerPos).magnitude;
		xzDist = new Vector3 (this.transform.position.x - playerPos.x, 0, this.transform.position.z - playerPos.z).magnitude;
        //is.transform.position
		//xzDist = distanceToPlayer;
	}
	
	// Update is called once per frame
	void Update () {

        try
        {
            transform.Rotate(0, 0, 1);
            this.transform.position = splineNode.transform.position + splineNode.transform.up.normalized * (lvlControl.getAngleThreshold() / 5);
            playerPos = GameObject.Find("basic body").transform.position;
            distanceToPlayer = (this.transform.position - playerPos).magnitude;
            xzDist = (lvlControl.pathFollower.transform.position - splineNode.transform.position).magnitude;
            //xzDist = distanceToPlayer;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }
}
