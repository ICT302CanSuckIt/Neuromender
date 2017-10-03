using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuActionEasy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MENU_EASY_GoToPage(string EasyRoute)
    {
        SceneManager.LoadScene(EasyRoute); // altenartive "EasyRoute"
    }
}
