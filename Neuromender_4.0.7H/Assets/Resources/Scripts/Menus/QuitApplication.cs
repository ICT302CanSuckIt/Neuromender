using UnityEngine;
using System.Collections;

public class QuitApplication : MonoBehaviour {

    private LoginControl _loginController;

	// Use this for initialization
	void Start ()
    {
        _loginController = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        //check if session is opened
        if (_loginController.config.SessionID > 0)
            _loginController.logoutSetAverage(); //close the session

        Application.Quit();
    }
}
