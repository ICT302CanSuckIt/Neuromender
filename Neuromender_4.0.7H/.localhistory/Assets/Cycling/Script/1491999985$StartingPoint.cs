using UnityEngine;
using System.Collections;

public class StartingPoint : MonoBehaviour
{
    private LoginControl login = null;
    private database DB = null;
    private GameObject DBCons = null;
    public GameObject Bike;// -RR

    public Polyline path;
    public Transform FinishLine;
    public Transform Diamond1;
    public Transform Diamond2;
    public Transform Diamond3;
    public Transform Diamond4;
    public Transform Diamond5;
    public Transform Diamond6;
    public Transform Diamond7;
    public Transform Diamond8;
    public Transform Diamond9;
    public Transform Diamond10;

    public int Nod;
    public int[] Nodes;

    public int route = 100;
    public float DiamondDist = 10;

    // Use this for initialization
    void Start()
    {
        DBCons = GameObject.Find("DatabaseController");
        if (DBCons != null)
        {
            login = DBCons.GetComponent<LoginControl>();

            if (login.selectedDistance == DistanceLevel.Short)
            {
                route = login.config.DistanceShort;
                Debug.Log("route : short");

            }
            else if (login.selectedDistance == DistanceLevel.Medium)
            {
                route = login.config.DistanceMedium;
                Debug.Log("route : Medium");

            }
            else if (login.selectedDistance == DistanceLevel.Long)
            {
                route = login.config.DistanceLong;
                Debug.Log("route : Long");

            }
            else route = 500;

            if (login.selectedGraphic == GraphicName.Low)
            {
                GameObject.Find("Medium Game Object").SetActive(false);
                GameObject.Find("High Game Object").SetActive(false);
            }
            else if (login.selectedGraphic == GraphicName.Medium)
            {
                GameObject.Find("High Game Object").SetActive(false);
            }

        }
        else route = 100;



        Nodes = new int[12];

        // DBcons = GameObject.Find("DatabaseController");
        //route = (int) (DBcons.GetComponent<LoginControl>().config.sensorDistance/1000);


        Nodes[0] = 1;
        // Nod = route / 500 * 11;


        /*
        switch (route)
        {
            case 100:
                Nodes[1] = 5;
                Nodes[2] = 9;
                Nodes[3] = 12;
                Nodes[4] = 17;
                Nodes[5] = 20;
                Nodes[6] = 25;
                Nodes[7] = 29;
                Nodes[8] = 35;
                Nodes[9] = 39;
                Nodes[10] = 42;
                Nodes[11] = 45;
                break;
            case 200:
                Nodes[1] = 9;
                Nodes[2] = 17;
                Nodes[3] = 25;
                Nodes[4] = 35;
                Nodes[5] = 42;
                Nodes[6] = 46;
                Nodes[7] = 49;
                Nodes[8] = 53;
                Nodes[9] = 61;
                Nodes[10] = 66;
                Nodes[11] = 69;
                break;
            case 300:
                Nodes[1] = 12;
                Nodes[2] = 25;
                Nodes[3] = 39;
                Nodes[4] = 45;
                Nodes[5] = 51;
                Nodes[6] = 61;
                Nodes[7] = 69;
                Nodes[8] = 77;
                Nodes[9] = 82;
                Nodes[10] = 90;
                Nodes[11] = 93;
                break;
            case 400:
                Nodes[1] = 17;
                Nodes[2] = 35;
                Nodes[3] = 45;
                Nodes[4] = 53;
                Nodes[5] = 66;
                Nodes[6] = 75;
                Nodes[7] = 86;
                Nodes[8] = 96;
                Nodes[9] = 119;
                Nodes[10] = 122;
                Nodes[11] = 124;
                break;
            case 500:
                Nodes[1] = 22;
                Nodes[2] = 42;
                Nodes[3] = 52;
                Nodes[4] = 66;
                Nodes[5] = 77;
                Nodes[6] = 91;
                Nodes[7] = 102;
                Nodes[8] = 120;
                Nodes[9] = 137;
                Nodes[10] = 151;
                Nodes[11] = 154;
                break;
            default:
                Nodes[1] = 22;
                Nodes[2] = 42;
                Nodes[3] = 52;
                Nodes[4] = 66;
                Nodes[5] = 77;
                Nodes[6] = 91;
                Nodes[7] = 102;
                Nodes[8] = 120;
                Nodes[9] = 137;
                Nodes[10] = 151;
                Nodes[11] = 154;
                break;
        }
        */
        for (int l = 1; l <= 10; l++)
        {
            for (int k = Nodes[l - 1]; k <= 154; k++)
            {
                if (Bike.GetComponent<CyclistController>().Distanceupdate(Nodes[l - 1], k) > route / 10)
                {
                    Nodes[l] = k - 1;
                    break;
                }
            }
        }


        for (int p = 0; p <= 154; p++)
        {
            if (Bike.GetComponent<CyclistController>().Distanceupdate(1, p) > 30 )
            {
                
                Debug.Log("nodes : " + p + " distance " + Bike.GetComponent<CyclistController>().Distanceupdate(1, p));
                break;
            }
        }

        


            Nodes[11] = Nodes[10];
        Debug.Log("totaru distance : " + Bike.GetComponent<CyclistController>().Distanceupdate(1, Nodes[11]-2));


        Bike.GetComponent<CyclistController>().numnode = Nodes[11];



        Diamond1.position = new Vector3(path.nodes[Nodes[1]].x, path.nodes[Nodes[1]].y, path.nodes[Nodes[1]].z); 
        Diamond2.position = new Vector3(path.nodes[Nodes[2]].x, path.nodes[Nodes[2]].y, path.nodes[Nodes[2]].z);
        Diamond3.position = new Vector3(path.nodes[Nodes[3]].x, path.nodes[Nodes[3]].y, path.nodes[Nodes[3]].z);
        Diamond4.position = new Vector3(path.nodes[Nodes[4]].x, path.nodes[Nodes[4]].y, path.nodes[Nodes[4]].z);
        Diamond5.position = new Vector3(path.nodes[Nodes[5]].x, path.nodes[Nodes[5]].y, path.nodes[Nodes[5]].z);
        Diamond6.position = new Vector3(path.nodes[Nodes[6]].x, path.nodes[Nodes[6]].y, path.nodes[Nodes[6]].z);
        Diamond7.position = new Vector3(path.nodes[Nodes[7]].x, path.nodes[Nodes[7]].y, path.nodes[Nodes[7]].z);
        Diamond8.position = new Vector3(path.nodes[Nodes[8]].x, path.nodes[Nodes[8]].y, path.nodes[Nodes[8]].z);
        Diamond9.position = new Vector3(path.nodes[Nodes[9]].x, path.nodes[Nodes[9]].y, path.nodes[Nodes[9]].z);
        Diamond10.position = new Vector3(path.nodes[Nodes[10]].x, path.nodes[Nodes[10]].y, path.nodes[Nodes[10]].z);
        FinishLine.position = new Vector3(path.nodes[Nodes[11]].x, path.nodes[Nodes[11]].y, path.nodes[Nodes[11]].z);
    for (int u = 1; u <= 11; u++)
        {
            Debug.Log("Dia : " + u + " nodes " +Nodes[u]);
           double distt = Bike.GetComponent<CyclistController>().Distanceupdate(Nodes[u - 1], Nodes[u]);
            Debug.Log("gap dia : " + u + " is " + distt);
        }
        /*
          
        */

     
    }

    // Update is called once per frame
    void Update()
    {



    }



    public double Gapdistance(int l, int k)
    {
        double dist = 0;

        dist = Bike.GetComponent<CyclistController>().Distanceupdate(Nodes[l], Nodes[k]);

        return dist;

    }

}
