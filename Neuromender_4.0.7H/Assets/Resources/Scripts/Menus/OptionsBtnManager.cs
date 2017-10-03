using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsBtnManager : MonoBehaviour {

	public bool debugData;
	public string menu;

	private GameObject menuControl;
	public GameObject debug;

	private GameObject bg1;
	public GameObject bg2;
	private LoginControl userConfig;

	private bool hoverDebugOn = false;
	private bool hoverDebugOff = false;
	private bool hoverHowTo = false;
	private bool hoverCredits = false;
	private bool hoverBack = false;
	private bool showingCredits = false;

	public GameObject levelSelectPanel;


	void Start()
	{
		debug = GameObject.Find ("ToggleDebug");
		menuControl = GameObject.Find ("OptionsCanvas");

		if (GameObject.Find("DatabaseController"))
		{
			userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>();
		}

		bg1 = GameObject.Find ("BackGround1");
		bg1.SetActive(false);
		//bg2 = GameObject.Find("HowToVideo");
		bg2.SetActive(false);

		debugData = false;
		levelSelectPanel.SetActive(false);
	}

	void Update()
	{

		if (menu == "options") 
		{
			hoverTextPopulator ();
		}

		if (showingCredits == true) 
		{
			Debug.Log("Showing credits");
			foreach(Transform child in menuControl.transform)
			{
				if(child.name != "RtnOptions" || child.name != "ShowPrevTeams")
				{
					child.gameObject.SetActive(false);
				}
				else
				{
					child.gameObject.SetActive(true);
				}
			}
		}


		if (menu == "options" && bg2.GetComponent<PlayVideo> ().videoPlaying == false && bg2.activeSelf == false && showingCredits == false) 
		{
			foreach (Transform child in menuControl.transform) 
			{
				if(child.name != "RtnOptions" && child.name != "ShowPrevTeams") // disable the return to menu and go to list of previous teams buttons when on the credit screen
                {
					child.gameObject.SetActive (true);
				}
			} 
		} 
		else
			if (bg1.activeSelf != false) 
			{
				foreach (Transform child in menuControl.transform) 
				{
					if(child.name != "RtnOptions" && child.name != "ShowPrevTeams") // disable the return to menu and go to list of previous teams buttons when on the credit screen
                {
						child.gameObject.SetActive (false);
					}
					else
					{
					child.gameObject.SetActive (true); // displays the return to menu and go to list of previous teams buttons when on the credit screen
					}
				}
			} 
			else 
			{
				{
					foreach(Transform child in menuControl.transform)
					{
						child.gameObject.SetActive(false);
					}
				}
			}
	}

	public void ToggleDebugging()
	{
		if (!bg1.activeSelf && !bg2.GetComponent<PlayVideo> ().videoPlaying) 
		{
			bg2.SetActive (false);
			if (GameObject.Find ("DatabaseController").GetComponent<LoginControl> ().config.showDebug == false) 
			{

				debugData = true;
				GameObject.Find ("DatabaseController").GetComponent<LoginControl> ().config.showDebug = true;
			} 
			else 
			{
                
				debugData = false;
				GameObject.Find ("DatabaseController").GetComponent<LoginControl> ().config.showDebug = false;
			}
			if(GameObject.Find ("DatabaseController").GetComponent<LoginControl> ().config.showDebug == false)
				debug.GetComponentInChildren<Text>().text = "Debug is: Off";
			else
				debug.GetComponentInChildren<Text>().text = "Debug is: On";
			Debug.Log("Debug :" + GameObject.Find ("DatabaseController").GetComponent<LoginControl> ().config.showDebug);
		}
	}

	public void HowToPlay()
	{
		//menu = "howtoplay";
		SetBG(2);
		hoverDebugOn = false;
		hoverDebugOff = false;
		hoverHowTo = false;
		hoverCredits = false;
		hoverBack = false;
		levelSelectPanel.SetActive(false);
		bg2.GetComponent<PlayVideo>().PlayHowToVideo();
	}

	public void ShowCredits()
	{
		SetBG(1);
		hoverDebugOn = false;
		hoverDebugOff = false;
		hoverHowTo = false;
		hoverCredits = false;
		hoverBack = false;
		showingCredits = true;
		levelSelectPanel.SetActive(false);
		menu = "credits";
	}

	public void ReturnToMenu()
	{

		menu = "Main";

	}
	public void ReturnToOptions()
	{
		showingCredits = false;
        menu = "options";
        foreach (Transform child in menuControl.transform)
		{
			if(child.name != "RtnOptions" && child.name != "ShowPrevTeams")
			{
				child.gameObject.SetActive(true);
			}
			else
			{
				child.gameObject.SetActive(false);
				bg1.SetActive(false);
			}
		}
	}

    public void ViewPastTeams()
    {
		Application.OpenURL("http://vegas.murdoch.edu.au/neuromender3/Main/About.php");
    }


	public void HoverDebug()
	{
		if (debugData == false) 
		{
			hoverDebugOn = true;
			hoverDebugOff = false;
			hoverHowTo = false;
			hoverCredits = false;
			hoverBack = false;
		} 
		else 
		{
			hoverDebugOn = false;
			hoverDebugOff = true;
			hoverHowTo = false;
			hoverCredits = false;
			hoverBack = false;
		}
	}

	public void HoverHowTo()
	{
		hoverDebugOn = false;
		hoverDebugOff = false;
		hoverHowTo = true;
		hoverCredits = false;
		hoverBack = false;
	}

	public void HoverCredits()
	{
		//print("Beach (Low Detail)");
		hoverDebugOn = false;
		hoverDebugOff = false;
		hoverHowTo = false;
		hoverCredits = true;
		hoverBack = false;
	}

	public void HoverReturn()
	{
		hoverDebugOn = false;
		hoverDebugOff = false;
		hoverHowTo = false;
		hoverCredits = false;
		hoverBack = true;
	}

	void hoverTextPopulator()
	{
		if (hoverDebugOn)
		{
			// print("Temple (High Detail)");
			
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Debug On. \nThis will show the debug panel in the level.";
			
		}
		else if (hoverDebugOff)
		{
			// print("Temple (High Detail)");
			
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Debug Off. \nThis will hide the debug panel in the level.";
			
		}
		else if (hoverHowTo)
		{
			// print("Temple (High Detail)");
			
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "How To Video. \nClick on this to view the Tutorial Video.";
			
		}
		else if (hoverCredits)
		{
			// print("Temple (High Detail)");
			
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Credits. \nClick on this to view the credits.";
			
		}
		else if (hoverBack)
		{
			// print("Temple (High Detail)");
			
			levelSelectPanel.SetActive(true);
			GameObject.Find("LevelSelection").GetComponent<Text>().text = "Back. \nClick on this to go back to the main menu.";
			
		}
	}


	public void SetBG(int bg)
	{
		if (bg == 1) {
			bg1.SetActive(!bg1.activeSelf);
		}
		else if (bg == 2)
		{
			bg2.SetActive(!bg2.activeSelf);
		}
		else {
			print("Error BG");
		}
	}
}
