using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
    public bool ActiveTarget = false;
    public GameObject Target;

    private float Depth;

    public GameObject Stars;

    // Use this for initialization
    void Start()
    {
        Depth = GetComponentInParent<Grid>().Depth;
		//Target.tag = "Target";
    }

    // Update is called once per frame
    void Update()
    {
        Depth = GetComponentInParent<Grid>().Depth;

        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, Depth);
    }

    public bool SpawnTarget()
    {
        if (ActiveTarget)
        {
            return false;
        }
        else
        {
            // Activate target
            Target.SetActive(true);
			Target.transform.GetChild(0).tag = "Target";
            ActiveTarget = true;

            return true;
        }
    }

    public void DespawnTarget(bool stars)
    {
        ActiveTarget = false;
        Target.SetActive(false);

        if (stars)
        {
            GameObject newStars = Instantiate(Stars, this.transform.position, Quaternion.identity) as GameObject;
        }
    }
}
