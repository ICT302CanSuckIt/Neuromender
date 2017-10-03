using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DisclaimerAgree : MonoBehaviour {

    private database _dbController;
    private UserConfig _userConfig;

	// Use this for initialization
	void Start ()
    {
        _dbController = GameObject.Find("DatabaseController").GetComponent<database>();
        _userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>().config; //to get the session id
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        string sessionId = _userConfig.SessionID.ToString();

        string table = "Session";
        string fields = "Disclaimer = 1"; //set to 1 for Agree
        string where = "SessionID =" + sessionId;
        _dbController.UpdateData(table, fields, where);

        SceneManager.LoadScene("MainMenu");
    }
}
