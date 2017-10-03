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
   
    public int route;
    public float DiamondDist = 10;

    // Use this for initialization
    void Start()
    {

        /*
        if (DBCons != null)
        {
            login = DBCons.GetComponent<LoginControl>();

            // Update what games are able to be shown on the menu.
            if (login != null)
                login.UpdateGameAvailability();

            DB = DBCons.GetComponent<database>();
        }
        else
        {
            //enabled = false;
            Debug.LogError("Could not find database controller.");
        }*/

        route = 100;

        // DBcons = GameObject.Find("DatabaseController");
        //route = (int) (DBcons.GetComponent<LoginControl>().config.sensorDistance/1000);


        Nodes[0] = 1;
        Nod = route / 500 * 11;
        /*
                Nodes1 = Nod;
                Nodes2 = Nod * 2;
                Nodes3 = Nod * 3;
                Nodes4 = Nod * 4;
                Nodes5 = Nod * 5;
                Nodes6 = Nod * 6;
                Nodes7 = Nod * 7;
                Nodes8 = Nod * 8;
                Nodes9 = Nod * 9;
                Nodes10 = Nod * 10;
                Nodes11 = Nod * 11;
                */

        
        switch (route)
        {
            case 100:
                Nodes[1] = 26;
                Nodes[2] = 106;
                Nodes[3] = 111;
                Nodes[4] = 130;
                Nodes[5] = 134;
                Nodes[6] = 149;
                Nodes[7] = 164;
                Nodes[8] = 187;
                Nodes[9] = 249;
                Nodes[10] = 310;
                Nodes[11] = 340;
                break;
            case 200:
                Nodes[1] = 26;
                Nodes[2] = 106;
                Nodes[3] = 111;
                Nodes[4] = 130;
                Nodes[5] = 134;
                Nodes[6] = 149;
                Nodes[7] = 164;
                Nodes[8] = 187;
                Nodes[9] = 249;
                Nodes[10] = 310;
                Nodes[11] = 340;
                break;
            case 300:
                Nodes[1] = 26;
                Nodes[2] = 106;
                Nodes[3] = 111;
                Nodes[4] = 130;
                Nodes[5] = 134;
                Nodes[6] = 149;
                Nodes[7] = 164;
                Nodes[8] = 187;
                Nodes[9] = 249;
                Nodes[10] = 310;
                Nodes[11] = 340;
                break;
            default:
                Nodes[1] = 26;
                Nodes[2] = 106;
                Nodes[3] = 111;
                Nodes[4] = 130;
                Nodes[5] = 134;
                Nodes[6] = 149;
                Nodes[7] = 164;
                Nodes[8] = 187;
                Nodes[9] = 249;
                Nodes[10] = 310;
                Nodes[11] = 340;
                break;
        }

        

        Bike.GetComponent<CyclistController>().numnode = Nodes[11];



        Diamond1.position = new Vector3(path.nodes[106].x, path.nodes[106].y, path.nodes[106].z);
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
        Debug.Log("futurePosition :" + path.nodes[28]);

        /*
          
        */

        /*
        switch (route)
        {
            case 100:
             Nodes1 = 8
             Nodes2 = 16
             Nodes3 = 24
             Nodes4 = 32
             Nodes5 = 40
             Nodes6 = 48
             Nodes7 = 56
             Nodes8 = 64
             Nodes9 = 72
             Nodes10 = 80
             Nodes11 = 88
                break;
            case 200:
             Nodes1 = 8
             Nodes2 = 8
             Nodes3 = 8
             Nodes4 = 8
             Nodes5 = 8
             Nodes6 = 8
             Nodes7 = 8
             Nodes8 = 8
             Nodes9 = 8
             Nodes10 = 8
             Nodes11 = 8
             break;
            case 300:
             Nodes1 = 8
             Nodes2 = 8
             Nodes3 = 8
             Nodes4 = 8
             Nodes5 = 8
             Nodes6 = 8
             Nodes7 = 8
             Nodes8 = 8
             Nodes9 = 8
             Nodes10 = 8
             Nodes11 = 8
             break;
            default:
                Nodes1 = 8
             Nodes2 = 8
             Nodes3 = 8
             Nodes4 = 8
             Nodes5 = 8
             Nodes6 = 8
             Nodes7 = 8
             Nodes8 = 8
             Nodes9 = 8
             Nodes10 = 8
             Nodes11 = 8
                break;
        }*/

        //Website
        //   DiamondDist = route / 11 ;      //for the diamond

        //FinishLine.position = new Vector3(0, 0, route);
        /* Diamond1.position = new Vector3(0, 2f, DiamondDist);
         Diamond2.position = new Vector3(0, 2f, DiamondDist * 2);
         Diamond3.position = new Vector3(0, 2f, DiamondDist * 3);
         Diamond4.position = new Vector3(0, 2f, DiamondDist * 4);
         Diamond5.position = new Vector3(0, 2f, DiamondDist * 5);
         Diamond6.position = new Vector3(0, 2f, DiamondDist * 6);
         Diamond7.position = new Vector3(0, 2f, DiamondDist * 7);
         Diamond8.position = new Vector3(0, 2f, DiamondDist * 8);
         Diamond9.position = new Vector3(0, 2f, DiamondDist * 9);
         Diamond10.position = new Vector3(0, 2f, DiamondDist * 10);*/
    }

    // Update is called once per frame
    void Update()
    {



    }
}
