using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NodeTarget : MonoBehaviour
{
    public Grid parentGrid;
    public Node parentNode;

    public GameObject BT10;
    public GameObject Score;

    public Camera cam;
    public GameObject Points;

    // Use this for initialization
    void Start()
    {
        parentGrid = GetComponentInParent<Grid>();
        parentNode = GetComponentInParent<Node>();



        if (!Score)
        {
            Score = GameObject.Find("ScoreBoard");
        }

        Points = GameObject.Find("Points");
        cam = Camera.main;
    }

    public void Break()
    {

        parentGrid.SpawnTarget();
        parentNode.DespawnTarget(true);

        GameObject newBroken = Instantiate(BT10, this.transform.position, Quaternion.identity) as GameObject;

        newBroken.transform.localScale = new Vector3(0.19f, 0.19f, 0.19f);

    }

    public void TooHard()
    {
        Debug.Log("Too Hard");
        parentGrid.SpawnTarget();
        parentNode.DespawnTarget(false);
    }
}
