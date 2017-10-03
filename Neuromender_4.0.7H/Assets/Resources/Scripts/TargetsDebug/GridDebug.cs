using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridDebug : MonoBehaviour {

    public LoginControl userConfig;
    public GameObject slots;
    public Text OrderNo;
    public Text CellNo;

    int x = -300;
    int y = -250;
   

	// Use this for initialization
	void Start ()
    {
        userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
        CellNo.GetComponent<Text>();

        int count = 1;
        for (int rows = 0; rows <=7 ; rows++)
        {
            
            for (int cols = 0; cols <= 9; cols++)
            {
                GameObject slot = Instantiate(slots);
                slot.transform.parent = this.gameObject.transform;
                slot.name = "Slot" + rows + "." + cols;
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + 60;  

                if (cols == 9)
                {
                    x = -300;
                    y = y + 60;
                    
                }
                count = count + 1;
                CellNo.text = count.ToString();             
            }

        }
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
