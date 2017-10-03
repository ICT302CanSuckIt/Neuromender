using UnityEngine;
using System.Collections;

public class LaneMarkerLine : MonoBehaviour
{
    public GameObject buoy;

    public float RaceDistance = 2000.0f;

    // Use this for initialization
    void Start()
    {
        Color marker = Color.white;

        // first hundred meters
        // red buoy every 5 metres (not on start line)
        for (int d = 5; d <= 100; d = d + 5)
        {
            CreateBuoy(d, Color.red);
        }
        
        // after first hundred meters
        // red buoy every 5 metres (not on start line)
        for (int d = 110; d < RaceDistance; d = d + 10)
        {
            marker = Color.white;

            if (d % 250 == 0)
            {
                marker = Color.yellow;
            }
            else
            {
                if (d >= RaceDistance - 250)
                {
                    marker = Color.red;
                }
            }

            CreateBuoy(d, marker);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateBuoy(int distance, Color marker)
    {
        GameObject newBuoy = Instantiate(buoy, Vector3.zero, Quaternion.identity) as GameObject;
        newBuoy.transform.parent = transform;
        newBuoy.transform.localPosition = Vector3.forward * distance;
        newBuoy.name = "Bouy" + distance;



        MeshRenderer gameObjectRenderer = newBuoy.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Diffuse"));
        newMaterial.color = marker;
        gameObjectRenderer.material = newMaterial;
    }
}
