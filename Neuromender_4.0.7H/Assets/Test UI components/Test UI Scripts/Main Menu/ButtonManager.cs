using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	public GameObject menuControl;

	private bool hoverLvlSelect = false;
	private bool hoverOptions = false;
	private bool hoverWebsite = false;
	private bool hoverLogout = false;
	public GameObject levelSelectPanel;

	void Start()
	{
		this.gameObject.GetComponent<OptionsBtnManager>().menu = "Main";
		levelSelectPanel.SetActive(false);
	}

	void Update()
	{
		if (this.gameObject.GetComponent<OptionsBtnManager> ().menu == "Main") 
		{
			hoverTextPopulator ();
		}

		if (this.gameObject.GetComponent<OptionsBtnManager>().menu == "Main") 
		{
			foreach (Transform child in menuControl.transform) 
			{
				child.gameObject.SetActive (true);
			} 
		} 
		else 
		{
			foreach (Transform child in menuControl.transform) 
			{
				child.gameObject.SetActive (false);
			} 
		}
	}

	public void StartButton()
	{
		Debug.Log("Start button clicked");
		this.gameObject.GetComponent<OptionsBtnManager>().menu = "TaskSelect";
	}

	public void OptionsButton()
	{
		Debug.Log("Options button clicked");
		this.gameObject.GetComponent<OptionsBtnManager>().menu = "options";

		//Application.LoadLevel ("Options");
	}

	public void WebsiteButton()
	{
		Debug.Log("Website button clicked");
		Application.OpenURL("http://vegas.murdoch.edu.au/neuromender3/Main/Login.php");
	}

	public void LogoutButton()
	{
		Debug.Log ("Log Out button clicked");
		if (GameObject.Find("DatabaseController"))
		{
			GameObject.Find("DatabaseController").GetComponent<LoginControl>().logoutSetAverage();
			GameObject.Destroy(GameObject.Find("DatabaseController"));
		}
        SceneManager.LoadScene("Login");
	}

	public void HoverStart()
	{
		hoverLvlSelect = true;
		hoverOptions = false;
		hoverWebsite = false;
		hoverLogout = false;
	}

	public void HoverOptions()
	{
		hoverLvlSelect = false;
		hoverOptions = true;
		hoverWebsite = false;
		hoverLogout = false;
	}

	public void HoverWebsite()
	{
		hoverLvlSelect = false;
		hoverOptions = false;
		hoverWebsite = true;
		hoverLogout = false;
	}

	public void HoverLogout()
	{
		hoverLvlSelect = false;
		hoverOptions = false;
		hoverWebsite = false;
		hoverLogout = true;
	}

	void hoverTextPopulator()
	{
		if (hoverLvlSelect)
		{
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Task Selection. \nClick on this to go to the task selection.";	
		}
		else if (hoverOptions)
		{
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Options Menu. \nClick on this to turn on Debug or view Credits.";	
		}
		else if (hoverWebsite)
		{
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Website. \nClick on this to go to the Neuromender Web Portal.";
		}
		else if (hoverLogout)
		{
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Logout. \nClick on this to log out and end this Session.";	
		}
	}
}
