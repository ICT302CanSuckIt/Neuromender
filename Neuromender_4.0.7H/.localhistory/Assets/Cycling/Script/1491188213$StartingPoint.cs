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
    public int Nodes1;
    public int Nodes2;
    public int Nodes3;
    public int Nodes4;
    public int Nodes5;
    public int Nodes6;
    public int Nodes7;
    public int Nodes8;
    public int Nodes9;
    public int Nodes10;
    public int Nodes11;

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
                Nodes1 = 26;
                Nodes2 = 106;
                Nodes3 = 111;
                Nodes4 = 130;
                Nodes5 = 134;
                Nodes6 = 149;
                Nodes7 = 164;
                Nodes8 = 187;
                Nodes9 = 249;
                Nodes10 = 310;
                Nodes11 = 340;
                break;
            case 200:
                Nodes1 = 8;
                Nodes2 = 10;
                Nodes3 = 12;
                Nodes4 = 14;
                Nodes5 = 16;
                Nodes6 = 18;
                Nodes7 = 20;
                Nodes8 = 22;
                Nodes9 = 24;
                Nodes10 = 26;
                Nodes11 = 28;
                break;
            case 300:
                Nodes1 = 8;
                Nodes2 = 8;
                Nodes3 = 8;
                Nodes4 = 8;
                Nodes5 = 8;
                Nodes6 = 8;
                Nodes7 = 8;
                Nodes8 = 8;
                Nodes9 = 8;
                Nodes10 = 8;
                Nodes11 = 8;
                break;
            default:
                Nodes1 = 8;
                Nodes2 = 8;
                Nodes3 = 8;
                Nodes4 = 8;
                Nodes5 = 8;
                Nodes6 = 8;
                Nodes7 = 8;
                Nodes8 = 8;
                Nodes9 = 8;
                Nodes10 = 8;
                Nodes11 = 8;
                break;
        }

        

        Bike.GetComponent<CyclistController>().numnode = Nodes11;



        Diamond1.position = new Vector3(path.nodes[Nodes1].x, path.nodes[Nodes1].y, path.nodes[Nodes1].z);
        Diamond2.position = new Vector3(path.nodes[Nodes2].x, path.nodes[Nodes2].y, path.nodes[Nodes2].z);
        Diamond3.position = new Vector3(path.nodes[Nodes3].x, path.nodes[Nodes3].y, path.nodes[Nodes3].z);
        Diamond4.position = new Vector3(path.nodes[Nodes4].x, path.nodes[Nodes4].y, path.nodes[Nodes4].z);
        Diamond5.position = new Vector3(path.nodes[Nodes5].x, path.nodes[Nodes5].y, path.nodes[Nodes5].z);
        Diamond6.position = new Vector3(path.nodes[Nodes6].x, path.nodes[Nodes6].y, path.nodes[Nodes6].z);
        Diamond7.position = new Vector3(path.nodes[Nodes7].x, path.nodes[Nodes7].y, path.nodes[Nodes7].z);
        Diamond8.position = new Vector3(path.nodes[Nodes8].x, path.nodes[Nodes8].y, path.nodes[Nodes8].z);
        Diamond9.position = new Vector3(path.nodes[Nodes9].x, path.nodes[Nodes9].y, path.nodes[Nodes9].z);
        Diamond10.position = new Vector3(path.nodes[Nodes10].x, path.nodes[Nodes10].y, path.nodes[Nodes10].z);
        FinishLine.position = new Vector3(path.nodes[Nodes11].x, path.nodes[Nodes11].y, path.nodes[Nodes11].z);
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
