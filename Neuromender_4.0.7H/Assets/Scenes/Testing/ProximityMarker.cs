using UnityEngine;
using System.Collections;

public class ProximityMarker : MonoBehaviour
{
    public GameObject Target;

    public float Proximity; // how close is the target to this node
    public float ProximityPercentage; // of the distance between this marker and the target compared to the centre
    public float FromCentre; // how far is the target from centre of ring

    public float Range;

    public Vector3 FromCentreV;

    // Use this for initialization
    void Start()
    {
        Target = GetComponentInParent<ProximityRing>().Target;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Target) Target = GetComponentInParent<ProximityRing>().Target;
        else
        {
            Range = GetComponentInParent<ProximityRing>().Proximity;
            Proximity = Vector3.Distance(this.transform.position, Target.transform.position);

            FromCentre = Vector3.Distance(Target.transform.position, GetComponentInParent<ProximityRing>().Centre);
            FromCentreV = GetComponentInParent<ProximityRing>().Centre;
            
            // calc value of distance



           // ProximityPercentage = Proximity / Range;

            if(FromCentre < Range) // inside ring
            {
                if (FromCentre < Range * 0.25) // close enough to centre
                {
                    // Set to Green
                    ProximityPercentage = 1.0f;
                }
                else // give a warning
                {
                    // gradual colour change
                    ProximityPercentage = (Proximity - Range * 0.5f) / (Range * 0.75f);
                }
            }
            else // outside ring
            {
                if(Proximity > FromCentre) // on opposite side from centre
                {
                    // Set to Green
                    ProximityPercentage = 1.0f;
                }
                else // outside this side
                {
                    // Set to Red
                    ProximityPercentage = 0.0f;
                }
            }


            Color proxyColour = Color.Lerp(Color.red, Color.green, ProximityPercentage);

            MeshRenderer gameObjectRenderer = this.GetComponent<MeshRenderer>();

            Material newMaterial = new Material(Shader.Find("Diffuse"));

            newMaterial.color = proxyColour;
            gameObjectRenderer.material = newMaterial;

        }

    }
}
